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
  public bool isPlaying, isSelectedDifficulty, isStart = false;
  public Button startButton, difficultyButton;
  public float timeRemain = 3.0f;
  public Text startText;
  public Text difficultyText;
  private int maxDifficulty = 3;
  protected int difficulty;


  private void Start()
  {
    startButton.onClick.AddListener(gameStart);
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
        difficultyButtonClone.transform.parent = transform;
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

    isSelectedDifficulty = true;
    difficulty = inputDifficulty;
    //Debug.Log("gameStart" + inputDifficulty);
  }

  void Update()
  {
    if (isStart)
    {
      
    }
    if (isSelectedDifficulty)
    {
      timeRemain -= Time.deltaTime;

      startText.text = ((int)timeRemain).ToString();
      if (timeRemain < 1)
      {
        startText.text = "start";
      }
      if (timeRemain < 0)
      {
        isPlaying = true;
      }
    }


    if (isPlaying)
    {
      //startText.enabled = false;

    }


  }
}
