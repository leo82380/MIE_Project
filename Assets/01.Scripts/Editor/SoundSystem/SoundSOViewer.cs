using MIE.Runtime.SoundSystem.Data;
using MIE.Editor.Common;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.SoundSystem
{
    public class SoundSOViewer : SOViewer<SoundSO>
    {
        protected override string SOFolderPath => "Assets/06.SO/Sounds";
        protected override string WindowTitle => "SoundSO Viewer";
        protected override string InspectorTitle => "SoundSO";
        protected override string CreateButtonText => "Create SoundSO";
        protected override string SelectionHint => "왼쪽에서 SoundSO를 선택하세요";
        protected override string DefaultAssetName => "SoundSO";

        [MenuItem("MIE/Tool/SoundSO Viewer")]
        public static void ShowWindow()
        {
            GetWindow<SoundSOViewer>("SoundSO Viewer");
        }
    }
}
