using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TransformExt
{
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
    public static IEnumerable<Transform> DirectChildren(this Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
            yield return t.GetChild(i);
    }

    public static IEnumerable<GameObject> DirectChildGameObjects(this Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
            yield return t.GetChild(i).gameObject;
    }
}