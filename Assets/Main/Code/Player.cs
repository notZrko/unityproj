using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;
    public bool is_dead = false;
    public Slider health_slider;
    public Image heart;
    public GameObject LooseState;

    [Header("Movement")]
    public float speed = 40.0f;
    public bool facing_right = true;

    [Header("References")]
    public GameObject Pivot;
    private Rigidbody2D rb;
    private Animator animator;

    public SpriteRenderer Sprite; public SpriteRenderer Gun;
    public ParticleSystem dust;

    private float dash_cd = 1.5f;
    public AudioClip dashSFX;

    public GunStat gunStat;
    private bool can_shoot = true;
    private float next_shot;

    public GameObject Muzzle;
    public Rigidbody2D Bullet;
    public GameObject FireFX;
    public GameObject ghost;
    public AudioClip the_funny;

    void Start()
    {
        Bullet = gunStat.bullet.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            if(is_dead == true) { return; }

            AudioManager.Instance.Play(the_funny);
            is_dead = true;
            health_slider.gameObject.SetActive(false);
            LooseState.SetActive(true);
            Gun.gameObject.SetActive(false); Sprite.gameObject.SetActive(false);
            speed = 0;
        }

        health_slider.value = health;

        if (Input.GetKey(KeyCode.LeftShift) && Time.time > dash_cd)
        {
            can_shoot = false;
            animator.SetTrigger("Roll");
            AudioManager.Instance.Play(dashSFX);

            StartCoroutine(LazyDash());
            StartCoroutine(Ghost());
            dash_cd = Time.time + 1.0f;
        }

        if (Input.GetKey(KeyCode.Mouse0) && Time.time > next_shot && can_shoot == true)
        {
            Shoot();
        }   

        Gun.GetComponent<SpriteRenderer>().sprite = gunStat.gun_texture;
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
    }
    public IEnumerator Flash()
    {
        Sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Sprite.color = Color.white;
    }

    IEnumerator Ghost()
    {
        for(int i = 1; i < 10; i++)
        {
            GameObject fx = Instantiate(ghost, new Vector2(transform.position.x, transform.position.y), transform.rotation);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator LazyDash()
    {
        speed = 90.0f;
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.2f);
        speed = 25.0f;
        
        GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.05f);
        can_shoot = true;

    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Shoot()
    {
        Rigidbody2D clone;
        clone = Instantiate(Bullet, new Vector2(Muzzle.transform.position.x, Muzzle.transform.position.y), Muzzle.transform.rotation);
        GameObject fx = Instantiate(FireFX, new Vector2(Muzzle.transform.position.x, Muzzle.transform.position.y), Muzzle.transform.rotation);

        clone.gameObject.transform.localScale = gunStat.scale;
        clone.gameObject.GetComponent<Hit>().dmg = gunStat.damage;

        clone.AddForce(Muzzle.transform.up * 100f, ForceMode2D.Impulse);
        animator.SetTrigger("Shoot");

        AudioManager.Instance.RandomSoundEffect(gunStat.sounds);
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
        var horizontalInput = Input.GetAxisRaw("Horizontal"); 
        var verticalInput = Input.GetAxisRaw("Vertical");
        var mouse_x = Input.GetAxis("Mouse X"); 
        var mouse_y = Input.GetAxis("Mouse Y");

        rb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        if (rb.velocity != Vector2.zero)
        {
            dust.emissionRate = 8;
        } else
        {
            dust.emissionRate = 0;
        }
               

        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        if (mouseScreenPosition.x > transform.position.x && !facing_right)
        {
            Flip();
        }
        else if (mouseScreenPosition.x < transform.position.x && facing_right)
        {
            Flip();
        }
            
        Pivot.transform.up = direction;
    }
}
