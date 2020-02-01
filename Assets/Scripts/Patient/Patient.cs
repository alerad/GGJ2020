using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Patient : MonoBehaviour {
    public GameManager.Difficulty difficulty;
    private List<Problem> problems;
    public float patienceTime;

    private bool IsPotionOkay(Potion p) {
        var potionOk = problems
            .First(x => x.potions.First(y => y == p));

        return potionOk == null;
    }

}
