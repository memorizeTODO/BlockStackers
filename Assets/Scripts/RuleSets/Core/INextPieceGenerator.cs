using System.Collections.Generic;

/// <summary>
/// 다음 블록 생성 순서 및 예측(NEXT UI) 로직을 정의하는 기본 인터페이스
/// </summary>
public interface INextPieceGenerator
{
    string PopNextPieceId();
    string PeekNextPieceId(int index = 0);
    void Reset();
}