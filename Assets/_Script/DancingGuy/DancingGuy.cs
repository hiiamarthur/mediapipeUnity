using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class DancingGuy : MonoBehaviour
{
  public GameObject arrow;
  public GameObject slash;
  public GameObject test;
  public GameObject panel2;
  public GameObject poseDetectorGameObject;

  public List<string> poseButtonMap = new List<string>() { "raiseLeftHand", "raiseRightHandHead", "raiseRightHand", "raiseLeftHandHead", "sammiflower" };
  private List<int> answers = new List<int>();
  private List<GameObject> answersObject = new List<GameObject>();
  private int answeredIndex = 0;
  private bool updated = false;
  //private PoseDetector poseDetector;

  // Use this for initialization
  void Start()
  {
    generateRandomQuestion();
    if (poseDetectorGameObject.TryGetComponent<PoseDetector>(out PoseDetector poseDetector))
    {
      poseDetector.onPoseDetected += handlePoseDetected;
    }


  }

  // Update is called once per frame
  void Update()
  {
    List<KeyCode> keys = new List<KeyCode> { KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow };
    keys.ForEach(key =>
    {
      if (Input.GetKeyDown(key))
      {
        handleAnswerQuestion(keys.IndexOf(key));
      }

      //if(Input.key)
    });
  }

  public void generateRandomQuestion()
  {

    for (int i = 0; i < 3; i++)
    {
      int x = UnityEngine.Random.Range(0, 5);
      GameObject prefab;
      if (x == 4)
      {
        prefab = Instantiate(test);
      }
      else
      {
        prefab = Instantiate(x % 2 == 0 ? slash : arrow);
      }
      

      if (prefab.TryGetComponent<Image>(out Image image))
      {
        image.color = Color.white;
      }
      prefab.transform.parent = panel2.transform;
      prefab.SetActive(true);
      prefab.transform.localScale = Vector3.one;
      Quaternion a = Quaternion.identity;
      prefab.transform.SetLocalPositionAndRotation(new Vector3(200 * (i - 1), 0, 0), Quaternion.Euler(0, (x < 2) ? 0 : 180, 0));
      answers.Add(x);
      answersObject.Add(prefab);
    }
  }

  public void handleAnswerQuestion(int index)
  {
    Debug.Log("handleAnserQuestion" + index + " " + answers[answeredIndex] + " " + answeredIndex);
    if (answers[answeredIndex] == index)
    {
      handleCorrect();
    }
    else
    {
      handleWrong();
    }
  }

  public void handleCorrect()
  {
    if (answersObject[answeredIndex].TryGetComponent<Image>(out Image image))
    {
      image.color = Color.green;
    }
    answeredIndex += 1;

    if (answeredIndex >= answers.Count)
    {
      answeredIndex = 0;
      handleNextQuestion();
    }
  }

  public void handleWrong()
  {
    if (answersObject[answeredIndex].TryGetComponent<Image>(out Image image))
    {
      image.color = Color.red;
    }
  }

  public void handleNextQuestion()
  {
    try
    {
      answers.Clear();
      for (int i = 0; i < answersObject.Count; i++)
      {
        Destroy(answersObject[i]);
      }
      answersObject.Clear();
      generateRandomQuestion();
    }
    catch (Exception e)
    {
      Debug.Log("Exception e" + e);
    }

  }

  public void handlePoseDetected(object sender, string result)
  {
    //Debug.Log("handlePoseDetected final" + result);
    if (poseButtonMap.Contains(result))
    {

      if (!updated)
      {
        handleAnswerQuestion(poseButtonMap.IndexOf(result));
        updated = true;
      }

    }
    else
    {
      updated = false;
    }

  }
}

