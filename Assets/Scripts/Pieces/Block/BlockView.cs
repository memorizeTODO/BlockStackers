using UnityEngine;
using UnityEngine.Rendering; // Sorting Group 제어용
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(SortingGroup))]
public class BlockView : MonoBehaviour
{
    [Header("5단계 세부 레이어 (자식이 추가되면 자동 연결)")]
    [SerializeField] private SpriteRenderer _fillRenderer;
    [SerializeField] private SpriteRenderer _patternRenderer;
    [SerializeField] private SpriteRenderer _iconRenderer;
    [SerializeField] private SpriteRenderer _borderRenderer;
    [SerializeField] private SpriteRenderer _overlayRenderer;

    [Header("1. Fill 설정")]
    [SerializeField] private Color _fillColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _fillGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _fillAlpha = 1.0f;

    [Header("2. Pattern 설정")]
    [SerializeField] private Color _patternColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _patternGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _patternAlpha = 1.0f;

    [Header("3. Icon 설정")]
    [SerializeField] private Color _iconColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _iconGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _iconAlpha = 1.0f;

    [Header("4. Border 설정")]
    [SerializeField] private Color _borderColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _borderGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _borderAlpha = 1.0f;

    [Header("5. Overlay 설정")]
    [SerializeField] private Color _overlayColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _overlayGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _overlayAlpha = 1.0f;

    private void Awake()
    {
        AutoAssignRenderers();
        UpdateView();
    }

    private void OnValidate()
    {
        UpdateView();
    }

#if UNITY_EDITOR
    private void OnEnable() => EditorApplication.hierarchyChanged += OnHierarchyChanged;
    private void OnDisable() => EditorApplication.hierarchyChanged -= OnHierarchyChanged;

    private void OnHierarchyChanged()
    {
        if (this == null || Application.isPlaying) return;
        AutoAssignRenderers();
        UpdateView();
    }
#endif

    public void AutoAssignRenderers()
    {
        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        SpriteRenderer newFill = null;
        SpriteRenderer newPattern = null;
        SpriteRenderer newIcon = null;
        SpriteRenderer newBorder = null;
        SpriteRenderer newOverlay = null;

        foreach (SpriteRenderer sr in allRenderers)
        {
            string childName = sr.name.ToLower().Trim();

            if (childName.Contains("fill") || childName.Contains("bg") || childName.Contains("background"))
                newFill = sr;
            else if (childName.Contains("pattern") || childName.Contains("texture"))
                newPattern = sr;
            else if (childName.Contains("icon") || childName.Contains("symbol"))
                newIcon = sr;
            else if (childName.Contains("border") || childName.Contains("frame"))
                newBorder = sr;
            else if (childName.Contains("overlay") || childName.Contains("effect"))
                newOverlay = sr;
        }

        if (_fillRenderer != newFill || _patternRenderer != newPattern || 
            _iconRenderer != newIcon || _borderRenderer != newBorder || _overlayRenderer != newOverlay)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) Undo.RecordObject(this, "Auto Assign Block Renderers");
#endif
            _fillRenderer = newFill;
            _patternRenderer = newPattern;
            _iconRenderer = newIcon;
            _borderRenderer = newBorder;
            _overlayRenderer = newOverlay;

#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(this);
#endif
        }
    }

    public void UpdateView()
    {
        ApplyLayerSettings(_fillRenderer, _fillColor, _fillGamma, _fillAlpha, 0);
        ApplyLayerSettings(_patternRenderer, _patternColor, _patternGamma, _patternAlpha, 1);
        ApplyLayerSettings(_iconRenderer, _iconColor, _iconGamma, _iconAlpha, 2);
        ApplyLayerSettings(_borderRenderer, _borderColor, _borderGamma, _borderAlpha, 3);
        ApplyLayerSettings(_overlayRenderer, _overlayColor, _overlayGamma, _overlayAlpha, 4);
    }

    private void ApplyLayerSettings(SpriteRenderer renderer, Color color, float gamma, float alpha, int sortingOrder)
    {
        if (renderer == null) return;

#if UNITY_EDITOR
        // 에디터 배치 중에만 위치 (0,0,0) 및 스케일 (1,1,1) 정렬
        if (!Application.isPlaying)
        {
            renderer.transform.localPosition = Vector3.zero;
            renderer.transform.localScale = Vector3.one;
        }
#endif

        renderer.sortingOrder = sortingOrder;
        renderer.color = ApplyGammaCorrection(color, gamma, alpha);
    }

    private Color ApplyGammaCorrection(Color sourceColor, float gammaValue, float alphaValue)
    {
        return new Color(
            Mathf.Pow(sourceColor.r, gammaValue),
            Mathf.Pow(sourceColor.g, gammaValue),
            Mathf.Pow(sourceColor.b, gammaValue),
            alphaValue
        );
    }

    public void Initialize(Color newColor)
    {
        AutoAssignRenderers();
        _fillColor = newColor;
        UpdateView();
    }
}