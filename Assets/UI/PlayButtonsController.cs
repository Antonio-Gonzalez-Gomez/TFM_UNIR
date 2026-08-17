using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonsController : MonoBehaviour
{
    [SerializeField] DeckManager deckManager;
    [SerializeField] ScoreManager scoreManager;
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
        if (scoreManager.EsBazaGanada())
        {
            Debug.Log("Ganas la baza");
            Cante cante = scoreManager.EvaluarCante();
            Debug.Log(cante);
            foreach (CardInstance carta in scoreManager.scoringCards)
            {
                Debug.Log(carta.cartaBase.ToString());
            }
        }
        else
        {
            Debug.Log("Pierdes la baza");
        }
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
