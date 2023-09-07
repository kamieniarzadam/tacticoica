
using UnityEngine;
using System;

[ExecuteInEditMode]
public class FieldCrammer : MonoBehaviour
    , HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor
{
    public GameObject crammed;
    public Vector3Int columnLoc;

    public UnityEngine.Events.UnityEvent OnCram;
    public UnityEngine.Events.UnityEvent OnUncram;

    private void Awake()
    {
        var loc = gameObject.GetComponent<LocationOnTilemapHelper>().Location;
        if (loc.x % 2 != 0)
        {
            var pos = crammed.transform.localPosition;
            pos.x = -0.4f;
            crammed.transform.SetLocalPositionAndRotation(pos, crammed.transform.rotation);
        }

        var intersectingWalls = HoneycombGridsCalculator.GetWallsLocationsIntersectingWithField(loc);
        foreach (Enum relation in new System.Collections.Generic.List<Enum> { HexagonAxialNeighboring.Relation.SouthEast, HexagonAxialNeighboring.Relation.SouthWest, HexagonAxialNeighboring.Relation.NorthWest, HexagonAxialNeighboring.Relation.NorthEast})
        {
            Vector3Int wallLoc;
            if (intersectingWalls.TryGetValue(relation, out wallLoc))
            {
                if(HoneycombGridsCalculator.CalculateWallFacing(wallLoc) == HexagonNeighboring.Relation.East) 
                {
                    var columns = HoneycombGridsCalculator.GetWallTerminatingColumnsPosition(wallLoc);
                    columnLoc = Convert.ToInt32(relation) > Convert.ToInt32(HexagonNeighboring.Relation.West) ? columns[0] : columns[1];
                    HierarchyMsg<ColumnPresenceCheck, bool>.Request(this);
                    break;
                }
            }
        }
    }

    public void SetCrammed(bool c)
    {
        if (c)
        {
            OnCram.Invoke();
        }
        else
        {
            OnUncram.Invoke();
        }
    }

    ColumnPresenceCheck HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor.FormRequest()
    {        
        return new ColumnPresenceCheck { location = columnLoc };
    }

    void HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor.Handle(ColumnPresenceCheck request, bool response) => SetCrammed(response);
}