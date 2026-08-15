using UnityEngine;

public class PlayButtonsController : MonoBehaviour
{
    [SerializeField] DeckManager deckManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void ConnectEvents(DragController drag)
    {
        drag.playButtonsShow += OnButtonsShow;
        drag.playButtonsHide += OnButtonsHide;
    }

    private void OnButtonsShow()
    {
        canvas.enabled = true;
    }

    private void OnButtonsHide()
    {
        canvas.enabled = false;
    }
    public void MoverCartaBaza()
    {
        deckManager.MoveSelectedCardsToSpot(false);
    }

    public void MoverCartasCante()
    {
        deckManager.MoveSelectedCardsToSpot(true);
    }
    public void ConfirmarBaza()
    {
        Debug.Log("TODO: calcular score");
    }
}
