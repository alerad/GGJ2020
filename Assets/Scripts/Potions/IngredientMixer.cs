using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientMixer : MonoBehaviour {
    public List<Potion> potions;

    public Potion MixPotion(List<Potion.Ingredient> ingredients) {
        var matchingPotions = potions
            .Where(x => x.ingredients.All(ingredients.Contains)) //Devuelve todas las pociones donde los ingredientes matcheen
            .ToList();

        if (matchingPotions.Count > 1)
            throw new Exception("Che hay mas de una pocion para estos ingredientes" +
                                ", seguro hiciste 2 scriptable objects mal chinguenguencha");

        return matchingPotions.First();
    }

    
}
