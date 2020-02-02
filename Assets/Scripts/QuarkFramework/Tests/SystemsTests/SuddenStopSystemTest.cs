using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace QuarkFramework {
    public class SuddenStopSystemTest {
        [Test]
        public void Test() {
            var fakeUpdate = new Subject<long>();
            var suddenStop = new GameObject().AddComponent<SuddenStopSystem>();
            var fakeMovStream = new Subject<QuarkNode>();
            var stopTime = 0.2f;
            suddenStop.Awake();
            suddenStop.CreateUpdateStream(fakeUpdate);
            suddenStop.SetThresholds(0, 0, 0.01f, stopTime);
            suddenStop.SubscribeToMovementStream(fakeMovStream);
            
            var suddenStopCount = 0;
            suddenStop.suddenStopStream.Subscribe(_ => suddenStopCount++);
            
            
            var mockNodesFirstSegment = LerpNodes(10, 1);
            
            Assert.AreEqual(mockNodesFirstSegment.Count, 10);
            
            var mockNodesSecondSegment = Enumerable.Range(0, 10)
                .Select(x => new QuarkNode {
                    Position = mockNodesFirstSegment.Last().Position,
                    rotation = null,
                    TimeCreated = mockNodesFirstSegment.Last().TimeCreated + stopTime + 0.1f
                });

            var nodesMerge = mockNodesFirstSegment.Concat(mockNodesSecondSegment);
            
            foreach (var quarkNode in nodesMerge) {
                fakeMovStream.OnNext(quarkNode);
                fakeUpdate.OnNext(0l);
            }
            
            Assert.AreEqual(1, suddenStopCount);



        }
        
        
        private List<QuarkNode> LerpNodes(float distanceToCover, float stepSize) {
            var originVector = Vector3.zero;
            var endVector = new Vector3(distanceToCover, 0, 0);
            var lerpSteps = distanceToCover / stepSize;
            
            return Enumerable
                .Range(0, (int)lerpSteps)
                .Select(x => (float)x * stepSize)
                .Select(x => Vector3.Lerp(originVector, endVector, x))
                .Select(x => new QuarkNode(x, null))
                .ToList();

        }
        
    }
}
