using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptableObjectContainer : Singleton<ScriptableObjectContainer> {
    [NonSerialized]
    public List<Potion> potions;
    [NonSerialized]
    public List<Problem> problems;
    private void Awake() {
        potions = Resources.LoadAll<Potion>("ScriptableObjects/Potions").ToList();
        problems = Resources.LoadAll<Problem>("ScriptableObjects/Problems").ToList();
    }
}
