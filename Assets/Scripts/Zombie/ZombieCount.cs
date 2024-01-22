using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCount
{
    private static int zobmbieCount = 0;

    public static void Decrease(int value)
    {
        zobmbieCount -= value;

        if (zobmbieCount < 0) zobmbieCount = 0;

        Debug.Log($"Zombies left : {ZombieCount.zobmbieCount}");
    }
    
    public static void Increase(int value) 
    { 
        zobmbieCount += value; 

        Debug.Log($"Zombies added : {ZombieCount.zobmbieCount}");
    }

    public static int Count() => zobmbieCount;
}
