using System;
using System.Collections;
using System.Collections.Generic;
using QuarkFramework;
using UnityEngine;
using UniRx;

namespace QuarkFramework {
    public class RotationSystem : MonoBehaviour, IQuarkSystem {

        [Tooltip("Defines how different the quaternions have to be in order to emit the event. 1 means they should be completely opposite (0 deg vs 180deg quaternion). 0 would emit same values")]
        public float rotationVarianceThreshold = 0.0035f;
        public Subject<QuarkNode> rotationStream { get; private set; }
        private Pair<QuarkNode> nodes;

        private void Awake() {
            rotationStream = new Subject<QuarkNode>();
        }

        private void Start() {
            GetComponent<MovementDetectionSystem>().movementStream.Subscribe(EmitRotation); 
        }

        private void EmitRotation(QuarkNode node) {
            if (nodes.Current == null) {
                rotationStream.OnNext(node);
                nodes = new Pair<QuarkNode>(null, node);
            }
            else if (!QuaternionIsSimilar(nodes.Current, node)) {
                rotationStream.OnNext(node);
                nodes = new Pair<QuarkNode>(nodes.Current, node);
            }
        }

        private bool QuaternionIsSimilar(QuarkNode prevNode, QuarkNode nextNode) {
            var prevNorm = prevNode.rotation.quaternion;
            var nextNorm = nextNode.rotation.quaternion;
            var similarity = Mathf.Abs(Quaternion.Dot(prevNorm, nextNorm));
            return similarity >= 1f - rotationVarianceThreshold;
        }
    }
}
