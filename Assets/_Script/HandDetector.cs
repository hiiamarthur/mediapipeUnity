using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;
using UnityEngine.UI;
using Assets;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using Mediapipe.Unity.HandTracking;
using Mediapipe.Unity;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
#endif


[Serializable]
public class Gesture
{
  public string gestureName;
  public Vector3 handCoordinate;
  public List<Vector3> positionsPerFinger; // Relative to hand

  [HideInInspector]
  public float time = 0.0f;

  public Gesture(string name, List<Vector3> positions, Vector3 coordinate)
  {
    handCoordinate = coordinate;
    gestureName = name;
    positionsPerFinger = positions;
  }
}

public class HandDetector : MonoBehaviour
{
  public MultiHandLandmarkListAnnotation HandlandMarks;

  private Helper helper = new Helper();
  //public  HandlandMarks;

  public Transform LW, LPT, LF1, LF2, LF3, LF4, RW, RPT, RF1, RF2, RF3, RF4;
  public GameObject LHC, RHC;

  public Text rightTxt, leftTxt;

  public bool leftHandExist, rightHandExist, leftHandClose, rightHandclose;
  public Camera mainCam;

  public Grabable rightGrabedObj, leftGrabedObj, droppingObj;


  public List<GameObject> positionList = new List<GameObject>();

  public RectangleListAnnotation RectangleListAnnotation;
  public List<Gesture> gesture = new List<Gesture>();

  //private UnityEngine.Screen screenObj = new UnityEngine.Screen(); 

  [Header("Physics")]
  public bool withPhysics = false;
  public float gravity = 5.0f;
  public Vector3 ground;
  public Vector3 floor;
  public Vector3[] boxDirectionVector = new Vector3[2];
  public Grabable boxObj;
  public float ySpeed = 0;

  // Start is called before the first frame update
  void Start()
  {
    ground = mainCam.WorldToScreenPoint(Vector3.down);
    floor = mainCam.WorldToScreenPoint(Vector3.one);
    LHC = new GameObject();
    RHC = new GameObject();
    //ground = mainCam.ScreenToWorldPoint(new Vector3(0, UnityEngine.Screen.height, 0));
    //children = Helper.GetChildren(HandlandMarks);
  }

  public void SaveAsGesture()
  {
    //List<Vector3> positions = fingers.Select(t => hand.transform.InverseTransformPoint(t.transform.position)).ToList();
    //List<Vector3> positions = positionList.Select(t => hand.transform.InverseTransformPoint(t.transform.position)).ToList();
    Debug.Log("positionList" + positionList.Count);
    List<Vector3> positions = positionList.Select(t => t.transform.localPosition).ToList();

    //savedGestures.Add(new Gesture("New Gesture", positions, hand.transform.localPosition));
    gesture.Add(new Gesture("New Gesture", positions, (positions[0] + positions[2] + positions[9] + positions[13] + positions[17]) / 5.0f));
  }

  // Update is called once per frame
  void Update()
  {

    leftHandExist = false;
    rightHandExist = false;
    if (HandlandMarks.children.Count > 0)
    {
      if (HandlandMarks.children[0].isActive)
      {
        // Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0]);
        // Debug.Log(("count "+HandlandMarks.children.Count));
        if (HandlandMarks.children[0].TryGetComponent<HandLandmarkListAnnotation>(out HandLandmarkListAnnotation hand))
        {
          // Debug.Log("### hand  is "+hand.whichHand);
          // HandlandMarks.children[0].
          //Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
          if (hand.whichHand == "Left")
          {
            LW = hand.transform.GetChild(0).GetChild(0);
            LPT = hand.transform.GetChild(0).GetChild(9);
            LF1 = hand.transform.GetChild(0).GetChild(8);
            LF2 = hand.transform.GetChild(0).GetChild(12);
            LF3 = hand.transform.GetChild(0).GetChild(16);
            LF4 = hand.transform.GetChild(0).GetChild(20);
            LHC.transform.position = (LW.position + LPT.position + LF1.position + LF2.position + LF3.position + LF4.position) / 5.0f;

          }
          else
          {
            RW = hand.transform.GetChild(0).GetChild(0);
            RPT = hand.transform.GetChild(0).GetChild(9);
            RF1 = hand.transform.GetChild(0).GetChild(8);
            RF2 = hand.transform.GetChild(0).GetChild(12);
            RF3 = hand.transform.GetChild(0).GetChild(16);
            RF4 = hand.transform.GetChild(0).GetChild(20);
            RHC.transform.position = (RW.position + RPT.position + RF1.position + RF2.position + RF3.position + RF4.position) / 5.0f;
          }
        }
        else
        {
          Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0].name);
        }
      }

