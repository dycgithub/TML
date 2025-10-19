using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BundleConfig
{
    public string bundleName;
    public string[] assetNames;
}

public class LoadFromLocal : MonoBehaviour
{
    [Header("配置加载的AssetBundle")]
    [SerializeField] private List<BundleConfig> bundles = new List<BundleConfig>();

    [Header("UI进度反馈")]
    [SerializeField] private Text statusText;
    [SerializeField] private Slider progressBar;

    private string manifestBundleName = "AssetBundles"; // 默认主Manifest包名
    private AssetBundleManifest manifest;

    void Start()
    {
        StartCoroutine(LoadManifestAndBundles());
    }

    IEnumerator LoadManifestAndBundles()
    {
        string manifestPath = GetBundlePath(manifestBundleName);
        if (string.IsNullOrEmpty(manifestPath))
        {
            UpdateStatus("找不到Manifest包！");
            yield break;
        }

        var manifestRequest = AssetBundle.LoadFromFileAsync(manifestPath);
        yield return manifestRequest;

        var manifestBundle = manifestRequest.assetBundle;
        manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        manifestBundle.Unload(false);

        yield return StartCoroutine(LoadBundlesInOrder());
    }

    IEnumerator LoadBundlesInOrder()
    {
        foreach (var config in bundles)
        {
            string path = GetBundlePath(config.bundleName);
            if (string.IsNullOrEmpty(path))
            {
                UpdateStatus($"跳过：未找到 {config.bundleName}");
                continue;
            }

            // 加载依赖
            string[] dependencies = manifest.GetAllDependencies(config.bundleName);
            foreach (var dep in dependencies)
            {
                string depPath = GetBundlePath(dep);
                if (!string.IsNullOrEmpty(depPath))
                {
                    var depRequest = AssetBundle.LoadFromFileAsync(depPath);
                    yield return depRequest;
                }
            }

            UpdateStatus($"加载包：{config.bundleName}");
            yield return StartCoroutine(LoadAndInstantiate(path, config.assetNames));
        }

        UpdateStatus("全部加载完成！");
    }

    IEnumerator LoadAndInstantiate(string path, string[] assetNames)
    {
        var bundleRequest = AssetBundle.LoadFromFileAsync(path);
        yield return bundleRequest;

        var bundle = bundleRequest.assetBundle;
        if (bundle == null)
        {
            UpdateStatus($"加载失败：{path}");
            yield break;
        }

        foreach (string assetName in assetNames)
        {
            UpdateStatus($"加载资源：{assetName}");
            var assetRequest = bundle.LoadAssetAsync<GameObject>(assetName);

            while (!assetRequest.isDone)
            {
                UpdateProgress(assetRequest.progress);
                yield return null;
            }

            if (assetRequest.asset != null)
            {
                Instantiate(assetRequest.asset);
                UpdateStatus($"实例化成功：{assetName}");
            }
            else
            {
                UpdateStatus($"资源未找到：{assetName}");
            }
        }

        bundle.Unload(false);
    }

    string GetBundlePath(string bundleName)
    {
        string localPath = Path.Combine(Application.persistentDataPath, bundleName);
        if (File.Exists(localPath)) return localPath;

        string streamingPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", bundleName);
        if (File.Exists(streamingPath)) return streamingPath;

        return null;
    }

    void UpdateStatus(string message)
    {
        if (statusText) statusText.text = message;
        Debug.Log(message);
    }

    void UpdateProgress(float value)
    {
        if (progressBar) progressBar.value = value;
    }
}
