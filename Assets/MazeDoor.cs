using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    public FirstPersonMovements player;

    void Update()
    {
        if (player.keyCount == 2)
        {
            gameObject.SetActive(false);
        }
    }
}
