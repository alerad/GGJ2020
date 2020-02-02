using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEditor;

//Authors: Nachoco420 and alerad
namespace QuarkFramework {
    public class MovementDetectionSystem : MonoBehaviour, IQuarkSystem, IQuarkNodeSystem
    {
        public Transform basisVectors;
        public IObservable<QuarkNode> movementStream { get; private set; }
    
        private float varianceThreshold;

        private float accumulatedRotation;
        
        private RotationSystemHelper rh => new RotationSystemHelper();

        private void Awake() => movementStream = CreateMovementStream(transform);
        

        private IObservable<QuarkNode> CreateMovementStream(Transform transform) =>
            transform
                .FixedUpdateAsObservable()
                .Select(x => new QuarkNode(GetPosition(transform), GetRotation(transform)))
                .StartWith(new QuarkNode(GetPosition(transform),
                    GetRotation(transform)));
        

        private Vector3 GetPosition(Transform transform) {
            if (basisVectors == null)
                return transform.position;
        
            return basisVectors.InverseTransformPoint(transform.position);
        }

        private QuarkQuaternion GetRotation(Transform q) {
            var ir = q.rotation;//  TransformUtils.GetInspectorRotation(q);  ToDO fix this, temporary build fix @aleRad
            return new QuarkQuaternion(ir.x, ir.y, ir.z, q.rotation);
//            return rh.GetRotation(q); //TODO Hacer que esto ande sin inspector rotation, poco performante y no se si anda afuera de editor mode
        }


        public IObservable<QuarkNode> stream() => movementStream;
    }
}
