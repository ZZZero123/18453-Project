
using UnityEngine;
using UnityEditor;

public class ResetPivotTool : ScriptableObject
{
    [MenuItem("Tools/MyTool/ResetPivotToCenter")]
    static void ResetPivotToCenter()
    {
        GameObject target = Selection.activeGameObject;
        string dialogTitle = "Tools/MyTool/ResetPivotToCenter";

        if (target == null)
        {
            EditorUtility.DisplayDialog(dialogTitle, "û��ѡ����Ҫ�������ĵ�����!!!", "ȷ��");
            return;
        }

        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderers.Length == 0)
        {
            EditorUtility.DisplayDialog(dialogTitle, "ѡ�е����岻����Чģ������!!!", "ȷ��");
            return;
        }
        Bounds centerBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            centerBounds.Encapsulate(meshRenderers[i].bounds);
        }
        Transform targetParent = new GameObject(target.name + "-Parent").transform;

        Transform originalParent = target.transform.parent;
        if (originalParent != null)
        {
            targetParent.SetParent(originalParent);
        }
        targetParent.position = centerBounds.center;
        target.transform.parent = targetParent;

        Selection.activeGameObject = targetParent.gameObject;
        EditorUtility.DisplayDialog(dialogTitle, "����ģ��������������������!", "ȷ��");
    }
    [MenuItem("Tools/MyTool/ResetPivotToBottom")]
    static void ResetPivotToBottom()
    {
        GameObject target = Selection.activeGameObject;
        string dialogTitle = "Tools/MyTool/ResetPivotToBottom";

        if (target == null)
        {
            EditorUtility.DisplayDialog(dialogTitle, "û��ѡ����Ҫ�������ĵ�����!!!", "ȷ��");
            return;
        }

        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderers.Length == 0)
        {
            EditorUtility.DisplayDialog(dialogTitle, "ѡ�е����岻����Чģ������!!!", "ȷ��");
            return;
        }

        Bounds combinedBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            combinedBounds.Encapsulate(meshRenderers[i].bounds);
        }

        Transform targetParent = new GameObject(target.name + "-Parent").transform;

        Transform originalParent = target.transform.parent;
        if (originalParent != null)
        {
            targetParent.SetParent(originalParent);
        }

        Vector3 bottomCenter = new Vector3(combinedBounds.center.x, combinedBounds.min.y, combinedBounds.center.z);

        targetParent.position = bottomCenter;

        target.transform.parent = targetParent;

        Selection.activeGameObject = targetParent.gameObject;
        EditorUtility.DisplayDialog(dialogTitle, "����ģ����������ĵ��ײ����!", "ȷ��");
    }
    [MenuItem("Tools/MyTool/ResetPivotToTop")]
    static void ResetPivotToTop()
    {

        GameObject target = Selection.activeGameObject;
        string dialogTitle = "Tools/MyTool/ResetPivotToTop";

        if (target == null)
        {
            EditorUtility.DisplayDialog(dialogTitle, "û��ѡ����Ҫ�������ĵ�����!!!", "ȷ��");
            return;
        }

        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderers.Length == 0)
        {
            EditorUtility.DisplayDialog(dialogTitle, "ѡ�е����岻����Чģ������!!!", "ȷ��");
            return;
        }

        Bounds combinedBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            combinedBounds.Encapsulate(meshRenderers[i].bounds);
        }

        Transform targetParent = new GameObject(target.name + "-Parent").transform;

        Transform originalParent = target.transform.parent;
        if (originalParent != null)
        {
            targetParent.SetParent(originalParent);
        }

        Vector3 bottomCenter = new Vector3(combinedBounds.center.x, combinedBounds.max.y, combinedBounds.center.z);

        targetParent.position = bottomCenter;

        target.transform.parent = targetParent;

        Selection.activeGameObject = targetParent.gameObject;
        EditorUtility.DisplayDialog(dialogTitle, "����ģ����������ĵ��������!", "ȷ��");
    }
}
