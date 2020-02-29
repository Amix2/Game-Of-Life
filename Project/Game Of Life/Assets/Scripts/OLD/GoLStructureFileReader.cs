using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLStructureFileReader
{
    public readonly string filePath = "";

    public static IEnumerable<GoLStructure> GetStructures()
    {
        using (System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + @"\config\GoLStructures.txt."))
        {
            string ln;

            while ((ln = file.ReadLine()) != null)
            {
                yield return new GoLStructure(ln);
            }
            file.Close();
        }
    }
}
