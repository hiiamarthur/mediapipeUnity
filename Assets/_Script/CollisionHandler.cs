// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Assets._Script
{
  public class CollisionHandler : MonoBehaviour
  {


    private Rigidbody _rb;
    private Vector3 _velocity;

    // Use this for initialization
    void Start()
    {
      _rb = this.GetComponent<Rigidbody>();

      //_velocity = new Vector3(0f, 4f, 0f);
      _rb.AddForce(_velocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
      Debug.Log("OnCollisionEnter"+ collision);
      //ReflectProjectile(_rb, collision.contacts[0].normal);
    }

    private void OnTriggerEnter(Collider other)
    {
      Debug.Log("OnTriggerEnter" + other);
    }

    private void ReflectProjectile(Rigidbody rb, Vector3 reflectVector)
    {
      _velocity = Vector3.Reflect(_velocity, reflectVector);
      _rb.velocity = _velocity;
    }


    // Update is called once per frame
    void Update()
    {

    }
  }
}
