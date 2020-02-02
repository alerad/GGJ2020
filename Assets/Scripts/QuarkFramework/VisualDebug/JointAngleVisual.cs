using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using QuarkFramework;

public class JointAngleVisual : MonoBehaviour {
    static readonly float MAX_FILL_AMOUNT_ARMPIT = 0.468f;
    static readonly float MIN_FILL_AMOUNT_ARMPIT = 0.005f;
    static readonly float MAX_ANGLE_ARMPIT = 175f;


    public AngleDetectionSystem angleDetector;
    public AngleType angleToTrack;
    Image img;
    Transform txtInfo;

    private Text angleInfo;
    // Start is called before the first frame update
    void Start() {
        angleInfo = GetComponentInChildren<Text>();
        txtInfo = transform.Find("TxtInfo");
        angleDetector.angleStream.Subscribe(UpdateAngleVisual);
        img = GetComponent<Image>();
    }

    void UpdateAngleVisual(BodyAngle angle) {
        if (angle.angleType == angleToTrack) {
            angleInfo.text = $"{Mathf.Round(angle.angle)}°";
            img.fillAmount = GetImgFillAmount(angle.angle);
            txtInfo.localPosition = GetInfoPosition(angle);
        }
    }

    Vector3 GetInfoPosition(BodyAngle angle) {
        Vector3 res = ((angle.Vec1 + angle.Vec2).normalized /7f);
        if (angleToTrack == AngleType.RightArmpit) {
            res.x = -res.x;
        }
        res.z = 0f;
        return res;
    }

    float GetImgFillAmount(float angle) {
        return angle * MAX_FILL_AMOUNT_ARMPIT / MAX_ANGLE_ARMPIT + MIN_FILL_AMOUNT_ARMPIT;
    }

}
