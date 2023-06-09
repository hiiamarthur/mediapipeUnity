using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.HandTracking;
using Mediapipe.Unity;

using Mediapipe;

public class GestureTrackingSolution : HandTrackingSolution
{
  // Start is called before the first frame update
  //HandTrackingSolution solution = new HandTrackingSolution();

  protected override IEnumerator Start()
  {
    Debug.Log("onstart run gesture start");
    Clicked += OnButtonClicked;
    return default;
    
    //return base.Start();
  }

  //protected override void OnStartRun()
  //{
  //  base.OnStartRun();
  //}
  protected override void OnStartRun()
  {
    
    base.OnStartRun();
    Debug.Log("onstart run gesture");
  }

  public override List<NormalizedLandmark> getNormalPointToInherit()
  {
    var baseReturn = base.getNormalPointToInherit();
    Debug.Log("GestureTrackingSolution count" + baseReturn.Count);
    return baseReturn;
  }

  private void OnButtonClicked(object sender, List<NormalizedLandmark> e)
  {
    Debug.Log("GestureTrackingSolution count" + e.Count);
    //return "xd";
    // TODO: 處理事件
  }

  // Update is called once per frame
  //void Update()
  //{
  //  Debug.Log("GestureTrackingSolution count" + normalizeLandMark.Count);
  //  showMarkList();
  //}

}
