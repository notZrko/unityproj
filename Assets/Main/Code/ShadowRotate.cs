using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRotate : MonoBehaviour
{
    public GameObject plr;
    public SpriteRenderer SpriteShadow; public SpriteRenderer GunShadow;

    
    void Update()
    {
        SpriteShadow.transform.rotation = plr.transform.rotation;
        GunShadow.transform.rotation = plr.transform.rotation;

    }
}
