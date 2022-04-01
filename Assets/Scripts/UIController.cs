using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    TextMeshProUGUI iron;
    Transform charge;
    void Start() {
        foreach (Transform child in transform) {
            if (child.name == "Iron") {
                iron = child.gameObject.GetComponent<TextMeshProUGUI>();
            } else if (child.name == "Charge") {
                charge = child;
            }
        }
    }

    public void UpdateResources(int i) {
        iron.text = i.ToString();
    }

    public void UpdateCharge(float c) {
        charge.localScale = new Vector3(c, 1, 1);
    }
}
