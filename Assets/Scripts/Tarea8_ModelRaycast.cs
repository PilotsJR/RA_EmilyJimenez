using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;
using UnityEngine.UI;

public class Tarea8_ModelRaycast : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject banner;    
    [SerializeField] private Button closeButton;  
    [SerializeField] private TMP_Text modelName;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image iconImage;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        banner.SetActive(false);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        closeButton.onClick.AddListener(CloseBanner);
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        closeButton.onClick.RemoveListener(CloseBanner);
    }

    void Update()
    {
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (!touch.began)
                continue;

            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.finger.index))
                continue;

            Ray ray = arCamera.ScreenPointToRay(touch.screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InfoModel modelInfo = hit.collider.GetComponent<InfoModel>();

                if (modelInfo != null && modelInfo.Data != null)
                {
                    ShowInfo(modelInfo.Data);
                }
            }
        }
    }

    private void ShowInfo(DataModel data)
    {
        modelName.text = "Name: " + data.name;
        descriptionText.text = "Description: " + data.description;
        categoryText.text = "Category: " + data.category;
        levelText.text = "Level: " + data.level;
        iconImage.sprite = data.icon;

        banner.SetActive(true);

        if (audioSource != null && data.sound != null)
            audioSource.PlayOneShot(data.sound);

        Debug.Log("Objeto seleccionado: " + data.name);
    }

    private void CloseBanner()
    {
        banner.SetActive(false);
    }
}