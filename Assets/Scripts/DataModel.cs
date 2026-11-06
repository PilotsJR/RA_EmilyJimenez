using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewModelData", 
    menuName = "AR/Model Data"
    )]
public class DataModel : ScriptableObject
{
    public string modelName;

    [TextArea(3, 6)]
    public string description;

    public string category;
    public int level;
    public Sprite icon;
    
}
