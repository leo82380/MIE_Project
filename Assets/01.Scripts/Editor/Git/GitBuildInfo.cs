using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.Git
{
    public class GitBuildInfo
    {
        private const string SO_PATH = "Assets/Resources/GitInfoSO.asset";

        [MenuItem("MIE/Tool/Update GitInfoSO")]
        public static void UpdateGitInfoSO()
        {
            var gitInfoSO = AssetDatabase.LoadAssetAtPath<GitInfoSO>(SO_PATH);
            if (gitInfoSO == null)
            {
                gitInfoSO = ScriptableObject.CreateInstance<GitInfoSO>();
                AssetDatabase.CreateAsset(gitInfoSO, SO_PATH);
            }

            gitInfoSO.Version = Application.version;
            gitInfoSO.CommitHash = RunGitCommand("rev-parse --short HEAD");
            gitInfoSO.BranchName = RunGitCommand("rev-parse --abbrev-ref HEAD");
            gitInfoSO.Date = RunGitCommand("log -1 --pretty=%cd --date=short");

            EditorUtility.SetDirty(gitInfoSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log($"GitInfoSO Updated: {gitInfoSO.GetDisplayText()}");
        }

        private static string RunGitCommand(string args)
        {
            var startInfo = new ProcessStartInfo("git", args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd().Trim();
        }
    }
}
