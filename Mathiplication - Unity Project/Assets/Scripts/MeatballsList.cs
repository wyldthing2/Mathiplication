using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballsList : MonoBehaviour
{
    [SerializeField] public List<GameObject> MeatBalls = new List<GameObject>();
    [SerializeField] GameObject MeatBallGroup;
    [SerializeField] public int NextMeatballInQueue = 0;

    void addMeatballsToList()
    {
        Transform[] allChildren = MeatBallGroup.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allChildren)
        {
            if (MeatBallGroup.transform != child.transform)
            {
                MeatBalls.Add(child.gameObject);

            }
        }
    }

    

    private void Awake()
    {
        addMeatballsToList();
    }

}
