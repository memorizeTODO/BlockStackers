#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EditorWorkflowTools
{
    // 상단 메뉴에 [Tools -> Snap Children] 메뉴를 만들고, 단축키 Alt + Shift + Z를 지정합니다.
    [MenuItem("Tools/Snap Selected Children &%z")] 
    private static void SnapChildren()
    {
        // 현재 마우스로 선택한 오브젝트들
        foreach (GameObject obj in Selection.gameObjects)
        {
            // 선택한 오브젝트의 모든 자식들을 0, 0, 0으로 정렬
            foreach (Transform child in obj.transform)
            {
                // 실행 취소(Ctrl+Z)가 가능하도록 기록을 남겨주는 유니티 에디터 기능
                Undo.RecordObject(child, "Snap Child To Zero");
                child.localPosition = Vector3.zero;
            }
        }
        Debug.Log("선택한 오브젝트들의 모든 자식이 로컬 (0,0,0)으로 정렬되었습니다!");
    }
}
#endif