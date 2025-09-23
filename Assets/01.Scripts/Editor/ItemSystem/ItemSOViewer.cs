using MIE.BoardSystem.Item.Data;
using MIE.Editor.Common;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.ItemSystem
{
    public class ItemDataSOViewer : SOViewer<ItemDataSO>
    {
        protected override string SOFolderPath => "Assets/06.SO/Items";
        protected override string WindowTitle => "ItemDataSO Viewer";
        protected override string InspectorTitle => "ItemDataSO";
        protected override string CreateButtonText => "Create ItemDataSO";
        protected override string SelectionHint => "왼쪽에서 ItemDataSO를 선택하세요";
        protected override string DefaultAssetName => "ItemDataSO";

        [MenuItem("MIE/Tool/ItemDataSO Viewer")]
        public static void ShowWindow()
        {
            GetWindow<ItemDataSOViewer>("ItemDataSO Viewer");
        }
    }
}