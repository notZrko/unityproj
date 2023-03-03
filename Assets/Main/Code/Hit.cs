using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public int dmg;
    public bool is_enemy;

    private Enemy enemy;
    private Player plr;
    public GameObject hitFX;
    public AudioClip[] sound;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (is_enemy == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                plr = other.GetComponent<Player>();
                if (plr.health <= 0) { return; }

                plr.StartFlash();
                plr.health -= dmg;
                AudioManager.Instance.RandomSoundEffect(sound);

                GameObject fx = Instantiate(hitFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Destroy(this.gameObject);
            }
        } 
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                enemy = other.GetComponent<Enemy>();
                if (enemy.health <= 0) { return; }

                enemy.StartFlash();
                enemy.health -= dmg;
                AudioManager.Instance.RandomSoundEffect(sound);

                GameObject fx = Instantiate(hitFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
