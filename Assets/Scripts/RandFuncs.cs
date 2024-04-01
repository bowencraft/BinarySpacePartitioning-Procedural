using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandFuncs {


    public static T randPick<T>(T[] deck) {
        int randomIndex = Random.Range(0, deck.Length);
        return deck[randomIndex];
    }

    public static T randPick<T>(List<T> deck) {
        int randomIndex = Random.Range(0, deck.Count);
        return deck[randomIndex];
    }

    public static void shuffle<T>(T[] deck) {
        for (int i = deck.Length - 1; i >= 1; i--) {
            int randomIndex = Random.Range(0, i + 1);
            T swapTemp = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = swapTemp;
        }
    }

    public static void shuffle<T>(List<T> deck) {
        for (int i = deck.Count - 1; i >= 1; i--) {
            int randomIndex = Random.Range(0, i + 1);
            T swapTemp = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = swapTemp;
        }
    }
}
