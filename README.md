# Unity AssetBundles

This article will introduce how to use unity assetBundles with `UnityWebRequestAssetBundle.GetAssetBundle`.

# 1. AssetBundles Build Script

We create `Editor` directory and have new script for building.

AssetBundles are different according to platform.

Therefore, we need to check `BuildTarget`.

Besides, we need to create directory to place our bundles.

For example : 

```

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetsBundleBuilder : MonoBehaviour
{
	[MenuItem("Assets/Build AssetBundles")]
	public static void BuildAllAssetBundles()
	{
		string directory = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
#if UNITY_STANDALONE_OSX
		BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
#elif UNITY_IOS
		BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
#elif UNITY_ANDROID
		BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
#elif UNITY_STANDALONE_WIN
		BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
#endif
	}
}

```

# 2. Mark AssetBundles Label

After build script is done, we mark assetBundle label for assets.

Unity will create Assetbundles with assetBundle label.

For example, I selected the `loginbutton` label for `Login` Prefab.

![Image of MD1](https://raw.githubusercontent.com/tsen1220/UnityAssetBundle/master/Image/MD1.png)

# 3. Build

We can build AssetBundles after clicking customized menuitem.

# 4. Http Server

We need to create Http server to place and download our AssetBundles.

For example, I used `php` to create server. 

```
php -S 127.0.0.1:8888
```

# 5. Load AssetBundles

We download AssetBundles from http server and extract assets from AssetBundles with coroutine.

However, We should check the Assetbundles version from manifest with `Hash128`.

If Assetbundles version is newest and newest bundles had been ever downloaded, Unity will load AssetBundles from cache.

For example:

```
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class LoadAssetBundleFromWebServer : MonoBehaviour
{
	private AssetBundleManifest manifest;

	private async void Start()
	{
		StartCoroutine(GetAssetBundleManifest("http://127.0.0.1:8888/AssetBundles"));
		while(manifest == null)
		{
			await Task.Yield();
		}
		Hash128 hash = manifest.GetAssetBundleHash("loginbutton");
		StartCoroutine(GetLoginAssetBundle("http://127.0.0.1:8888/loginbutton", hash));
	}

	private IEnumerator GetLoginAssetBundle(string uri, Hash128 hash)
	{
		UnityWebRequest bundleWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri, hash);
		yield return bundleWebRequest.SendWebRequest();

		if (bundleWebRequest.isHttpError)
		{
			yield break;
		}
		else
		{
			AssetBundle loginBundle = DownloadHandlerAssetBundle.GetContent(bundleWebRequest);

			if (loginBundle == null)
			{
				Debug.Log("Failed to load AssetBundle!");
				yield break;
			}
			GameObject prefab = loginBundle.LoadAsset<GameObject>("Login");
			GameObject login = Instantiate(prefab);
			
			bundleWebRequest.Dispose();
		}
	}

	private IEnumerator GetAssetBundleManifest(string uri)
	{
		UnityWebRequest bundleWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri);
		yield return bundleWebRequest.SendWebRequest();

		if (bundleWebRequest.isHttpError)
		{
			yield break;
		}
		else
		{
			AssetBundle manifestBundle = DownloadHandlerAssetBundle.GetContent(bundleWebRequest);

			AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			this.manifest = manifest;
			bundleWebRequest.Dispose();
		}
	}
}
```

# 6. Play

Finally, We load assets with AssetBundles.

![Image of MD2](https://raw.githubusercontent.com/tsen1220/UnityAssetBundle/master/Image/MD2.png)