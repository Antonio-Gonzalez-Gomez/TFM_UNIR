using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;

public class InfoPopUp : MonoBehaviour
{
    [SerializeField] public TMP_Text cardTitle;
    [SerializeField] public TMP_Text cardDescription;
    [SerializeField] CanvasScaler parentCanvasScale;
    [SerializeField] public VerticalLayoutGroup verticalGroup;
    [SerializeField] public GameObject auxiliarInfoPrefab;

    private Canvas canvas;
    private RectTransform rect;
    //Variables para calcular posiciones en pantalla
    private float maxHeight;
    private float maxWidth;
    private Vector2 screenSize;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        rect = GetComponent<RectTransform>();

        maxHeight = Camera.main.orthographicSize;
        maxWidth = maxHeight * Screen.width / Screen.height;
        screenSize = parentCanvasScale.referenceResolution;
    }

    //Calcula el punto de anclaje (coordenadas sobre el canvas padre)
    //A partir de la posicion de un objeto de juego
    private void AnchorToWorldPosition(Vector3 worldPosition)
    {
        rect.anchoredPosition = new Vector2(0.5f * screenSize.x * worldPosition.x / maxWidth,
            0.5f * screenSize.y * worldPosition.y / maxHeight);
    }

    public void ConnectEvents(DragController drag)
    {
        drag.cardInfoShow += OnCardShown;
        drag.cardInfoHide += OnCardHidden;
    }

    List<GameObject> infoChildren = new List<GameObject>();
    private void OnCardShown(HoverInfo info)
    {
        canvas.enabled = true;
        cardTitle.text = info.Title;
        cardDescription.text = info.Description;

        for (int i = 0; i < info.AdditionalTitle.Count; i++)
        {
            GameObject additionalInfo = Instantiate(auxiliarInfoPrefab, verticalGroup.transform);

            TMP_Text[] additionalText = additionalInfo.GetComponentsInChildren<TMP_Text>();
            additionalText[0].text = info.AdditionalTitle[i];
            additionalText[1].text = info.AdditionalDescription[i];
            infoChildren.Add(additionalInfo);
        }

        //Conversion de posicion de la carta a posicion en el canvas
        AnchorToWorldPosition(info.Position);
    }

    private void OnCardHidden()
    {
        canvas.enabled = false;
        foreach (GameObject additionalInfo in infoChildren)
        {
            Destroy(additionalInfo);
        }
    }
}
