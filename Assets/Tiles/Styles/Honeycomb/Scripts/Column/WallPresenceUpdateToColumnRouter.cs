using UnityEngine;

[ExecuteInEditMode]
public class WallPresenceUpdateToColumnRouter : MonoBehaviour
    , HierarchyMsg<WallPresenceCheck, bool>.IRequestor
{
    public UnityEngine.Tilemaps.TileBase b;

    WallPresenceCheck HierarchyMsg<WallPresenceCheck, bool>.IRequestor.FormRequest()
    {
        return new WallPresenceCheck { location = GetComponent<LocationOnTilemapHelper>().Location };
    }

    private void SetTile(Vector3Int colLoc, HexagonNeighboring.Relation wallFacing, bool present)
    {
        var tm = GetComponent<UnityEngine.Tilemaps.Tilemap>();
        if (tm.GetTile(colLoc)==null)
        {
            tm.SetTile(colLoc, b);
        }
        var go = tm.GetInstantiatedObject(colLoc);
        go.GetComponent<ColumnDestructor>().SetAdjacent(wallFacing, present);
    }

    void HierarchyMsg<WallPresenceCheck, bool>.IRequestor.Handle(WallPresenceCheck request, bool response)
    {
        HexagonNeighboring.Relation wallFacing = HoneycombGridsCalculator.CalculateWallFacing(request.location);
        var columns = HoneycombGridsCalculator.GetWallTerminatingColumnsPosition(request.location);
        SetTile(columns[0], wallFacing, response);
        SetTile(columns[1], wallFacing, response);
    }
}