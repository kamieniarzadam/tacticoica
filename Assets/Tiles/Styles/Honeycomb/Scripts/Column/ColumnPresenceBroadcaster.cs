using UnityEngine;

[ExecuteInEditMode]
public class ColumnPresenceBroadcaster : MonoBehaviour
    , HierarchyMsg<ColumnPresenceUpdate, bool>.IResponder
    , HierarchyMsg<ColumnPresenceCheck, bool>.IResponder
{
    public GameObject tilemapOfColumns;

    bool HierarchyMsg<ColumnPresenceUpdate, bool>.IResponder.Respond(ColumnPresenceUpdate request)
    {
        HierarchyMsg<ColumnPresenceCheck, bool>.Publish(request.present, new ColumnPresenceCheck { location = request.location }, gameObject);
        return false;
    }

    bool HierarchyMsg<ColumnPresenceCheck, bool>.IResponder.Respond(ColumnPresenceCheck request)
    {
        return tilemapOfColumns.GetComponent<UnityEngine.Tilemaps.Tilemap>().HasTile(request.location);
    }

}