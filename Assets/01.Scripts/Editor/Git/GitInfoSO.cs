using UnityEngine;

namespace MIE.Editor.Git
{
    [CreateAssetMenu(fileName = "GitInfoSO", menuName = "MIE/Git Info SO")]
    public class GitInfoSO : ScriptableObject
    {
        public string Version;
        public string CommitHash;
        public string BranchName;
        public string Date;

        public string GetDisplayText()
        {
            return $"{Version}/({CommitHash})/{BranchName}/{Date}";
        }
    }
}
