using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuarkFramework.EnumHelper;
using UniRx;

namespace QuarkFramework {

    [Serializable]
    public class UnityProperties {
        public bool computeTrajectory = true;
        public bool computeVelocityVector = true;
    }

    public class SegmentComponent : MonoBehaviour {
        private Dictionary<Axis, QuarkSegment> currentSegment { set; get; }
        public UnityProperties settings;
        private QuarkNode prevNode;
        
        private void Start() {
            InitializeSegments();
            GetComponent<NodeCreatedSystem>().nodeCreatedStream.Subscribe(UpdateSegment);
            GetComponent<DirectionChangeSystemV0>().directionChangeStream.Subscribe(ResetSegment);
        }

        
        private void UpdateSegment(QuarkNode node) {
            if (prevNode != null) {
                var variance = CalculateVariance(prevNode.Position, node.Position);

                if (settings.computeTrajectory)
                    UpdateTrajectory(variance);

                if (settings.computeVelocityVector)
                    UpdateVelocity(prevNode, node);
            }

            GetSegments()
                .ForEach(s=>s.nodes.Add(node));
            
            prevNode = node;
        }

        private void ResetSegment(DirectionChange dc) {
            if (dc == null)
                return;
            currentSegment[dc.axis] = new QuarkSegment();
            currentSegment[dc.axis].incrementing = dc.incrementing;
            currentSegment[dc.axis].nodes.Add(dc.newNode);
        }

        private void InitializeSegments() {
            currentSegment = new Dictionary<Axis, QuarkSegment>();
            currentSegment[Axis.X] = new QuarkSegment();
            currentSegment[Axis.Y] = new QuarkSegment();
            currentSegment[Axis.Z] = new QuarkSegment();
        }

        private void UpdateTrajectory(Vector3 variance) {
            Vector3 absoluteVariance = new Vector3(Mathf.Abs(variance.x), Mathf.Abs(variance.y), Mathf.Abs(variance.z));
            GetSegments().ForEach(s => s.trajectory += absoluteVariance);
        }

        
        
        private void UpdateVelocity(QuarkNode prevNode, QuarkNode newNode) => GetSegments()
            .ForEach(seg => {
                seg.velocity += GetVelocity(prevNode, newNode, seg.axis);
            });
        
        
        private float GetVelocity(QuarkNode previousNode, QuarkNode newNode, Axis axis) {
            var prevPos = previousNode.Position;
            var newPos = newNode.Position;
            float deltaT = newNode.TimeCreated - previousNode.TimeCreated;
            var deltaDistance = VecAxis(axis, prevPos) - VecAxis(axis, newPos);
            return Mathf.Abs(deltaDistance) / deltaT;
        }

        private List<QuarkSegment> GetSegments() => new List<QuarkSegment>() {
            currentSegment[Axis.X],
            currentSegment[Axis.Y],
            currentSegment[Axis.Z]
        };
        
        private Vector3 CalculateVariance(Vector3 prevNode, Vector3 newNode) => newNode - prevNode;
        public QuarkSegment x => currentSegment[Axis.X];
        public QuarkSegment y => currentSegment[Axis.Y];
        public QuarkSegment z => currentSegment[Axis.Z];
        public QuarkSegment ByAxis(Axis axis) => currentSegment[axis];

    }

}
