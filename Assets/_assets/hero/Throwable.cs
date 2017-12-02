using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;

public class Throwable : MonoBehaviour, IPropelBehavior
{

    private Rigidbody _body;

    public void React(Vector3 velocity)
    {
        _body = GetComponent<Rigidbody>();
        if (_body)
        {
            _body.velocity = velocity;
        }
    }
}
