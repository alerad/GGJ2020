using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static QuarkFramework.EnumHelper;
using QuarkFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class NarutoRun : MonoBehaviour {

    public QuarkVRTracer leftHand;
    public QuarkVRTracer rightHand;
    public QuarkVRTracer headset;



    public Vector3 headsetOutOfPoseThreshold;

    public Vector3 handOutOfPoseThreshold; 
    
    //Min xxxVel
    //Min xxxTj
    public Vector3 headsetVel;
    public Vector3 headsetTrajectory;
    public Vector3 leftHandTrajectory;
    public Vector3 rightHandTrajectory;
    public Vector3 leftHandVel;
    public Vector3 rightHandVel;


    private bool inNarutoPose = false;
    
    private void Start() {
        var lhpc = leftHand.phyisicsComponent();
        var rhpc = leftHand.phyisicsComponent();
        var hpc = leftHand.phyisicsComponent();
        
        
        Observable
            .EveryUpdate()
            .Where(_ => 
                ValidVelocity(lhpc, leftHandTrajectory) &&
                ValidVelocity(rhpc, rightHandTrajectory) &&
                ValidVelocity(hpc, headsetTrajectory) &&
//                ValidTrajectory(lhpc, leftHandVel) &&
//                ValidTrajectory(rhpc, rightHandVel) &&
//                ValidTrajectory(hpc, headsetVel) &&
                !inNarutoPose
                )
            .Do(_ => inNarutoPose = true)
            .Subscribe(_ => CreatePoseWatcher());

    }

    private void CreatePoseWatcher() {
        Debug.Log("Creating poswe watcher :D");
        var poseWatcherh = new GameObject().AddComponent<PoseWatcher>();
        var poseWatcherlh = new GameObject().AddComponent<PoseWatcher>();
        var poseWatcherrh = new GameObject().AddComponent<PoseWatcher>();
        
        poseWatcherh.CreateWatcher(headset.transform, headsetOutOfPoseThreshold);
        poseWatcherrh.CreateWatcher(rightHand.transform, handOutOfPoseThreshold);
        poseWatcherlh.CreateWatcher(leftHand.transform, handOutOfPoseThreshold);

        poseWatcherh
            .OnDestroyAsObservable()
            .Merge(poseWatcherlh.OnDestroyAsObservable())
            .Merge(poseWatcherrh.OnDestroyAsObservable())
            .Subscribe(_ => inNarutoPose = false);
    }
    
    private bool ValidVelocity(PhysicsComponent tracer, Vector3 threshold) => tracer.velocity.AsList().All(x => x.value < threshold.ByAxis(x.axis));
    private bool ValidTrajectory(PhysicsComponent tracer, Vector3 threshold) => tracer.trajectory.AsList().All(x => x.value < threshold.ByAxis(x.axis));

    private bool PointDecrementing(Vector3 prev, Vector3 curr, Axis axis) => prev.ByAxis(axis) > curr.ByAxis(axis);
    private bool PointIncrementing(Vector3 prev, Vector3 curr, Axis axis) => prev.ByAxis(axis) <= curr.ByAxis(axis);
    

}
