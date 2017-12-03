using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;


public class ThrowerTarget : MonoBehaviour
{
    private Hero _hero;
    //private Rigidbody _body;
    private LineRenderer _lr;
    //private MeshRenderer _renderer;
    //private SphereCollider _collider;
    private bool _pressed;

    public float Speed;
    public float MaximalDistance;
    public float ReachTimeMax = 3;
    private float XDirection;
    private float ZDirection;

    private GameObject Ripple;

    private void Start()
    {
        _hero = GameObject.Find("Hero").GetComponent<Hero>();
        _lr = GetComponent<LineRenderer>();
        Ripple = transform.GetChild(0).gameObject;
        Ripple.SetActive(false);
    }

    private void Update()
    {
        if (_hero._dead)
            return;

        XDirection = Input.GetAxis("Horizontal");
        ZDirection = Input.GetAxis("Vertical");


        if (Input.GetAxis("ThrowAim") > 0.1f && !_pressed)
        {
            _pressed = true;
            _hero.Movable(false);
            Ripple.SetActive(true);
            transform.position = new Vector3(_hero.transform.position.x + MaximalDistance / 2, _hero.transform.position.y, _hero.transform.position.z);
        }
        else if (Input.GetAxis("ThrowAim") <= 0.1f && _pressed)
        {
            Ripple.SetActive(false);
            _pressed = false;
            _hero.Movable(true);
        }

        if (_pressed)
        {
            _lr.enabled = true;
            float reachTime = _hero.ReachTime;
            bool canThrow = false;
            List<Vector3> positions = new List<Vector3>();

            
            do
            {
                positions = GetTrajectoryPoints(_hero.transform.position, transform.position, reachTime, Color.blue);

                bool intersection = false;
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    var origin = positions[i];
                    var direction = positions[i + 1] - positions[i];
                    var maxDistance = Vector3.Distance(origin, positions[i + 1]);
                    if (Physics.Raycast(origin, direction, maxDistance))
                    {
                        intersection = true;
                        break;
                    }
                }
                if (intersection)
                {
                    reachTime += 0.1f;
                }
                else
                {
                    canThrow = true;
                }
            } while (!canThrow && reachTime < ReachTimeMax);

            if (!canThrow)
            {
                _lr.positionCount = 0;
                return;
            }

            _lr.positionCount = positions.Count;
            _lr.SetPositions(positions.ToArray());

            if (Input.GetButtonDown("ThrowArmor"))
            {
                _hero.ThrowArmor(reachTime);
            }
            if (Input.GetButtonDown("ThrowGold"))
            {
                _hero.ThrowGold(reachTime);
            }
        }
        else
        {
            _lr.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (_pressed)
        {
            if ((Mathf.Abs(XDirection) > 0.2f || Mathf.Abs(ZDirection) > 0.2f))
            {
                transform.position = new Vector3(
                    transform.position.x + XDirection * Time.deltaTime * Speed,
                    transform.position.y,
                    transform.position.z + ZDirection * Time.deltaTime * Speed);
            }
            if (Vector3.Distance(_hero.transform.position, transform.position) >= MaximalDistance)
            {
                var allowedPos = transform.position - _hero.transform.position;
                allowedPos = Vector3.ClampMagnitude(allowedPos, MaximalDistance);
                transform.position = _hero.transform.position + allowedPos;
            }
        }
    }

    public List<Vector3> GetTrajectoryPoints(Vector3 startPoint, Vector3 endPoint, float time, Color color)
    {
        Vector3 initialVelocity = TrajectoryMath.CalculateVelocity(startPoint, endPoint, time);
        float deltaTime = time / initialVelocity.magnitude;
        int drawSteps = (int)(initialVelocity.magnitude - 0.5f);
        Vector3 currentPosition = startPoint;
        Vector3 previousPosition = currentPosition;
        Gizmos.color = color;

        List<Vector3> positions = new List<Vector3>{ startPoint };

        if (IsParabolicVelocity(initialVelocity))
        {
            for (int i = 0; i < drawSteps; i++)
            {
                currentPosition += (initialVelocity * deltaTime) + (0.5f * Physics.gravity * deltaTime * deltaTime);
                initialVelocity += Physics.gravity * deltaTime;
                positions.Add(currentPosition);

                previousPosition = currentPosition;
            }
        }
        else
        {
            Debug.Log("error");
        }
        positions.Add(endPoint);
        return positions;
    }

   

    private static bool IsParabolicVelocity(Vector3 velocity)
    {
        return !(velocity.x == 0 && velocity.z == 0);
    }
}
