using MIE.Editor.Common;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.SkillSystem
{
    // 예시: SkillSO 뷰어 (SkillSO 클래스가 있다고 가정)
    /*
    public class SkillSOViewer : SOViewer<SkillSO>
    {
        protected override string SOFolderPath => "Assets/06.SO/Skills";
        protected override string WindowTitle => "SkillSO Viewer";
        protected override string InspectorTitle => "SkillSO";
        protected override string CreateButtonText => "Create SkillSO";
        protected override string SelectionHint => "왼쪽에서 SkillSO를 선택하세요";
        protected override string DefaultAssetName => "SkillSO";

        [MenuItem("MIE/Tool/SkillSO Viewer")]
        public static void ShowWindow()
        {
            GetWindow<SkillSOViewer>("SkillSO Viewer");
        }

        // 스킬 전용 추가 기능들
        protected override void DrawHeader()
        {
            base.DrawHeader();
            
            // 스킬 타입별 필터링 버튼 등 추가 가능
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Active Skills"))
            {
                // 액티브 스킬만 필터링
            }
            if (GUILayout.Button("Passive Skills"))
            {
                // 패시브 스킬만 필터링
            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void OnSOCreated(SkillSO createdSO)
        {
            // 새 스킬 생성시 기본값 설정
            // createdSO.cooldown = 1.0f;
            // createdSO.skillType = SkillType.Active;
        }
    }
    */
}