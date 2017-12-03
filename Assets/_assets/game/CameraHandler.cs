using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraHandler : MonoBehaviour
{

    public Vector3 DistanceFromHero;
    public GameObject Hero;


    void Update()
    {
        transform.position = Hero.transform.position + DistanceFromHero;
    }
}
