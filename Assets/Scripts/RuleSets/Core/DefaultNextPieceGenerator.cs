using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 표준 7-Bag 규칙 기반의 NEXT 피스 생성기
/// </summary>
public class DefaultNextPieceGenerator : BaseNextPieceGenerator
{
    private List<string> _bag = new List<string>();

    public DefaultNextPieceGenerator(int previewSize = 5) : base(previewSize)
    {
        EnsureQueueSize();
    }

    protected override string GenerateNextId()
    {
        if (_bag.Count == 0)
        {
            RefillBag();
        }

        string nextId = _bag[0];
        _bag.RemoveAt(0);
        return nextId;
    }

    private void RefillBag()
    {
        List<string> newBag = new List<string> { "I", "J", "L", "O", "S", "T", "Z" };
        while (newBag.Count > 0)
        {
            int rand = Random.Range(0, newBag.Count);
            _bag.Add(newBag[rand]);
            newBag.RemoveAt(rand);
        }
    }

    public override void Reset()
    {
        _bag.Clear();
        base.Reset();
    }
}