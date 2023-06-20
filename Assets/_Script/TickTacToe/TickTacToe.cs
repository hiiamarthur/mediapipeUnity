using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets;

public struct Point
{
  public Vector3 center { get; set; }
  public GameObject gameObject { get; set; }
}





public class TickTacToe : MonoBehaviour
{
  public Camera mainCam;
  public GameObject Tick;
  public GameObject Toe;
  public Dictionary<int, Point> list = new Dictionary<int, Point>();
  Grabable _tickGrabable, _toeGrabable;
  private Helper helper = new Helper();


  public Dictionary<string, List<int>> permutation =
    new Dictionary<string, List<int>>()
    {
      { "horizontalOne", new List<int>(){0, 1, 2 } },
      { "horizontalTwo", new List<int>(){3,4,5 } },
      { "horizontalThree", new List<int>(){6,7,8 } },
      { "VectiacalOne", new List<int>(){0,3,6 } },
      { "VectiacalTwo", new List<int>(){1,4,7} },
      { "VectiacalThree", new List<int>(){2,5,8} },
      { "DiagonalOne", new List<int>(){0,4,8} },
      { "DiagonalTwo", new List<int>(){2,4,6} },
    };
  // Start is called before the first frame update
  void Start()
  {
    var children = helper.GetChildren(gameObject);
    int count = 0;
    foreach (GameObject child in children)
    {
      if (child.name.Contains("point"))
      {
        Vector3 a = mainCam.WorldToScreenPoint(child.transform.position);
        Debug.Log("points" + child.name + " " + mainCam.WorldToScreenPoint(gameObject.transform.position) + a);
        Point point = new Point
        {
          center = a,
          gameObject = child
        };
        list.Add(count++, point);
      }
    }

    if (Tick.TryGetComponent<Grabable>(out Grabable tickGrabale))
    {
      _tickGrabable = tickGrabale;
    }

    if (Toe.TryGetComponent<Grabable>(out Grabable toeGrabale))
    {
      _toeGrabable = toeGrabale;
    }

    //if (gameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform))
    //{

    //  for (int j = 0; j < 3; j++)
    //  {
    //    for (int i = 0; i < 3; i++)
    //    {
    //      Vector3 a = mainCam.WorldToScreenPoint(gameObject.transform.position) + new Vector3((1 - i) * -1 * rectTransform.rect.width / 3, (1 - j) * 1 * rectTransform.rect.height / 3, 0);
    //      Debug.Log("points" + mainCam.WorldToScreenPoint(gameObject.transform.position) + a);
    //      list.Add(a);
    //    }
    //  }
    //}

  }


  // Update is called once per frame
  void Update()
  {
    if (_tickGrabable.enableGrabing)
    {
      Debug.Log("[TickTacToe] Tick(Cube) Turn");
    }
    if (_toeGrabable.enableGrabing)
    {
      Debug.Log("[TickTacToe] Toe(Sphere) Turn");
    }

    Dictionary<int, string> positionNameDict = new Dictionary<int, string>();
    RaycastHit hit;
    foreach (KeyValuePair<int, Point> item in list)
    {
      var a = item.Value.gameObject.GetComponent<BoardPoints>();
      if (a.occupied)
      {
        positionNameDict.Add(item.Key, a.occupiedObject.name);
      }
      //var screenPoint = mainCam.WorldToScreenPoint(item);
      if (item.Key == 2)
        Debug.Log("rightScreenPointItem index2" + item + mainCam.WorldToScreenPoint(Tick.transform.position));
      Ray ray = mainCam.ScreenPointToRay(item.Value.center);

      if (Physics.Raycast(ray, out hit, 100))
      {
        //Debug.Log("ticktactoe ray" + " " + item + mainCam.WorldToScreenPoint(Tick.transform.position));
        if (hit.transform.TryGetComponent<Grabable>(out Grabable grabable))
        {
          //Debug.Log("ticktactoe raycast " + grabable.gameObject.name + " " + item.Key + " " + grabable.Grabing);
          //}
          if (!grabable.Grabing && grabable.enableGrabing)
          {

            //Debug.Log("ticktactoe raycast2 " + " " + hit.transform.position + " " + item.Key + " " + grabable.gameObject.transform.position + " " +
            //grabable.gameObject.transform.localPosition + " " + grabable.gameObject.transform.TransformVector(item.Value.center) + " "
            //+ item.Value.center + " " + mainCam.ScreenToWorldPoint(item.Value.center) + " " + item.Value.gameObject.transform.position
            //+ Vector3.ProjectOnPlane(item.Value.center, Vector3.zero));
            //grabable.gameObject.transform.position = item.Value.center;
            Debug.Log("[TickTacToe] grab" + item.Value.center + grabable.gameObject.transform.position);
            if (grabable.grabObjName == _toeGrabable.grabObjName)
            {
              _toeGrabable.enableGrabing = false;
              _tickGrabable.enableGrabing = true;

            }
            else if (grabable.grabObjName == _tickGrabable.grabObjName)
            {
              _tickGrabable.enableGrabing = false;
              _toeGrabable.enableGrabing = true;
            }

            a.occupiedObject = grabable.gameObject;
          }

        }
      }
    }

    List<string> b = new List<string> { "Cube", "Sphere" };
    b.ForEach(str =>
    {
      if (permutation.Values.ToList().Any(list =>
      {
        var intersect = positionNameDict.Keys.ToList().Intersect(list);
        string c = "";
        positionNameDict.Keys.ToList().ForEach(x => { c = c += x.ToString(); });
        Debug.Log("intersect" + list[0] + list[1] + list[2] + intersect.Count() + " yo " + c

          );


        if (intersect.Count() == 3)
        {
          Debug.Log("intersect is 3" + list.Count() + intersect.ToList()[0] + intersect.ToList()[1] + intersect.ToList()[2] + positionNameDict.Values.ToList()[0] + positionNameDict.Values.ToList()[0] + positionNameDict.Values.ToList()[0] + intersect.All(y => positionNameDict[y].Contains(str)));
        }

        if (intersect.Count() == list.Count() && intersect.All(y => positionNameDict[y].Contains(str)))
        {
          return true;
        }
        return false;
      }))
      {
        Debug.Log("[TickTacToe]" + str + " win");
      }
      else
      {
      }
    });

    //var rightScreenPoint = mainCam.WorldToScreenPoint(Tick.transform.position);
    ////Debug.Log(rightScreenPoint);
    //Ray ray = mainCam.ScreenPointToRay(rightScreenPoint);
  }
}
