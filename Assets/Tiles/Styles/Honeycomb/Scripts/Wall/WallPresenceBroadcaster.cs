using UnityEngine;

[ExecuteInEditMode]
public class WallPresenceBroadcaster : MonoBehaviour
    , HierarchyMsg<WallPresenceUpdate, bool>.IResponder
    , HierarchyMsg<WallPresenceCheck, bool>.IResponder
{
    public GameObject tilemapOfWalls;

    bool HierarchyMsg<WallPresenceUpdate, bool>.IResponder.Respond(WallPresenceUpdate request)
    {
        HierarchyMsg<WallPresenceCheck, bool>.Publish(request.present, new WallPresenceCheck { location = request.location }, gameObject);
        return false;
    }

    bool HierarchyMsg<WallPresenceCheck, bool>.IResponder.Respond(WallPresenceCheck request)
    {
        return tilemapOfWalls.GetComponent<UnityEngine.Tilemaps.Tilemap>().HasTile(request.location);
    }

}