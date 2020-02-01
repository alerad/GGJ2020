using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Potion", order = 1)]
public class Potion : ScriptableObject {
    public List<Ingredient> ingredients;

    public enum Ingredient {
        Radish,
        Ginger,
        Rosemary,
        Coconut,
        Mushroom,
        Lemon,
        Strawberry,
        Scarab
    }
}


