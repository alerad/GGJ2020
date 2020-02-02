using System;
using System.Collections;
using System.Collections.Generic;
using QuarkFramework;
using UniRx;
using UnityEditor;
using UnityEngine;

public class QuarkVRTracer : MonoBehaviour {
    private DirectionChangeSystemV0 dcs;
    private MovementDetectionSystem mds;
    private PhysicsSystem ps;
    private PhysicsComponent pc;
    private NodeCreatedSystem ncs;
    private SegmentComponent sc;
    public EnumHelper.Limb limb;

    private void Awake() {
        dcs = GetComponent<DirectionChangeSystemV0>();
        mds = GetComponent<MovementDetectionSystem>();
        ncs = GetComponent<NodeCreatedSystem>();
        sc  = GetComponent<SegmentComponent>();
        ps  = GetComponent<PhysicsSystem>();
        pc  = GetComponent<PhysicsComponent>();
    }

    public IObservable<VRDirectionChange> directionChange() => dcs.directionChangeStream
        .Select(x => new VRDirectionChange(x, limb));
    public IObservable<VRQuarkNode> nodeCreated() => ncs.nodeCreatedStream
        .Select(x => new VRQuarkNode(x, limb));
    public IObservable<VRQuarkNode> movement() => mds.movementStream
        .Select(x => new VRQuarkNode(x, limb));
    public IObservable<VRQuarkPhysicsNode> phyisics() => ps.physicsNodeCreatedStream
        .Select(x => new VRQuarkPhysicsNode(x, limb));

    public PhysicsComponent phyisicsComponent() => pc;

   
    public SegmentComponent segmentComponent => sc;
}
