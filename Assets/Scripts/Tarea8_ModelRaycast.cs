using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;
using UnityEngine.UI;

public class Tarea8_ModelRaycast : MonoBehaviour
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

        void Update()
        {
            foreach(var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
            {
                if (!touch.began)
                    continue;

                Ray ray = arCamera.ScreenPointToRay(touch.screenPosition);

                if(Physics.Raycast(ray, out RaycastHit hit))
                {

                InfoModel modelInfo = hit.collider.GetComponent<InfoModel>();

                    if(modelInfo !=null && modelInfo.Data != null)
                    {

                    ShowInfo(modelInfo.Data);
                       //modelName.text = modelInfo.ModelName;
                    }
                }
            }
        }
    private void ShowInfo(DataModel data)
    {
        modelName.text = data.name;
        descriptionText.text = data.description;
        categoryText.text = data.category;
        levelText.text = "Nivel: " + data.level;
        iconImage.sprite = data.icon;

        Debug.Log("Objeto seleccionado: " + data);
    }
}




