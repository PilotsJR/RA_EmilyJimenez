using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ModelPlacer : MonoBehaviour
{
    [Header("Referencias AR")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        if (ModelSelector.Instance == null || !ModelSelector.Instance.HasSelected)
            return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            TryPlaceObject(touchPosition);
        }
    }

    private void TryPlaceObject(Vector2 screenPosition)
    {
        if (!raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
            return;

        ARRaycastHit hit = hits[0];
        Pose hitPose = hit.pose;

        ARPlane plane = planeManager.GetPlane(hit.trackableId);
        if (plane == null)
            return;

        bool isHorizontal = plane.alignment == PlaneAlignment.HorizontalUp ||
                             plane.alignment == PlaneAlignment.HorizontalDown;

        GameObject prefabToInstantiate = isHorizontal
            ? ModelSelector.Instance.HorizontalModelPrefab
            : ModelSelector.Instance.VerticalModelPrefab;

        if (prefabToInstantiate == null)
            return;

        Instantiate(prefabToInstantiate, hitPose.position, hitPose.rotation);

        if (StatusManager.Instance != null)
        {
            StatusManager.Instance.NotifyObjectPlaced(isHorizontal);
        }
    }
}