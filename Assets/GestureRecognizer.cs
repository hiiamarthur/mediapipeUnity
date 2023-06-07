using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Gesture
{
  public string gestureName;
  public List<Vector3> positionsPerFinger; // Relative to hand
  public UnityEvent onRecognized;

  [HideInInspector]
  public float time = 0.0f;

  public Gesture(string name, List<Vector3> positions, UnityEvent onRecognized)
  {
    gestureName = name;
    positionsPerFinger = positions;
    this.onRecognized = onRecognized;
  }

  public Gesture(string name, List<Vector3> positions)
  {
    gestureName = name;
    positionsPerFinger = positions;
    onRecognized = new UnityEvent();
  }
}

[DisallowMultipleComponent]
public class GestureRecognizer : MonoBehaviour
{
  [Header("Behaviour")]
  [SerializeField] private List<Gesture> savedGestures = new List<Gesture>();
  [SerializeField] private float threshold = 0.05f;
  [SerializeField] private float delay = 0.2f;
  [SerializeField] private UnityEvent onNothingDetected = default;

  [Header("Objects")]
  [SerializeField] private GameObject hand = default;
  [SerializeField] private GameObject[] fingers = default;

  [Header("Debugging")]
  [SerializeField] private Gesture gestureDetected = default;

  private Gesture _previousGestureDetected = null;

  //private GameObject _positionListObject;
  private Assets.Helper Helper = new Assets.Helper();
  private GameObject Annotation;
  private List<GameObject> positionList = new List<GameObject>();
  private bool haveGotPositionList;

  private void Start()
  {
    onNothingDetected.Invoke();
    Annotation = GameObject.Find("HandLandmarks Annotation");
    haveGotPositionList = false;

  }

  private void Update()
  {
    GetFingerPostiion();
    gestureDetected = Recognize();

    if (gestureDetected != _previousGestureDetected)
    {
      if (gestureDetected != null)
        gestureDetected.onRecognized.Invoke();
      else
        onNothingDetected.Invoke();

      _previousGestureDetected = gestureDetected;
    }


    //Debug.Log("positionObject" + positionObject.transform.position + " " + positionObject.transform.localPosition);
    Debug.Log("positionObject" + positionList.Count + haveGotPositionList);

    //ClearFingerPosition()
  }





  //  private GetAllChildren(obj : GameObject) : Array{
  //    var children : Array = GetChildren(obj);
  //    for(var i = 0; i<children.length; i++){
  //        var moreChildren : Array = GetChildren(children[i]);
  //        for(var j = 0; j<moreChildren.length; j++){
  //            children.Add(moreChildren[j]);
  //        }
  //    }
  //    return children;
  //}

  private void GetFingerPostiion()
  {

    //var listAnnotation = Annotation.GetComponentsInChildren<Transform>();
    var a = Annotation.transform.Find("HandLandmarkList Annotation");
    var listAnnotation = Helper.GetChildren(Annotation);
    //Debug.Log("GetFingerPostiion" + haveGotPositionList + listAnnotation);
    if (listAnnotation == null) return;
    if (listAnnotation.Count == 0) return;
    if (!haveGotPositionList)
    {
      var tempHandObject = listAnnotation[0];
      var c = tempHandObject.GetComponent<Mediapipe.Unity.HandLandmarkListAnnotation>();
      var tempHandPointObject = Helper.GetChildWithName(tempHandObject, "Point List Annotation");
      var pointAnnotationList = Helper.GetChildren(tempHandPointObject);
      //Mediapipe.Unity.HandLandmarkListAnnotation.Hand.Right;
      //(c.hand == Mediapipe.Unity.HandLandmarkListAnnotation.Hand.Right)
      //Debug.Log("GetFingerPosition xd" + listAnnotation + "  " + (c.hand == Mediapipe.Unity.HandLandmarkListAnnotation.Hand.Right));
      //var transformList = listAnnotation.GetComponentsInChildren<Transform>();
      //Debug.Log("list yo" + tempHandObject.name + " xd " + tempHandPointObject.name + " " +pointAnnotationList.Count());
      foreach (var t in pointAnnotationList)
      {
        if (t != null && t.gameObject != null)
        {
          positionList.Add(t.gameObject);
          //Debug.Log("xd");
        }
      }

      haveGotPositionList = true;
    }

  }

  private void ClearFingerPosition()
  {
    positionList.Clear();
  }

  public void SaveAsGesture()
  {
    //List<Vector3> positions = fingers.Select(t => hand.transform.InverseTransformPoint(t.transform.position)).ToList();
    List<Vector3> positions = positionList.Select(t => hand.transform.InverseTransformPoint(t.transform.position)).ToList();
    savedGestures.Add(new Gesture("New Gesture", positions));
  }

  private Gesture Recognize()
  {
    bool discardGesture = false;
    float minSumDistances = Mathf.Infinity;
    Gesture bestCandidate = null;

    // For each gesture
    for (int g = 0; g < savedGestures.Count; g++)
    {
      // If the number of fingers does not match, it returns an error
      //if (fingers.Length != savedGestures[g].positionsPerFinger.Count)
      if (positionList.Count != savedGestures[g].positionsPerFinger.Count)
        throw new Exception("Different number of tracked fingers");

      float sumDistances = 0f;

      // For each finger
      //for (int f = 0; f < fingers.Length; f++)
      for (int f = 0; f < positionList.Count; f++)
      {
        //Vector3 fingerRelativePos = hand.transform.InverseTransformPoint(fingers[f].transform.position);
        Vector3 fingerRelativePos = hand.transform.InverseTransformPoint(positionList[f].transform.position);

        // If at least one finger does not enter the threshold we discard the gesture
        if (Vector3.Distance(fingerRelativePos, savedGestures[g].positionsPerFinger[f]) > threshold)
        {
          discardGesture = true;
          savedGestures[g].time = 0.0f;
          break;
        }

        // If all the fingers entered, then we calculate the total of their distances
        sumDistances += Vector3.Distance(fingerRelativePos, savedGestures[g].positionsPerFinger[f]);
      }

      // If we have to discard the gesture, we skip it
      if (discardGesture)
      {
        discardGesture = false;
        continue;
      }

      // If it is valid and the sum of its distances is less than the existing record, it is replaced because it is a better candidate 
      if (sumDistances < minSumDistances)
      {
        if (bestCandidate != null)
          bestCandidate.time = 0.0f;

        minSumDistances = sumDistances;
        bestCandidate = savedGestures[g];
      }
    }

    if (bestCandidate != null)
    {
      bestCandidate.time += Time.deltaTime;

      if (bestCandidate.time < delay)
        bestCandidate = null;
    }

    // If we've found something, we'll return it
    // If we haven't found anything, we return it anyway (newly created object)
    return bestCandidate;
  }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GestureRecognizer))]
public class CustomInspectorGestureRecognizer : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    GestureRecognizer gestureRecognizer = (GestureRecognizer)target;
    if (!GUILayout.Button("Save current gesture")) return;
    gestureRecognizer.SaveAsGesture();
  }
}
#endif
