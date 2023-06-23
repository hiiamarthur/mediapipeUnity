// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;
using Assets;


public class PanelPoint : MonoBehaviour
{
  public GameObject occupiedObject;
  public bool occupied = false;
  public Helper helper = new Helper();
  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (occupiedObject != null && !occupied)
    {
      GameObject child = Instantiate(occupiedObject);
      helper.AssignChildToExisingGameObj(child, gameObject, 20.0f);
      child.transform.parent = gameObject.transform;
      occupied = true;

      if (occupiedObject.TryGetComponent<Grabable>(out Grabable grabable))
      {
        grabable.resetPosition();
      }
      if (child.TryGetComponent<Grabable>(out Grabable grabable2))
      {
        grabable2.enableGrabing = false;
        //grabable2.transform.localPosition = gameObject.transform.localPosition;

      }
    }
  }
}
