using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    public Vector2Int LocalOffset { get; private set; }
    public BlockEffect Effect { get; private set; }

    public void Initialize(BlockData data)
    {
        LocalOffset = data.LocalOffset;
        SetEffect(data.Effect);
    }

    /// <summary> 런타임 중 이 자식 블록의 효과와 비주얼을 즉시 교체합니다. </summary>
    public void SetEffect(BlockEffect newEffect)
    {
        Effect = newEffect;

        if (TryGetComponent<SpriteRenderer>(out var sr))
        {
            if (Effect != null)
            {
                if (Effect.customSprite != null) sr.sprite = Effect.customSprite;
                sr.color = Effect.defaultColor;
            }
            else
            {
                sr.color = Color.white; // 효과 해제 시 기본 상태
            }
        }
    }

    public void OnBlockDestroyed(BasePlayField playField, Vector2Int currentGridPos)
    {
        if (Effect != null)
        {
            Effect.OnDestroyed(playField, currentGridPos);
        }
    }
}