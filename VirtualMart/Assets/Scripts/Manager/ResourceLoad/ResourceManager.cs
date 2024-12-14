using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : SingletonBase<ResourceManager> 
{
	/// <summary>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <returns></returns>
	public T Load<T>(string path) where T: UnityEngine.Object
    {
		T resource = Resources.Load<T>(path);
		if (resource is GameObject)
			return GameObject.Instantiate(resource);
		else//TextAsset AudioClip
			return resource;

    }
	/// <summary>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <param name="callBack"></param>
	/// <param name="resourceLoadWay"></param>
	public void LoadAsync<T>(string path, Action<T> callBack, ResourceLoadWay resourceLoadWay = ResourceLoadWay.Addressables) where T : UnityEngine.Object
	{
		if (resourceLoadWay == ResourceLoadWay.Addressables)
		{
			AddressablesManager.Instance.LoadAssetAsync<T>(path, (res) =>
            {
				if (res is GameObject)
                {
					callBack?.Invoke(GameObject.Instantiate(res)) ;
				}
                else
                {
					callBack?.Invoke(res);
				}				
			}); 
		}
		else if (resourceLoadWay == ResourceLoadWay.Resources)
		{
			MonoManager.Instance.StartCoroutine(I_LoadResourceAsync<T>(path, callBack));
		}
	}
	IEnumerator I_LoadResourceAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
	{
		ResourceRequest request = Resources.LoadAsync<T>(path);
		yield return request;
		if (request.asset is GameObject)
		{			
			callBack(GameObject.Instantiate(request.asset) as T);
		}
		else callBack(request.asset as T);
	}
	

}

public enum ResourceLoadWay
{
	Addressables,
	Resources,
	SceneManagement
}
