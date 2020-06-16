using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class LoadAssetBundleFromWebServer : MonoBehaviour
{
	public GameObject parents;
	public GameObject icon;

	private void Start()
	{
		// Get Hash
		AssetBundle manifestBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "AssetBundles"));
		AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		Hash128 hash = manifest.GetAssetBundleHash("loginbutton");

		StartCoroutine(GetLoginAssetBundle("http://127.0.0.1:8888/loginbutton", hash, "loginbutton"));
	}

	private IEnumerator GetLoginAssetBundle(string uri, Hash128 hash, string bundleName)
	{
		while (!Caching.ready)
		{
			yield return null;
		}

		if (!Caching.IsVersionCached("http://127.0.0.1:8888/loginbutton", hash))
		{
			Debug.Log(Caching.IsVersionCached("http://127.0.0.1:8888/loginbutton", hash));
			Caching.ClearAllCachedVersions(bundleName);
		}

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
			login.transform.SetParent(parents.transform);
			login.transform.localPosition = new Vector2(-400, 0);
			icon.GetComponent<UnityEngine.UI.Image>().sprite = login.GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite;

			
			bundleWebRequest.Dispose();
		}
	}
}
