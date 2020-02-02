using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Cauldron : MonoBehaviour {
    private List<Potion.Ingredient> currentIngredients;
    private Potion currentPotion = null;
    private int handsInCauldron = 0;

    public void MixCauldron() {
        var potion = IngredientMixer.MixPotion(currentIngredients);
        
        if (potion == null)
            MixFailed();
        else
            PotionCreated(potion);
    }

    private void MixFailed() {
        currentPotion = null;
    }

    private void PotionCreated(Potion potion) {
        currentPotion = potion;
    }
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerHand"))
            handsInCauldron++;
        
        if (handsInCauldron == 2)
            GameManager.Instance.potionInHand = currentPotion;
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayerHand"))
            handsInCauldron--;
    }
    
}
