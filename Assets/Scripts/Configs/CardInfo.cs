using UnityEngine;

[CreateAssetMenu(fileName = "ACardDescription", menuName = "Config/Card Display")]
public class CardInfo : ScriptableObject
{
    [SerializeField] private CardEnum type;
    [SerializeField] private string displayName;
    [SerializeField] private string[] contents;
    public CardEnum Type => type;
    public string DisplayName => displayName;
    public string[] Contents => contents;
}