// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using TMPro;


public class PolygonMemorize : MonoBehaviour
{
  public bool isPlaying, isSelectedDifficulty, isStart = false, createdQuestion, answerQuestion;
  public Button startButton, difficultyButton, clearButton;
  public float startTimeRemain = 4.0f, questionTimeRemain = 5.0f;
  public Text startText;
  public Text difficultyText;
  private int maxDifficulty = 3;
  private bool isClearAnswer;
  protected int difficulty;
  public Dictionary<int, Point> list = new Dictionary<int, Point>();

  private void Start()
  {
    startButton.onClick.AddListener(gameStart);
    clearButton.onClick.AddListener(gameClearAnswer);
    //difficultyButton.onClick.AddListener(gameStart);
  }

  public void gameStart()
  {
    isStart = true;
    startButton.gameObject.SetActive(false);
    if (difficultyButton != null)
    {
      for (int i = 1; i <= maxDifficulty; i++)
      {
        Button difficultyButtonClone = Instantiate(difficultyButton);
        difficultyButtonClone.transform.parent = difficultyText.transform;
        difficultyButtonClone.gameObject.SetActive(true);
        difficultyButtonClone.enabled = true;
        Vector3 buttonPoint = difficultyButton.transform.position;
        Vector3 resultPoint = new Vector3(buttonPoint.x, buttonPoint.y - i * 5.0f, buttonPoint.z);
        TextMeshProUGUI b1text = difficultyButtonClone.GetComponentInChildren<TextMeshProUGUI>();
        b1text.text = $"Level {i}";
        difficultyButtonClone.transform.localScale = Vector3.one;
        difficultyButtonClone.transform.SetPositionAndRotation(resultPoint, Quaternion.identity);
        int _i = i;
        difficultyButtonClone.onClick.AddListener(() => selectDifficulty(_i));
      }

      //isSelectedDifficulty = true;
      difficultyText.text = "Select Difficulty!";

      difficultyText.enabled = true;
    }

  }

  public void selectDifficulty(int inputDifficulty)
  {
    difficultyText.gameObject.active = false;
    isSelectedDifficulty = true;
    difficulty = inputDifficulty;
    //Debug.Log("gameStart" + inputDifficulty);
  }

  public void gameClearAnswer() {
    isClearAnswer = true;
  }

  void Update()
  {
    if (isStart)
    {

    }
    if (isSelectedDifficulty)
    {
      startTimeRemain -= Time.deltaTime;

      startText.text = ((int)startTimeRemain).ToString();
      if (startTimeRemain < 1)
      {
        startText.text = "start";
      }
      if (startTimeRemain < 0)
      {
        isPlaying = true;
        startText.text = "";
      }
    }


    if (isPlaying)
    {
      clearButton.gameObject.active = true;
      //startText.enabled = false;
      if (gameObject.transform.TryGetComponent<ManagePolygon>(out ManagePolygon managePolygon))
      {
        if (!createdQuestion)
        {
          managePolygon.polygonPanel.active = true;
          createdQuestion = managePolygon.generatePolygonLevel(difficulty);
        }

        //Debug.Log("[PolygonMemorize]" + "countdown" + questionTimeRemain);
        if (questionTimeRemain < 0 && !answerQuestion)
        {
          List<GameObject> tempChildrenList = managePolygon.hideQuestionAndShowPanel();
          answerQuestion = true;
          int count = 0;
          foreach (GameObject child in tempChildrenList)
          {
            if (child.name.Contains("point"))
            {
              Vector3 a = Camera.main.WorldToScreenPoint(child.transform.position);
              Debug.Log("points" + child.name + " " + Camera.main.WorldToScreenPoint(gameObject.transform.position) + a);
              Point point = new Point
              {
                center = a,
                gameObject = child
              };
              list.Add(count++, point);
            }
          }
        }
        else
        {
          questionTimeRemain -= Time.deltaTime;
        }

        if (isClearAnswer)
        {
          isClearAnswer = false;
          list.Clear();
          managePolygon.ClearAnsewr();
        }

      }

      if (answerQuestion)
      {
        RaycastHit hit;
        foreach (KeyValuePair<int, Point> item in list)
        {
          var a = item.Value.gameObject.GetComponent<PanelPoint>();
          //if (a.occupied)
          //{
          //  positionNameDict.Add(item.Key, a.occupiedObject.name);
          //}
          //var screenPoint = mainCam.WorldToScreenPoint(item);
          Ray ray = Camera.main.ScreenPointToRay(item.Value.center);
          Debug.Log("[polygonMemorize] ray" + item);
          if (Physics.Raycast(ray, out hit, 100))
          {
            Debug.Log("[polygonMemorize] ray" + " " + item);
            if (hit.transform.TryGetComponent<Grabable>(out Grabable grabable))
            {
              Debug.Log("[polygonMemorize] raycast " + grabable.gameObject.name + " " + item.Key + " " + grabable.Grabing);

              if (!grabable.Grabing && grabable.enableGrabing)
              {

                //Debug.Log("ticktactoe raycast2 " + " " + hit.transform.position + " " + item.Key + " " + grabable.gameObject.transform.position + " " +
                //grabable.gameObject.transform.localPosition + " " + grabable.gameObject.transform.TransformVector(item.Value.center) + " "
                //+ item.Value.center + " " + mainCam.ScreenToWorldPoint(item.Value.center) + " " + item.Value.gameObject.transform.position
                //+ Vector3.ProjectOnPlane(item.Value.center, Vector3.zero));
                //grabable.gameObject.transform.position = item.Value.center;
                //Debug.Log("[TickTacToe] grab" + item.Value.center + grabable.gameObject.transform.position);
                //if (grabable.grabObjName == _toeGrabable.grabObjName)
                //{
                //  _toeGrabable.enableGrabing = false;
                //  _tickGrabable.enableGrabing = true;

                //}
                //else if (grabable.grabObjName == _tickGrabable.grabObjName)
                //{
                //  _tickGrabable.enableGrabing = false;
                //  _toeGrabable.enableGrabing = true;
                //}
                a.occupiedObject = grabable.gameObject;
              }

            }
          }
        }

        if (managePolygon.matchAnswer()) {
          managePolygon.NextQuestion();
          createdQuestion = false;
          answerQuestion = false;
          questionTimeRemain = 5.0f;
        }
      }
    }


  }

}
