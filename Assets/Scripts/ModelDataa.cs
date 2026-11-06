using UnityEngine;

[CreateAssetMenu(fileName = "ModelDataa", menuName = "Scriptable Objects/ModelDataa")]
public class ModelDataa : ScriptableObject
{
    public string modelName;

    [TextArea(3, 6)]
    public string description;

    public string category;
    public int level;
    public Sprite icon;
}
