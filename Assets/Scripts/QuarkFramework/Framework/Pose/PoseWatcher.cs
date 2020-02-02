using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuarkFramework;
using UniRx;
using UnityEngine;
using static QuarkFramework.Helpers;

//How to use:

//Instantiate a gameobject, use the createWatcher method, subscirbe to the gameobject OnDestroy, do your pose end logic.
//In case you need to use multiple pose watchers for a single pose, you should merge the OnDestroy Observables with the Merge() method.
namespace QuarkFramework {
    public class PoseWatcher : MonoBehaviour {
        private Transform objectToFollow;
        [Tooltip("The point in which the pose is described, calculations are based on distance from this point")]
        private Vector3 originPoint;
        [Tooltip("Describes how far the object to follow has to go in order to emit a 'no longer in pose' event. If a axis is set to 0, it won't have a limit")]
        private Vector3 poseArea;
        
        //Emits if point no longer considered in pose
        public Subject<long> outOfPoseStream = new Subject<long>();
    
    
        public void CreateWatcher(Transform objectToFollow, Vector3 poseArea) {
            this.objectToFollow = objectToFollow;
            originPoint = objectToFollow.position;
            this.poseArea = poseArea;
        }
    
        public void CreateWatcher(Transform objectToFollow, Vector3 originPoint, Vector3 poseArea) {
            this.objectToFollow = objectToFollow;
            this.originPoint = originPoint;
            this.poseArea = poseArea;
        }

        private bool InPoseArea(Vector3 origin, Vector3 objectToFollow, Vector3 poseArea) => axises
            .Where(x => !Mathf.Approximately(poseArea.ByAxis(x), 0f))
            .Count(x => Mathf.Abs(origin.ByAxis(x) - objectToFollow.ByAxis(x)) >  poseArea.ByAxis(x)) == 0;

        private void Start() => CreateUpdateLoop(Observable.EveryUpdate());
                
        public void CreateUpdateLoop(IObservable<long> update) => update
            .Where(_ => !InPoseArea(originPoint, objectToFollow.position, poseArea))
            .Do(outOfPoseStream.OnNext)
            .Subscribe(_ => DestroyImmediate(this));
    
    }
}
