using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IngredientMixer {
    public static Potion MixPotion(List<Potion.Ingredient> ingredients) {
        var matchingPotions = ScriptableObjectContainer.Instance.potions
            .Where(x => x.ingredients.OrderBy(ing => (int)ing).SequenceEqual(ingredients.OrderBy(z => (int)z))) //Devuelve todas las pociones donde los ingredientes matcheen
            .ToList();

        if (matchingPotions.Count > 1)
            throw new Exception("Che hay mas de una pocion para estos ingredientes" +
                                ", seguro hiciste 2 scriptable objects mal chinguenguencha");

        return matchingPotions.Count > 0 ? matchingPotions.First() : null;
    }

    public static Potion.Ingredient GetIngredientByName(string name) {
        Enum.TryParse(name, out Potion.Ingredient ing);
        return ing;
    }

    
}
