
using UnityEngine;
using System;
using System.Collections.Generic;

public class HoneycombGridsCalculator 
{
    static public int Mod(int a, int n)
    {
        int result = a % n;
        if ((result < 0 && n > 0) || (result > 0 && n < 0))
        {
            result += n;
        }
        return result;
    }

    public static Vector3Int OffsetToAxial(Vector3Int position) => position - new Vector3Int((position.y - Mod(position.y, 2)) / 2, 0);
    public static Vector3Int AxialToOffset(Vector3Int position) => position + new Vector3Int((position.y - Mod(position.y, 2)) / 2, 0);

    public static HexagonNeighboring.Relation CalculateWallFacing(Vector3Int location)
    {
        if (location.y % 2 != 0)
        {
            if (Math.Abs(location.x) % 2 != 0 ^ Math.Abs(location.y) % 4 != 1 ^ location.y < 0)
            {
                return HexagonNeighboring.Relation.SouthEast;
            }
            else
            {
                return HexagonNeighboring.Relation.SouthWest;
            }
        }
        else
        {
            if ((location.x + location.y / 2) % 2 != 0)
                return HexagonNeighboring.Relation.East;
            else
                return HexagonNeighboring.Relation.Center;
        }
    }
    
    public static bool IsColumnShiftNorth(Vector3Int location)
    {
        return Mod(location.x - Mod(location.y, 2), 3) == 2;
    }
    
    public static Dictionary<Enum, Vector3Int> GetWallsLocationsIntersectingWithField(Vector3Int fieldLoc)
    {
        fieldLoc = OffsetToAxial(fieldLoc);
        var offsetToCenter = new Vector3Int(Mod(fieldLoc.x, 2), -Mod(fieldLoc.y, 2));
        return new Dictionary<Enum, Vector3Int>
        {
            {HexagonAxialNeighboring.relativeVectorsInverse[offsetToCenter], AxialToOffset(fieldLoc+offsetToCenter)/2 },
            {HexagonAxialNeighboring.relativeVectorsInverse[-offsetToCenter], AxialToOffset(fieldLoc-offsetToCenter)/2 }
        };
    }

    public static List<Vector3Int> GetWallTerminatingColumnsPosition(Vector3Int wallLoc)
    {
        HexagonNeighboring.Relation wallFacing = CalculateWallFacing(wallLoc);
        var colLoc = new Vector3Int(wallLoc.y - Math.DivRem(wallLoc.y, 4, out _), wallLoc.x, 0);
        var result = new List<Vector3Int>();
        switch (wallFacing)
        {
            case HexagonNeighboring.Relation.East:
                result.Add(colLoc);
                if (wallLoc.y > 0 || wallLoc.y % 4 == 0)
                {
                    colLoc.x--;
                    result.Insert(0, colLoc);
                }
                else
                {
                    colLoc.x++;
                    result.Add(colLoc);
                }
                break;
            case HexagonNeighboring.Relation.SouthEast:
                if (wallLoc.y > 0)
                {
                    colLoc.x--;
                }
                result.Add(colLoc);
                colLoc.y++;
                if (wallLoc.y % 4 == 1 || wallLoc.y % 4 == -3)
                {
                    colLoc.x++;
                }
                result.Add(colLoc);
                break;
            case HexagonNeighboring.Relation.SouthWest:
                if (wallLoc.y > 0)
                {
                    colLoc.x--;
                }
                colLoc.y++;
                result.Add(colLoc);
                colLoc.y--;
                if (wallLoc.y % 4 == 1)
                {
                    colLoc.x++;
                }
                if (wallLoc.y % 4 == -3)
                {
                    colLoc.x++;
                }
                result.Add(colLoc);
                break;
        };
        return result;
    }

    public static List<Vector3Int> GetWallsLocationsTerminatedByColumn(Vector3Int colLoc)
    {
        Vector3Int eastFacingWall;
        if (colLoc.y % 2 == 0)
            eastFacingWall = new Vector3Int(colLoc.y, 2 * (2*(colLoc.x / 3) + Math.Sign(colLoc.x)));
        else
            eastFacingWall = new Vector3Int(colLoc.y, 4 * ((colLoc.x + 1 - Mod(colLoc.x + 1, 3)) / 3));

        var neighbours = HexagonNeighboring.GetNeighborsLocation(eastFacingWall);
        if (IsColumnShiftNorth(colLoc))
        {
            return new List<Vector3Int>
            {
                { eastFacingWall },
                { neighbours[HexagonNeighboring.Relation.NorthWest] },
                { neighbours[HexagonNeighboring.Relation.NorthEast] }
            };
        }
        else
        {
            return new List<Vector3Int>
            {
                { eastFacingWall },
                { neighbours[HexagonNeighboring.Relation.SouthEast] },
                { neighbours[HexagonNeighboring.Relation.SouthWest] }
            };
        }
    }
}