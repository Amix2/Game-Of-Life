using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoLStructureList : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject listContent;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GoLStructure structure in GoLStructureFileReader.GetStructures())
        {
            GameObject item = Instantiate(itemPrefab);
            item.transform.SetParent(listContent.transform, false);
            GoLStructureButton itemCode = item.GetComponent<GoLStructureButton>();
            itemCode.Build(structure);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
