using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbsUp : MonoBehaviour {
    [SerializeField]
    private OVRHand handL;
    [SerializeField]
    private OVRHand handR;

    public TextMesh text;

    private void Update() {
        text.text = $"Right hand middlef: {handR.GetFingerPinchStrength(OVRHand.HandFinger.Middle)}\nLeft hand middlef: {handL.GetFingerPinchStrength(OVRHand.HandFinger.Middle)}";
    }
    
    
}
