using UnityEngine;

public class DefaultPlayField : BasePlayField
{
    public int Width => 10;
    public int Height => 20;

    public enum CoordinateSystemType { RoundToInt, FloorToInt }
    private CoordinateSystemType coordinateSystem;
    private Vector3 boardOriginPosition;
    private Transform[,] grid;

    public DefaultPlayField(CoordinateSystemType coordSystem, Vector3 originPos = default)
    {
        this.coordinateSystem = coordSystem;
        this.boardOriginPosition = originPos;
        this.grid = new Transform[Width, Height];
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 relativePos = worldPos - boardOriginPosition;
        int x = (coordinateSystem == CoordinateSystemType.FloorToInt) ? Mathf.FloorToInt(relativePos.x) : Mathf.RoundToInt(relativePos.x);
        int y = (coordinateSystem == CoordinateSystemType.FloorToInt) ? Mathf.FloorToInt(relativePos.y) : Mathf.RoundToInt(relativePos.y);
        return new Vector2Int(x, y);
    }

    public bool IsValidPosition(Transform pieceTransform)
    {
        foreach (Transform child in pieceTransform)
        {
            Vector2Int gridPos = WorldToGrid(child.position);
            if (gridPos.x < 0 || gridPos.x >= Width) return false;
            if (gridPos.y < 0) return false;
            if (gridPos.y >= Height) continue;
            if (grid[gridPos.x, gridPos.y] != null) return false;
        }
        return true;
    }

    public bool IsValidPosition(ControlledPiece piece)
    {
        if (piece == null) return false;
        return IsValidPosition(piece.transform);
    }
}