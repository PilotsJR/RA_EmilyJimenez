using UnityEngine;

public class ModelInfoo : MonoBehaviour
{
    [SerializeField] private string modelName;

    public string ModelName => modelName;
    //public string MyModelName { get => modelName; }
    //public string SetModelName { set => modelName = value; }
    //public string SelectedModelName { get => modelName; set => modelName = value; }
    
    //public string GetModelName()
    //{
    //    return modelName;
    //}

    //public void SetNewModelName(string newModelName)
    //{
    //    modelName = newModelName;
    //}
}
