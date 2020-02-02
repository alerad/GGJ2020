using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using static QuarkFramework.EnumHelper;

namespace QuarkFramework {
    
    public class RotationChangeSystem : MonoBehaviour, IQuarkSystem {
         [Tooltip("Determines how much variation on a given axis is required to create a node.")]
            public float varianceThreshold = 0.02f;

            public IObservable<DirectionChange> rotationChangeStream;
            
            private LinkedList<QuarkNode> relevantNodes;

            private void Awake() {
                relevantNodes = new LinkedList<QuarkNode>();
                rotationChangeStream = new ReactiveProperty<DirectionChange>();
            }

            private void Start() {
                GetComponent<RotationSystem>().rotationStream
                    .Do(UpdateRelevantNodes)
                    .Where(WaitEmissions)
                    .Select(IsValidVarianceNodes)
                    .Select(DirectionChanged)
                    .SelectMany(directionChangeNodes => directionChangeNodes.Select(ToDirectionChange))
                    .Share();
            }

        private DirectionChange ToDirectionChange(QuarkAxisNode node) {
                var segment = relevantNodes;
                var incrementing = segment.ElementAt(segment.Count - 2).rotation.GetAxis(node.Axis) <
                                   node.node.rotation.GetAxis(node.Axis);
                return new DirectionChange(incrementing, node.Axis, node.node);
            }

            private void UpdateRelevantNodes(QuarkNode node) {
                if (relevantNodes.Count == 3) {
                    relevantNodes.RemoveFirst();
                }
                relevantNodes.AddLast(node);
            }

            private bool WaitEmissions(QuarkNode node) {
                return relevantNodes.Count == 3;
            }

            private Dictionary<Axis, bool> IsValidVarianceNodes(QuarkNode currNode) {
                Dictionary<Axis, bool> bools = new Dictionary<Axis, bool>();
                var prevNode = relevantNodes.ElementAt(1);
                var prevRot = prevNode.rotation;
                var currRot = currNode.rotation;
                var varianceVector = new Vector3(prevRot.x - currRot.x, prevRot.y - currRot.y, prevRot.z - currRot.z);
                bools[Axis.X] = Mathf.Abs(varianceVector.GetAxis(Axis.X)) > varianceThreshold;
                bools[Axis.Y] = Mathf.Abs(varianceVector.GetAxis(Axis.Y)) > varianceThreshold;
                bools[Axis.Z] = Mathf.Abs(varianceVector.GetAxis(Axis.Z)) > varianceThreshold;
                return bools;
            }
            
            private List<QuarkAxisNode> DirectionChanged(Dictionary<Axis, bool> node) {
                return GetDirChangeNodes(node);
            }

            private List<QuarkAxisNode> GetDirChangeNodes(Dictionary<Axis, bool> validVariance) {
                List<QuarkAxisNode> directionChanges = new List<QuarkAxisNode>();
                QuarkNode prevNode = null;

                if (relevantNodes.Count != 3)
                    return directionChanges;
                var secondNode = relevantNodes.ElementAt(1).rotation;
                var firstNode = relevantNodes.First().rotation;
                var lastNode = relevantNodes.Last();

                for (int i = 0; i <= 2; i++) {
                    Axis axis = (Axis) i;
                    if (!validVariance[axis])
                        continue;
                    var secondNodeAxis = secondNode.GetAxis(axis);
                    var incremeting = firstNode.GetAxis(axis) < secondNodeAxis;
                    var dirChange = (secondNodeAxis < lastNode.rotation.GetAxis(axis)) != incremeting;
                    if (dirChange)
                        directionChanges.Add(new QuarkAxisNode(lastNode, axis));
                }
                
                return directionChanges;
            }
    }
}
