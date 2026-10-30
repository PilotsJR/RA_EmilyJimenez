using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImageTracking_E : MonoBehaviour
{
    [Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public string displayName;
        public GameObject imagePrefab;
    }

    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

    [Header("Selección / UI")]
    [SerializeField] private Camera arCamera;
    [SerializeField] private TMP_Text selectedModelText;
    [SerializeField] private float maxRayDistance = 20f;

    private readonly Dictionary<TrackableId, GameObject> spawnedObjects = new();
    private GameObject selectedModel;

    private void Start()
    {
        if (arCamera == null) arCamera = Camera.main;
        UpdateSelectionUI(null);
    }

    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImagesChanged);
    }

    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImagesChanged);
    }

    private void Update()
    {
        HandleTouchSelection();

        if (selectedModel != null && !selectedModel.activeInHierarchy)
        {
            selectedModel = null;
            UpdateSelectionUI(null);
        }
    }


    private void OnImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var image in args.added)
        {
            foreach (var item in imagePrefabs)
            {
                if (image.referenceImage.name == item.imageName && item.imagePrefab != null)
                {
                    GameObject instancedPrefab = Instantiate(item.imagePrefab, image.transform);
                    instancedPrefab.transform.localPosition = Vector3.zero;

                    var info = instancedPrefab.GetComponent<ModelInfo>();
                    if (info == null) info = instancedPrefab.AddComponent<ModelInfo>();
                    info.displayName = string.IsNullOrEmpty(item.displayName) ? item.imageName : item.displayName;

                    spawnedObjects[image.trackableId] = instancedPrefab;
                    instancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);
                    break;
                }
            }
        }

        foreach (var image in args.updated)
        {
            if (spawnedObjects.TryGetValue(image.trackableId, out GameObject instancedPrefab))
            {
                instancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);
            }
        }

        foreach (var removed in args.removed)
        {
            if (spawnedObjects.TryGetValue(removed.Key, out GameObject instancedPrefab))
            {
                if (instancedPrefab == selectedModel)
                {
                    selectedModel = null;
                    UpdateSelectionUI(null);
                }

                Destroy(instancedPrefab);
                spawnedObjects.Remove(removed.Key);
            }
        }
    }

    private void HandleTouchSelection()
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;

        var touch = touchscreen.primaryTouch;
        if (!touch.press.wasPressedThisFrame) return;

        Vector2 screenPos = touch.position.ReadValue();
        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            var info = hit.collider.GetComponentInParent<ModelInfo>();
            if (info != null)
            {
                selectedModel = info.gameObject;
                UpdateSelectionUI(info.displayName);
                return;
            }
        }

        selectedModel = null;
        UpdateSelectionUI(null);
    }

    private void UpdateSelectionUI(string modelName)
    {
        if (selectedModelText == null) return;

        selectedModelText.text = string.IsNullOrEmpty(modelName)
            ? "Ningún modelo seleccionado"
            : $"Modelo seleccionado: {modelName}";
    }
}

public class ModelInfo : MonoBehaviour
{
    public string displayName;
}