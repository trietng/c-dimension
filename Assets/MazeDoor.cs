using Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    public FirstPersonMovements player;
    public GhostScript ghost;

    public int requirement = 0;

    void Update()
    {
        if (player.keyCount == requirement || ghost.keyCount == requirement)
        {
            gameObject.SetActive(false);
        }
    }
}
