// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
  public struct Point
  {
    public Vector3 center { get; set; }
    public GameObject gameObject { get; set; }
  }

  public class Helper : MonoBehaviour
  {

    public List<GameObject> GetChildren(GameObject obj)
    {
      var children = new List<GameObject>();
      foreach (Transform child in obj.transform)
      {
        children.Add(child.gameObject);
      }
      return children;
    }

    //public List<dynamic> GetChildren(dynamic x) {
    //}

    public GameObject GetChildWithName(GameObject obj, string name)
    {
      return obj.transform.Find(name).gameObject;

      //resultChild.transform.SetParent(obj.transform);
      //foreach (Transform child in obj.transform)
      //{
      //  //Debug.Log("list yo" + );
      //  if (child.name == name)
      //  {
      //    return child.gameObject;

      //  }

      //}
    }

    public void AssignChildToExisingGameObj(GameObject childObject, GameObject parentObject, float zDistance)
    {
      Vector3 m_DistanceFromCamera;
      m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + zDistance);
      Plane m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);

      Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(parentObject.transform.position));
      float enter = 0;
      if (m_Plane.Raycast(ray, out enter))
      {
        Vector3 hitPoint = ray.GetPoint(enter);
        childObject.transform.position = hitPoint;
      }

      //polygonCopy.transform.position = panelPoint[j].transform.position;
      childObject.transform.parent = parentObject.transform;

    }

    public MathFunc mathFunc = new MathFunc();

  }

  public class MathFunc
  {
    public Vector3 GetBoxCenter(Vector3[] points)
    {
      var pointList = points.ToList();
      return new Vector3(pointList.Sum(coor => coor.x) / pointList.Count, pointList.Sum(coor => coor.y) / pointList.Count, pointList.Sum(coor => coor.z) / pointList.Count);
    }

    //public Vector3 Distance(Vector3 point1, Vector3 point2) {
    //  return Vector3.Distance(point1, point2);
    //}
  }
}


