using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static QuarkFramework.EnumHelper;

namespace QuarkFramework {

    //Yo, como programador, quiero poder comunicarme con cualquier evento de quark, y poder tener tambien datos de los limbs. Mi manera principal de comunicarme es a travges del VRComposite. 

    public class VRQuarkSegment {
        public QuarkSegment data;
        public Limb Limb;
        
        public VRQuarkSegment(QuarkSegment data, Limb limb) {
            this.data = data;
            Limb = limb;
        }
    }

    public class VRDirectionChange: DirectionChange {
        public Limb limb;
        public VRDirectionChange(DirectionChange dc, Limb limb) : base(dc.incrementing, dc.axis, dc.newNode) {
            this.limb = limb;
        }
    }

    public class VRQuarkNode : QuarkNode {
        public Limb limb;

        public VRQuarkNode(QuarkNode n, Limb l) : base(n.Position, n.rotation) {
            limb = l;
        }
    }

    public class VRQuarkPhysicsNode : QuarkPhysicsNode {
        public Limb limb;
        public VRQuarkPhysicsNode(QuarkPhysicsNode p, Limb l) : base(p.node, p.prevNode, p.velocityVector, p.distance) {
            limb = l;
        }
    }

    public class DirectionChange {
        public bool incrementing;
        public Axis axis;
        public QuarkNode newNode;

        public DirectionChange(bool incrementing, Axis axis, QuarkNode newNode) {
            this.incrementing = incrementing;
            this.axis = axis;
            this.newNode = newNode;
        }

    }

    //The main class of any event emission that has to do with data extracted from just snapshots of data available from the sensors
    public abstract class QuarkData {}

    //The enriched data created by Quark, which can't be obtained easly from just the sensors.
    public abstract class QuarkEnrichedData: QuarkData {}

    public class QuarkNode: QuarkData {
        private Vector3 position;
        public QuarkQuaternion rotation;

        //Using unity Time.time
        private float timeCreated;

        public QuarkNode(Vector3 position, QuarkQuaternion rotation) {
            this.position = position;
            timeCreated = Time.time;
            this.rotation = rotation;
        }
        
        public QuarkNode() {
            
        }

        public Vector3 Position {
            get => position;
            set => position = value;
        }

        public float TimeCreated {
            get => timeCreated;
            set => timeCreated = value;
        }
    }

    public class QuarkSegment: QuarkEnrichedData {
        public List<QuarkNode> nodes;
        private float segmentSpeed;
        public float velocity;
        public Vector3 vectorialVelocity;
        public Vector3 trajectory;
        public bool incrementing;
        public Axis axis;

        public QuarkSegment() {
            nodes = new List<QuarkNode>();
            velocity = 0f;
            trajectory = Vector3.zero;
            segmentSpeed = 0f;
        }
        
        public float GetAverageVelocity() {
            return velocity / nodes.Count;
        }
    }

    public static class Helpers {
        public static List<Axis> axises => new List<Axis> {Axis.X, Axis.Y, Axis.Z};
    }

    
    public class QuarkPhysicsNode : QuarkNode {
        public QuarkNode node { get; private set; }

        public QuarkNode prevNode;

        public Vector3 velocityVector { get; private set; }
        public Vector3 distance { get; private set; }
        public Vector3 absoluteVariance => new Vector3(Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z));
        
        public QuarkPhysicsNode(QuarkNode node, QuarkNode prevNode, Vector3 velocityVector, Vector3 distance) : base(node.Position, node.rotation) {
//            velocityVector = velocityVector;
            this.node = node;
            this.prevNode = prevNode;
            this.velocityVector = velocityVector;
            this.distance = distance;
        }
    }

    public class VectorAxis {
        public float value;
        public Axis axis;

        public VectorAxis(float v, Axis a) {
            value = v;
            axis = a;
        }
    }

    //This class is used to get a sum of euler angles, to facilitate some quat calculations
    public class QuarkQuaternion: QuarkData {
        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }

        public Quaternion quaternion;

        public QuarkQuaternion(float x, float y, float z, Quaternion quaternion) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.quaternion = quaternion;
        }
        
        public static float Dot(QuarkQuaternion qa, QuarkQuaternion qb) {
            var a = qa.quaternion;
            var b = qb.quaternion;
            return (float) ((double) a.x * (double) b.x + (double) a.y * (double) b.y + (double) a.z * (double) b.z + (double) a.w * (double) b.w);
        }
    }

    public class QuarkSuddenStop {
        public QuarkNode stopNode;
        
    }
    
    
    #region Internal classes
    internal class QuarkAxisNode
    {
        public QuarkNode node;
        public Axis Axis;
    
        public QuarkAxisNode(QuarkNode node, Axis axis) {
            this.node = node;
            this.Axis = axis;
        }
        
    
    }

    public static class QuarkExtensions {
        public static float ByAxis(this Vector3 v, Axis axis)
        {
            switch (axis) {
                case Axis.X: return v.x;
                case Axis.Y: return v.y;
                case Axis.Z: return v.z;
                default: throw new Exception();
            }
        }
    }
    #endregion
}

