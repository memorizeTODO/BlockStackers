using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BlockData
{
    [SerializeField] private Vector2Int _localOffset;
    [SerializeField] private BlockEffect _effect;

    // 외부에서 읽기 전용으로 접근하는 프로퍼티 (PascalCase)
    public Vector2Int LocalOffset => _localOffset;
    public BlockEffect Effect => _effect;

    // 생성자
    public BlockData(Vector2Int localOffset, BlockEffect effect)
    {
        _localOffset = localOffset;
        _effect = effect;
    }
}

[CreateAssetMenu(fileName = "PieceData_", menuName = "BlockStackers/Piece Data")]
public class PieceData : ScriptableObject
{
    [SerializeField] private string _pieceId;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _previewIcon;
    [SerializeField] private List<BlockData> _blocks = new List<BlockData>();

    // 외부 C# 코드에서는 읽기 전용 프로퍼티(PascalCase)로 접근
    public string PieceId => _pieceId;
    public GameObject Prefab => _prefab;
    public Sprite PreviewIcon => _previewIcon;
    public IReadOnlyList<BlockData> Blocks => _blocks;
}