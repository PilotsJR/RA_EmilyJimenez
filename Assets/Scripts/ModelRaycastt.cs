using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class ModelRaycastt : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private TMP_Text modelName;
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
                if (modelInfo != null)
                {
                    modelName.text = modelInfo.ModelName;
                    //modelInfo.SetModelName = "Pan";
                    //modelInfo.SelectedModelName = "Bolillo";
                    //modelInfo.SetNewModelName("Concha");
                }
            }
        }
    }
}
