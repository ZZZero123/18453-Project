using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScaleUtil
{
    public static Vector3 ScaleToFit(GameObject item, Bounds targetBounds)
    {
        MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>();
        //将所有的网格渲染的边界进行合并
        Bounds centerBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            centerBounds.Encapsulate(meshRenderers[i].bounds);
        }
        Vector3 targetScale = GetScaleToFitWithinBound(centerBounds, targetBounds); 
        return targetScale;
    }
    /// <summary>
    /// 把目标缩放到特定物体的缩放
    /// </summary>
    /// <param name="boundsToChange">需要缩放的物体Bounds</param>
    /// <param name="targetBounds">target需要缩放到的目标Bounds</param>
    /// <returns></returns>
    public static Vector3 GetScaleToFitWithinBound(Bounds boundsToChange, Bounds targetBounds)
    {
        //模型最终的缩放大小(localScale) / 模型初始的缩放大小  = 目标边界盒(Bounds)的大小 / 模型网格的边界盒大小
        //模型最终的缩放大小(localScale) = (目标边界盒(Bounds)的大小 / 模型网格的边界盒大小) * 模型初始的缩放大小
        float targetScale = targetBounds.size.magnitude / boundsToChange.size.magnitude;  
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        return targetScaleVector;
    }
}
