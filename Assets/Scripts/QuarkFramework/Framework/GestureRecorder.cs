using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuarkFramework;
using UnityEngine;
using UniRx;

public class GestureRecorder : MonoBehaviour {
  public List<QuarkVRTracer> tracers;
  public List<QuarkGestureHistory> gestureHistories;
  

  private void Awake() {
    gestureHistories = InstantiateGestureHistories();
  }

  private void Start() {
    tracers.ForEach(HandleSubscriptions);
  }

  private void HandleSubscriptions(QuarkVRTracer tracer) {
    
    tracer
      .nodeCreated()
      .Subscribe(x => GetQuarkHistory(x.limb).nodes.Add(x)
      );

    tracer
      .directionChange()
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      .Subscribe(x => GetQuarkHistory(x.limb).directionChanges.Add(x));
    
  }

  private QuarkGestureHistory GetQuarkHistory(EnumHelper.Limb limb) => gestureHistories.Single(x => x.limb == limb);

  private List<QuarkGestureHistory> InstantiateGestureHistories() => tracers.Select(x => new QuarkGestureHistory(x.limb)).ToList();


}

public class QuarkGestureHistory {
  public List<QuarkNode> nodes;
  public List<DirectionChange> directionChanges;
  public List<QuarkSegment> segments;
  public EnumHelper.Limb limb;

  public QuarkGestureHistory(EnumHelper.Limb limb) {
    directionChanges = new List<DirectionChange>();
    segments = new List<QuarkSegment>();
    nodes = new List<QuarkNode>();
    this.limb = limb;
  }
    
}