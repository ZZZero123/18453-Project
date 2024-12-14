using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : SingletonBase<AddressablesManager>
{
    private Dictionary<string, IEnumerator> resDic = new Dictionary<string, IEnumerator>();
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="onSuccess">Action<AsyncOperationHandle<T>></param>
    /// <param name="onFailure"></param>
    public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> onSuccess, Action onFailure=null)
    {
        string keyName = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;
        if (resDic.ContainsKey(keyName))
        {
            handle = (AsyncOperationHandle<T>)resDic[keyName];
            if (handle.IsDone)
            {
                onSuccess?.Invoke(handle);
            }
            else
            {
                handle.Completed += (obj) => {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        onSuccess?.Invoke(obj);
                    }
                };
            }
            return;
        }
        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += (obj) => {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                onSuccess?.Invoke(obj);
            }
            else
            {
                Debug.LogWarning(keyName + "resource loading failure");
                onFailure?.Invoke();
                if (resDic.ContainsKey(keyName))
                    resDic.Remove(keyName);
            }
        };
        resDic.Add(keyName, handle);
    }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="onSuccess">Action<T></param>
    /// <param name="onFailure"></param>
    public void LoadAssetAsync<T>(string name, Action<T> onSuccess, Action onFailure=null)
    {

        string keyName = name + "_" + typeof(T).Name;
        AsyncOperationHandle<T> handle;
        if (resDic.ContainsKey(keyName))
        {

            handle = (AsyncOperationHandle<T>)resDic[keyName];
 
            if (handle.IsDone)
            {
                onSuccess?.Invoke(handle.Result); 
            }
            else
            {
                handle.Completed += (obj) => {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        onSuccess?.Invoke(obj.Result);
                    }
                };
            }
            return;
        }

        handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += (obj) => {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                onSuccess?.Invoke(obj.Result);
            }
            else
            {
                Debug.LogWarning(keyName + "resource loading failure");
                onFailure?.Invoke();
                if (resDic.ContainsKey(keyName))
                    resDic.Remove(keyName);
            }
        };
        resDic.Add(keyName, handle);
    }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mode"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <param name="key"></param>
    public void LoadAssetAsync<T>(Addressables.MergeMode mode, Action<T> onSuccess, Action onFailure, params string[] keys)
    {
        AsyncOperationHandle<IList<T>> handle;
        List<string> list = new List<string>(keys);
        string keyName = GetKeyNameFromKeys(keys, typeof(T).Name); ;
        if (resDic.ContainsKey(keyName))
        {
            handle = (AsyncOperationHandle<IList<T>>)resDic[keyName];
            if (handle.IsDone)
            {
                foreach (T item in handle.Result)
                {
                    onSuccess?.Invoke(item);
                }
            }
            else
            {
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (T item in handle.Result)
                    {
                        onSuccess?.Invoke(item);
                    }
                }
            }
            return;
        }
        handle = Addressables.LoadAssetsAsync(list, onSuccess, mode);
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning(keyName + "resource loading failure");
                onFailure?.Invoke();
                if (resDic.ContainsKey(keyName))
                    resDic.Remove(keyName);
            }
        };
        resDic.Add(keyName, handle);
    }
    /// <summary>
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="onFinish"></param>
    /// <param name="onFailure"></param>
    public void LoadSceneAsync(string sceneName, Action onFinish, Action onFailure=null)
    {
        var handle= Addressables.LoadSceneAsync(sceneName);
        handle.Completed += (obj) =>
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                onFinish?.Invoke();
                Addressables.Release(handle);
            }
            else
            {
                Debug.LogWarning($"{sceneName} scene loading failure");
                onFailure?.Invoke();
            }
            
        };
    }
    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onSuccess"></param>
    public void GetDownloadSizeAsync(string name, Action<float> onSuccess, Action onFailure=null)
    {
        MonoManager.Instance.StartCoroutine(I_GetDownLoadSizeAsync(name, onSuccess));
    }
    IEnumerator I_GetDownLoadSizeAsync(string name, Action<float> onSuccess, Action onFailure=null)
    {
        var handleSize = Addressables.GetDownloadSizeAsync(name); 
        yield return handleSize;
        if (handleSize.Result > 0)
        {
            onSuccess?.Invoke(handleSize.Result);
            Addressables.Release(handleSize);
        }
        else
        {
            onFailure?.Invoke();
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onFinish"></param>
    /// <param name="onProgress"></param>
    public void PreDownloadAsync(string name, Action onFinish, Action<DownloadStatus> onProgress)
    {
        MonoManager.Instance.StartCoroutine(I_PreDownloadAsync(name,onFinish,onProgress));
    }
    IEnumerator I_PreDownloadAsync(string name, Action onFinish, Action<DownloadStatus> onProgress)
    {
        var handle = Addressables.DownloadDependenciesAsync(name);
        while (!handle.IsDone)
        {
            onProgress?.Invoke(handle.GetDownloadStatus());
            yield return 0;
        }
        onFinish?.Invoke();
        yield return 0;
       Addressables.Release(handle);
    }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void Release<T>(string name)
    {
        string keyName = name + "_" + typeof(T).Name;
        if (resDic.ContainsKey(keyName))
        {
            Addressables.Release((AsyncOperationHandle<T>)resDic[keyName]);
            resDic.Remove(keyName);
        }
    }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    public void Release<T>(params string[] keys)
    {
        string keyName = GetKeyNameFromKeys(keys, typeof(T).Name);
        if (resDic.ContainsKey(keyName))
        {
            Addressables.Release((AsyncOperationHandle<IList<T>>)resDic[keyName]);
            resDic.Remove(keyName);
        }
    }
    /// <summary>
    /// </summary>
    public void Clear()
    {
        resDic.Clear();
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    /// <summary>
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="typeName"></param>
    /// <returns></returns>
    private string GetKeyNameFromKeys(string[] keys, string typeName)
    {
        List<string> list = new List<string>(keys); 
        string keyName = string.Empty;
        foreach (var key in list)
        {
            keyName += key + "_";
        }
        keyName += typeName;
        return keyName;
    }

}


