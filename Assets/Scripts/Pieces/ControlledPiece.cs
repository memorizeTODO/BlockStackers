using UnityEngine;

public class ControlledPiece : MonoBehaviour
{
    public PieceData Data { get; private set; }

    public void SetupPiece(PieceData data)
    {
        this.Data = data;

        for (int i = 0; i < transform.childCount && i < data.Blocks.Count; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.TryGetComponent<BlockBehavior>(out var block))
            {
                block = child.gameObject.AddComponent<BlockBehavior>();
            }
            block.Initialize(data.Blocks[i]);
        }
    }

    /// <summary> 상대 좌표(예: -1, 1) 위치 자식 블록의 효과를 실시간 변경 </summary>
    public void ChangeBlockEffectByOffset(Vector2Int localOffset, BlockEffect newEffect)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<BlockBehavior>(out var block) && block.LocalOffset == localOffset)
            {
                block.SetEffect(newEffect);
                break;
            }
        }
    }

    /// <summary> n번째 자식 블록의 효과를 실시간 변경 </summary>
    public void ChangeBlockEffectAtIndex(int index, BlockEffect newEffect)
    {
        if (index >= 0 && index < transform.childCount)
        {
            if (transform.GetChild(index).TryGetComponent<BlockBehavior>(out var block))
            {
                block.SetEffect(newEffect);
            }
        }
    }

    public Vector3Int[] GetCurrentGridPositions()
    {
        Vector3Int[] positions = new Vector3Int[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            positions[i++] = new Vector3Int(
                Mathf.RoundToInt(child.position.x),
                Mathf.RoundToInt(child.position.y),
                0
            );
        }
        return positions;
    }
}