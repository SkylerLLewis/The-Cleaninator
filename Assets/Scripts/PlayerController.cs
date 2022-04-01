using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody body;

    // * Locomotion and looking
    float horizontalInput, verticalInput, deltaY;
    Vector2 mouse;
    float speed = 20, maxSpeed = 4, sensitivity = 10, maxTurn = 100;
    CameraController camera;

    // * Resources
    UIController uiController;
    int iron = 0;

    // * Survivals

    float charge, maxCharge = 100;

    // * Right and left equipment
    bool rightHandActive = false;
    public bool hammerActive = false;
    float rightHandCount = 0;
    Transform hammer;
    float hammerBase = 0.1f, hammerExtend = 1f;

    Transform gun;
    ParticleSystem bulletEmitter;

    void Awake() {
        body = gameObject.GetComponent<Rigidbody>();
        uiController = GameObject.FindWithTag("Canvas").GetComponent<UIController>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();

        foreach (Transform child in transform) {
            if (child.name == "Hammer") {
                hammer = child;
            } else if (child.name == "Gun") {
                gun = child;
                // The bullet bulletEmitter is the gun's first child
                bulletEmitter = child.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            }
        }

        mouse = Vector2.zero;

        charge = 100;

        InvokeRepeating("UpdateCharge", 0.25f, 0.25f);
    }

    void Update() {
        mouse.x += Input.GetAxis("Mouse X") * sensitivity;

        // Gun adjustment
        Vector3 gunAng = transform.eulerAngles;
        if (camera.transform.eulerAngles.x <= 0) {
            gun.eulerAngles = new Vector3(camera.transform.eulerAngles.x-5, gunAng.y, gunAng.z);
        } else {
            gun.eulerAngles = new Vector3(camera.transform.eulerAngles.x/10-5, gunAng.y, gunAng.z);
        }
        /*Vector3 bulletAng = camera.transform.eulerAngles;
        bulletAng.x -= 5;
        bulletAng.y += 1;*/

        // Left click
        if (Input.GetMouseButtonDown(0)) {
            bulletEmitter.transform.eulerAngles = camera.transform.eulerAngles;
            bulletEmitter.Emit(1);
        }
        
        // Right click (right hand item)
        if (Input.GetMouseButtonDown(1) && !rightHandActive) {
            rightHandActive = true;
            hammerActive = true;
            rightHandCount = 0.5f;
            charge -= 1;
            UpdateCharge();
        }

        // handle hammer/ right hand
        if (rightHandCount > 0) { // Hammer strikes for .05 sec, ret .45
            Vector3 pos = new Vector3(0.25f, 0.2f, 0);
            if (rightHandCount > 0.45f) {
                pos.z = hammerBase + hammerExtend * (0.5f - rightHandCount)/0.05f;
            } else {
                pos.z = hammerBase + hammerExtend * rightHandCount/0.45f;
            }
            hammer.localPosition = pos;
            rightHandCount -= Time.deltaTime;
            if (rightHandCount <= 0) {
                rightHandActive = false;
                hammerActive = false;
            }
        }
    }

    void FixedUpdate() {
        // Look around
        //mouse.x += Input.GetAxis("Mouse X") * sensitivity;

        deltaY = Mathf.DeltaAngle(transform.eulerAngles.y, mouse.x);
        if (Mathf.Abs(deltaY) > 0.1f) {
            if (deltaY > 0) {
                body.AddTorque(Vector3.up*Mathf.Clamp(deltaY/10+1, -maxTurn, maxTurn));
            } else {
                body.AddTorque(Vector3.up*Mathf.Clamp(deltaY/10-1, -maxTurn, maxTurn));
            }
        }

        // Move!
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0) {
            Vector3 moveBy = transform.forward * verticalInput + transform.right * horizontalInput;
            if (body.velocity.magnitude < maxSpeed) {
                body.AddForce(moveBy.normalized * speed);
            }
        }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "Charger") {
            charge = 100;
        }
    }

    // A once per second charge updater
    void UpdateCharge() {
        charge -= 0.25f;
        uiController.UpdateCharge(charge/maxCharge);
        if (charge <= 0) {
            Die();
        }
    }

    public void HammerHit(bool destroyed=false) {
        body.AddForce(transform.forward*-200 - body.velocity);
        body.AddForce(transform.up*100);
        if (destroyed) {
            iron += 100;
            uiController.UpdateResources(iron);
        }
    }

    void Die() {
        SceneManager.LoadScene("Menu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
