
using UnityEngine;

[ExecuteInEditMode]
public class WallOrientationProvider : MonoBehaviour
    , HierarchyMsg<WallOrientation>.IProvider
{
    WallOrientation HierarchyMsg<WallOrientation>.IProvider.Provide()
    {
        return new WallOrientation { 
            facing = System.Convert.ToInt32(HoneycombGridsCalculator.CalculateWallFacing(GetComponentInParent<LocationOnTilemapHelper>().Location)) 
        };
    }
}