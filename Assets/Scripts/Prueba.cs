using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Prueba : MonoBehaviour
{
    [Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public GameObject imagePrefab;
    }

    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

    private Dictionary<TrackableId, GameObject> spawnedObjects = new Dictionary<TrackableId, GameObject>();

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
                    //instancedPrefab.transform.localRotation = Quaternion.identity;

                    spawnedObjects.Add(image.trackableId, instancedPrefab);

                    instancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);

                    break;
                }
            }
        }

        foreach(var image in args.updated)
        {
            if(spawnedObjects.TryGetValue(image.trackableId, out GameObject instancedPrefab))
            {
                instancedPrefab.SetActive(image.trackingState == TrackingState.Tracking);
            }
        }

        foreach(var removed in args.removed)
        {
            if(spawnedObjects.TryGetValue(removed.Key, out GameObject instancedPrefab))
            {
                Destroy(instancedPrefab);
                spawnedObjects.Remove(removed.Key);
            }
        }
    }
}