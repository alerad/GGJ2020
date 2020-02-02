using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace QuarkFramework {
    public class AngleDetectionSystem : MonoBehaviour, IQuarkSystem {
    
    public IObservable<BodyAngle> angleStream { get; set; }

    public Transform rightShoulder;

    public Transform leftShoulder;

    public Transform rightHand;

    public Transform leftHand;
    
    // Start is called before the first frame update
    void Awake() {
        angleStream = Observable.Create<BodyAngle>(observer => {
            Observable.EveryFixedUpdate().Subscribe(x => EmitAngles(observer));
            return Disposable.Empty;
        });
    }

    
    private void EmitAngles(IObserver<BodyAngle> observer) {
        observer.OnNext(GetBodyAngle(AngleType.RightArmpit));
        observer.OnNext(GetBodyAngle(AngleType.LeftArmpit));
        observer.OnNext(GetBodyAngle(AngleType.LeftFlex));
        observer.OnNext(GetBodyAngle(AngleType.RightFlex));
    }

    //Usar Y - x para tercer punto
    private BodyAngle GetBodyAngle(AngleType type) {
        Transform shoulder = GetShoulder(type);
        Transform hand = GetHand(type);
        Vector3 thirdPoint = GetPointForRightTriangle(shoulder);
        var position = shoulder.position;
        Vector3 shoulderToHandVector = VectorToVector(hand.position, position); 
        Vector3 shoulderToThirdPoint = VectorToVector(thirdPoint, position);
        float angle = Vector3.Angle(shoulderToHandVector, shoulderToThirdPoint);
        return new BodyAngle(type, angle, shoulderToHandVector, shoulderToThirdPoint);
    }

    private Transform GetShoulder(AngleType type) {
        switch (type) {
            case AngleType.LeftArmpit: return leftShoulder;
            case AngleType.LeftFlex: return leftShoulder;
            case AngleType.RightArmpit: return rightShoulder;
            case AngleType.RightFlex: return rightShoulder;
            default: throw new InvalidDataException("Enum received is not contemplated");
        }
    }
    
    private Transform GetHand(AngleType type) {
        switch (type) {
            case AngleType.LeftArmpit: return leftHand;
            case AngleType.LeftFlex: return leftHand;
            case AngleType.RightArmpit: return rightHand;
            case AngleType.RightFlex: return rightHand;
            default: throw new InvalidDataException("Enum received is not contemplated");
        }
    }

    private Vector3 GetPointForRightTriangle(Transform shoulder) {
      return new Vector3(shoulder.position.x, shoulder.position.y - 0.4f, shoulder.position.z); 
    }

    //Points a vector from a initial point to another
    private Vector3 VectorToVector(Vector3 targetPoint, Vector3 initialPoint) {
        return targetPoint - initialPoint;
    }
    
   
    
    


}

public class BodyAngle {

    private AngleType AngleType;
    private float Angle;
    public Vector3 Vec1 { get; private set; }
    public Vector3 Vec2 { get; private set; }

    public BodyAngle(AngleType angleType, float angle, Vector3 vec1, Vector3 vec2) {
        AngleType = angleType;
        Angle = angle;
        Vec1 = vec1;
        Vec2 = vec2;
    }

    public float angle {
        get => Angle;
        private set => Angle = value;
    }
    
    public AngleType angleType {
        get => AngleType;
        private set => AngleType = value;
    }
}

public enum AngleType { RightFlex, LeftFlex, RightArmpit, LeftArmpit}


}

