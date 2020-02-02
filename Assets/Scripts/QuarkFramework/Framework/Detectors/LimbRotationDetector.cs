using System;
using System.Collections;
using System.Collections.Generic;
using QuarkFramework;
using UniRx;
using UnityEngine;
using static QuarkFramework.EnumHelper;

public class LimbRotationDetector : MonoBehaviour {
     public IObservable<LimbRotationData> observable;
     public GameObject tracer;
     public Limb limb;

     private void Awake() {
          observable = Observable.Create<LimbRotationData>(x => Disposable.Empty);
     }

     private void Start() {
          observable =
               tracer.GetComponent<RotationChangeSystem>()
                    .rotationChangeStream
                    .Where(x=> x != null)
                    .Where(x => x.axis == Axis.Z)
                    .Select(AsLimbRotationData);

     }

     private LimbRotationData AsLimbRotationData(DirectionChange change) {
          var type = LimbRotationType.Supination;
          if (change.incrementing)
               type = LimbRotationType.Pronation;
               
          return new LimbRotationData(type, limb);
     }
}

public class LimbRotationData {
     public LimbRotationType type;
     public Limb limb;
     public LimbRotationData(LimbRotationType type, Limb limb) {
          this.type = type;
          this.limb = limb;
     }
}

public enum LimbRotationType {
     Supination,
     Pronation
}