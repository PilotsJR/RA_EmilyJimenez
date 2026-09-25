using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    [Header("Referencias AR")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;

    [Header("Modelos a instanciar")]
    [SerializeField] private GameObject horizontalModelPrefab;
    [SerializeField] private GameObject verticalModelPrefab;

    // Lista reutilizable para no generar basura (GC) en cada frame
    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update()
    {
        // Con el New Input System detectamos el toque desde Touchscreen.current
        if (Touchscreen.current == null)
            return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            TryPlaceObject(touchPosition);
        }
    }

    private void TryPlaceObject(Vector2 screenPosition)
    {
        // Solo nos interesan los hits dentro del polígono detectado del plano
        if (!raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
            return;

        ARRaycastHit hit = hits[0];
        Pose hitPose = hit.pose;

        // Obtenemos el ARPlane concreto que fue golpeado, a partir de su trackableId
        ARPlane plane = planeManager.GetPlane(hit.trackableId);
        if (plane == null)
            return;

        GameObject prefabToInstantiate = GetPrefabForAlignment(plane.alignment);

        if (prefabToInstantiate != null)
        {
            Instantiate(prefabToInstantiate, hitPose.position, hitPose.rotation);
        }
    }

    private GameObject GetPrefabForAlignment(PlaneAlignment alignment)
    {
        switch (alignment)
        {
            case PlaneAlignment.HorizontalUp:
            case PlaneAlignment.HorizontalDown:
                return horizontalModelPrefab;

            case PlaneAlignment.Vertical:
                return verticalModelPrefab;

            default:
                return null;
        }
    }
}