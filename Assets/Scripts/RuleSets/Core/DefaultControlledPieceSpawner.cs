using System.Collections.Generic;
using UnityEngine;

public class DefaultControlledPieceSpawner : BaseControlledPieceSpawner
{
    [SerializeField] private PlayFieldView _playFieldView;
    [SerializeField] private Vector3 _localSpawnPosition = new Vector3(4f, 19f, 0f);
    [SerializeField] private List<PieceData> _pieceDataList;

    private Dictionary<string, PieceData> _pieceDictionary = new Dictionary<string, PieceData>();
    private INextPieceGenerator _pieceGenerator;

    private void Awake()
    {
        foreach (var data in _pieceDataList)
        {
            if (data != null && !_pieceDictionary.ContainsKey(data.PieceId))
            {
                _pieceDictionary.Add(data.PieceId, data);
            }
        }
        SetGenerator(new DefaultNextPieceGenerator());
    }

    public override void SetGenerator(INextPieceGenerator generator)
    {
        _pieceGenerator = generator;
    }

    public override ControlledPiece SpawnNextPiece()
    {
        if (_pieceGenerator == null) return null;

        string nextId = _pieceGenerator.PopNextPieceId();
        if (!_pieceDictionary.TryGetValue(nextId, out PieceData data) || data.Prefab == null)
        {
            Debug.LogError($"[Spawner] {nextId} ID에 해당하는 PieceData 또는 프리팹을 찾을 수 없습니다.");
            return null;
        }

        Transform boardParent = (_playFieldView != null) ? _playFieldView.transform : transform;
        GameObject pieceObj = Instantiate(data.Prefab, boardParent);
        pieceObj.transform.localPosition = _localSpawnPosition;
        pieceObj.transform.localRotation = Quaternion.identity;

        ControlledPiece piece = pieceObj.GetComponent<ControlledPiece>();
        if (piece != null)
        {
            piece.SetupPiece(data);
        }
        return piece;
    }

    public override string PeekNextPieceId(int index = 0)
    {
        return _pieceGenerator != null ? _pieceGenerator.PeekNextPieceId(index) : string.Empty;
    }
}