using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoLStructureButton : MonoBehaviour
{

    private GoLStructure m_struct { get; set; }
    private Button m_button { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Build(GoLStructure GoLStructure)
    {
        this.m_struct = GoLStructure;
        this.m_button = GetComponent<Button>();
        Text text = GetComponentInChildren<Text>();
        text.text = m_struct.name;
    }
    
    public void OnClick()
    {
        print(m_struct.name);
    }
}
