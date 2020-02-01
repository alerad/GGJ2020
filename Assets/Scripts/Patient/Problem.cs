using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Problem", order = 1)]
public class Problem : ScriptableObject {
    public List<Potion> potions;
    public String name;
    public Texture icon;
    public float credibilityRestoration;
    public SpawnLocation spawnLocation;
    public GameManager.Difficulty difficulty;
    
    public enum SpawnLocation {
        Intestines,
        Head,
        Hands
    }
    
}
