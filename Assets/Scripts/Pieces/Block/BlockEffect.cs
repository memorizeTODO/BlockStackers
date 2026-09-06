using UnityEngine;

public abstract class BlockEffect : ScriptableObject
{
    public string effectName;
    public Color defaultColor = Color.white;
    public Sprite customSprite;

    public virtual void OnPlaced(BasePlayField playField, Vector2Int gridPos) { }
    public virtual void OnDestroyed(BasePlayField playField, Vector2Int gridPos) { }
    public virtual bool PreventDestruction(BasePlayField playField, Vector2Int gridPos) => false;
}