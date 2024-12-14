using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScaleUtil
{
    public static Vector3 ScaleToFit(GameObject item, Bounds targetBounds)
    {
        MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>();
        //�����е�������Ⱦ�ı߽���кϲ�
        Bounds centerBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            centerBounds.Encapsulate(meshRenderers[i].bounds);
        }
        Vector3 targetScale = GetScaleToFitWithinBound(centerBounds, targetBounds); 
        return targetScale;
    }
    /// <summary>
    /// ��Ŀ�����ŵ��ض����������
    /// </summary>
    /// <param name="boundsToChange">��Ҫ���ŵ�����Bounds</param>
    /// <param name="targetBounds">target��Ҫ���ŵ���Ŀ��Bounds</param>
    /// <returns></returns>
    public static Vector3 GetScaleToFitWithinBound(Bounds boundsToChange, Bounds targetBounds)
    {
        //ģ�����յ����Ŵ�С(localScale) / ģ�ͳ�ʼ�����Ŵ�С  = Ŀ��߽��(Bounds)�Ĵ�С / ģ������ı߽�д�С
        //ģ�����յ����Ŵ�С(localScale) = (Ŀ��߽��(Bounds)�Ĵ�С / ģ������ı߽�д�С) * ģ�ͳ�ʼ�����Ŵ�С
        float targetScale = targetBounds.size.magnitude / boundsToChange.size.magnitude;  
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        return targetScaleVector;
    }
}
