using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles : EditorWindow
{
    private string assetBundleName = "main.ab";
    private BuildTarget selectedPlatform;
    // 用于存储当前平台信息
    private string currentPlatformText;

    [MenuItem("Tools/AssetBundle Tool")]
    public static void ShowWindow()
    {
        GetWindow<CreateAssetBundles>("AssetBundle工具");
    }

    private void OnEnable()
    {
        // 窗口启用时获取当前平台
        UpdateCurrentPlatformText();
    }

    private void UpdateCurrentPlatformText()
    {
        // 获取当前活跃的构建目标（当前平台）
        BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;
        currentPlatformText = "当前平台: " + currentTarget.ToString();
    }

    private void OnGUI()
    {
        // 显示当前平台
        GUILayout.Label(currentPlatformText, EditorStyles.boldLabel);

        // Config 按钮
        if (GUILayout.Button("Config"))
        {
            EditorUtility.DisplayDialog("提示", "配置 AssetBundle 相关参数", "确定");
        }

        // Refresh 按钮
        if (GUILayout.Button("Refresh"))
        {
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "AssetBundle 已刷新", "确定");
        }

        // Explorer 按钮
        if (GUILayout.Button("Explorer"))
        {
            string outputPath = Path.Combine(Application.dataPath, "..", "AssetBundles");
            if (Directory.Exists(outputPath))
            {
                EditorUtility.RevealInFinder(outputPath);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "AssetBundle 输出目录不存在", "确定");
            }
        }

        // Clear 按钮
        if (GUILayout.Button("Clear"))
        {
            EditorUtility.DisplayDialog("提示", "已清除相关内容", "确定");
        }

        GUILayout.Space(10);
        GUILayout.Label("AssetBundles");

        // 显示 AssetBundle 名称输入框
        assetBundleName = EditorGUILayout.TextField(assetBundleName);

        GUILayout.Space(10);

        // 平台选择按钮
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("PC"))
        {
            selectedPlatform = BuildTarget.StandaloneWindows;
        }
        if (GUILayout.Button("Android"))
        {
            selectedPlatform = BuildTarget.Android;
        }
        if (GUILayout.Button("WebGL"))
        {
            selectedPlatform = BuildTarget.WebGL;
        }
        if (GUILayout.Button("All Platform"))
        {
            EditorUtility.DisplayDialog("提示", "构建所有平台的 AssetBundle", "确定");
        }
        GUILayout.EndHorizontal();

        // 构建按钮
        if (GUILayout.Button("Build AssetBundle"))
        {
            if (string.IsNullOrEmpty(assetBundleName))
            {
                EditorUtility.DisplayDialog("错误", "AssetBundle 名称不能为空", "确定");
                return;
            }

            string outputPath = Path.Combine(Application.dataPath, "..", "AssetBundles");//创建一个与Asset同级的文件夹
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            // 构建 AssetBundle
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, selectedPlatform);
            AssetDatabase.Refresh();

            // 自动打开输出文件夹
            EditorUtility.RevealInFinder(outputPath);

            EditorUtility.DisplayDialog("提示", "AssetBundle 构建完成\n已打开输出文件夹", "确定");
        }
    }
}