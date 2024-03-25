using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CL_Task 
{
    public Type taskType;
    private UnityEvent[] procedures;
    public float innatePriority;

    public UnityEvent getProcedures(int index)
    {
        return (procedures[index]);
    }
    
    //all procedure available :

    public enum Type
    {
        military,
        engeniering,
        doctor,
        craft,
        tidy
    }
}
