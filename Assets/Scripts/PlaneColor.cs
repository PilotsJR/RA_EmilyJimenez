using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneColor : MonoBehaviour
{
    [Header("Colores")]
    [SerializeField] private Color horizontalColor = Color.yellow;
    [SerializeField] private Color verticalColor = Color.blue;

    [Header("Etiqueta de dimensiones")]
    [Tooltip("Texto en World Space (hijo de este prefab) que muestra ancho x largo del plano.")]
    [SerializeField] private TextMeshPro dimensionsLabel;

    private ARPlane plane;
    private MeshRenderer meshRenderer;
    private Material materialInstance;

    private void Awake()
    {
        plane = GetComponent<ARPlane>();
        meshRenderer = GetComponent<MeshRenderer>();
        materialInstance = meshRenderer.material;
    }

    private void OnEnable()
    {
        plane.boundaryChanged += OnBoundaryChanged;
        UpdateVisuals();
    }

    private void OnDisable()
    {
        plane.boundaryChanged -= OnBoundaryChanged;
    }

    private void OnBoundaryChanged(ARPlaneBoundaryChangedEventArgs args)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        bool isHorizontal = plane.alignment == PlaneAlignment.HorizontalUp ||
                             plane.alignment == PlaneAlignment.HorizontalDown;

        materialInstance.color = isHorizontal ? horizontalColor : verticalColor;

        if (dimensionsLabel != null)
        {
            Vector2 size = plane.size;
            dimensionsLabel.text = $"{size.x:F2} m x {size.y:F2} m";

            if (Camera.main != null)
            {
                dimensionsLabel.transform.rotation = Quaternion.LookRotation(
                    dimensionsLabel.transform.position - Camera.main.transform.position);
            }
        }
    }
}