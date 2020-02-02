using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace QuarkFramework {
    public class NodeCreatedSystem : MonoBehaviour, IQuarkSystem, IQuarkNodeSystem {

        [Tooltip("Determines how much distance should be travelled in order to create a node")]
        public float distanceThreshold = 0.05f;
        private Pair<QuarkNode> nodes;

        public Subject<QuarkNode> nodeCreatedStream { get; private set; }

        public void Awake() {
            nodeCreatedStream = new Subject<QuarkNode>();
        }

        private void Start() => CreateSubscription(movStream);

        public IDisposable CreateSubscription(IObservable<QuarkNode> movementStream) =>
            movementStream.Subscribe(UpdateNodes);

        private IObservable<QuarkNode> movStream => GetComponent<MovementDetectionSystem>().movementStream;

        void UpdateNodes(QuarkNode node) {
            if (nodes.Current == null) {
                nodes = new Pair<QuarkNode>(null, node);
                nodeCreatedStream.OnNext(node);
            } else if (IsValidDistance(nodes.Current, node)) {
                nodes = new Pair<QuarkNode>(nodes.Current, node);
                nodeCreatedStream.OnNext(node);
            }
        }

        private bool IsValidDistance(QuarkNode prevNode, QuarkNode nextNode) {
            return Vector3.Distance(prevNode.Position, nextNode.Position) > distanceThreshold;
        }

        IObservable<QuarkNode> IQuarkNodeSystem.stream() => nodeCreatedStream;

    }
}


