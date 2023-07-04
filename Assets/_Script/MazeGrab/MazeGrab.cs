using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using System.Linq;
using UnityEngine.UI;
using Assets._Script;
using System;
public class MazeGrab : MonoBehaviour
{
  public GameObject Sphere, Cylinder, Cube, Maze, EndGamePanel;
  public Button ReplayButton;
  private CollisionHandler collisionHandler;

  // Use this for initialization
  void Start()
  {

    RectTransform mazeRectTranform = Maze.AddComponent<RectTransform>();
    RectTransform rectTransform = GetComponent<RectTransform>();
    collisionHandler = Sphere.AddComponent<CollisionHandler>();
    collisionHandler.collisionFunction += WinGame;
    mazeRectTranform.pivot = rectTransform.pivot;
    //Maze.transform.position = Camera.main.WorldToScreenPoint(Camera.main.transform.localPosition);
    //this.gameObject.transform.SetPositionAndRotation(Camera.main.transform.position,Quaternion.identity);
    Sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    Sphere.transform.position = new Vector3(6.5f, -1, 0);
    Cylinder.transform.position = new Vector3(-6.5f, 3.5f, 0);
    Cylinder.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    Cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    ReplayButton.onClick.AddListener(ResumeGame);
  }

  // Update is called once per frameÏ
  void Update()
  {
    //RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(Sphere.transform.position));
    RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
    //if (Physics.Raycast(ray, out hit, 100))
    //{
    //    Debug.Log("[MazeGrab] collider" + hit);
    //  if (hit.transform.TryGetComponent<Collider>(out Collider collider))
    //  {
    //    Debug.Log("[MazeGrab] collider hit" + collider.name);
    //  }
    //}
    foreach (RaycastHit hit in hits)
    {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Maze")
      {
        //EndGamePanel.active = true;
        ReplayButton.gameObject.active = true;
        new List<GameObject> { Sphere, Cylinder, Cube }.ForEach((gameObject) =>
        {
          if (gameObject.TryGetComponent<Grabable>(out Grabable grabable))
          {
            grabable.Grabing = false;
            grabable.followTarget = null;
            //grabable.resetPosition();
            grabable.enableGrabing = false;
          }
        });

        //PauseGame();
        //return;
        //Application.Quit();
      }
      Debug.Log("[MazeGrab] collider hit" + hits.ToList().IndexOf(hit) + LayerMask.LayerToName(hit.transform.gameObject.layer));
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log("COllision" + collision);
  }

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Trigger" + other);
  }

  void PauseGame()
  {
    Time.timeScale = 0;
  }

  void WinGame(object sender, EventArgs args)
  {
    EndGamePanel.active = true;
  }
  void ResumeGame()
  {
    EndGamePanel.active = false;
    ReplayButton.gameObject.active = false;
    new List<GameObject> { Sphere, Cube }.ForEach((gameObject) =>
    {
      if (gameObject.TryGetComponent<Grabable>(out Grabable grabable))
      {
        grabable.enableGrabing = true;
        grabable.resetPosition();
      }
    });
    //Time.timeScale = 1;
  }
}

