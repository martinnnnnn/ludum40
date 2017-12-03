using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{

    public GameObject[] ToHide;
    public Material Fog;
    public float FogAlpha;

    bool show;


    private void Start()
    {
        show = false;
        Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, FogAlpha);
        foreach (GameObject obj in ToHide)
        {
            obj.GetComponent<Renderer>().enabled = show;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        UpdateFog(other);
    }

    private void OnTriggerExit(Collider other)
    {
        UpdateFog(other);
    }

    void UpdateFog(Collider other)
    {
        if (!other.GetComponent<Hero>())
            return;

        show = !show;

        foreach (GameObject obj in ToHide)
        {
            obj.GetComponent<Renderer>().enabled = show;
        }

        if (show)
        {
            Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, 0);
        }
        else
        {
            Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, FogAlpha);
        }
    }
}
