using UnityEngine;

[ExecuteInEditMode]
public class ColumnPresenceUpdateHandler : MonoBehaviour
    , HierarchyMsg<ColumnPresenceUpdate, bool>.IRequestor
{
    bool present = true;

    private void Start()
    {
        present = true;
        HierarchyMsg<ColumnPresenceUpdate, bool>.Get((this as HierarchyMsg<ColumnPresenceUpdate, bool>.IRequestor).FormRequest(),transform.parent.parent.gameObject);
    }

    private void OnDestroy()
    {
        present = false;
        HierarchyMsg<ColumnPresenceUpdate, bool>.Request(this);
    }

    ColumnPresenceUpdate HierarchyMsg<ColumnPresenceUpdate, bool>.IRequestor.FormRequest()
    {
        return new ColumnPresenceUpdate { location = GetComponent<LocationOnTilemapHelper>().Location, present = present };
    }

    void HierarchyMsg<ColumnPresenceUpdate, bool>.IRequestor.Handle(ColumnPresenceUpdate request, bool response) {}
}