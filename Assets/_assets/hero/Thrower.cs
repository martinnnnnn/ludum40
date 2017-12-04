using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;

public class Thrower : MonoBehaviour
{
    public GameObject Target;
    public GameObject Gold;
    public GameObject ThrowPoint;
    public float SoundRadius;
    public float WaveParticuleLifeTime;
    public GameObject WaveParticuleEmiter;
    private Hero _hero;

    private void Start()
    {
        _hero = GetComponent<Hero>(); ;
    }

    public void Throw(string itemName, float reachtime)
    {
        GameObject objToThrow = null;
        

        if (itemName == "gold")
        {
            objToThrow = Instantiate(Gold, ThrowPoint.transform.position, new Quaternion());
        }
        else
        {
            foreach(var armor in _hero.Armor)
            {
                if (armor.name == itemName)
                {
                    objToThrow = Instantiate(armor, ThrowPoint.transform.position, new Quaternion());
                    break;
                }
            }
        }
   
        if (!objToThrow)
            return;

        objToThrow.SetActive(true);
        objToThrow.AddComponent<Rigidbody>();
        var throwable = objToThrow.AddComponent<Throwable>();
        throwable.SoundRadius = SoundRadius;
        throwable.WaveParticuleLifeTime = WaveParticuleLifeTime;
        throwable.WaveParticuleEmiter = WaveParticuleEmiter;
        objToThrow.GetComponent<Collider>().enabled = true;

        Vector3 velocity = TrajectoryMath.CalculateVelocity(objToThrow.GetComponent<Collider>().bounds.center, Target.transform.position, reachtime);

        var objectInterface = objToThrow.GetComponent(typeof(IPropelBehavior)) as IPropelBehavior;

        if (objectInterface != null)
        {
            objectInterface.React(velocity);
        }
    }

}
