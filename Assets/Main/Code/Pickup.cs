using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GunStat weapon;
    public GameObject getFX;
    public AudioClip[] sfx;

    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = weapon.gun_texture;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().gunStat = weapon;
            AudioManager.Instance.RandomSoundEffect(sfx);

            GameObject fx = Instantiate(getFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
