using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using static QuarkFramework.Helpers;
using static QuarkFramework.EnumHelper;

namespace QuarkFramework.Tests.SystemsTests {
    public class DirectionChangeSystemTest {

        [Test]
        public void IntegrationTest() {
            var expectedChanges = new Vector3(313, 401, 1234);
            //Mock NodeCreatedStream
            Subject<QuarkNode> nodeStream = new Subject<QuarkNode>();
            DirectionChangeSystemV0 dcs = MockDcs(nodeStream);

            //Subscribe to direction change stream, add direction changes to changesPerAxis Vector
            var changesPerAxis = Vector3.zero;
            dcs.directionChangeStream.Subscribe(x => changesPerAxis += changesPerAxis.IncrementByAxis(x.axis, 1));

            //Emit mock nodes in the node created stream
            GetMockNodes(expectedChanges).ForEach(nodeStream.OnNext);

            expectedChanges.AsList().ForEach(x => Assert.AreEqual(expectedChanges.ByAxis(x.axis), changesPerAxis.ByAxis(x.axis)));
        }

        //Mock direction change system
        private DirectionChangeSystemV0 MockDcs(Subject<QuarkNode> nodeStream) {
            DirectionChangeSystemV0 dcs = new GameObject().AddComponent<DirectionChangeSystemV0>();
            dcs.Awake();
            dcs.CreateDirectionChangeSystem(nodeStream);
            return dcs;
        }
        
        //Generate N Direction changes
        //Param changes should have expected amount of changes for each axis
        private List<QuarkNode> GetMockNodes(Vector3 changes) {
            return axises
                .Select(x => GetMockNodesForAxis(x, (int) changes.ByAxis(x)))
                .SelectMany(x => x)
                .ToList();
        }

        private List<QuarkNode> GetMockNodesForAxis(Axis axis, int changes) {
            if (changes == 0)
                return new List<QuarkNode>();

            var currNodes = new Pair<Vector3>(Vector3.zero, Vector3.zero);

            List<QuarkNode> dirchangeNodesX = new List<QuarkNode>();
            dirchangeNodesX.Add(new QuarkNode(currNodes.Previous, null));
            dirchangeNodesX.Add(new QuarkNode(currNodes.Current, null));

            for (int i = 0; i <= changes; i++) {
                var nextNode = GetNextNodeForChange(currNodes, axis);
                dirchangeNodesX.Add(nextNode);
                currNodes = new Pair<Vector3>(currNodes.Current, nextNode.Position);
            }

            return dirchangeNodesX;
        }

        private QuarkNode GetNextNodeForChange(Pair<Vector3> prevNodes, Axis axis) {
            var incrementing = prevNodes.Previous.GetAxis(axis) <= prevNodes.Current.GetAxis(axis);
            Vector3 newPos = prevNodes.Current + GetChangeVector(incrementing, axis);
            return new QuarkNode(newPos, null);
        }

        private Vector3 GetChangeVector(bool incrementing, Axis axis) {
            var d = incrementing ? -1 : 1;
            return Vector3.zero.IncrementByAxis(axis, d);
        }

    }

}