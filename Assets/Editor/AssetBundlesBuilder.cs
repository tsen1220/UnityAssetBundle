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
		string directory = "Assets/AssetBundles";
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
