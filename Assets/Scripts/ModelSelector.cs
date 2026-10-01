using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelSelector : MonoBehaviour
{
    public static ModelSelector Instance { get; private set; }

    [Header("Los dos modelos disponibles")]
    [SerializeField] private GameObject modelAPrefab;
    [SerializeField] private GameObject modelBPrefab;

    [Header("UI de selección")]
    [SerializeField] private Button modelAButton;
    [SerializeField] private Button modelBButton;
    [SerializeField] private TextMeshProUGUI instructionText;
    [Tooltip("GameObject que agrupa los botones y el texto, para ocultarlo todo de una vez tras elegir.")]
    [SerializeField] private GameObject selectionPanel;

    public GameObject HorizontalModelPrefab { get; private set; }
    public GameObject VerticalModelPrefab { get; private set; }
    public bool HasSelected { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (instructionText != null)
        {
            instructionText.text = "Escoge un modelo para poner en los planos horizontales. El otro será solo para los planos verticales.";
        }

        modelAButton.onClick.AddListener(() => SelectModel(modelAPrefab, modelBPrefab));
        modelBButton.onClick.AddListener(() => SelectModel(modelBPrefab, modelAPrefab));
    }

    private void SelectModel(GameObject chosenForHorizontal, GameObject remainingForVertical)
    {
        HorizontalModelPrefab = chosenForHorizontal;
        VerticalModelPrefab = remainingForVertical;
        HasSelected = true;

        if (selectionPanel != null)
        {
            selectionPanel.SetActive(false);
        }
    }
}