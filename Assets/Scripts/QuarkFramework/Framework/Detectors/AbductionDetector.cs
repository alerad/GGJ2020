using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using QuarkFramework;
using static QuarkFramework.EnumHelper;

public class AbductionDetector : MonoBehaviour {

    public GameObject quarkSystem;
    public float minTrajectory = 0.3f;
    SegmentComponent quarkSegment;
    public Limb limb;
    public ReactiveProperty<AbductionData> observable { get; set; }

    public class AbductionData {
        public bool incrementing;
        public Limb limb;
        public AbductionData(bool incrementing, Limb limb) {
            this.incrementing = incrementing;
            this.limb = limb;
        }
    }
    private void Awake() {
        observable = new ReactiveProperty<AbductionData>();
        observable = new ReactiveProperty<AbductionData>();
    }

    void Start() {
//        quarkSystem.GetComponent<DirectionChangeSystem>().directionChangeStream.Subscribe(DetectAbudction);
        quarkSegment = quarkSystem.GetComponent<SegmentComponent>();
    }

    void DetectAbudction(DirectionChange change) {
        if (change == null)
            return;
        if (change.axis == Axis.Y && quarkSegment.ByAxis(change.axis).trajectory.y >= minTrajectory) {
            observable.Value = new AbductionData(change.incrementing, limb);
        }
    }


}

