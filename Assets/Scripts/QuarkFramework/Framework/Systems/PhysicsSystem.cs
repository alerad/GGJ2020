using System;
using System.Collections;
using System.Collections.Generic;
using QuarkFramework;
using UniRx;
using UnityEngine;

namespace QuarkFramework {
    
    public class PhysicsSystem : MonoBehaviour {
        public Subject<QuarkPhysicsNode> physicsNodeCreatedStream { get; private set; }
        public bool isLightWeight;

        private void Awake() {
            physicsNodeCreatedStream = new Subject<QuarkPhysicsNode>();
        }

        private void Start() {
            IObservable<QuarkNode> stream = PickSystem(isLightWeight);
            stream
                .Pairwise()
                .Select(ToPhysicsNode)
                .Subscribe(physicsNodeCreatedStream.OnNext);
        }
    
        private IObservable<QuarkNode> PickSystem(bool isLightWeight) => 
            isLightWeight ? 
                GetComponent<NodeCreatedSystem>()
                    .nodeCreatedStream :
                GetComponent<MovementDetectionSystem>()
                    .movementStream;



        private QuarkPhysicsNode ToPhysicsNode(Pair<QuarkNode> node) {
            var prevNode = node.Previous;
            var currNode = node.Current;
            var Δd = currNode.Position - prevNode.Position;
            var Δt = currNode.TimeCreated - prevNode.TimeCreated;
            var vel = Δd / Δt;

        
            return new QuarkPhysicsNode(
                currNode,
                prevNode,
                vel,
                Δd
            );
        }
    

    }

}
