using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJumping : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject.GetComponent<Player>();
    }

    void OnCollisionEnter2D(Collision2D collisor)
    {
        if (collisor.gameObject.tag == "Ground" || collisor.gameObject.tag == "Enemy" || collisor.gameObject.tag == "Parede")
        {
            player.noJumping = true;
        }
    }
}
