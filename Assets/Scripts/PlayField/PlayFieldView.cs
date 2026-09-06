using UnityEngine;
using UnityEngine.Rendering; // Sorting Group 제어용
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// BasePlayField 데이터 로직을 생성하고, 하위 세부 레이어(Fill, Grid, Border, Overlay 등)의 
/// 감마, 알파(투명도), 색상을 실시간으로 제어하는 View 컴포넌트
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(SortingGroup))]
public class PlayFieldView : MonoBehaviour
{
    [Header("좌표계 대응 설정")]
    [SerializeField] private DefaultPlayField.CoordinateSystemType _coordinateSystem = DefaultPlayField.CoordinateSystemType.RoundToInt;

    [Header("세부 레이어 (자식 오브젝트 추가 시 자동 연결)")]
    [SerializeField] private SpriteRenderer _fillRenderer;
    [SerializeField] private SpriteRenderer _gridRenderer;
    [SerializeField] private SpriteRenderer _borderRenderer;
    [SerializeField] private SpriteRenderer _overlayRenderer;

    [Header("1. Fill (배경) 설정")]
    [SerializeField] private Color _fillColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _fillGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _fillAlpha = 1.0f;

    [Header("2. Grid (격자무늬) 설정")]
    [SerializeField] private Color _gridColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _gridGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _gridAlpha = 1.0f;

    [Header("3. Border (테두리) 설정")]
    [SerializeField] private Color _borderColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _borderGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _borderAlpha = 1.0f;

    [Header("4. Overlay (이펙트/덮개) 설정")]
    [SerializeField] private Color _overlayColor = Color.white;
    [Range(0.1f, 3.0f)] [SerializeField] private float _overlayGamma = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _overlayAlpha = 1.0f;

    /// <summary> 논리 보드 데이터 (RuleSet, InputHandler 등의 시스템이 참조) </summary>
    public BasePlayField LogicBoard { get; private set; }

    private void Awake()
    {
        AutoAssignRenderers();
        UpdateView();

        if (Application.isPlaying)
        {
            // 현재 오브젝트 위치(transform.position)를 원점으로 표준 로직 보드 생성
            LogicBoard = new DefaultPlayField(_coordinateSystem, transform.position);
        }
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
        SpriteRenderer newGrid = null;
        SpriteRenderer newBorder = null;
        SpriteRenderer newOverlay = null;

        foreach (SpriteRenderer sr in allRenderers)
        {
            string childName = sr.name.ToLower().Trim();

            if (childName.Contains("fill") || childName.Contains("bg") || childName.Contains("background"))
                newFill = sr;
            else if (childName.Contains("grid") || childName.Contains("tile") || childName.Contains("pattern"))
                newGrid = sr;
            else if (childName.Contains("border") || childName.Contains("frame") || childName.Contains("wall"))
                newBorder = sr;
            else if (childName.Contains("overlay") || childName.Contains("effect"))
                newOverlay = sr;
        }

        if (_fillRenderer != newFill || _gridRenderer != newGrid || 
            _borderRenderer != newBorder || _overlayRenderer != newOverlay)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) Undo.RecordObject(this, "Auto Assign PlayField Renderers");
#endif
            _fillRenderer = newFill;
            _gridRenderer = newGrid;
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
        ApplyLayerSettings(_gridRenderer, _gridColor, _gridGamma, _gridAlpha, 1);
        ApplyLayerSettings(_borderRenderer, _borderColor, _borderGamma, _borderAlpha, 2);
        ApplyLayerSettings(_overlayRenderer, _overlayColor, _overlayGamma, _overlayAlpha, 3);
    }

    private void ApplyLayerSettings(SpriteRenderer renderer, Color color, float gamma, float alpha, int sortingOrder)
    {
        if (renderer == null) return;

#if UNITY_EDITOR
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
}