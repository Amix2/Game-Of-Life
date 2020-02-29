using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    Texture2D m_Texture;
    int unit, width, height, dWidth, dHeight;

    SpriteRenderer m_renderer;
    // Start is called before the first frame update
    void Start()
    {

        // Init test constants
        unit = 2;
        width = 50;
        height = 50;
        dWidth = (width * unit);
        dHeight = (height * unit);

        //Fill texture with test pattern
        // XX
        //  X
        m_Texture = new Texture2D(dWidth, dHeight);
        Color[] cBlue = new Color[unit * unit];
        Color[] cCyan = new Color[unit * unit];
        for (int i = 0; i < unit * unit; i++)
        {
            cBlue[i] = Color.blue;
            cCyan[i] = Color.cyan;
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((i + j) % 2 == 0) //newTexture.SetPixel(i, j, Color.blue);
                    m_Texture.SetPixels(i * unit, j * unit, unit, unit, cBlue);
                else 
                    m_Texture.SetPixels(i * unit, j * unit, unit, unit, cCyan);
            }
        }
        m_Texture.Apply();
        // Set texture in sprite renderer
        m_renderer = (SpriteRenderer)gameObject.AddComponent<SpriteRenderer>();
        m_renderer.sprite = Sprite.Create(m_Texture, new Rect(0, 0, dWidth, dHeight), new Vector2(0.5f, 0.5f));
        transform.localScale = new Vector3(100.0f, 100.0f, 1.0f);
    }

    // Update is called once per frame  
    void Update()
    {
        Color[] cBlue = new Color[unit * unit];
        Color[] cCyan = new Color[unit * unit];
        for (int i = 0; i < unit * unit; i++)
        {
            cBlue[i] = Color.red;
            cCyan[i] = Color.yellow;
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((i + j) % 2 == 0) //newTexture.SetPixel(i, j, Color.blue);
                    m_Texture.SetPixels(i * unit, j * unit, unit, unit, cBlue);
                else
                    m_Texture.SetPixels(i * unit, j * unit, unit, unit, cCyan);
            }
        }
        m_Texture.Apply();
    }
}
