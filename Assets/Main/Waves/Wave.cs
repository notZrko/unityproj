using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/New Wave", order = 1)]
public class Wave : ScriptableObject
{
    public bool is_boss = false;
    [SerializeField] public GameObject[] enemies;
    public double damage_increase_per_wave = 1;
}
