using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomHelper {
    public static bool Over100(int chance) {
        int num = Random.Range(0, 100);
        return num <= chance;
    }

    public static T Get<T>(T[] items) {
        int num = Random.Range(0, items.Length);
        return items[num];
    }

    public static T Get<T>(T[] items, T avoidElement) {
        int num = Random.Range(0, items.Length);
        if (items[num].Equals(avoidElement)) {
            return Get(items, avoidElement);
        }

        return items[num];
    }

    public static T Get<T>(List<T> items) {
        int num = Random.Range(0, items.Count);
        return items[num];
    }

    public static List<T> RandomizeList<T>(List<T> songs) => songs
        .OrderBy(x => Guid.NewGuid())
        .ToList();
    
    public static bool Bool() {
        return Random.Range(0, 1) == 1;
    }

    //From 0 to num
    public static int RandomNatural(int num) {
        return (int) Random.Range(0, num);
    }

    public static Color RandomColor(Color prev) {
        var random = new Color(Random.Range(0, 1.01f), Random.Range(0, 1.01f), Random.Range(0, 1.01f));
        if (prev != null && random == prev) {
            return RandomColor(prev);
        }

        return random;
    }


    public static Vector3 SignVector3(float val, bool positive = true) {
        return SignVector3(val, val, val, positive);
    }

    public static Vector3 SignVector3(float x, float y, float z, bool positive = true) {
        return positive
            ? new Vector3(Random.Range(0, x), Random.Range(0, y), Random.Range(0, z))
            : -new Vector3(Random.Range(0, x), Random.Range(0, y), Random.Range(0, z));
    }

    public static Vector3 Vector3(float minMax) {
        return new Vector3(Random.Range(-minMax, minMax), Random.Range(-minMax, minMax), Random.Range(-minMax, minMax));
    }

    public static Vector3 Vector3(float minMaxX, float minMaxY, float minMaxZ) {
        return new Vector3(Random.Range(-minMaxX, minMaxX), Random.Range(-minMaxY, minMaxY),
            Random.Range(-minMaxZ, minMaxZ));
    }

    public static Vector3 Vector3(Vector3 vector3) {
        return Vector3(vector3.x, vector3.y, vector3.z);
    }
}