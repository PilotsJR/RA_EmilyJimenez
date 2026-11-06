using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;
using UnityEngine.UI;

public class ModelRaycastt : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private TMP_Text modelName;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image iconImage;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
    private void Update()
    {
        foreach(var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (!touch.began)
            {
                continue;
            }
            Ray ray = arCamera.ScreenPointToRay(touch.screenPosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                ModelInfoo modelInfo = hit.collider.GetComponent<ModelInfoo>();
                if (modelInfo != null && modelInfo.Data != null)
                {
                    ShowInfo(modelInfo.Data);
                    //modelName.text = modelInfo.ModelName;
                    //modelInfo.SetModelName = "Pan";
                    //modelInfo.SelectedModelName = "Bolillo";
                    //modelInfo.SetNewModelName("Concha");
                }
            }
        }
    }

    private void ShowInfo(ModelDataa data)
    {
        modelName.text = data.name;
        descriptionText.text = data.description;
        categoryText.text = data.category;
        levelText.text = "Nivel: "+ data.level;
        iconImage.sprite = data.icon;
    }
}
