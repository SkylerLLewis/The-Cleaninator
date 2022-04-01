using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static Vector3 standoff = new Vector3(0, 1.4f, -3);
    Vector3 targetVec, rotatedStandoff, targetEuler, playerAngles;
    float sensitivity = 10,
          headRotationLimit = 45f;
    float yAdjust, standoffAdjust;
    PlayerController player;
    public Vector2 mouse;
    void Awake() {
        Application.targetFrameRate = 60;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        mouse = Vector2.zero;
        rotatedStandoff = standoff;
    }

    void Update() {
        // Rotate
        mouse.x += Input.GetAxis("Mouse X") * sensitivity;
        mouse.y -= Input.GetAxis("Mouse Y") * sensitivity;

        // Pitch
        mouse.y = Mathf.Clamp(mouse.y, -headRotationLimit, headRotationLimit);
        transform.localEulerAngles = new Vector3(mouse.y, 0, 0);
        yAdjust = mouse.y/22.5f;
        standoffAdjust = standoff.z + (mouse.y+45)/90f;
        // Yaw
        Vector3 rot = transform.eulerAngles;
        rot.y = mouse.x;
        transform.eulerAngles = rot;
        
        // Position
        //rotatedStandoff.x = standoff.z*Mathf.Sin(player.transform.eulerAngles.y * Mathf.Deg2Rad);
        //rotatedStandoff.z = standoff.z*Mathf.Cos(player.transform.eulerAngles.y * Mathf.Deg2Rad);
        rotatedStandoff.x = standoffAdjust*Mathf.Sin(mouse.x * Mathf.Deg2Rad);
        rotatedStandoff.z = standoffAdjust*Mathf.Cos(mouse.x * Mathf.Deg2Rad);
        targetVec = player.transform.position + rotatedStandoff;
        targetVec.y += yAdjust;
        transform.position = targetVec;
    }
}
