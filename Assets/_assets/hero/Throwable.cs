using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;

public class Throwable : MonoBehaviour, IPropelBehavior
{

    public GameObject WaveParticuleEmiter;
    public float SoundRadius;
    public float WaveParticuleLifeTime;

    private Rigidbody _body;

    public void React(Vector3 velocity)
    {
        _body = GetComponent<Rigidbody>();
        if (_body)
        {
            _body.velocity = velocity;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            // particule emission
            // sound wave emission
            var spawnPosition = new Vector3(transform.position.x, 0.40f, transform.position.z);
            GameObject wave = Instantiate(WaveParticuleEmiter, spawnPosition, WaveParticuleEmiter.transform.rotation) as GameObject;
            wave.transform.localScale = new Vector3(SoundRadius, SoundRadius, SoundRadius);
            Destroy(wave, WaveParticuleLifeTime);

            // sound emission
            foreach (var monster in FindObjectsOfType<Monster>())
            {
                if (monster.gameObject.activeSelf)
                {
                    monster.GetComponent<Monster>().HearObject(this);
                }
            }

            // destroy self
            Destroy(gameObject);
        }
    }
}
