using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour, IReset
{
    [HideInInspector]
    public List<GameObject> ToHide;
    public Material Fog;
    public float FogAlpha;

    bool show;


    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        show = false;
        Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, FogAlpha / 255f);

        ToHide = new List<GameObject>();
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Loot>() || t.GetComponent<Monster>())
            {
                ToHide.Add(t.gameObject);
                t.GetComponent<Renderer>().enabled = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        UpdateFog(other);
    }


    void UpdateFog(Collider other)
    {
        if (!other.GetComponent<Hero>())
            return;

        show = !show;



        if (show)
        {
            foreach (GameObject obj in ToHide)
            {
                if (obj.GetComponent<Collider>().enabled)
                {
                    obj.GetComponent<Renderer>().enabled = true;
                }
            }
            Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, 0);
        }
        else
        {
            foreach (GameObject obj in ToHide)
            {
                obj.GetComponent<Renderer>().enabled = false;
            }
            Fog.color = new Color(Fog.color.r, Fog.color.g, Fog.color.b, FogAlpha / 255f);
        }
    }
}
