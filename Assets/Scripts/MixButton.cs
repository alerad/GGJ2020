using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixButton : MonoBehaviour {
    public Cauldron c;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerHand"))
            c.MixCauldron();
    }
}
