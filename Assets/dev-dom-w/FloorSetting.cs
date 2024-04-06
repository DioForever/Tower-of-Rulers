using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class FloorSetting : MonoBehaviour 
{
    private FloorSystem floorSystem;
    public float[] SpeedDebuffValues {get; private set;}
    public float[] HealthDebuffValues {get; private set;}
    public float[] StrengthDebuffValues  {get; private set;}
    public float[] ManaDebuffValues  {get; private set;}
    public int Floornumber {get; private set;}

    public FloorSetting()
    {

        SpeedDebuffValues = new float[] {2.0f,0.5f};

        HealthDebuffValues = new float[] {1.25f, 2.0f,0.5f};

        StrengthDebuffValues = new float[] {0.8f,0.5f,0.2f};

        ManaDebuffValues = new float[] {5.0f,0.25f,0.2f};

        Floornumber = floorSystem.floorNumber; 
    }

    

    
    
}