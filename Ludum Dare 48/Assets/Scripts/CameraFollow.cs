using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public UI UI;
    [SerializeField] private float offset = 1;
    private bool hitGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hitGround)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y + offset, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Ground")
        {
            hitGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UI.InstanciateAlert(collision.gameObject);
    }

}
