using System.Collections;
using System.Collections.Generic;
using QuarkFramework;
using UnityEngine;

public class DashData {
    public QuarkSegment segment;
    public bool goingRight;
    
    public DashData(QuarkSegment segment, bool goingRight) {
        this.segment = segment;
        this.goingRight = goingRight;
    }
}
