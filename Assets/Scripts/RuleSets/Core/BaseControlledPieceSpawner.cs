using UnityEngine;

/// <summary>
/// 유니티 필드 상에 블록을 실체화(Instantiate)하는 스포너의 기본 추상 클래스
/// </summary>
public abstract class BaseControlledPieceSpawner : MonoBehaviour
{
    public abstract ControlledPiece SpawnNextPiece();
    public abstract string PeekNextPieceId(int index = 0);
    public abstract void SetGenerator(INextPieceGenerator generator);
}