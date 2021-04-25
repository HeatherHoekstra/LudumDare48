using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource sfx;

    [Header("References")]
    public UI UI;
    public GameObject blood;
    public AudioSource windAudio;
    [SerializeField] AudioClip soundCloud;
    [SerializeField] AudioClip soundCalcium;
    [SerializeField] AudioClip soundFeather;
    [SerializeField] AudioClip soundWeight;
    [SerializeField] AudioClip soundDead;
    [SerializeField] AudioClip soundAlive;

    [Header("Player stats")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float minSpeed = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float weight = 1;
    [SerializeField] private float baseFallSpeed = 1;
    public float fallSpeed = 1;
    [SerializeField] private float minBaseFallSpeed = 1;
    [SerializeField] private float maxBaseFallSpeed = 15;
    public float strength = 1;

    private float timer = .05f;
    private float moveX;
    private bool touchingLeftWall;
    private bool touchingRightWall;
    private bool falling = true;
    public bool dangerZone;

    [Header("Item stats")]
    [SerializeField] private float featherStat = -1;
    [SerializeField] private float weightStat = 1;
    [SerializeField] private float calciumStat = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();
        baseFallSpeed = minBaseFallSpeed;
        fallSpeed = baseFallSpeed + weight;
    }

    private void Update()
    {
        
        if (falling)
        {
            if(weight < 0)
            {
                weight = 0;
            }

            if(fallSpeed > strength * 1.5)
            {
                dangerZone = true;
            }
            else
            {
                dangerZone = false;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                if (baseFallSpeed <= maxBaseFallSpeed) {
                    baseFallSpeed += .1f;
                }

                if (fallSpeed < baseFallSpeed + weight)
                {
                    fallSpeed += .1f;
                }
                else if (fallSpeed > baseFallSpeed + weight)
                {
                    fallSpeed -= .1f;
                }

                timer = .05f;
            }


            if (baseFallSpeed > maxBaseFallSpeed)
            {
                baseFallSpeed = maxBaseFallSpeed;
            }
            if (baseFallSpeed < minBaseFallSpeed)
            {
                baseFallSpeed = minBaseFallSpeed;
            }

            speed = fallSpeed / 1.3f;

            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            if (!touchingLeftWall && !touchingRightWall)
            {
                moveX = Input.GetAxisRaw("Horizontal");
            }

            if (touchingLeftWall && Input.GetAxisRaw("Horizontal") < 0)
            {
                moveX = 0;
            }
            else if (touchingLeftWall && Input.GetAxisRaw("Horizontal") > 0)
            {
                moveX = Input.GetAxisRaw("Horizontal");
            }

            if (touchingRightWall && Input.GetAxisRaw("Horizontal") > 0)
            {
                moveX = 0;
            }
            else if (touchingRightWall && Input.GetAxisRaw("Horizontal") < 0)
            {
                moveX = Input.GetAxisRaw("Horizontal");
            }
        }

        if (!falling)
        {
            moveX = 0;
        }
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2( moveX* speed, -fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Feather")
        {
            if (weight > 0)
            {
                weight -= featherStat;
            }
            PlaySound(soundFeather, .3f);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "FeatherBig")
        {
            if (weight > 0)
            {
                weight -= featherStat *2;
            }
            PlaySound(soundFeather, .3f);
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Weight")
        {            
            weight += weightStat;

            PlaySound(soundWeight, .3f);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "WeightBig")
        {
            weight += weightStat *2;

            PlaySound(soundWeight, .3f);
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Calcium")
        {
            strength += calciumStat;
            PlaySound(soundCalcium, .3f);
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Wind")
        {
            baseFallSpeed = minBaseFallSpeed;
            fallSpeed = baseFallSpeed + weight;
            PlaySound(soundCloud, .3f);
            anim.SetTrigger("Cloud");
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wind")
        {
            anim.SetTrigger("Falling");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "WallLeft")
        {
            touchingLeftWall = true;
        }
        if (collision.gameObject.name == "WallRight")
        {
            touchingRightWall = true;
        }

        if (collision.gameObject.name == "Ground")
        {
            falling = false;

            PlaySound(soundDead, 1);
            windAudio.enabled = false;

            if (dangerZone)
            {
                
                anim.SetTrigger("Dead");
                GameObject newBlood = Instantiate(blood);
                newBlood.transform.position = new Vector2(transform.position.x + .7f, transform.position.y - 2);
            }
            else
            {
                anim.SetTrigger("Alive");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "WallLeft")
        {
            touchingLeftWall = false;
        }
        if (collision.gameObject.name == "WallRight")
        {
            touchingRightWall = false;
        }
    }

    public void PlaySound(AudioClip sound, float vol)
    {
        sfx.PlayOneShot(sound, vol);
    }

    public void AliveSound()
    {
        PlaySound(soundAlive, 1);
    }

    public void EndGame()
    {
        bool alive;
        if (dangerZone)
        {
            alive = false;
        }
        else
        {
            alive = true;
        }
        UI.EndGameMenu(alive);
    }
}
