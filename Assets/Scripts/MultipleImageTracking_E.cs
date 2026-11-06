using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImageTracking_E : MonoBehaviour
{
    [Serializable]
    public class ImagePrefab
    {
        public string imageName;        // Nombre exacto en la Reference Image Library
        public GameObject imagePrefab;
    }

    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

    [Header("Collider")]
    [Tooltip("Agrega un BoxCollider en la raíz del modelo que cubre todos sus meshes")]
    [SerializeField] private bool autoFitCollider = true;

    private readonly Dictionary<TrackableId, GameObject> spawnedObjects = new();

    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImagesChanged);
    }

    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImagesChanged);
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

                    if (autoFitCollider) FitBoxCollider(instancedPrefab);

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
                Destroy(instancedPrefab);
                spawnedObjects.Remove(removed.Key);
            }
        }
    }

    // Crea un BoxCollider en la raíz que cubre todos los meshes hijos
    private void FitBoxCollider(GameObject root)
    {
        // Si el prefab ya trae un BoxCollider en la raíz, se reemplaza por uno ajustado
        var existing = root.GetComponent<BoxCollider>();
        if (existing != null) Destroy(existing);

        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        bool first = true;
        Bounds local = new Bounds();

        foreach (var r in renderers)
        {
            Bounds b = r.bounds; // espacio mundo
            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = b.center + Vector3.Scale(b.extents, new Vector3(
                    (i & 1) == 0 ? -1 : 1,
                    (i & 2) == 0 ? -1 : 1,
                    (i & 4) == 0 ? -1 : 1));

                Vector3 p = root.transform.InverseTransformPoint(corner);

                if (first) { local = new Bounds(p, Vector3.zero); first = false; }
                else local.Encapsulate(p);
            }
        }

        var box = root.AddComponent<BoxCollider>();
        box.center = local.center;
        box.size = local.size;
    }
}