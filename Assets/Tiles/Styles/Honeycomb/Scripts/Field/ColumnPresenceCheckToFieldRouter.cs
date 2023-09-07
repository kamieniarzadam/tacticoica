using UnityEngine;

[ExecuteInEditMode]
public class ColumnPresenceCheckToFieldRouter : MonoBehaviour
    , HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor
{
    ColumnPresenceCheck HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor.FormRequest()
    {
        return new ColumnPresenceCheck { location = GetComponent<LocationOnTilemapHelper>().Location };
    }

    void HierarchyMsg<ColumnPresenceCheck, bool>.IRequestor.Handle(ColumnPresenceCheck request, bool response)
    {
        Vector3Int fieldLoc = new(2 * request.location.y, 3 * request.location.x + 1 - System.Math.DivRem(request.location.x, 3, out _) - request.location.x % 3, request.location.z);
        if (request.location.x < 0 && request.location.y % 2 == 0)
        {
            fieldLoc.y -= 2;
        }
        if (request.location.x > 0 && request.location.x % 3 != 0 && request.location.y % 2 != 0)
        {
            fieldLoc.y += 2;
        }
        var tilemap = GetComponent<UnityEngine.Tilemaps.Tilemap>();
        var fieldGO = tilemap.GetInstantiatedObject(fieldLoc);
        if (fieldGO)
            fieldGO.GetComponent<FieldCrammer>().SetCrammed(response);
        fieldLoc.x--;
        fieldGO = tilemap.GetInstantiatedObject(fieldLoc);
        if (fieldGO)
            fieldGO.GetComponent<FieldCrammer>().SetCrammed(response);
    }
}