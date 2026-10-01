using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance { get; private set; }

    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private TextMeshProUGUI statusText;

    private bool horizontalDetected = false;
    private bool verticalDetected = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
        SetStatus("Escaneando entorno ...");
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach (ARPlane plane in args.added)
        {
            bool isHorizontal = plane.alignment == PlaneAlignment.HorizontalUp ||
                                 plane.alignment == PlaneAlignment.HorizontalDown;

            if (isHorizontal && !horizontalDetected)
            {
                horizontalDetected = true;
                SetStatus("Superficie horizontal detectada!");
            }
            else if (plane.alignment == PlaneAlignment.Vertical && !verticalDetected)
            {
                verticalDetected = true;
                SetStatus("Superficie vertical detectada!");
            }
        }
    }

    public void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    public void NotifyObjectPlaced(bool onHorizontalPlane)
    {
        SetStatus(onHorizontalPlane
            ? "Objeto colocado en plano horizontal"
            : "Objeto colocado en plano vertical");
    }
}