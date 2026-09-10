using PrimeTween;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] DeckManager deckManager;
    [SerializeField] ScoreManager scoreManager;
    [Header("Effects timing")]
    [SerializeField] float initialEffectDuration = 0.4f;
    [SerializeField] float finalEffectDuration = 0.02f;
    [SerializeField] float decreaseEffectDuration = 0.02f;
    [Header("Scoreboard")]
    [SerializeField] TMP_Text handType;
    [SerializeField] Canvas scoreEquation;
    [SerializeField] TMP_Text scoreLeft;
    [SerializeField] TMP_Text scoreEx;
    [SerializeField] TMP_Text scoreMiddle;
    [SerializeField] TMP_Text scorePlus;
    [SerializeField] TMP_Text scoreRight;
    [SerializeField] TMP_Text totalScore;

    private Canvas buttonsCanvas;
    private float effectDuration;
    private ColorText ct = new ColorText();
    private Vector3 scoreboardShake = new Vector3(0, 20, 0);
    private Vector3 cardShake = new Vector3(0, 0.1f, 0);

    private void Start()
    {
        buttonsCanvas = GetComponent<Canvas>();
        effectDuration = initialEffectDuration;

        handType.alpha = 0;
        scoreEquation.enabled = false;
        totalScore.text = "0 / " + scoreManager.targetScore.ToString();

        scoreManager.PointScoreAction += OnPointIncrease;
        scoreManager.ValueScoreAction += OnValueIncrease;
        scoreManager.BonusScoreAction += OnBonusIncrease;
        scoreManager.ScoreReadyAction += OnScoreEndCalculate;
    }


    public void ConnectEvents(DragController drag)
    {
        drag.cardDragBegin += OnCardDrag;
        drag.cardDragEnd += OnCardDrop;
    }
    //Al arrastrar cartas, se ocultan los botones de juego
    private void OnCardDrag()
    {
        buttonsCanvas.enabled = false;
    }
    private void OnCardDrop()
    {
        buttonsCanvas.enabled = true;
        UpdateScoreboardHand();
    }

    private void UpdateScoreboardHand()
    {
        scoreEquation.enabled = false;
        if (scoreManager.bazaSpot.cardList.Count == 0)
        {
            handType.alpha = 0;
            return;
        }

        handType.alpha = 1;
        if (!scoreManager.EsBazaGanada())
        {
            handType.text = "Baza perdida";
            return;
        }

        scoreEquation.enabled = true;
        Cante canteActual = scoreManager.EvaluarCante();
        handType.text = CanteDicts.text[canteActual];
        scoreLeft.text = ct.Color("0", "points");
        scoreMiddle.text = ct.Color(CanteDicts.valores[canteActual].ToString(), "value");
        scoreRight.text = ct.Color(CanteDicts.bonus[canteActual].ToString(), "bonus");
    }


    public void MoverCartaBaza()
    {
        deckManager.MoveSelectedCardsToSpot(false);
        UpdateScoreboardHand();
    }

    public void MoverCartasCante()
    {
        deckManager.MoveSelectedCardsToSpot(true);
        UpdateScoreboardHand();
    }
    public async void ConfirmarBaza()
    {
        if (scoreManager.EsBazaGanada())
        {
            await scoreManager.ScorePlayedHand();
        }
        else
        {
            scoreManager.LosePlayedHand();
        }
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    int score = 0;
    public void ScoreTest()
    {
        score += 100;
        scoreLeft.text = ct.Color(score.ToString(), "points");
        //Tween.ShakeLocalRotation(scorePoints.transform, rot, effectDuration);
    }

    private void testTweens()
    {
        //Tween.ShakeLocalRotation(scoreEquation.transform, rot, effectDuration);
    }

    private void ReduceEffectDuration()
    {
        effectDuration = effectDuration <= finalEffectDuration ?
            finalEffectDuration :
            effectDuration - decreaseEffectDuration;
    }

    //TODO: Carta/Aumento -> efecto de subida de puntos (hacia arriba desde la carta?)
    private async Task OnPointIncrease(DragController drag, int puntos)
    {
        scoreLeft.text = ct.Color(scoreManager.puntosJugada.ToString(), "points");
        await PrimeTween.Sequence.Create().
            Group(Tween.ShakeLocalPosition(scoreLeft.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.PunchLocalPosition(drag.transform, strength: cardShake, duration: effectDuration)).
            ChainCallback(() => ReduceEffectDuration());
    }
    private async Task OnValueIncrease(DragController drag, int valor)
    {
        scoreMiddle.text = ct.Color(scoreManager.valorJugada.ToString(), "value");
        await PrimeTween.Sequence.Create().
            Group(Tween.ShakeLocalPosition(scoreMiddle.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.ShakeLocalPosition(drag.transform, strength: cardShake, duration: effectDuration)).
            ChainCallback(() => ReduceEffectDuration());
    }
    private async Task OnBonusIncrease(DragController drag, int bonus)
    {
        scoreRight.text = ct.Color(scoreManager.bonusJugada.ToString(), "bonus");
        await PrimeTween.Sequence.Create().
            Group(Tween.ShakeLocalPosition(scoreRight.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.ShakeLocalPosition(drag.transform, strength: cardShake, duration: effectDuration)).
            ChainCallback(() => ReduceEffectDuration());
    }

    private void UpdatePseudoScore(int pseudoScore)
    {
        scoreMiddle.text = ct.Color(pseudoScore.ToString(), "bonus");
    }

    private void UpdateFinalScore(int score)
    {
        scoreMiddle.text = score.ToString();
    }

    private void UpdateTotalScore()
    {
        totalScore.text = scoreManager.totalScore.ToString() + " / " + scoreManager.targetScore.ToString();
    }

    //TODO: investigar otros efectos (el fade in/fade out le falta chicha)
    private async Task OnScoreEndCalculate()
    {
        int pseudoScore = scoreManager.puntosJugada * scoreManager.valorJugada;
        int score = pseudoScore + scoreManager.bonusJugada;

        await PrimeTween.Sequence.Create().
            Group(Tween.Alpha(target: scoreLeft, endValue: 0f, duration: initialEffectDuration)).
            Group(Tween.Alpha(target: scoreEx, endValue: 0f, duration: initialEffectDuration)).
            Group(Tween.Alpha(target: scoreMiddle, endValue: 0f, duration: initialEffectDuration)).
            ChainDelay(initialEffectDuration).
            ChainCallback(() => UpdatePseudoScore(pseudoScore)).
            Chain(Tween.Alpha(target: scoreMiddle, endValue: 1f, duration: initialEffectDuration)).
            ChainDelay(initialEffectDuration).
            Group(Tween.Alpha(target: scoreMiddle, endValue: 0f, duration: initialEffectDuration)).
            Group(Tween.Alpha(target: scorePlus, endValue: 0f, duration: initialEffectDuration)).
            Group(Tween.Alpha(target: scoreRight, endValue: 0f, duration: initialEffectDuration)).
            ChainDelay(initialEffectDuration).
            ChainCallback(() => UpdateFinalScore(score)).
            Chain(Tween.Alpha(target: scoreMiddle, endValue: 1f, duration: initialEffectDuration)).
            ChainDelay(initialEffectDuration).
            ChainCallback(() => UpdateTotalScore()).
            Chain(Tween.ShakeLocalPosition(target: totalScore.transform, strength: scoreboardShake, duration: initialEffectDuration)).
            ChainCallback(() => deckManager.PrepareNextHand());
    }
}
