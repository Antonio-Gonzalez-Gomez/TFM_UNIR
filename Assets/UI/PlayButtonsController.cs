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

        scoreManager.pointScoreAction += OnPointIncrease;
        scoreManager.valueScoreAction += OnValueIncrease;
        scoreManager.bonusScoreAction += OnBonusIncrease;
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
            int score = scoreManager.CalculateScore();
            Debug.Log("Puntuación final: " + score.ToString());
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

    //TODO: Efectos de interfaz cada vez que la puntuacion sube
    private void OnPointIncrease(int puntos)
    {
        Debug.Log("+ " + puntos.ToString() + " puntos!");
    }
    private void OnValueIncrease(int valor)
    {
        Debug.Log("+ " + valor.ToString() + " valor!");
    }
    private void OnBonusIncrease(int bonus)
    {
        Debug.Log("+ " + bonus.ToString() + " bonus!");
    }
}
