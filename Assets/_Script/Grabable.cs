using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
  public bool grabing;

  public Transform followTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (grabing)
      {
        // transform.position = new Vector3(followTarget.position.x, followTarget.position.y, this.transform.position.z);
        // transform.position = followTarget.position;
        var followScreenPos = Camera.main.WorldToScreenPoint(followTarget.transform.position);
        Debug.Log(("followScreenPos "+followScreenPos));
        
        var worldPos =
          Camera.main.ScreenToWorldPoint(new Vector3(followScreenPos.x, followScreenPos.y, 10f));
        // worldPos.z = 2f;
        this.transform.position = worldPos;
      }
        
    }
}
