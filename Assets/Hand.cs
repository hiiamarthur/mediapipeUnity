using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

  private GameObject handLandMarkAnnotation;
  private GameObject handHandRectMarkAnnotation;
  private GameObject palmAnnotation;
  private Vector3[] rectCoordiante = new Vector3[4];
  private Quaternion rectAngle = new Quaternion();
  private Assets.Helper Helper = new Assets.Helper();
  // Start is called before the first frame update
  void Start()
  {
    handLandMarkAnnotation = GameObject.Find("HandLandmarks Annotation");
    handHandRectMarkAnnotation = GameObject.Find("HandRectsFromLandmarks Annotation");
    palmAnnotation = GameObject.Find("PalmDetections Annotation");
    //Instantiate(palmAnnotation, transform);
  }

  // Update is called once per frame
  void Update()
  {
    UdpateCall();
    //this.gameObject.
  }

  private void UdpateCall()
  {

    var temphandList = Helper.GetChildren(handLandMarkAnnotation);
    if (temphandList == null || temphandList.Count == 0) return;
    var tempHandRectList = Helper.GetChildren(handHandRectMarkAnnotation);
    if (tempHandRectList == null || tempHandRectList.Count == 0) return;
    var tempHandObject = temphandList[0];
    var temphandRectObject = tempHandRectList[0];
    var tempPointList = Helper.GetChildWithName(tempHandObject, "Point List Annotation");

    var rectangleInstance = temphandRectObject.GetComponent<LineRenderer>();
    if (rectangleInstance != null)
    {
      rectangleInstance.GetPositions(rectCoordiante);
      rectAngle = rectangleInstance.transform.rotation;
      var handCoordinate = Helper.mathFunc.GetBoxCenter(rectCoordiante);
      gameObject.transform.SetPositionAndRotation(handCoordinate, rectAngle);
      Debug.Log("Hand Update" + " " + rectCoordiante[0] + rectCoordiante[1] + rectCoordiante[2] + rectCoordiante[3] + temphandRectObject.name + temphandRectObject.transform.position + " " + tempPointList.name + " " + tempPointList.transform.position + gameObject.transform.position);
    }

    if (GetComponent<LineRenderer>() != null)
    {
      gameObject.AddComponent<LineRenderer>();
    }
    else {
      var lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

  }
}
