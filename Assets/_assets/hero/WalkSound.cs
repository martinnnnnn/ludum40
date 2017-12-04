using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    Hero _hero;

    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
    }

    public void PlayWalkSound()
    {
        if (_hero.Life == 0)
        {
            _hero._soundHandler.PlayFromPool(SoundType.WALK_LIGHT);
        }
        else
        {
            _hero._soundHandler.PlayFromPool(SoundType.WALK_ARMOR);
        }
    }
}
