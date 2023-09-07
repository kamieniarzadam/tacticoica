
using UnityEngine;
using System;

[ExecuteInEditMode]
public class ColumnShifter : MonoBehaviour
{
    public void Start()
    {
        Vector3Int location = GetComponentInParent<LocationOnTilemapHelper>().Location;
        transform.position += new Vector3(0, 0, (HoneycombGridsCalculator.IsColumnShiftNorth(location)?-1:1) * 0.133333f);
    }
}