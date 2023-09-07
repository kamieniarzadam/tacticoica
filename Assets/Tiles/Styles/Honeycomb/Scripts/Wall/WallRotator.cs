
using UnityEngine;
using System;

[ExecuteInEditMode]
public class WallRotator : MonoBehaviour
    , HierarchyMsg<WallOrientation>.IHandler
{
    static readonly float slopeAngle = (float)Math.Atan(4d / 3d) * Mathf.Rad2Deg;

    void Start() => HierarchyMsg<WallOrientation>.Request(this);

    private void SetRotation(int f)
    {
        if(Convert.ToInt32(HexagonNeighboring.Relation.East) != f)
        {
            if (Convert.ToInt32(HexagonNeighboring.Relation.SouthEast) == f)
                transform.localRotation = Quaternion.Euler(0, slopeAngle, 0);
            if (Convert.ToInt32(HexagonNeighboring.Relation.SouthWest) == f)
                transform.localRotation = Quaternion.Euler(0, 180-slopeAngle, 0);
        }
    }

    void HierarchyMsg<WallOrientation>.IHandler.Handle(WallOrientation payload) => SetRotation(payload.facing);
}