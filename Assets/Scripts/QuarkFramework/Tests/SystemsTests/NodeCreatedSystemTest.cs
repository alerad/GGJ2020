using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using static QuarkFramework.Helpers;
using static QuarkFramework.EnumHelper;

namespace QuarkFramework.Tests.SystemsTests {
    public class NodeCreatedSystemTest {
        
        [Test]
        public void IntegrationTest() {
            var distanceThreshold = 0.4999f; //Esto termina offseteando ala larga con como esta hecho el test, asique con muchos expectedNodes se rompe.. // TODO Fix
            var expectedNodes = 50;
            var distanceBetweenMockNodes = 0.1f;

            var movStream = new Subject<QuarkNode>();
            var ncs = GetNcs(distanceThreshold, movStream);


            List<QuarkNode> mockNodes = LerpNodes(distanceThreshold, expectedNodes, distanceBetweenMockNodes);

            var createdNodes = 0;
            ncs.nodeCreatedStream.Subscribe(_ => createdNodes++);

            
            mockNodes.ForEach(movStream.OnNext);
            
            
            Assert.AreEqual(expectedNodes, createdNodes);
        }


        private float SineGenerator(float time, float freq, float amplitude) =>
            amplitude * Mathf.Cos(Mathf.PI * 2 * (freq) * time);

        private List<QuarkNode> LerpNodes(float distanceThreshold, float expectedNodes, float distanceBetweenNodes) {
            float distanceToCover = distanceThreshold * expectedNodes;
            var originVector = Vector3.zero;
            var endVector = new Vector3(distanceToCover, 0, 0);
            var lerpSteps = expectedNodes * 100;
            
            return Enumerable
                .Range(0, (int)lerpSteps)
                .Select(x => (float)x/lerpSteps)
                .Select(x => Vector3.Lerp(originVector, endVector, x))
                .Select(x => new QuarkNode(x, null))
                .ToList();

        }
        
        private NodeCreatedSystem GetNcs(float distanceThreshold, IObservable<QuarkNode> movementStream) {
            var ncs = new GameObject().AddComponent<NodeCreatedSystem>();
            ncs.Awake();
            ncs.CreateSubscription(movementStream);
            ncs.distanceThreshold = distanceThreshold;
            return ncs;
        }
    }
}