using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Cauldron : MonoBehaviour {
    private List<Potion.Ingredient> currentIngredients;
    private Potion currentPotion = null;
    private int handsInCauldron = 0;

    private void Awake()
    {
        currentIngredients = new List<Potion.Ingredient>();
    }

    private void Start()
    {
        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.M)).Subscribe(_ => MixCauldron());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            HardcodeCure();
    }

    public void MixCauldron() {
        var potion = IngredientMixer.MixPotion(currentIngredients);

        if (potion == null) {
            MixFailed();
            currentIngredients = new List<Potion.Ingredient>();
        }
        else {
            currentIngredients = new List<Potion.Ingredient>();
            PotionCreated(potion);
        }
    }


    void HardcodeCure()
    {
        currentIngredients = new List<Potion.Ingredient>();
        GameManager.Instance.currentPatient.TryCurePlayer(currentPotion, Problem.SpawnLocation.Brain);
    }

    private void MixFailed() {
        currentPotion = null;
    }

    public void AddIngredient(Potion.Ingredient i) {
        currentIngredients.Add(i);
        if (currentIngredients.Count == 2)
            Invoke("HardcodeCure", 2f);
    }

    private void PotionCreated(Potion potion) {
        currentPotion = potion;
    }
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerHand")) {
            Debug.Log("Hand in cauldron enter!");
            handsInCauldron++;
        }
        
        if (handsInCauldron == 2)
            GameManager.Instance.potionInHand = currentPotion;
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayerHand")) {
            handsInCauldron--;
            Debug.Log("Hand in cauldron exit!");
        }
    }
    
}
