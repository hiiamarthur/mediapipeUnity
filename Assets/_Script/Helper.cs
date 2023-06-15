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


