using UnityEngine;
using Unity.Mathematics;
using System;


public class Cell : MonoBehaviour
{
    //[HideInInspector]
    public bool isOnEdge { get; set; } = false;
    //[HideInInspector]
    public Cell[] neighbourList { get; set; } = null;
    //[HideInInspector]
    public State m_state { get; set; }
    private State m_nextState { get; set; }


    private int2 myPosition;
    private Transform myTransform;
    private SpriteRenderer mySpriteRenderer { get; set; }

    private float nextColorChangeTime;

    private System.Random random;

    void Start()
    {
        myTransform = this.GetComponent<Transform>();
        myPosition.x = (int)math.ceil(myTransform.position.x);
        myPosition.y = (int)math.ceil(myTransform.position.y);
        if ((float)myPosition.x != myTransform.position.x || (float)myPosition.y != myTransform.position.y)
        {
            myTransform.position = new Vector3((float)myPosition.x, (float)myPosition.y, myTransform.position.z);
            Debug.LogWarningFormat("Cell position ({0}, {1}) is not an integer", myTransform.position.x, myTransform.position.y);
        }

        mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = new Color { r = 0.0f, g = 0.0f, b = 0.0f, a = 0.0f };

        nextColorChangeTime = Time.time + 1.0f;

        random = new System.Random(Utils.RandomInt(0, 100000));
    }

    public void PararelUpdatePrep()
    {
        if (isOnEdge)
        {
            m_nextState = m_state;
        }
        else
        {

            int count = 0;
            foreach (Cell otherCell in this.neighbourList)
            {
                if (otherCell.m_state == State.ALIVE)
                {
                    count++;
                }
            }

            m_nextState = World.gameOfLife.GetDecision(count, m_state);
        }
    }

    public void SequentialUpdateFinal()
    {
        m_state = m_nextState;
        mySpriteRenderer.color = m_state.ToColor();
       
    }

    public void SetState(State state)
    {
        m_state = state;
        mySpriteRenderer.color = m_state.ToColor();
    }
}




    //public void SequentialUpdate()
    //{
    //    if (Time.time > nextColorChangeTime && isOnEdge)
    //    {
    //        nextColorChangeTime = Time.time + Utils.RandomFloat(random, 0.5f, 2.0f);
    //        float red = Utils.RandomFloat(0.0f, 1.0f);
    //        float blue = Utils.RandomFloat(0.0f, 1.0f);
    //        float green = Utils.RandomFloat(0.0f, 1.0f);
    //        float sum = red + blue + green;
    //        mySpriteRenderer.color = new Color { r = red / sum, g = green / sum, b = blue / sum, a = 1.0f };
    //    }
    //    else if (Time.time > nextColorChangeTime && !isOnEdge)
    //    {
    //        nextColorChangeTime = Time.time - Utils.RandomFloat(0.5f, 1.0f);
    //        Color color = new Color();

    //        float count = 0.0f;
    //        foreach (Cell otherCell in this.neighbourList)
    //        {
    //            color += otherCell.mySpriteRenderer.color;
    //            count += otherCell.mySpriteRenderer.color.a;
    //        }
    //        if (count > 0.0f)
    //            mySpriteRenderer.color = color / count;
    //    }
    //}