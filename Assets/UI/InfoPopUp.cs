using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private UITools uit;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        rect = GetComponent<RectTransform>();

        uit = new UITools(parentCanvasScale.referenceResolution);
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
        rect.anchoredPosition = uit.WorldPositionToAnchor(info.Position);
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
