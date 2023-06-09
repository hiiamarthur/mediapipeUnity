using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.HandTracking;
using Mediapipe.Unity;
using Mediapipe;

public class GestureTracking : MonoBehaviour
{

  [SerializeField] private HandTrackingSolution solution = null;
  // Start is called before the first frame update
  //HandTrackingSolution solution = new HandTrackingSolution();

  protected void Start()
  {
    Debug.Log("onstart run gesture start");
    solution.Clicked += OnButtonClicked;
    //return base.Start();
  }

  //protected override void OnStartRun()
  //{
  //  base.OnStartRun();
  //}


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
