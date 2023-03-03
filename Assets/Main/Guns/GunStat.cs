using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStat", menuName = "ScriptableObjects/Create Gun", order = 1)]
public class GunStat : ScriptableObject
{
    public float fire_delay = 0.1f;
    public int damage = 10;
    public GameObject bullet;
    public AudioClip[] sounds;
    public Vector3 scale = new Vector3(1, 1, 1);

    public Sprite gun_texture;
    public int value = 0;
}
