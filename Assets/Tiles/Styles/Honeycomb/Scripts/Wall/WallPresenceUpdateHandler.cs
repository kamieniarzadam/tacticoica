using UnityEngine;

[ExecuteInEditMode]
public class WallPresenceUpdateHandler : MonoBehaviour
    , HierarchyMsg<WallPresenceUpdate, bool>.IRequestor
{
    bool present = true;

    private void Start()
    {
        present = true;
        HierarchyMsg<WallPresenceUpdate, bool>.Get((this as HierarchyMsg<WallPresenceUpdate, bool>.IRequestor).FormRequest(),transform.parent.parent.gameObject);
    }

    private void OnDestroy()
    {
        present = false;
        HierarchyMsg<WallPresenceUpdate, bool>.Request(this);
    }

    WallPresenceUpdate HierarchyMsg<WallPresenceUpdate, bool>.IRequestor.FormRequest()
    {
        return new WallPresenceUpdate { location = GetComponent<LocationOnTilemapHelper>().Location, present = present };
    }

    void HierarchyMsg<WallPresenceUpdate, bool>.IRequestor.Handle(WallPresenceUpdate request, bool response) {}
}