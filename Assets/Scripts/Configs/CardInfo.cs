using UnityEngine;

[CreateAssetMenu(fileName = "ACardDescription", menuName = "Config/Card Display")]
public class CardInfo : ScriptableObject
{
    [SerializeField] private CardEnum type;
    [SerializeField] private string displayName;
    [SerializeField] private string[] contents;
    [SerializeField] private string playDescription;
    public CardEnum Type => type;
    public string DisplayName => displayName;
    public string[] Contents => contents;
    public string PlayDescription => playDescription;
}