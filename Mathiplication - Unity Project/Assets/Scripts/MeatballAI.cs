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
    

    public void ThrustDelay()
    {
        Invoke("TurnOnThrust", 2);
    }

    public void TurnOnThrust()
    {
        ThrustOn = !ThrustOn;
    }

    void movement()
    {
        this.transform.position += new Vector3(meatballVelocity.x,meatballVelocity.y,0);

    }

    private void applyFriction()
    {
            meatballVelocity -= Friction * meatballVelocity.normalized;
    }

    public void ThrustTowardTarget()
    {
        //Obstacle? Does World space change when object rotates?
        Vector2 vectorA = new Vector2(this.transform.position.x, this.transform.position.y);

        //How to split thrust between X and Y
        Vector2 thrustVector = MeatballTarget - vectorA;
        

        meatballVelocity += thrustVector.normalized * Thrust;
    }


    private void Update()
    {
        if (ThrustOn)
        {
            ThrustTowardTarget();
        }

        if (meatballVelocity.x != 0 || meatballVelocity.y != 0)
        {
            movement();
            applyFriction();
        }

        if (5 > Mathf.Abs(this.transform.position.x - MeatballTarget.x) && 5 > Mathf.Abs(this.transform.position.y - MeatballTarget.y))
        {
            TurnOnThrust();
        }
        
    }

    public void Deactivate()
    {
        TurnOnThrust();
        this.gameObject.SetActive(false);
    }
}
