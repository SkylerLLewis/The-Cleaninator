using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralController : MonoBehaviour
{
    PlayerController player;
    void Awake() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision c) {
        Debug.Log(c.gameObject.name);
        if (c.gameObject.name == "Hammer") {
            player.HammerHit();
        }
    }
}
