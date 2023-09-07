
using UnityEngine;

[ExecuteInEditMode]
public class ColumnDestructor : MonoBehaviour
    , HierarchyMsg<WallPresenceCheck, bool>.IRequestor
{
    Vector3Int requestedWall;
    bool anyWallPresent;

    public void SetAdjacent(HexagonNeighboring.Relation wallFacing, bool present)
    {
        if (!present)
        {
            var loc = GetComponent<LocationOnTilemapHelper>().Location;
            var walls = HoneycombGridsCalculator.GetWallsLocationsTerminatedByColumn(loc);
            anyWallPresent = false;
            foreach (var wall in walls)
            {
                if (HoneycombGridsCalculator.CalculateWallFacing(wall) != wallFacing)
                {
                    requestedWall = wall;
                    HierarchyMsg<WallPresenceCheck, bool>.Request(this);
                }
            }
            if(!anyWallPresent)
                GetComponent<LocationOnTilemapHelper>().GetTilemap().SetTile(GetComponent<LocationOnTilemapHelper>().Location, null);
        }
    }

    WallPresenceCheck HierarchyMsg<WallPresenceCheck, bool>.IRequestor.FormRequest()
    {
        return new WallPresenceCheck { location = requestedWall };
    }

    void HierarchyMsg<WallPresenceCheck, bool>.IRequestor.Handle(WallPresenceCheck request, bool response)
    {
        anyWallPresent |= response;
    }
}