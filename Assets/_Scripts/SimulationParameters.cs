using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationParameters : MonoBehaviour
{
    public static SimulationParameters Instance {get; private set;}

    //Amount of time it takes to finish one turn
    public float TurnTime = 5.0f;

    private void Awake()
    {
        if(Instance != null)
        {
            UnityEngine.Debug.LogError("More than one Simulation Parameters singleton detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
