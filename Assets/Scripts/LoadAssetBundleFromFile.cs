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

		// Load Sprite
		AssetBundle loginImage = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "loginimage"));

		if (loginImage == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			return;
		}
		Sprite image = loginImage.LoadAsset<Sprite>("line-icon");
		icon.GetComponent<UnityEngine.UI.Image>().sprite = image;
	}
}
