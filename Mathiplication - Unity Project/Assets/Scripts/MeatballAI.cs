using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballAI : MonoBehaviour
{
    //[SerializeField] float maxVelocity;
    [SerializeField] public Vector2 MeatballTarget;
    [SerializeField] public Vector2 meatballVelocity = new Vector2(0, 0);
    [SerializeField] public float Friction;
    [SerializeField] public float Thrust;
    [SerializeField] public bool ThrustOn = false;
    [SerializeField] float MaxVelocity = 30;

    public void ThrustDelay()
    {
        Invoke("TurnOnThrust", 2);
    }

    public void TurnOnThrust()
    {
        ThrustOn = true;
    }

    void movement()
    {
        this.transform.position += new Vector3(meatballVelocity.x,meatballVelocity.y,0);

    }

    private void applyFriction()
    {
        if (Mathf.Abs(meatballVelocity.magnitude) > .2)
        {
            meatballVelocity -= Friction * meatballVelocity.normalized;
        }
        else
        {
            meatballVelocity.x = 0;
            meatballVelocity.y = 0;
        }

    }

    public void ThrustTowardTarget()
    {
        //Obstacle? Does World space change when object rotates?
        Vector2 vectorA = new Vector2(this.transform.position.x, this.transform.position.y);

        //How to split thrust between X and Y
        Vector2 thrustVector = MeatballTarget - vectorA;
        

        meatballVelocity += thrustVector.normalized * Thrust;
    }

    bool FrictionIsOn = true;

    public void DelayFriction(float delayTime)
    {
        FrictionIsOn = false;
        Invoke("frictionBackOn", delayTime);
    }

    void frictionBackOn()
    {
        FrictionIsOn = true;
    }

    private void Update()
    {
        if (ThrustOn && Mathf.Abs(meatballVelocity.magnitude) < MaxVelocity)
        {
            ThrustTowardTarget();
        }

        if (meatballVelocity.x != 0 || meatballVelocity.y != 0)
        {
            movement();
            if (FrictionIsOn)
            {
                applyFriction();
            }
        }

        //if (5 > Mathf.Abs(this.transform.position.x - MeatballTarget.x) && 5 > Mathf.Abs(this.transform.position.y - MeatballTarget.y))
        //{
        //    TurnOnThrust();
        //}

        if (1 > Mathf.Abs(this.transform.position.x - MeatballTarget.x) && 1 > Mathf.Abs(this.transform.position.y - MeatballTarget.y))
        {
            ThrustOn = false;
            this.gameObject.SetActive(false);
        }

    }

    public void Deactivate()
    {
        TurnOnThrust();
        this.gameObject.SetActive(false);
    }
}
