using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool is_boss = false;
    public int max_health = 100;
    public int health = 100;

    public GameObject target;
    private bool can_shoot = false;

    // Movement
    public float speed = 25.0f;
    public bool facing_right = true;

    // Gameobjects & References
    public GameObject Pivot;
    public GameObject explosion;
    public AudioClip[] deathSounds;
    private Rigidbody2D rb;
    private Animator animator;

    public SpriteRenderer Sprite; public SpriteRenderer Gun;

    // Gun
    public GunStat gunStat;
    private float next_shot;
    public GameObject Muzzle;
    public Rigidbody2D Bullet;
    public GameObject FireFX;

    public GameObject ItemPickup;

    public Rigidbody2D body;

    public GameObject Warning;
    private bool boss_loop;

    void Start()
    {
        GameManager.Instance.enemies_in_wave += 1;

        health = max_health;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(Delay());
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(1,2));
        can_shoot = true;
    }

    public IEnumerator Flash()
    {
        Sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Sprite.color = Color.white;
    }

    private void Update()
    {
        if (is_boss == true)
        {
            GameManager.Instance.boss_label.text = "BOSS // " + health.ToString() + "/" + max_health.ToString();

            if (boss_loop == false)
            {
                boss_loop = true;
                StartCoroutine(BossLoop());
            }
        }

        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.enemies_in_wave -= 1;

            if (target.GetComponent<Player>().gunStat.value < gunStat.value)
            {
                GameObject pickup = Instantiate(ItemPickup, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                pickup.GetComponent<Pickup>().weapon = gunStat;
            }

            Rigidbody2D clone;
            Vector2 direction = new Vector2((float)Random.Range(-1000, 1000), (float)Random.Range(-1000, 1000));
            float force = (float)Random.Range(-100, 100);

            clone = Instantiate(body, new Vector2(transform.position.x, transform.position.y), transform.rotation);
            GameObject fx = Instantiate(explosion, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            clone.AddForce(direction * force);

            var euler = transform.eulerAngles;
            euler.z = Random.Range(0.0f, 360.0f);
            clone.gameObject.transform.eulerAngles = euler;
            GameManager.Instance.kills++;

            AudioManager.Instance.RandomSoundEffect(deathSounds);
            Destroy(this.gameObject);
        }
    }

    IEnumerator BossLoop()
    {
        yield return new WaitForSeconds(1);
        var get_pos_x = target.transform.position.x; var get_pos_y = target.transform.position.y;

        GameObject pickup = Instantiate(Warning, new Vector2(get_pos_x, get_pos_y), Quaternion.identity);
        StartCoroutine(BossLoop());
    }
    private void FixedUpdate()
    {
        Movement();

        if (can_shoot == true && Time.time > next_shot)
        {
            Shoot();
        }

        Gun.GetComponent<SpriteRenderer>().sprite = gunStat.gun_texture;
    }

    private void Shoot()
    {
        Rigidbody2D clone;
        clone = Instantiate(Bullet, new Vector2(Muzzle.transform.position.x, Muzzle.transform.position.y), Muzzle.transform.rotation);
        GameObject fx = Instantiate(FireFX, new Vector2(Muzzle.transform.position.x, Muzzle.transform.position.y), Quaternion.identity);

        clone.gameObject.transform.localScale = gunStat.scale;
        clone.gameObject.GetComponent<Hit>().dmg = gunStat.damage;

        AudioManager.Instance.RandomSoundEffect(gunStat.sounds);

        clone.AddForce(Muzzle.transform.up * 50f, ForceMode2D.Impulse);
        animator.SetTrigger("Shoot");

        next_shot = Time.time + gunStat.fire_delay;
    }

    private void Flip()
    {
        facing_right = !facing_right;

        Sprite.flipY = facing_right;
        Gun.flipY = facing_right;
    }

    private void Movement()
    {
        var mouse_x = Input.GetAxis("Mouse X");
        var mouse_y = Input.GetAxis("Mouse Y");
        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        Vector2 direction = (target.transform.position - transform.position).normalized;

        if (target.transform.position.x > transform.position.x && !facing_right)
        {
            Flip();
        }
        else if (target.transform.position.x < transform.position.x && facing_right)
        {
            Flip();
        }

        Pivot.transform.up = direction;
    }
}
