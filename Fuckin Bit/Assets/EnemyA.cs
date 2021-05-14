using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    public float runDistance;
    private bool run;
    private bool attack = false;
    public float fireRate = 0.5f;
    private float nextFire;
    public GameObject bulletPrefab;
    public Transform shootSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        anim.SetBool("isEnemy_Running", run);

        if (attack == true)
        {
            Fire();
        }

        if (Mathf.Abs(targetDistance) < runDistance)
        {
            run = true;
            attack = false;
        }

        if (Mathf.Abs(targetDistance) < attackDistance)
        {
            attack = true;
            run = false;
        }
    }

    private void FixedUpdate()
    {
        if (run && !attack)
        {
            if (targetDistance < 0)
            {
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
                if (facingRight)
                {
                    Flip();
                }
            }else
            {
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
                if (!facingRight)
                {
                    Flip();
                }
            }
            
        }
    }

    public void ResetAttack()
    {
        attack = false;
    }

    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletPrefab, shootSpawner.position, shootSpawner.rotation);
            if (facingRight)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            
        }
    }
}
