using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using QuarkFramework;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Assert = UnityEngine.Assertions.Assert;

public class PoseWatcherTest : MonoBehaviour {
    
    [Test]
    public void Test() {
        PoseWatcher watcher = new GameObject().AddComponent<PoseWatcher>();
        Transform t = new GameObject().GetComponent<Transform>();
        var fakeUpdateLoop = new Subject<long>();
        watcher.CreateUpdateLoop(fakeUpdateLoop);
        watcher.CreateWatcher(t, new Vector3(2,0,1.5f));

        var watcherDestroyed = false;
        watcher.outOfPoseStream.Subscribe(x => watcherDestroyed = true);

        
        UpdateAndAssert(false, ref watcherDestroyed, fakeUpdateLoop);
        
        t.position += new Vector3(0,0,1);
        UpdateAndAssert(false, ref watcherDestroyed, fakeUpdateLoop);
        
        t.position += new Vector3(1.9f,0,0);
        UpdateAndAssert(false, ref watcherDestroyed, fakeUpdateLoop);
        
        t.position += new Vector3(0,100,0);
        UpdateAndAssert(false, ref watcherDestroyed, fakeUpdateLoop);
        
        t.position += new Vector3(0,100,0.6f);
        UpdateAndAssert(true, ref watcherDestroyed, fakeUpdateLoop);
    }

    private void UpdateAndAssert(bool expected, ref bool actual, Subject<long> fakeUpdateLoop) {
        fakeUpdateLoop.OnNext(0l);
        Assert.AreEqual(expected, actual);
    }
    
}
