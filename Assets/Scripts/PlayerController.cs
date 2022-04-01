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

    void Awake() {
        body = gameObject.GetComponent<Rigidbody>();
        uiController = GameObject.FindWithTag("Canvas").GetComponent<UIController>();

        mouse = Vector2.zero;

        charge = 100;

        // For now, the transform of the hammer is the 0th child
        hammer = transform.GetChild(0);

        InvokeRepeating("UpdateCharge", 0.25f, 0.25f);
    }

    void Update() {
        mouse.x += Input.GetAxis("Mouse X") * sensitivity;

        if (Input.GetMouseButtonDown(1) && !rightHandActive) {
            rightHandActive = true;
            hammerActive = true;
            rightHandCount = 0.5f;
            charge -= 1;
            UpdateCharge();
        }

        if (rightHandCount > 0) { // Hammer strikes for .05 sec, ret .45
            Vector3 pos = new Vector3(0.25f, 2, 0);
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

        if (mouse.x != 0) {
            deltaY = Mathf.DeltaAngle(transform.eulerAngles.y, mouse.x);
            body.AddTorque(Vector3.up*Mathf.Clamp(deltaY/10, -maxTurn, maxTurn));
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
        body.AddForce(transform.forward*-200);
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
