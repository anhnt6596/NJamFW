using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/State")]
public class GameState : ScriptableObject
{
    public float energy;

    public void Reset()
    {
        energy = 0;
    }
}
