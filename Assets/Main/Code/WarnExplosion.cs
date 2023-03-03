using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnExplosion : MonoBehaviour
{
    private Player plr;

    public AudioClip warn;
    public AudioClip explodeSFX;
    public GameObject explosion;
    public Collider2D hitbox;

    void Start()
    {
        AudioManager.Instance.Play(warn);

        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.25f);

        AudioManager.Instance.Play(explodeSFX);
        this.GetComponent<SpriteRenderer>().enabled = false;
        GameObject fx = Instantiate(explosion, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        hitbox.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            plr = other.GetComponent<Player>();
            if (plr.health <= 0) { return; }

            plr.StartFlash();
            plr.health -= 40;

            Destroy(this.gameObject);
        }
    }
}
