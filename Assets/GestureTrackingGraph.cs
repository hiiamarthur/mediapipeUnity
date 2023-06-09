using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.HandTracking;
using Mediapipe.Unity;
using Mediapipe;
public class GestureTrackingGraph : HandTrackingGraph
{
  //public delegate void GestureTrackingValueEvent(HandTrackingValue handTrackingValue);
  //public event GestureTrackingValueEvent OnHandTrackingValueFetched = (h) => { };
  //public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame)
  //{
  //  var handTrackingValue = FetchNextHandTrackingValue();
  //  RenderAnnotation(screenController, handTrackingValue);
  //  screenController.DrawScreen(textureFrame);
  //  OnHandTrackingValueFetched(handTrackingValue);
  //}

  //protected override void Start() {
  //  base.Start();
  //  OnHandWorldLandmarksOutput += OnGetEvent;
  //  OnHandLandmarksOutput += OnGetEvent2;
  //}

  protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
  {
    var a = base.ConfigureCalculatorGraph(config);
    OnHandWorldLandmarksOutput += OnGetEvent;
    OnHandLandmarksOutput += OnGetEvent2;
    return a;

  }

  void OnGetEvent(object sender, OutputEventArgs<List<LandmarkList>> value)
  {
    Debug.Log("afterGetEvent value");
  }

  void OnGetEvent2(object sender, OutputEventArgs<List<NormalizedLandmarkList>> value)
  {
    Debug.Log("afterGetEvent2 value");
  }

  //public v()
  //{

  //}



}
