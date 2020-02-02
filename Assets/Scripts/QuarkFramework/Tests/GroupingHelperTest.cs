using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

public class GroupingHelperTest {
    [Test]
    public void TestGroupByZ() {
        var toGroup = new List<Vector3>() {
            new Vector3(0, 0, 25),
            new Vector3(0, 0, 27),
            new Vector3(0, 0, 40),
            new Vector3(0, 0, 21),
            new Vector3(0, 0, 34),
            new Vector3(0, 0, 49),
            new Vector3(0, 0, 30),
            new Vector3(0, 0, 1)
        };
        
        toGroup.GroupByZ(10).ForEach(x => {
            Debug.Log("----");
            x.ForEach(y => Debug.Log(y.z));
        });

    }
    
    [Test]
    public void GroupOptimizerTest() {
        var toGroup = new List<Vector3>() {
            new Vector3(0, 0, 25),
            new Vector3(0, 0, 27),
            new Vector3(0, 0, 40),
            new Vector3(0, 0, 21),
            new Vector3(0, 0, 34),
            new Vector3(0, 0, 49),
            new Vector3(0, 0, 30),
            new Vector3(0, 0, 1)
        };
        
        toGroup.GroupByZ(10).ForEach(x => {
            Debug.Log("----");
            x.ForEach(y => Debug.Log(y.z));
        });
        
        

    }
}
