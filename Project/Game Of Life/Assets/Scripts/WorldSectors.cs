using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WorldSectors : MonoBehaviour
{
    private static readonly float RANDOM_INIT_ALIVE_CHANCE = 0.050f;

    public Slider slider;
    //public string gameRules = "23/3";
    public ComputeShader shader;

    private readonly int m_SizeX = 512;
    private readonly int m_SizeY = 512;
    private readonly int m_unit = 2;
    private int m_TextureSizeX;
    private int m_TextureSizeY;
    SpriteRenderer m_renderer;
    Texture2D m_Texture;
    RenderTexture m_renderTexture;
    private Vector2Int m_lastClickPos;

    int[,] m_State;
    Color[] m_ColorState;

   // static public GameOfLifeRules gameOfLife;
    // Start is called before the first frame update
    void Start()
    {
        m_TextureSizeX = m_SizeX * m_unit;
        m_TextureSizeY = m_SizeY * m_unit;

        m_ColorState = new Color[m_TextureSizeX * m_TextureSizeY];
        m_State = new int[m_SizeX, m_SizeY];
        for (int i = 0; i < m_SizeX; i++)
        {
            for (int j = 0; j < m_SizeY; j++)
            {
                m_State[i, j] = UnityEngine.Random.value < RANDOM_INIT_ALIVE_CHANCE ? 1 : 0;
                
            }
        }


        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        //boxCollider.offset = new Vector2(m_SizeX / 2.0f, m_SizeY / 2.0f);
        boxCollider.size = new Vector2(m_SizeX, m_SizeY);

        //gameOfLife = new GameOfLifeRules(gameRules);

        m_renderTexture = new RenderTexture(m_SizeX * m_unit, m_SizeY * m_unit, 1);
        m_renderTexture.enableRandomWrite = true;
        m_renderTexture.Create();

        m_Texture = new Texture2D(m_SizeX* m_unit, m_SizeY* m_unit, TextureFormat.RGBA32, false);

        for (int i = 0; i < m_SizeX; i++)
        {
            for (int j = 0; j < m_SizeY; j++)
            {
                InsertColorLocal(StateToColor(m_State[i, j]), i, j);

            }
        }

        m_Texture.SetPixels(m_ColorState);

        m_Texture.Apply();

        m_renderer = (SpriteRenderer)gameObject.AddComponent<SpriteRenderer>();
        m_renderer.sprite = Sprite.Create(m_Texture, new Rect(0, 0, m_TextureSizeX, m_TextureSizeY), new Vector2(0.0f, 0.0f));
        transform.localScale = new Vector3(100.0f, 100.0f, 1.0f);


        //Shader.SetGlobalTexture(Shader.PropertyToID("Result"), m_Texture);
        shader.SetTexture(shader.FindKernel("CSMain"), "inputTexture", m_Texture);
        shader.SetTexture(shader.FindKernel("CSMain"), "outputTexture", m_renderTexture);
        shader.SetInt("u_unit", m_unit);
        shader.SetInt("u_sizeX", m_SizeX);
        shader.SetInt("u_sizeY", m_SizeY);

        RenderTexture.active = m_renderTexture;
        Graphics.Blit(m_Texture, m_renderTexture);
        RenderTexture.active = null;

        Invoke("UpdateGame", this.slider.value);
    }

    private void OnMouseDrag()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int clickPos = new Vector2Int((int)(pz.x / m_unit), (int)(pz.y / m_unit));
        if (clickPos.x < 0 || clickPos.x >= m_SizeX || clickPos.y < 0 || clickPos.y >= m_SizeY) return;
        if (m_lastClickPos != clickPos)
        {
            m_lastClickPos = clickPos;
            InsertColorTexture(Color.white, clickPos.x, clickPos.y);
            m_Texture.Apply();
        }
    }

    void InsertColorLocal(Color color, int x, int y)
    {
        int uX = m_unit * x;
        int uY = m_unit * y;
        for (int iY = 0; iY < m_unit; iY++)
        {
            for (int iX = 0; iX < m_unit; iX++)
            {
                m_ColorState[uX + iX + (uY + iY) * m_TextureSizeX] = color;
            }
        }
    }

    void InsertColorTexture(Color color, int x, int y)
    {
        int uX = m_unit * x;
        int uY = m_unit * y;
        for (int iY = 0; iY < m_unit; iY++)
        {
            for (int iX = 0; iX < m_unit; iX++)
            {
                m_Texture.SetPixel(uX + iX, uY + iY, color);
            }
        }
    }

    Color StateToColor(int state)
    {
        if (state == 0) return Color.black;
        return Color.white;
    }


    // Update is called once per frame
    void UpdateGame()
    {
        Invoke("UpdateGame", this.slider.value);

        shader.Dispatch(shader.FindKernel("CSMain"), m_SizeX / 16, m_SizeY / 16, 1);
        RenderTexture.active = m_renderTexture;
        m_Texture.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0, false);
        m_Texture.Apply();
        RenderTexture.active = null;
    }
}
