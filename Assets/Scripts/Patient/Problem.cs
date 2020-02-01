using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : ScriptableObject {
    public List<Potion> potions;
    public String name;
    public Texture icon;
    public float credibilityRestoration;
    public SpawnLocation spawnLocation;
    
    public enum SpawnLocation {
        Intestines,
        Head,
        Hands
    }
    
}
