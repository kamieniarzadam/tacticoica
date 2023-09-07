
using UnityEngine;

[ExecuteInEditMode]
public class WallColumnsController : MonoBehaviour
    , HierarchyMsg<WallOrientation>.IHandler
{
    public bool visible = true;
    void Start() => HierarchyMsg<WallOrientation>.Request(this);

    private void SetVisibility(int f)
    {
        if(System.Convert.ToInt32(HexagonNeighboring.Relation.East) == f)
        {
            gameObject.SetActive(visible);
        }
        else
        {
            gameObject.SetActive(!visible);
        }
    }

    void HierarchyMsg<WallOrientation>.IHandler.Handle(WallOrientation payload) => SetVisibility(payload.facing);
}