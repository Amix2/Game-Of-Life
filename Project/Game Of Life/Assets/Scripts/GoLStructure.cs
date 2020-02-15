using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLStructure : IFormattable
{
    public readonly string name;

    public GoLStructure(string name)
    {
        this.name = name;
    }

    public string ToString(string format, IFormatProvider formatProvider = null)
    {
        return name;
    }
}