      if (HandlandMarks.children.Count > 1)
      {
        if (HandlandMarks.children[0].isActive)
        {
          // Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0]);
          // Debug.Log(("count "+HandlandMarks.children.Count));
          if (HandlandMarks.children[0]
              .TryGetComponent<HandLandmarkListAnnotation>(out HandLandmarkListAnnotation hand))
          {
            Debug.Log("whichHand" + hand.whichHand);
            var handList = hand.transform.GetChild(0);
            var handChildList = helper.GetChildren(handList.gameObject);
            if (positionList.Count == 0)
            {
              positionList = handChildList;
              Debug.Log("assign" + helper.GetChildren(handList.gameObject).Count);
            }


            //          if (RectangleListAnnotation.children.Count > 0)
            //          {
            //            if (RectangleListAnnotation.transform.GetChild(0).TryGetComponent<LineRenderer>(out LineRenderer linerenderer1))
            //            {
            //              Vector3[] rectanglePosition = new Vector3[4];
            //              linerenderer1.GetPositions(rectanglePosition);
            //              var scale = Vector3.Distance(rectanglePosition[0], rectanglePosition[1]);
            //              Debug.Log("hand distance " + scale + " " + handList.GetChild(5).localPosition + handList.GetChild(8).localPosition
            //+ " " + Vector3.Distance(handList.GetChild(5).position, handList.GetChild(8).position) + " " + Vector3.Distance(handList.GetChild(5).position, handList.GetChild(8).position) / scale * 1000);
            //              //if (hand.transform.GetChild(1).GetChild(0).TryGetComponent<LineRenderer>(out LineRenderer lineRenderer))
            //              //{
            //              //  Vector3[] positions = new Vector3[2];
            //              //  lineRenderer.GetPositions(positions);
            //              //  Debug.Log("LineRenderer" + positions[0] + positions[1] + " " + scale + " " + Vector3.Distance(positions[0], positions[1]) / scale);
            //              //};
            //            }
            //          }
            // Debug.Log("### hand  is "+hand.whichHand);
            // HandlandMarks.children[0].
            // Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
            if (hand.whichHand == "Left")
            {
              LW = handList.GetChild(0);
              LPT = handList.GetChild(9);
              LF1 = handList.GetChild(8);
              LF2 = handList.GetChild(12);
              LF3 = handList.GetChild(16);
              LF4 = handList.GetChild(20);
              //leftHandClose = IsGrab(LW, LPT, LF1, LF2, LF3, LF4);
              leftHandClose = SimilarityGesture(handChildList) == "Stone";
              leftHandExist = true;
            }
            else
            {
              RW = handList.GetChild(0);
              RPT = handList.GetChild(9);
              RF1 = handList.GetChild(8);
              RF2 = handList.GetChild(12);
              RF3 = handList.GetChild(16);
              RF4 = handList.GetChild(20);
              //rightHandclose = IsGrab(RW, RPT, RF1, RF2, RF3, RF4);
              rightHandclose = SimilarityGesture(handChildList) == "Stone";
              rightHandExist = true;
            }

            Debug.Log("whichHand3" + rightHandclose + leftHandClose);
          }
          else
          {
            Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0].name);
          }
        }

        if (HandlandMarks.children[1].isActive)
        {
          // Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0]);
          // Debug.Log(("count "+HandlandMarks.children.Count));
          if (HandlandMarks.children[1]
              .TryGetComponent<HandLandmarkListAnnotation>(out HandLandmarkListAnnotation hand))
          {
            Debug.Log("whichHand 2" + hand.whichHand);
            var handList = hand.transform.GetChild(0);
            var handChildList = helper.GetChildren(handList.gameObject);
            // Debug.Log("### hand  is "+hand.whichHand);
            // HandlandMarks.children[0].
            Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
            if (hand.whichHand == "Left")
            {
              LW = handList.GetChild(0);
              LPT = handList.GetChild(9);
              LF1 = handList.GetChild(8);
              LF2 = handList.GetChild(12);
              LF3 = handList.GetChild(16);
              LF4 = hand.transform.GetChild(0).GetChild(20);
              //leftHandClose = IsGrab(LW, LPT, LF1, LF2, LF3, LF4);
              leftHandClose = SimilarityGesture(handChildList) == "Stone";
              leftHandExist = true;
            }
            else
            {
              RW = handList.GetChild(0);
              RPT = handList.GetChild(9);
              RF1 = hand.transform.GetChild(0).GetChild(8);
              RF2 = hand.transform.GetChild(0).GetChild(12);
              RF3 = hand.transform.GetChild(0).GetChild(16);
              RF4 = hand.transform.GetChild(0).GetChild(20);
              //rightHandclose = IsGrab(RW, RPT, RF1, RF2, RF3, RF4);
              rightHandclose = SimilarityGesture(handChildList) == "Stone";
              rightHandExist = true;
            }
          }
          else
          {
            Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0].name);
          }
        }
      }
    }
    Debug.Log("whichHand rightHandclose" + rightHandclose + leftHandClose);
    CheckGrabing();
    CheckReleaseObj();
    UpdateDirectionVector();
    if (!leftHandExist)
    {
      leftHandClose = false;
    }
    if (!rightHandExist)
    {
      rightHandclose = false;
    }
  }

  public void CheckReleaseObj()
  {
    if (rightGrabedObj != null && !rightHandclose)
    {
      droppingObj = rightGrabedObj;
      rightGrabedObj.grabing = false;
      rightGrabedObj.followTarget = null;
      rightGrabedObj = null;
      //grabableObj.enabled = false;
    }
    if (leftGrabedObj != null && !leftHandClose)
    {
      droppingObj = leftGrabedObj;
      leftGrabedObj.grabing = false;
      leftGrabedObj.followTarget = null;
      leftGrabedObj = null;
    }
    if (droppingObj != null && leftGrabedObj == null && rightGrabedObj == null)
    {
      if (UnityEngine.Screen.height <= mainCam.WorldToScreenPoint(droppingObj.transform.position).y)
      {
        Debug.Log("hit floor" + " " + ySpeed + UnityEngine.Screen.height + " " + floor.y + mainCam.WorldToScreenPoint(droppingObj.transform.position));
        if (ySpeed > 0f)
        {
          ySpeed = 0;
        }

        ySpeed -= gravity * Time.deltaTime;
        droppingObj.transform.Translate(new Vector3(0, ySpeed, 0));
      }
      else if (ground.y <= mainCam.WorldToScreenPoint(droppingObj.transform.position).y)
      {

        Vector3 resultVector = (boxDirectionVector[1] - boxDirectionVector[0]);
        //resultVector = Vector3.Normalize(resultVector);
        double a = Math.Sqrt((double)((boxDirectionVector[1].x - boxDirectionVector[0].x) * (boxDirectionVector[1].x - boxDirectionVector[0].x) +
         (boxDirectionVector[1].y - boxDirectionVector[0].y) * (boxDirectionVector[1].y - boxDirectionVector[0].y)));

        float ySpeedMagnitude = ((float)a);
        float ySpeed = resultVector.y;
        ySpeed -= gravity * Time.deltaTime;

        //droppingObj.transform.Trans
        Debug.Log("dropping Above Ground" + ySpeedMagnitude + " " + floor + " " + ground + " " + droppingObj.transform.position.y + " " + UnityEngine.Screen.height + " " + ySpeed + " " + resultVector + " " + Time.deltaTime);
        droppingObj.transform.Translate(new Vector3(0, ySpeed, 0));
      }
      else
      {
        Debug.Log("dropping Below Ground" + " " + droppingObj.transform.localPosition + droppingObj.transform.position + mainCam.WorldToScreenPoint(droppingObj.transform.position));
      }

    }
  }

  public void CheckGrabing()
  {
    if (rightHandclose)
    {
      //var rightScreenPoint = mainCam.WorldToScreenPoint(RHC.transform.position);
      var rightScreenPoint = mainCam.WorldToScreenPoint(RPT.position);
      //Debug.Log(rightScreenPoint);
      Ray ray = mainCam.ScreenPointToRay(rightScreenPoint);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100))
      {
        Debug.Log("###HIT " + hit.transform.name + "right");
        if (hit.transform.TryGetComponent<Grabable>(out Grabable grableObj))
        {
          //if (!grableObj.grabing)
          //{
          grableObj.followTarget = RPT;
          //grableObj.followTarget = RHC.transform;
          grableObj.grabing = true;
          rightGrabedObj = grableObj;
          //}

        }
        // Debug.Log("hit");
      }
    }


    if (leftHandClose)
    {
      var leftScreenPoint = mainCam.WorldToScreenPoint(LPT.position);
      //var leftScreenPoint = mainCam.WorldToScreenPoint(LHC.transform.position);
      //Debug.Log(leftScreenPoint);
      Ray ray = mainCam.ScreenPointToRay(leftScreenPoint);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100))
      {
        Debug.Log("###HIT " + hit.transform.name + "left");
        if (hit.transform.TryGetComponent<Grabable>(out Grabable grableObj))
        {
          //if (!grableObj.grabing)
          //{
          //grableObj.followTarget = LHC.transform;
          grableObj.followTarget = LPT;
          grableObj.grabing = true;
          leftGrabedObj = grableObj;
          //}

        }
        // Debug.Log("hit");
      }
    }

  }

  public bool IsGrab(Transform w, Transform pt, Transform f1, Transform f2, Transform f3, Transform f4)
  {
    var result = true;
    if (IsBetween(f1.position.y, w.position.y, pt.position.y))
    {
      // Debug.Log("F1 "+f1.position.y+" w "+ w.position.y+" pt "+ pt.position.y);
      result = false;
    }

    if (IsBetween(f2.position.y, w.position.y, pt.position.y))
    {
      // Debug.Log("F2 "+f2.position.y+" w "+ w.position.y+" pt "+ pt.position.y);
      result = false;
    }

    if (IsBetween(f3.position.y, w.position.y, pt.position.y))
    {
      // Debug.Log("F3 "+f3.position.y+" w "+ w.position.y+" pt "+ pt.position.y);
      result = false;
    }

    if (IsBetween(f4.position.y, w.position.y, pt.position.y))
    {
      // Debug.Log("F4 "+f4.position.y+" w "+ w.position.y+" pt "+ pt.position.y);
      result = false;
    }

    return result;
  }

  public bool IsBetween(float testValue, float bound1, float bound2)
  {

    return !(testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2));
  }

  public string SimilarityGesture(List<GameObject> vectors)
  {
    Dictionary<string, float> similiarityScore = new Dictionary<string, float>();
    for (int i = 0; i < gesture.Count; i++)
    {
      for (int j = 0; j < vectors.Count(); j++)
      {
        var handPosition = (vectors[0].transform.localPosition + vectors[2].transform.localPosition + vectors[9].transform.localPosition + vectors[13].transform.localPosition + vectors[17].transform.localPosition) / 5.0f;
        var d2 = Vector3.Distance(handPosition, vectors[j].transform.localPosition);
        var d3 = Vector3.Distance(gesture[i].handCoordinate, gesture[i].positionsPerFinger[j]);
        if (similiarityScore.ContainsKey(gesture[i].gestureName))
          similiarityScore[gesture[i].gestureName] += Math.Abs(d3 - d2);
        else
          similiarityScore.Add(gesture[i].gestureName, Math.Abs(d3 - d2));
      }
    }

    return similiarityScore.OrderBy(x => x.Value).First().Key;
  }

  public void ApplyPhysic()
  {
    if (!withPhysics) return;

  }

  public void UpdateDirectionVector()
  {
    boxDirectionVector[0] = boxDirectionVector[1];
    boxDirectionVector[1] = boxObj.transform.position;

  }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HandDetector))]
public class CustomInspectorGestureRecognizer : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    HandDetector handDetector = (HandDetector)target;
    if (!GUILayout.Button("Save current gesture")) return;
    handDetector.SaveAsGesture();
  }
}
#endif
