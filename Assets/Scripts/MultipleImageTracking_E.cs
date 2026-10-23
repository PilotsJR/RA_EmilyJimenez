using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MultipleImageTracking_E : MonoBehaviour
{
    [Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public GameObject imagePrefab;
    }

    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

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
       foreach(var image in args.added)
        {
            foreach(var item in imagePrefabs)
            {
                if(image.referenceImage.name == item.imageName && item.imagePrefab != null)
                {
                    GameObject instancedPrefab = Instantiate(item.imagePrefab, image.transform);

                    instancedPrefab.transform.localPosition = Vector3.zero;
                }
                break;
            }
        }
    }
}
