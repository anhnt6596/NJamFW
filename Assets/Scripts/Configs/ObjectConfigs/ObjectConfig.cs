using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "An Object Config", menuName = "Config/Object")]
public class ObjectConfig : ScriptableObject
{
    [SerializeField] PlaceObjectEnum type;
    [SerializeField] GridSize size;

    public PlaceObjectEnum Type => type;
    public GridSize Size => size;
}
