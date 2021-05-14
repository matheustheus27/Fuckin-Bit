using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health = 3;
    public int gun = 1;
    public Animator lifeStatus;
    public Animator gunStatus;

    public float damageTime = 1f;
    private bool tookDamage = false;

    public GameObject gameOver;
    public GameObject deathAnimation;

    public float Speed;
    public float JumpForce;

    public bool noJumping;
    private Animator anim;

    private Rigidbody2D rig;
    private Transform groundCheck;
    private float fireRate = 0.5f;
    private float nextFire;
    public GameObject bulletPrefabP;
    public GameObject bulletPrefabR;
    public GameObject bulletPrefabS;
    public Transform shootSpawner;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("NoJumping");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifebar();
        UpdateWeaponbar();
        Run();
        Jump();
        Fire();
    }

    void Run()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        float inputAxis = Input.GetAxis("Horizontal");

        if (inputAxis > 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
            anim.SetBool("isRunning", true);
        }

        if (inputAxis < 0)
        {
            transform.eulerAngles = new Vector2(0f, 180);
            anim.SetBool("isRunning", true);
        }
        
        if(inputAxis == 0)
        {
            anim.SetBool("isRunning", false);
        }

    }

    void Jump() 
    {
        if (Input.GetButtonDown("Jump") && noJumping) 
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            noJumping = false;
        }
        else if(!noJumping)
        {
            anim.SetBool("isJumping", false);
        }
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            if (gun == 1)
            {
                nextFire = Time.time + fireRate;
                GameObject tempBullet = Instantiate(bulletPrefabP, shootSpawner.position, shootSpawner.rotation);
            }

            if (gun == 2)
            {
                nextFire = Time.time + fireRate;
                GameObject tempBullet = Instantiate(bulletPrefabR, shootSpawner.position, shootSpawner.rotation);
            }

            if (gun == 3)
            {
                nextFire = Time.time + fireRate;
                GameObject tempBullet = Instantiate(bulletPrefabS, shootSpawner.position, shootSpawner.rotation);
            }
        }
    }


    public void UpdateLifebar()
    {
        lifeStatus.SetInteger("vidas", health);
    }

    public void UpdateWeaponbar()
    {
        if (gun == 1)
        {
            gunStatus.SetBool("isRevolver", false);
            gunStatus.SetBool("isShotgun", false);
        }

        if (gun == 2)
        {
            gunStatus.SetBool("isRevolver", true);
            gunStatus.SetBool("isShotgun", false);
        }

        if (gun == 3)
        {
            gunStatus.SetBool("isRevolver", false);
            gunStatus.SetBool("isShotgun", true);
        }

        anim.SetInteger("iGotAGun", gun);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) && !tookDamage)
        {
            StartCoroutine(TookDamage());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet")) && !tookDamage)
        {
            StartCoroutine(TookDamage());
        }

        if (other.gameObject.CompareTag("Ammu"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Life"))
        {
            Destroy(other.gameObject);
            if (health < 3)
            {
                health++;
            }  
        }

        if (other.gameObject.CompareTag("Pistol"))
        {
            Destroy(other.gameObject);
            gun = 1;
        }

        if (other.gameObject.CompareTag("Revolver"))
        {
            Destroy(other.gameObject);
            gun = 2;
        }

        if (other.gameObject.CompareTag("Shotgun"))
        {
            Destroy(other.gameObject);
            gun = 3;
        }
    }

    IEnumerator TookDamage()
    {
        tookDamage = true;
        health--;
        UpdateLifebar();

        if (health <= 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
            Instantiate(deathAnimation, transform.position, transform.rotation);
            gameObject.SetActive(false);
            Invoke("ReloadScene", 2f);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(11, 13);
            for (double i = 0; i < damageTime; i+= 0.2)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            Physics2D.IgnoreLayerCollision(11, 13, false);
            tookDamage = false;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
