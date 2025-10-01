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

    public static T GetSet<T>() where T : ResourceSet
    {
        if (resourceSets.ContainsKey(typeof(T))) return (T)resourceSets[typeof(T)];
        return default;
    }

    public static SoundResourceSet Sound => GetSet<SoundResourceSet>();
    public static IconResourceSet Icon => GetSet<IconResourceSet>();
    public static GameComponentSet Component => GetSet<GameComponentSet>();
    public static EffectSet Effect => GetSet<EffectSet>();
    public static Level GetLevel(int levelIdx) => GetSet<LevelSet>().levels[levelIdx];

    public static Sprite GetCardArt(CardEnum card)
    {
        return Resources.Load<Sprite>($"Images/CardArts/{card}");
    }

    public static EnemyVisual GetEnemyVisual(EnemyEnum enemy)
    {
        return Resources.Load<EnemyVisual>($"Prefabs/Enemies/{enemy}");
    }
}
