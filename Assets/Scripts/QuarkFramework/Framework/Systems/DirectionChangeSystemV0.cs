using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using static QuarkFramework.EnumHelper;


namespace QuarkFramework {
    public class DirectionChangeSystemV0 : MonoBehaviour, IQuarkSystem {
        public Subject<DirectionChange> directionChangeStream;

        
        [Tooltip("Determines how much variation on a given axis is required to create a node.")]
        public float varianceThreshold = 0.02f;
        private readonly Queue<QuarkNode> nodeHistory = new Queue<QuarkNode>(10);
        private Dictionary<Axis, bool> incrementing;
        private QuarkNode prevNode => nodeHistory.ElementAt(nodeHistory.Count-2);
        private QuarkNode newNode => nodeHistory.Last();

        public void Awake() {
            directionChangeStream = new Subject<DirectionChange>();
            incrementing = incrementingDict;
        }
        
        IDisposable Start() => CreateDirectionChangeSystem(nodeStream);
        
        public IDisposable CreateDirectionChangeSystem(IObservable<QuarkNode> ncs) => ncs
            .Do(InsertNode)
            .Skip(3)
            .Subscribe(OnNodeCreated);

        private IObservable<QuarkNode> nodeStream => GetComponent<NodeCreatedSystem>()
            .nodeCreatedStream;
        

        private void OnNodeCreated(QuarkNode node) {
            Vector3 variance = CalculateVariance(prevNode.Position, newNode.Position);
            
            for (int i = 0; i < Enum.GetNames(typeof(Axis)).Length; i++) {
                Axis axis = (Axis)i;
                float axisVariance = VecAxis(axis, variance);
                //This is done because we only care how much it travelled in the relevant axis in order to detect a direction change.
                if (Mathf.Abs(axisVariance) > varianceThreshold) {
                    if (DirectionChanged(prevNode.Position, newNode.Position, axis)) {
                        incrementing[axis] ^= true;
                        directionChangeStream.OnNext(new DirectionChange(incrementing[axis], axis, node));
                    }
                }

            }
        }

        private void InsertNode(QuarkNode n) => nodeHistory.Enqueue(n);
        
        bool DirectionChanged(Vector3 previousNode, Vector3 nNode, Axis axis) {
            var inc = incrementing[axis];
            switch (axis)
            {
                case Axis.X:
                    return (inc && nNode.x <= previousNode.x) || (!inc && nNode.x > previousNode.x);
                case Axis.Y:
                    return (inc && nNode.y <= previousNode.y) || (!inc && nNode.y > previousNode.y);
                case Axis.Z:
                    return (inc && nNode.z <= previousNode.z) || (!inc && nNode.z > previousNode.z);
                default:
                    return false;
            }
        }
        
        private Vector3 CalculateVariance(Vector3 previousNode, Vector3 newNode) => newNode - previousNode;
        
        private Dictionary<Axis, bool> incrementingDict =>
            new Dictionary<Axis, bool> {
                [Axis.X] = false,
                [Axis.Y] = false,
                [Axis.Z] = false
            };
     
    }
}
