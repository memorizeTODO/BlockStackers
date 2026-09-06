using UnityEngine;

public abstract class BaseRuleSet
{
    [SerializeField] protected BasePlayField _playField;

    [SerializeField] protected BasePieceInputHandler _pieceInputHandler;

    protected BasePlayField PlayField => _playField;
    protected BasePieceInputHandler PieceInputHandler => _pieceInputHandler;

    public BaseRuleSet(BasePlayField field, BasePieceInputHandler input)
    {
        _playField = field;
        _pieceInputHandler = input;
    }

    // 이 룰이 활성화되어 있는 동안 매 프레임 실행될 루프
    public abstract void OnUpdate();
    
    // 플레이어의 입력 요청을 각 룰에 맞게 처리
    public abstract void ProcessMovement(Vector3 direction);
    public abstract void ProcessRotation(float angle);
}
