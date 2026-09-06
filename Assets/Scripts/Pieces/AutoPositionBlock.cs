#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class AutoPositionBlock : MonoBehaviour
{
    private void Update()
    {
        // 런타임 플레이 중에는 실행하지 않음
        if (Application.isPlaying) return;

        AlignChildrenPositions();
    }

    private void AlignChildrenPositions()
    {
        foreach (Transform child in transform)
        {
            // 'Block_X_Y' 형식을 갖췄는지 검사
            string[] parts = child.name.Split('_');
            if (parts.Length != 3 || parts[0] != "Block") continue;

            if (TryParseCoordinate(parts[1], out float x) && TryParseCoordinate(parts[2], out float y))
            {
                Vector3 targetPos = new Vector3(x, y, child.localPosition.z);

                // 위치 오차가 존재할 때만 스크립트로 위치 이동 및 Undo 기록
                if (Vector3.Distance(child.localPosition, targetPos) > 0.0001f)
                {
                    Undo.RecordObject(child, "Auto Align Block Position");
                    child.localPosition = targetPos;
                }
            }
        }
    }

    /// <summary>
    /// P/M 및 p(소수점) 문자열을 float 좌표값으로 파싱
    /// </summary>
    private bool TryParseCoordinate(string input, out float value)
    {
        value = 0f;
        if (string.IsNullOrEmpty(input)) return false;

        // "0" 예외 처리
        if (input == "0")
        {
            value = 0f;
            return true;
        }

        float sign = 1f;
        string numStr = input;

        // 양수(P) / 음수(M) 접두사 처리
        if (input.StartsWith("M"))
        {
            sign = -1f;
            numStr = input.Substring(1);
        }
        else if (input.StartsWith("P"))
        {
            sign = 1f;
            numStr = input.Substring(1);
        }

        // 소수점 구분자 'p'를 '.'으로 변경
        numStr = numStr.Replace('p', '.');

        if (float.TryParse(numStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsed))
        {
            value = parsed * sign;
            return true;
        }

        return false;
    }
}
#endif
