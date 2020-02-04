using System;
using UnityEngine;

public class Utils
{

    static private System.Random random = new System.Random();

    static public float RandomFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    static public float RandomFloat(System.Random customRandom, float min, float max)
    {
        return (float)customRandom.NextDouble() * (max - min) + min;
    }

    static public int RandomInt(int min, int max)
    {
        return random.Next(min, max);
    }



    static public float RandomParallelFloat(float min, float max, params float[] values)
    {
        //return (float)random.NextDouble() * (max - min) + min;
        double sum = 1;
        foreach(float value in values)
        {
            sum += Math.Pow(Math.E, Math.Sin(value* 236461)) ;
        }

        double result = 0.0;
        for(int i=0; i<2; i++)
        {
            result += (i % 2 == 0 ? 1 : -1) * (2 * i + 1) * (2 * i + 1) * Math.Sin(2 * Math.PI * (2 * i + 1) * sum);
        }
        float randomNumber = Math.Abs((float)(result * 8 / (Math.PI * Math.PI)));
        //Debug.Log(randomNumber);
        return randomNumber * (max - min) + min;
    }
}
