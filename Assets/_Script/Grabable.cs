using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
  private bool grabing;
  public bool enableGrabing = true;
  public bool Grabing
  {
    get { return grabing; }
    set
    {
      if (enableGrabing)
      {
        grabing = value;
      }
    }
  }

  public Vector3 originPosition;
  public Transform followTarget;
  public string grabObjName;
  public int grabableID;

  // Start is called before the first frame update
  void Start()
  {
    originPosition = gameObject.transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (grabing)
    {
      this.transform.rotation = Quaternion.identity;
      // transform.position = new Vector3(followTarget.position.x, followTarget.position.y, this.transform.position.z);
      // transform.position = followTarget.position;
      var followScreenPos = Camera.main.WorldToScreenPoint(followTarget.transform.position);
      Debug.Log(("followScreenPos " + followScreenPos));

      var worldPos =
        Camera.main.ScreenToWorldPoint(new Vector3(followScreenPos.x, followScreenPos.y, 10f));
      // worldPos.z = 2f;
      this.transform.position = worldPos;
    }

  }

  public void resetPosition()
  {
    gameObject.transform.position = originPosition;
  }
}
