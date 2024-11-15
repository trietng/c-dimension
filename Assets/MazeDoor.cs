using Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeDoor : MonoBehaviour
{
    public FirstPersonMovements player;
    public GhostScript ghost;

    public int requirement = 0;

    private TextMeshProUGUI keysText;

    void Start () {
        keysText = GameObject.Find("LevelUI").GetComponentInChildren<TextMeshProUGUI>();
        keysText.text = "0 / " + requirement.ToString();
        if (player != null) player.onKeyCollected = (key) => {
            keysText.text = key.ToString() + " / " + requirement.ToString();
            if (player.keyCount == requirement)
            {
                keysText.color = Color.green;
                gameObject.SetActive(false);
            }
        };
        if (ghost != null) ghost.onKeyCollected = (key) => {
            keysText.text = key.ToString() + " / " + requirement.ToString();
            if (ghost.keyCount == requirement)
            {
                keysText.color = Color.green;
                gameObject.SetActive(false);
            }
        };
    }

    void Update()
    {

    }
}
