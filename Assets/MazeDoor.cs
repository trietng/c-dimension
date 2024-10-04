using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    public FirstPersonMovements player;
    public int requirement = 0;
    void Update()
    {
        if (player.keyCount == requirement)
        {
            gameObject.SetActive(false);
        }
    }
}
