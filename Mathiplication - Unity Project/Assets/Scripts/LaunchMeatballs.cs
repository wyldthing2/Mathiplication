using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMeatballs : MonoBehaviour
{
    // Alternate method would be having each of the launchers have their own list (right now each launcher has to ask the canvas, and it
    //moves ito them)


    [SerializeField] public int NumberOfMeatballsToLaunch;
    [SerializeField] MeatballsList meatballsList;
    [SerializeField] Vector3 origin;
    [SerializeField] Vector3 target;
    [SerializeField] float driftSpeed = 4f;

    public void CommandLaunchXNumberOfMeatballsAtThisYTransform(int meatballnumber, Transform launcherOrigin, Transform launcherTarget)
    {
        // This system launches all at once, we could have it gradual, but it would need to be able to alternate launch positions if two
        //places were launching and we only have one launcher


        NumberOfMeatballsToLaunch += meatballnumber;
        origin = launcherOrigin.position;
        target = launcherTarget.position;

    }

    void LaunchAMeatBall()
    {
        NumberOfMeatballsToLaunch--;
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].transform.position = origin;
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].SetActive(true);
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].GetComponent<MeatballAI>().meatballVelocity = new Vector2(Random.value*driftSpeed, Random.value*driftSpeed);
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].GetComponent<MeatballAI>().MeatballTarget = new Vector2(target.x, target.y);
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].GetComponent<MeatballAI>().ThrustDelay();
        meatballsList.MeatBalls[meatballsList.NextMeatballInQueue].GetComponent<MeatballAI>().DelayFriction(1f);

        if (meatballsList.MeatBalls.Count-1 > meatballsList.NextMeatballInQueue)
        {
            meatballsList.NextMeatballInQueue++;
        }
        else
        {
            meatballsList.NextMeatballInQueue = 0;
        }
        

        
    }

    void Update()
    {
        //Current launcher fires each frame
        if (NumberOfMeatballsToLaunch > 0)
        {
            LaunchAMeatBall();
        }
        
    }
}
