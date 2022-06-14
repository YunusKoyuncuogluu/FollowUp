using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public Transform JumpingBoardParent;
    public Transform ObstacleParent;
    internal List<GameObject> JumpPlatformList = new List<GameObject>();
    internal List<GameObject> Obstacle = new List<GameObject>();
    void Start()
    {
        foreach (Transform item in JumpingBoardParent)
        {
            JumpPlatformList.Add(item.gameObject);
        }

        foreach (Transform item in ObstacleParent)
        {
            Obstacle.Add(item.gameObject);
        }
    }
}
