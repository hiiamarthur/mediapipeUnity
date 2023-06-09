using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;
using UnityEngine.UI;
using Assets;

public class HandDetector : MonoBehaviour
{
  public MultiHandLandmarkListAnnotation HandlandMarks;

  public Transform LW, LPT, LF1, LF2, LF3, LF4, RW, RPT, RF1, RF2, RF3, RF4;

  public Text rightTxt, leftTxt;

  public bool leftHandClose, rightHandclose;
  public Camera mainCam;

  public Grabable rightGrabedObj, leftGrabedObj;

  public List<GameObject> children;

  // Start is called before the first frame update
  void Start()
  {
    //children = Helper.GetChildren(HandlandMarks);
  }

  // Update is called once per frame
  void Update()
  {
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
          Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
          if (hand.whichHand == "Left")
          {
            LW = hand.transform.GetChild(0).GetChild(0);
            LPT = hand.transform.GetChild(0).GetChild(9);
            LF1 = hand.transform.GetChild(0).GetChild(8);
            LF2 = hand.transform.GetChild(0).GetChild(12);
            LF3 = hand.transform.GetChild(0).GetChild(16);
            LF4 = hand.transform.GetChild(0).GetChild(20);
          }
          else
          {
            RW = hand.transform.GetChild(0).GetChild(0);
            RPT = hand.transform.GetChild(0).GetChild(9);
            RF1 = hand.transform.GetChild(0).GetChild(8);
            RF2 = hand.transform.GetChild(0).GetChild(12);
            RF3 = hand.transform.GetChild(0).GetChild(16);
            RF4 = hand.transform.GetChild(0).GetChild(20);
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
            // Debug.Log("### hand  is "+hand.whichHand);
            // HandlandMarks.children[0].
            // Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
            if (hand.whichHand == "Left")
            {
              LW = hand.transform.GetChild(0).GetChild(0);
              LPT = hand.transform.GetChild(0).GetChild(9);
              LF1 = hand.transform.GetChild(0).GetChild(8);
              LF2 = hand.transform.GetChild(0).GetChild(12);
              LF3 = hand.transform.GetChild(0).GetChild(16);
              LF4 = hand.transform.GetChild(0).GetChild(20);
              leftHandClose = IsGrab(LW, LPT, LF1, LF2, LF3, LF4);
            }
            else
            {
              RW = hand.transform.GetChild(0).GetChild(0);
              RPT = hand.transform.GetChild(0).GetChild(9);
              RF1 = hand.transform.GetChild(0).GetChild(8);
              RF2 = hand.transform.GetChild(0).GetChild(12);
              RF3 = hand.transform.GetChild(0).GetChild(16);
              RF4 = hand.transform.GetChild(0).GetChild(20);
              rightHandclose = IsGrab(RW, RPT, RF1, RF2, RF3, RF4);
            }
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
            // Debug.Log("### hand  is "+hand.whichHand);
            // HandlandMarks.children[0].
            Debug.Log("hand mark child 00" + hand.transform.GetChild(0).name);
            if (hand.whichHand == "Left")
            {
              LW = hand.transform.GetChild(0).GetChild(0);
              LPT = hand.transform.GetChild(0).GetChild(9);
              LF1 = hand.transform.GetChild(0).GetChild(8);
              LF2 = hand.transform.GetChild(0).GetChild(12);
              LF3 = hand.transform.GetChild(0).GetChild(16);
              LF4 = hand.transform.GetChild(0).GetChild(20);
              leftHandClose = IsGrab(LW, LPT, LF1, LF2, LF3, LF4);
            }
            else
            {
              RW = hand.transform.GetChild(0).GetChild(0);
              RPT = hand.transform.GetChild(0).GetChild(9);
              RF1 = hand.transform.GetChild(0).GetChild(8);
              RF2 = hand.transform.GetChild(0).GetChild(12);
              RF3 = hand.transform.GetChild(0).GetChild(16);
              RF4 = hand.transform.GetChild(0).GetChild(20);
              rightHandclose = IsGrab(RW, RPT, RF1, RF2, RF3, RF4);
            }
          }
          else
          {
            Debug.Log("HandlandMarks.children[0] " + HandlandMarks.children[0].name);
          }
        }
      }
    }
    CheckGrabing();
    CheckReleaseObj();
  }

  public void CheckReleaseObj()
  {
    if (rightGrabedObj != null && !rightHandclose)
    {
      rightGrabedObj.grabing = false;
      rightGrabedObj.followTarget = null;
      rightGrabedObj = null;
    }
    if (leftGrabedObj != null && !leftHandClose)
    {
      leftGrabedObj.grabing = false;
      leftGrabedObj.followTarget = null;
      leftGrabedObj = null;
    }
  }

  public void CheckGrabing()
  {
    if (rightHandclose)
    {
      var rightScreenPoint = mainCam.WorldToScreenPoint(RPT.position);
      Debug.Log(rightScreenPoint);
      Ray ray = mainCam.ScreenPointToRay(rightScreenPoint);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100))
      {
        Debug.Log("###HIT "+hit.transform.name);
        if (hit.transform.TryGetComponent<Grabable>(out Grabable grableObj))
        {
          grableObj.followTarget = RPT;
          grableObj.grabing = true;
          rightGrabedObj = grableObj;
        }
        // Debug.Log("hit");
      }
    }
    
    
    if (leftHandClose)
    {
      var leftScreenPoint = mainCam.WorldToScreenPoint(LPT.position);
      Debug.Log(leftScreenPoint);
      Ray ray = mainCam.ScreenPointToRay(leftScreenPoint);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100))
      {
        Debug.Log("###HIT "+hit.transform.name);
        if (hit.transform.TryGetComponent<Grabable>(out Grabable grableObj))
        {
          grableObj.followTarget = LPT;
          grableObj.grabing = true;
          leftGrabedObj = grableObj;
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
}
