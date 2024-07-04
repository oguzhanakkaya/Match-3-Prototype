using Match3System.Core.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDetectors 
{
    public GridPoint[] lineDirections;

    public LineDetectors(GridPoint[] lineDirections)
    {
        this.lineDirections = lineDirections;
    }
}
