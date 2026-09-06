using UnityEngine;

public class VersusRuleSet : BaseRuleSet
{
    private VersusCommonSystem _versusCommon;
    
    // 2. 대전 모드용 캐릭터 고유 특성 인스턴스
    private VersusCharacterTrait _characterTrait;

    public VersusRuleSet(BasePlayField field, BasePieceInputHandler input, VersusCharacterTrait chosenTrait) 
        : base(field, input)
    {
        // 대전 모드 전용 공통 시스템(공격 룰, 아이템 타이머 등) 생성
        _versusCommon = new VersusCommonSystem(field);
        
        // 플레이어가 선택한 대전용 캐릭터 특성 장착
        _characterTrait = chosenTrait;
    }

    public override void OnUpdate()
    {
        // 대전 모드 고유의 공통 낙하 규칙 및 게이지 충전 등을 처리
        _versusCommon.UpdatePhysics();
        _versusCommon.CheckGarbageLines();
    }

    public override void ProcessMovement(Vector3 direction)
    {
        // 이동은 캐릭터의 고유 무브먼트 인스턴스에게 판단을 맡김
        _characterTrait.TryMove(PieceInputHandler.ControlledPiece, direction);
    }

    public override void ProcessRotation(float angle) { /* ... */ }
}

//임시 STUB

// 2. BlockInputHandler 껍데기
public class BasePieceInputHandler : MonoBehaviour
{
    // VersusModeRuleSet이 참조하고 있는 ActivePiece 변수가 있어야 에러가 안 납니다.
    [SerializeField] private ControlledPiece _controlledPiece; 
    public ControlledPiece ControlledPiece => _controlledPiece; 
}

// 3. 대전 모드 전용 공통 시스템 껍데기
public class VersusCommonSystem
{
    public VersusCommonSystem(BasePlayField field) { }
    public void UpdatePhysics() { }
    public void CheckGarbageLines() { }
}

// 4. 대전용 캐릭터 고유 특성 껍데기
public class VersusCharacterTrait
{
    public bool TryMove(ControlledPiece piece, Vector3 direction)
    {
        // 일단 무조건 움직임 성공으로 처리하는 임시 코드
        return true; 
    }
}