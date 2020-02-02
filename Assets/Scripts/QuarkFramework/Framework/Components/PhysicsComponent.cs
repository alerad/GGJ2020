using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuarkFramework;
using UnityEngine;
using UniRx;

namespace QuarkFramework {
    
    public class PhysicsComponent : MonoBehaviour {
        [SerializeField] private int nodesToUse = 3;

        private FixedSizedQueue<QuarkPhysicsNode> nodes;
    

        // Start is called before the first frame update
        void Start() {
            nodes = new FixedSizedQueue<QuarkPhysicsNode>(nodesToUse);
            transform.parent.GetComponentInChildren<PhysicsSystem>()
                .physicsNodeCreatedStream
                .Subscribe(AddNode);
        }
        
        private void AddNode(QuarkPhysicsNode node) {
            nodes.Enqueue(node);
        }

        public Vector3 velocity => nodes.Aggregate(Vector3.zero, (acc, x) => x.velocityVector);
        public Vector3 trajectory => nodes.Aggregate(Vector3.zero, (acc, x) => x.absoluteVariance);



    }

}
