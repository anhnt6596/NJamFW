using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ResourceProvider
{
    private static Dictionary<Type, ResourceSet> resourceSets = new Dictionary<Type, ResourceSet>();
    public static void LoadAllResourceSets()
    {
        ResourceSet[] sets = Resources.LoadAll<ResourceSet>("");

        foreach (var set in sets)
        {
            var type = set.GetType();
            if (!resourceSets.ContainsKey(type))
            {
                resourceSets.Add(type, set);
                Debug.Log($"[ResourceProvider] Loaded ResourceSet: {type.Name}");
            }
            else
            {
                Debug.LogWarning($"[ResourceProvider] Duplicate ResourceSet type: {type.Name}");
            }
        }
    }
}
