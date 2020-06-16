using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAssetBundleFromFile : MonoBehaviour
{
	public GameObject parents;
	public GameObject icon;

	private void Start()
	{
		// Get Hash
		AssetBundle manifestBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "AssetBundles"));
		AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		Hash128 hash = manifest.GetAssetBundleHash("loginbutton");
		Debug.Log(hash);

		// Load GameObject
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

		icon.GetComponent<UnityEngine.UI.Image>().sprite = login.GetComponentsInChildren<UnityEngine.UI.Image>()[2].sprite;
	}
}
