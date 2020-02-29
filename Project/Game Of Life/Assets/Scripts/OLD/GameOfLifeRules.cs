using System.Collections.Generic;
using UnityEngine;

public enum State
{
    DEAD = 0,
    ALIVE = 1
}

public static class Extensions
{
    public static Color ToColor(this State state)
    {
        if (state == State.ALIVE) return Color.white;
        else return Color.black;
    }
}

public class GameOfLifeRules
{
    private List<int> aliveFromAlive = new List<int>();
    private List<int> aliveFromDead = new List<int>();

    public GameOfLifeRules(string rules)
    {
        bool countAlive = true;
        foreach(char c in rules)
        {
            if (c == '/') countAlive = false;
            else
            {
                int val = int.Parse(c.ToString());
                if (countAlive) aliveFromAlive.Add(val);
                else aliveFromDead.Add(val);
            }
        }
        Debug.LogFormat("Game Of Life rules: alive [{0}], dead [{1}]", string.Join(", ", aliveFromAlive), string.Join(", ", aliveFromDead));
    }

    public State GetDecision(int count, State currentState)
    {
        if(currentState == State.ALIVE)
        {
            if (aliveFromAlive.Contains(count)) return State.ALIVE;
            else return State.DEAD;
        } 
        else
        {
            if (aliveFromDead.Contains(count)) return State.ALIVE;
            else return State.DEAD;
        }
    }

}
