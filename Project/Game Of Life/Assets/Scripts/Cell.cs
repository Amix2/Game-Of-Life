using UnityEngine;
using Unity.Mathematics;
using System;


public class Cell : MonoBehaviour
{
    public bool isOnEdge = false;
    public Cell[] neighbourList = null;

    private int2 myPosition;
    private Transform myTransform;
    private SpriteRenderer mySpriteRenderer { get; set; }
    static private System.Random random = new System.Random();

    private float nextColorChangeTime;
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

        nextColorChangeTime = Time.time + UnityEngine.Random.Range(0.5f, 2.0f); ;
    }

    /// <summary>
    /// ///////////////////////////
    /// </summary>
    // Update is called once per frame
    //void Update()
    //{
    //    mySpriteRenderer.color = nextColor;
    //}
///////////
    public void mUpdate()
    {
        if (Time.time > nextColorChangeTime && isOnEdge)
        {
            nextColorChangeTime = Time.time + UnityEngine.Random.Range(0.5f, 2.0f) + 6;
            float red = UnityEngine.Random.Range(0.0f, 1.0f);
            float blue = UnityEngine.Random.Range(0.0f, 1.0f);
            float green = UnityEngine.Random.Range(0.0f, 1.0f);
            float sum = red + blue + green;
            mySpriteRenderer.color = new Color { r = red / sum, g = green / sum, b = blue / sum, a = 1.0f };
        }
        else if(Time.time > nextColorChangeTime && !isOnEdge)
        {
            nextColorChangeTime = Time.time - UnityEngine.Random.Range(0.5f, 1.0f);
            Color color = new Color();
            mySpriteRenderer.color = color;
            float count = 0.0f;
            foreach(Cell otherCell in this.neighbourList)
            {
                color += otherCell.mySpriteRenderer.color;
                count += otherCell.mySpriteRenderer.color.a;
            }
            if(count > 0.0f)
                mySpriteRenderer.color = color / count;//
        }
    }
    /////////////////////////////
    //public void PararelUpdate(float time)
    //{
    //    if (time > nextColorChangeTime && !isOnEdge)
    //    {
    //        nextColorChangeTime = time + (float)random.NextDouble() * 2 + 0.5f;
    //        nextColor = new Color { r = (float)random.NextDouble(), g = (float)random.NextDouble(), b = (float)random.NextDouble(), a = 1.0f };
    //    }
    //}
}
