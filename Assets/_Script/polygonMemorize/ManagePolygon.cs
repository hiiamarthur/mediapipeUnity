// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Linq;

public class ManagePolygon : MonoBehaviour
{
  public GameObject polygonPanel, resultPanel;
  public List<GameObject> polygons;
  protected List<GameObject> panelPoint = new List<GameObject>();
  private List<int> answers = new List<int>();
  private int _difficulty;
  private GameObject questionObj, answerObj;
  private List<GameObject> answerObjList = new List<GameObject>();
  private Helper helper = new Helper();
  private float showResultTime = 2.0f;
  public bool generatePolygonLevel(int difficulty)
  {
    _difficulty = difficulty;
    RaycastHit hit;
    Vector3 m_DistanceFromCamera;
    m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 30.0f);
    Plane m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
    //Random random = Random();
    GameObject _questionObj = new GameObject("question");
    _questionObj.transform.parent = transform;
    _questionObj.transform.localScale = Vector3.one;
    for (int i = 0; i < difficulty; i++)
    {
      var tempPolygonPanel = Instantiate(polygonPanel);
      tempPolygonPanel.transform.parent = _questionObj.transform;
      tempPolygonPanel.transform.localScale = Vector3.one;
      RectTransform rectTransform = polygonPanel.GetComponent<RectTransform>();
      //tempPolygonPanel.transform.position = polygonPanel.transform.position;
      float a = (float)(i - ((_difficulty - 1) / 2.0f));
      tempPolygonPanel.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, -rectTransform.sizeDelta.y / 2 * a, 0) + Camera.main.WorldToScreenPoint(polygonPanel.transform.position));
      var children = helper.GetChildren(tempPolygonPanel);
      foreach (GameObject child in children)
      {
        if (child.name.Contains("point"))
        {
          panelPoint.Add(child);
        }
      }

      var tempAnswerList = new List<int>();
      for (int j = 0; j < panelPoint.Count; j++)
      {
        int x = Random.Range(0, polygons.Count);
        GameObject polygonCopy = Instantiate(polygons[x]);
        tempAnswerList.Add(x);
        polygonCopy.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(panelPoint[j].transform.position));
        //Ray ray = Camera.main.ScreenPointToRay(panelPoint[j].transform.position);
        float enter = 0.0f;
        Debug.Log("[PolygonMemorize]" + rectTransform.sizeDelta.y + rectTransform.rect.height + Camera.main.transform.position + m_DistanceFromCamera + m_Plane);
        if (Physics.Raycast(ray, out hit, 100))
        {
          Debug.Log("[PolygonMemorize]" + hit.transform.position);
        }

        helper.AssignChildToExisingGameObj(polygonCopy, panelPoint[j], 30.0f);
        answers.Add(x);
      }

      panelPoint.Clear();
    }
    polygonPanel.SetActive(false);
    questionObj = _questionObj;
    return true;
  }

  public List<GameObject> hideQuestionAndShowPanel()
  {
    polygonPanel.SetActive(true);
    EnablePolygonGrabable(true);
    Destroy(questionObj);
    GameObject _answerObj = new GameObject("answer");
    _answerObj.transform.parent = transform;
    _answerObj.transform.localScale = Vector3.one * 1.5f;
    List<GameObject> resultPointList = new List<GameObject>();
    for (int i = 0; i < _difficulty; i++)
    {
      var tempPolygonPanel = Instantiate(polygonPanel);
      tempPolygonPanel.transform.parent = _answerObj.transform;
      tempPolygonPanel.transform.localScale = Vector3.one;
      tempPolygonPanel.name = string.Concat(polygonPanel.name, i);
      RectTransform rectTransform = polygonPanel.GetComponent<RectTransform>();
      float a = (float)(i - ((_difficulty - 1) / 2.0f));
      Debug.Log("[polygonMemorize] foreach" + a);
      tempPolygonPanel.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, -rectTransform.sizeDelta.y * 1.5f / 2 * a, 0) + Camera.main.WorldToScreenPoint(polygonPanel.transform.position));
      List<GameObject> tempChildrenList = helper.GetChildren(tempPolygonPanel);

      resultPointList.AddRange(tempChildrenList);
      foreach (GameObject tempChild in tempChildrenList)
      {
        if (tempChild.name.Contains("point"))
        {

          tempChild.AddComponent<PanelPoint>();
          answerObjList.Add(tempChild);
        }

      }
    }
    polygonPanel.SetActive(false);
    answerObj = _answerObj;
    return resultPointList;


  }

  public void ClearAnswers(int index = -1)
  {
    for (int i = 0; i < answerObjList.Count; i++)
    {
      if (index >= 0 && i != index) continue;
      //Destroy(answerObjList[i]);
      if (answerObjList[i].TryGetComponent<PanelPoint>(out PanelPoint panelPoint))
      {
        panelPoint.occupied = false;
        //Destroy(panelPoint.occupiedObject);
        panelPoint.occupiedObject = null;
        var children = helper.GetChildren(answerObjList[i]);
        for (int j = 0; j < children.Count; j++)
        {
          Destroy(children[j]);
        }
      }
    }
  }

  //public void ClearOneAnswer(int index)
  //{
  //  if (answerObjList.ElementAtOrDefault(index) != null)
  //  {
  //    if (answerObjList[index].TryGetComponent<PanelPoint>(out PanelPoint panelPoint))
  //    {
  //      panelPoint.occupied = false;
  //      //Destroy(panelPoint.occupiedObject);
  //      panelPoint.occupiedObject = null;
  //      var children = helper.GetChildren(answerObjList[i]);
  //      for (int j = 0; j < children.Count; j++)
  //      {
  //        Destroy(children[j]);
  //      }
  //    }
  //  }
  //}

  public bool matchAnswer()
  {

    bool result = answerObjList.All((answerObj) =>
     {
       if (answerObj.transform.TryGetComponent<PanelPoint>(out PanelPoint panelPoint))
       {
         if (!panelPoint.occupied) return false;
         var b = polygons.FindIndex(polygon =>
       {
         return panelPoint.occupiedObject.name.Contains(polygon.name);
       });
         //Debug.Log("[polygonmemorize] match answer" + answers[answerObjList.IndexOf(answerObj)] + panelPoint.occupied + b);
         return panelPoint.occupied && b == answers[answerObjList.IndexOf(answerObj)];
       }
       else
       {
         return false;
       }
     });



    if (result)
    {
      if (!resultPanel.active)
        resultPanel.SetActive(true);
      showResultTime -= Time.deltaTime;
      if (showResultTime < 0 && resultPanel.active)
        resultPanel.SetActive(false);
      return showResultTime < 0 && result;
    }
    else
    {
      return false;
    }

    //Debug.Log("[polygonmemorize] match answer result" + answerObjList.Count + answers.Count + a);
    //return a;
  }

  public void NextQuestion()
  {
    EnablePolygonGrabable(false);
    Destroy(answerObj);
    answers.Clear();
    answerObjList.Clear();
    showResultTime = 2.0f;
  }

  public void EnablePolygonGrabable(bool enabled)
  {
    polygons.ForEach(polygon =>
    {
      if (polygon.TryGetComponent<Grabable>(out Grabable grabable))
      {
        grabable.enableGrabing = enabled;
      }
    });
  }
  // Use this for initialization
  void Start()
  {
    EnablePolygonGrabable(false);
  }
  // Update is called once per frame
  void Update()
  {

  }
}

