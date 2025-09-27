using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HaveTapSound : MonoBehaviour
{
    private void Awake()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SoundManager.Play(ResourceProvider.Sound.general.button));
    }
}