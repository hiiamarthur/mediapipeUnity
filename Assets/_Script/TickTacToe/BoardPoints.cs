using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class BoardPoints : MonoBehaviour
{
  public GameObject occupiedObject;
  public bool occupied = false;
  public void Start()
  {

  }

  public void Update()
  {
    if (occupiedObject != null && !occupied)
    {
      GameObject child = Instantiate(occupiedObject);
      child.transform.parent = gameObject.transform;
      occupied = true;

      if (occupiedObject.TryGetComponent<Grabable>(out Grabable grabable))
      {
        grabable.resetPosition();
      }
      if (child.TryGetComponent<Grabable>(out Grabable grabable2))
      {
        grabable2.enableGrabing = false;
        Debug.Log("ticktactoe raycast3 " + grabable2.transform.position + gameObject.transform.position + gameObject.transform.TransformPoint(grabable2.transform.position));
        //grabable2.transform.localPosition = gameObject.transform.localPosition;

      }

      Debug.Log("BoardPoint update");
    }
  }

}
