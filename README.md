# Unity AssetBundles

This article will introduce how to use unity assetBundles.

# 1. AssetBundles Build Script

We create Editor directory and have new script for building.

AssetBundles are different according to platform.

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

After build script being done, we mark assetBundle label for assets.

Unity will create Assetbundles with assetBundle label.

