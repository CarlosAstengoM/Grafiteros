using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SimulationParams
{
    public SimulationParams(int inBoxes, int outBoxes, int numSteps)
    {
        in_boxes = inBoxes;
        out_boxes = outBoxes;
        num_steps = numSteps;
    }

    public int in_boxes;
    public int out_boxes;
    public int num_steps;
}

public class SimulationParameters : MonoBehaviour
{
    public static SimulationParameters Instance {get; private set;}


    [Header("Unity Setup")]
    [field: SerializeField] public GameObject BoxPrefab;
    public float TurnTime = 5.0f;
    
    [Header("Python Setup")]
    //Amount of time it takes to finish one turn
    public int TotalSimulatedSteps = 100;
    public int InBoxesPerMinute = 2;
    public int OutBoxesPerMinute = 60;
    public int ChargeUpAmount = 10;
    [field: SerializeField] public int MoveEnergyConsumption { get; private set; }
    [field: SerializeField] public int PickDropEnergyConsumption { get; private set; }
    [field: SerializeField] public string ServerURL { get; private set; }

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

    public SimulationParams GetParameters()
    {
        return new SimulationParams(InBoxesPerMinute, OutBoxesPerMinute, TotalSimulatedSteps);
    }
}
