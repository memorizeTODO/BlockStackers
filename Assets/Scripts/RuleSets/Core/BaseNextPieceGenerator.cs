using System.Collections.Generic;

/// <summary>
/// 일반적인 FIFO 큐 기반 NEXT 피스 생성기들의 미리보기/큐 관리 공통 로직
/// </summary>
public abstract class BaseNextPieceGenerator : INextPieceGenerator
{
    protected List<string> _previewQueue = new List<string>();
    protected int _previewSize = 5;

    public BaseNextPieceGenerator(int previewSize = 5)
    {
        _previewSize = previewSize;
    }

    public virtual string PopNextPieceId()
    {
        EnsureQueueSize();
        string next = _previewQueue[0];
        _previewQueue.RemoveAt(0);
        EnsureQueueSize();
        return next;
    }

    public virtual string PeekNextPieceId(int index = 0)
    {
        EnsureQueueSize();
        return (index >= 0 && index < _previewQueue.Count) ? _previewQueue[index] : string.Empty;
    }

    public virtual void Reset()
    {
        _previewQueue.Clear();
        EnsureQueueSize();
    }

    protected void EnsureQueueSize()
    {
        while (_previewQueue.Count < _previewSize)
        {
            _previewQueue.Add(GenerateNextId());
        }
    }

    /// <summary> 자식 클래스에서 "다음 피스 ID 하나를 어떻게 뽑을지" 알고리즘만 정의합니다. </summary>
    protected abstract string GenerateNextId();
}