using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject plr;
    public Camera cam;
    public float threshold;

    public Vector3 cameraOffset;
    public float cameraSpeed = 0.1f;

    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (plr.transform.position + mousePos) / 2f;

        // targetPos.x = Mathf.Clamp(targetPos.x, -threshold + plr.transform.position.x, threshold + plr.transform.position.x);
        //  targetPos.y = Mathf.Clamp(targetPos.y, -threshold + plr.transform.position.y, threshold + plr.transform.position.y);

        Vector3 finalPosition = plr.transform.position + cameraOffset;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        transform.position = lerpPosition;
    }
}
