using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using UnityEngine;

public class IngredientSpawner : Singleton <IngredientSpawner>
{

    public Transform[] spawns;
    private List<SpawnPoint> spawnPoints;
    public int extraIngredients;

    private List<GameObject> currentIngredients;

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
        spawns.ForEach(s => spawnPoints.Add(new SpawnPoint(s)));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //SpawnIngredientsForPotion(IngredientMixer.MixPotion());
        }
    }

    public void SpawnIngredientsForPotion(Potion pot)
    {
        currentIngredients.ForEach(Destroy);
        pot.ingredients.ForEach(SpawnIngredientRandom);
        List<Potion.Ingredient> availableIngredients = Potion.GetAllIngredients();
        availableIngredients = availableIngredients.Except(pot.ingredients).ToList();
        for (int i = 0; i < extraIngredients; i++)
        {
            SpawnIngredientRandom(RandomHelper.Get(availableIngredients));
        }
    }

    void SpawnIngredientRandom(Potion.Ingredient ingr)
    {
        GameObject prefab = (GameObject)Resources.Load("prefabs/" + Enum.GetName(typeof(Potion.Ingredient), ingr), typeof(GameObject));
        Transform s = GetRandomSpawn();
        currentIngredients.Add(Instantiate(prefab, s.position, new Quaternion(), this.transform));
    }

    Transform GetRandomSpawn()
    {
        List<SpawnPoint> availableSpawns = spawnPoints.Where(s => !s.IsUsed()).ToList();
        return RandomHelper.Get(availableSpawns).spawn;
    }
    
}
