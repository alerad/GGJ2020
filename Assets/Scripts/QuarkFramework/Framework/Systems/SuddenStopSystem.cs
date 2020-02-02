using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuarkFramework;
using static QuarkFramework.EnumHelper;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace QuarkFramework {
    public class SuddenStopSystem : MonoBehaviour {
        //TODO Ver qeu onda esto, me parece que las configs tendrían que ir en otro lado. Si quiero tener dos systems
        [Tooltip("Trajectory needed for any given axis, in which the sudden stop system will start checking. If set to 0, it will always check for stops")]
        [SerializeField] private float trajectoryNeeded;
        [Tooltip("Minimum average velocity needed on the segment previous to stopping.")]
        [SerializeField] private float avgVelNeeded;
        [Tooltip("How much a point can move in a given radius to be considered in 'Stopped' state. If set too high, it will always be considered in 'Stopped' state")]
        [SerializeField] private float stopVariance;
        [Tooltip("How much time in seconds a point has to stay still, given the stop variance, in order to emit the stop event")]
        [SerializeField] private float stopTime = 1f;
        
        private QuarkVRTracer vt;

        public Subject<QuarkSuddenStop> suddenStopStream;
        private Pair<QuarkNode> latestNodes;

        public void Awake() {
            suddenStopStream = new Subject<QuarkSuddenStop>();
        }

        void Start() {
//            vt = GetComponent<QuarkVRTracer>();
            SubscribeToMovementStream(GetComponent<MovementDetectionSystem>().movementStream);
            CreateUpdateStream(Observable.EveryUpdate());
        }
        
        public void SubscribeToMovementStream(IObservable<QuarkNode> movStream) => movStream
            .Pairwise()
            .Subscribe(x => latestNodes = x);

        public void CreateUpdateStream(IObservable<long> update) => update
            .Where(_ => latestNodes.Previous != null && latestNodes.Current != null)
            .Subscribe(_ => OnUpdate());

        private float timerStopped;

        private void OnUpdate() {
    //        if (validTrajectoryEachAxis && validVelocityForEachAxis) {
            if (IsStopped(latestNodes)) {
                timerStopped += latestNodes.Current.TimeCreated - latestNodes.Previous.TimeCreated;
                if (timerStopped >= stopTime) {
                    var stopNode = new QuarkSuddenStop {
                        stopNode = latestNodes.Current
                    };
                    timerStopped = 0f;
                    suddenStopStream.OnNext(stopNode);
                }
            }
            else {
                timerStopped = 0f;
            }
    //    }
    }

        public void SetThresholds(float tjNeeded, float avgVel, float stopVar, float stopTime) {
            trajectoryNeeded = tjNeeded;
            avgVelNeeded = avgVel;
            stopVariance = stopVar;
            this.stopTime = stopTime;
        }
        
        private bool IsStopped(Pair<QuarkNode> n) => (n.Previous.Position - n.Current.Position).magnitude < stopVariance;
        private bool validVelocityForEachAxis => validSegments.All(ValidVelocity);
        private bool validTrajectoryEachAxis => validSegments.All(ValidTrajectory);
        private bool ValidTrajectory(QuarkSegment x) => x.trajectory.GetAxis(x.axis) >= trajectoryNeeded;
        private bool ValidVelocity(QuarkSegment x) => x.GetAverageVelocity() >= avgVelNeeded;


        private List<Axis> axises = new List<Axis> {Axis.X, Axis.Y, Axis.Z};
        private IEnumerable<QuarkSegment> validSegments => axises
            .Select(x => vt.segmentComponent.ByAxis(x)).Where(x => x != null);

    }
}
