using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{
    [Header("Sound Radius")]
    public float SoundRadius;
    public float InitialRadius = 2;
    public float ArmorRaduis = 2;
    public float GoldRadius = 1;

    public GameObject RippleEffect;
    private ParticleSystem _particles;

    private Hero _hero;
    private Rigidbody _heroBody;

    public bool IsEmitingSound;

    private void Start()
    {
        _hero = GetComponentInParent<Hero>();
        _heroBody = _hero.GetComponent<Rigidbody>();
        _hero.OnLootChange += OnRadiusChange;
        _particles = RippleEffect.GetComponent<ParticleSystem>();
        SoundRadius = InitialRadius;
        RippleEffect.transform.localScale = new Vector3(SoundRadius, SoundRadius, SoundRadius);
    }

    private void Update()
    {
        if (_heroBody.velocity.magnitude > 0.1f)
        {
            RippleEffect.SetActive(true);
            //if (!_particles.isPlaying) _particles.Play();
            IsEmitingSound = true;
        }
        else
        {
            RippleEffect.SetActive(false);
            //if (_particles.isPlaying) _particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            IsEmitingSound = false;
        }
    }

    void OnRadiusChange()
    {
        SoundRadius = InitialRadius;
        SoundRadius += ArmorRaduis * _hero.ActivatedArmor.Count;
        SoundRadius += GoldRadius * _hero.Gold;
        RippleEffect.transform.localScale = new Vector3(SoundRadius, SoundRadius, SoundRadius);
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    var start = new Vector3(transform.position.x, 0.1f, transform.position.z);
    //    var end = new Vector3(transform.position.x + _currentRadius, 0.1f, transform.position.z);
    //    Gizmos.DrawLine(start, end);
    //}
}
