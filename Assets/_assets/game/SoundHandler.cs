using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    WALK_LIGHT,
    WALK_ARMOR,
    MONSTER_ATTACK_ARMOR
}


public class SoundHandler : MonoBehaviour
{


    public AudioClip musics;

    public AudioClip[] clips;

    public AudioClip[] walk_light;
    public AudioClip[] walk_armor;
    public AudioClip[] monster_attack_armor;

    private AudioSource sounds;
    private AudioSource music;

    void Awake()
    {
        sounds = gameObject.AddComponent<AudioSource>();
        sounds.loop = false;

        music = gameObject.AddComponent<AudioSource>();
        music.loop = true;
        music.clip = musics;
        music.PlayDelayed(0);
    }

    public AudioClip GetAudioClip(string name)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }


    public void PlaySound(string name, float delay = 0f)
    {
        sounds.clip = GetAudioClip(name);
        sounds.PlayDelayed(delay);
    }

    public void PlayFromPool(SoundType type)
    {
        AudioSource source = sounds;

        switch (type)
        {
            case SoundType.WALK_LIGHT:
                source.PlayOneShot(walk_light[Random.Range(0, walk_light.Length)]);
                break;
            case SoundType.WALK_ARMOR:
                source.PlayOneShot(walk_armor[Random.Range(0, walk_armor.Length)]);
                break;
            case SoundType.MONSTER_ATTACK_ARMOR:
                source.PlayOneShot(monster_attack_armor[Random.Range(0, monster_attack_armor.Length)]);
                break;
        }
    }
}