using UnityEngine;
using System.Collections;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using System;

[Serializable]
public class Pose
{
  public string gestureName;
  public Vector3 handCoordinate;
  //public float handScale;
  public List<Vector3> _positions; // Relative to hand

  [HideInInspector]
  public float time = 0.0f;

  public Pose(string name, List<Vector3> positions)
  {
    gestureName = name;
    //handScale = scale;
    _positions = positions;
  }
}
public class PoseDetector : MonoBehaviour
{
  public PoseLandmarkListAnnotation poseLandmarkListAnnotation;
  public List<Pose> poses = new List<Pose>();
  public float meanErrorThreshold = 2;
  public float varienceErrorThreshold = 50;
  //public int x = 0;
  private Helper _helper = new Helper();
  private List<GameObject> annotationObjectList = new List<GameObject>();

  public event EventHandler<string> onPoseDetected;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    var children = _helper.GetChildren(poseLandmarkListAnnotation.gameObject);
    if (poseLandmarkListAnnotation && poseLandmarkListAnnotation.isActive)
    {
      //Debug.Log("[poseLandmarkListAnnotation]" + );
      //if()
      if (annotationObjectList.Count == 0)
      {
        annotationObjectList = _helper.GetChildren(poseLandmarkListAnnotation.transform.GetChild(0).gameObject);
      }
      Dictionary<string, dynamic> similarityRank = new Dictionary<string, dynamic>();
      Statisic runTimeStat = CalaulateStatisic(annotationObjectList.Select(gameObject =>
      {
        return gameObject.transform.localPosition;
      }).ToList(), "run Time");
      for (int i = 0; i < poses.Count; i++)
      {
        Statisic poseStat = CalaulateStatisic(poses[i]._positions, "test");
        Vector3 meanDiff = (poseStat.mean - runTimeStat.mean);

        Vector3 varienceDiff = poseStat.variance - runTimeStat.variance;
        Debug.Log("[poseLandmarkListAnnotation] loop" + poses[i].gestureName + meanDiff + varienceDiff);


        similarityRank.Add(poses[i].gestureName, new
        {
          mean = Vector3.Magnitude(meanDiff),
          varience = Vector3.Magnitude(varienceDiff)
        });
        //similarityRank.Add(Math.Abs(meanDiff.x / runTimeStat.mean.x))
      };

      var a = similarityRank.OrderBy(keyValuePair => keyValuePair.Value.mean).First();
      var b = similarityRank.OrderBy(keyValuePair => keyValuePair.Value.varience).First();
      Debug.Log("[poseLandmarkListAnnotation] result" + a.Key + b.Key + a.Value.mean + " " + b.Value.varience);

      if (a.Key == b.Key && a.Value.mean <= meanErrorThreshold && a.Value.varience <= varienceErrorThreshold)
      {
        Debug.Log("[poseLandmarkListAnnotation] final Result" + b.Key);
        string result = b.Key;
        //onPoseDetected?.Invoke(this, result);
        if (onPoseDetected != null)
        {
          onPoseDetected(this, result);
        }
      }
      else
      {
        onPoseDetected(this, "");
      }


      //Statistic 

    }
    else
    {
      Debug.Log("poseLandmarkListAnnoataion" + children.Count);
    }
  }

  public void SavePose()
  {
    List<Vector3> positions = annotationObjectList.Select(t => t.transform.localPosition).ToList();
    poses.Add(new Pose("New Pose", positions));
  }

  public void compareMeanAndVarience()
  {
    CalaulateStatisic(poses[0]._positions, "left hand");
    //CalaulateStatisic()
  }

  public struct Statisic
  {
    public Vector3 mean;
    public Vector3 variance;
  }

  public Statisic CalaulateStatisic(List<Vector3> positions, string test)
  {
    Vector3 meanVector = Vector3.zero;
    Vector3 varianceVector = Vector3.zero;
    if (positions.Count > 0)
    {
      ;
      positions.ForEach(position =>
      {
        meanVector += position;
      });
      meanVector /= positions.Count;
      positions.ForEach(position =>
      {
        Vector3 tempVector = (position - meanVector);
        varianceVector += new Vector3(tempVector.x * tempVector.x, tempVector.y * tempVector.y, tempVector.z * tempVector.z);
      });
      varianceVector /= positions.Count;
      Debug.Log("compareMeanAndVariance " + test + varianceVector + meanVector);
    }
    return new Statisic
    {
      mean = meanVector,
      variance = varianceVector
    };
  }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PoseDetector))]
public class CustomInspectorPoseRecognizer : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    PoseDetector poseDetector = (PoseDetector)target;
    if (!GUILayout.Button("Save current pose")) return;
    //handDetector.SaveAsGesture();
    poseDetector.SavePose();
  }
}

#endif
