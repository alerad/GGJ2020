using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using UnityEngine;

public class IngredientSpawner : Singleton<IngredientSpawner>
{

    public Transform[] spawns;
    private List<SpawnPoint> spawnPoints;
    public int extraIngredients;

    private List<GameObject> currentIngredients;

    private int currentExtras = 0;

    private Transform ingrParent;
    
    class SpawnPoint
    {
        public Transform spawn;
        public Potion pot;

        public SpawnPoint(Transform s)
        {
            this.spawn = s;
        }

        public bool IsUsed()
        {
            return pot != null;
        }
    }

    private void Awake()
    {
        spawnPoints = new List<SpawnPoint>();
        currentIngredients = new List<GameObject>();
        if (spawns == null) {
            Debug.Log("No spawns set!");
            return;
        }
        spawns.ForEach(s => spawnPoints.Add(new SpawnPoint(s)));
        ingrParent = new GameObject("ingredient-parent").transform;
        currentExtras = 0;
        Invoke("MoreExtras", 120f);
    }
    
    //HORRIBLE, ToDo FIX?????

    void MoreExtras()
    {
        extraIngredients++;
        Invoke("MoreExtras", 120f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //SpawnIngredientsForPotion(IngredientMixer.MixPotion());
        }
    }

    public void ClearRemaining()
    {
        currentExtras = 0;
        currentIngredients.ForEach(Destroy);
    }


    public void SpawnIngredientsForPotion(Potion pot)
    {
        pot.ingredients.ForEach(SpawnIngredientInRandomPos);

        //ToDo sacar esto afuera de aca y solo spawnear la cant. fija definida 1 vez teniendo en cuenta los available current
        if (currentExtras < extraIngredients)
        {
            
            List<Potion.Ingredient> availableIngredients = Potion.GetAllIngredients();
            availableIngredients = availableIngredients.Except(pot.ingredients).ToList();
            for (int i = 0; i < extraIngredients; i++)
            {
                SpawnIngredientInRandomPos(RandomHelper.Get(availableIngredients));
                currentExtras++;
            }
        }
    }

    void SpawnIngredientInRandomPos(Potion.Ingredient ingr)
    {
        Transform s = GetRandomSpawn();
        if (s == null)
            return;
        
        var ingrName = Enum.GetName(typeof(Potion.Ingredient), ingr);
        GameObject prefab = (GameObject) Resources.Load("prefabs/" + ingrName, typeof(GameObject));
        var ing = Instantiate(prefab, s.position, new Quaternion(), ingrParent);
        ing.name = ingrName;
        currentIngredients.Add(ing);
    }

    Transform GetRandomSpawn()
    {
        List<SpawnPoint> availableSpawns = spawnPoints.Where(s => !s.IsUsed()).ToList();
        if (availableSpawns == null || availableSpawns.Count == 0)
            return null;
        return RandomHelper.Get(availableSpawns).spawn;
    }
    
}
