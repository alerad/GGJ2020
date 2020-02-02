using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Potion", order = 1)]
public class Potion : ScriptableObject {
    public List<Ingredient> ingredients;


    public static List<Ingredient> GetAllIngredients()
    {
        //ToDo remove butter
        return Enum.GetValues(typeof(Ingredient)).Cast<Ingredient>().ToList();
    }
    
    public enum Ingredient {
        Radish,
        Ginger,
        Rosemary,
        Coconut,
        Amanita,
        Lemon,
        Strawberry,
        Scarab
        //Butter 
    }
}


