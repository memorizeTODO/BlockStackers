using UnityEngine;

/// <summary>
/// 테트리스 게임판의 크기, 좌표 변환, 충돌 검증 로직을 정의하는 기본 인터페이스
/// </summary>
public interface BasePlayField
{
    int Width { get; }
    int Height { get; }

    /// <summary> 월드 좌표를 격자 정수 좌표로 변환합니다. </summary>
    Vector2Int WorldToGrid(Vector3 worldPos);

    /// <summary> 블록 뼈대(Transform)가 합법적인 위치에 있는지 검증합니다. </summary>
    bool IsValidPosition(Transform pieceTransform);

    /// <summary> ControlledPiece 인스턴스가 합법적인 위치에 있는지 검증합니다. </summary>
    bool IsValidPosition(ControlledPiece piece);
}