using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAssetBundleFromFile : MonoBehaviour
{
	public GameObject parents;

	private void Start()
	{
		AssetBundle loginBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "loginbutton"));

		if (loginBundle == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			return;
		}
		GameObject prefab = loginBundle.LoadAsset<GameObject>("Login");
		GameObject login = Instantiate(prefab);
		login.transform.SetParent(parents.transform);
		login.transform.localPosition = new Vector2 (-400, 0);
	}
}
