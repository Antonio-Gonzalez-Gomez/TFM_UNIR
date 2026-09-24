using PrimeTween;
using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] DeckManager deckManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] InfoPopUp infoPopUp;
    [SerializeField] Canvas scorePopUp;
    [Header("Play buttons")]
    [SerializeField] Canvas buttonsCanvas;
    [SerializeField] Button canteButton;
    [SerializeField] Button bazaButton;
    [SerializeField] Button sortSuitButton;
    [SerializeField] Button sortValueButton;
    [SerializeField] Button confirmButton;
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

    private RectTransform scorePopUpRect;
    private TMP_Text scorePopUpText;
    private float effectDuration;
    private ColorText ct = new ColorText();
    private UITools uit;

    private float popupFontOriginalSize;
    private float scoreboardOriginalMaxSize;
    //Magic numbers para efectos con Tweens
    private readonly Vector3 scoreboardShake = new Vector3(0, 20, 0);
    private readonly Vector3 cardShake = new Vector3(0, 0.1f, 0);
    private readonly Vector3 popupShake = new Vector3(0, 0, 10);
    private readonly Vector3 popupDistance = new Vector3(0, 1.5f, 0);
    private readonly Vector3 popupDistanceAugment = new Vector3(3f, 0, 0);

    private void Start()
    {
        effectDuration = initialEffectDuration;
        scorePopUpRect = scorePopUp.GetComponent<RectTransform>();
        scorePopUpText = scorePopUp.GetComponentInChildren<TMP_Text>();
        uit = new UITools(this.GetComponent<CanvasScaler>().referenceResolution);
        scorePopUpText.alpha = 0;
        popupFontOriginalSize = scorePopUpText.fontSize;
        scoreboardOriginalMaxSize = scoreLeft.fontSizeMax;
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
        drag.cardsSelected += OnCardSelect;
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
        OnCardSelect();
    }

    private void UpdateScoreboardHand()
    {
        scoreEquation.enabled = false;
        //Si no hay carta de baza, no se muestra el cante ni la puntuacion base
        if (scoreManager.bazaSpot.cardList.Count == 0)
        {
            handType.alpha = 0;
            return;
        }

        handType.alpha = 1;
        //Si la baza se va a perder, no se muestra la puntuacion base del cante
        if (!scoreManager.EsBazaGanada())
        {
            handType.text = "Baza perdida";
            return;
        }
        //Si la baza se gana, se muestra la puntuacion bae
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
        OnCardSelect();
    }

    public void MoverCartasCante()
    {
        deckManager.MoveSelectedCardsToSpot(true);
        UpdateScoreboardHand();
        OnCardSelect();
    }

    public void OrdenarPorPalo()
    {
        scoreManager.manoSpot.SortCards(true);
    }

    public void OrdenarPorValor()
    {
        scoreManager.manoSpot.SortCards(false);
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

        OnCardSelect();
    }

    private void OnCardSelect()
    {
        bazaButton.interactable = false;
        canteButton.interactable = false;
        confirmButton.interactable = false;

        int selectedFromHand = scoreManager.manoSpot.GetSelectedCards().Count;
        if (selectedFromHand > 0)
        {
            canteButton.interactable = true;
        }

        if (selectedFromHand == 1)
        {
            bazaButton.interactable = true;
        }

        if (scoreManager.bazaSpot.cardList.Count > 0)
        {
            confirmButton.interactable = true;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ScoreTest()
    {

    }

    //Los efectos con tweens se hacen cada vez mas rapidos (en caso de que haya muchos, no se hace tan larga)
    private void ReduceEffectDuration()
    {
        effectDuration = effectDuration <= finalEffectDuration ?
            finalEffectDuration :
            effectDuration - decreaseEffectDuration;
    }

    private async Task OnPointIncrease(DragController drag, int puntos, bool isAugment)
    {
        scoreLeft.text = ct.Color(scoreManager.puntosJugada.ToString(), "points");
        scorePopUpText.text = ct.PointsText(puntos);
        await ScoreIncreaseEffects(drag, scoreLeft, isAugment);
    }
    private async Task OnValueIncrease(DragController drag, int valor, bool isAugment)
    {
        scoreMiddle.text = ct.Color(scoreManager.valorJugada.ToString(), "value");
        scorePopUpText.text = ct.ValueText(valor);
        await ScoreIncreaseEffects(drag, scoreMiddle, isAugment);
    }
    private async Task OnBonusIncrease(DragController drag, int bonus, bool isAugment)
    {
        scoreRight.text = ct.Color(scoreManager.bonusJugada.ToString(), "bonus");
        scorePopUpText.text = ct.BonusText(bonus);
        await ScoreIncreaseEffects(drag, scoreRight, isAugment);
    }

    //Secuencia de tweens que se lanzan cada vez que la puntuacion aumenta
    private async Task ScoreIncreaseEffects(DragController drag, TMP_Text scoreboardText, bool isAugment)
    {
        if (isAugment)
        {
            scorePopUpRect.anchoredPosition = uit.WorldPositionToAnchor(drag.transform.position + popupDistanceAugment);
        }
        else
        {
            scorePopUpRect.anchoredPosition = uit.WorldPositionToAnchor(drag.transform.position + popupDistance);
        }
        scorePopUpText.alpha = 1;
        scorePopUpText.fontSize = 0;

        await PrimeTween.Sequence.Create().
            Group(Tween.ShakeLocalPosition(scoreboardText.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.PunchLocalPosition(drag.transform, strength: cardShake, duration: effectDuration)).
            Group(TweenFontRestoreSize(scorePopUpText, duration: effectDuration)).
            Group(Tween.ShakeLocalRotation(scorePopUpText.transform, strength: popupShake, duration: effectDuration)).
            Chain(Tween.Alpha(scorePopUpText, endValue: 0f, duration: effectDuration)).
            ChainCallback(() => ReduceEffectDuration());
    }

    //Tween que aumenta el tamaño de texto
    private Tween TweenFontRestoreSize(TMP_Text text, float duration)
    {
        return Tween.Custom(startValue: text.fontSize, endValue: popupFontOriginalSize, duration: duration, onValueChange: val => text.fontSize = val);
    }
    //Tween que reduce el tamaño de texto (maximo) a 0
    private Tween TweenVanishScoreboardFont(TMP_Text text, float duration)
    {
        return Tween.Custom(startValue: text.fontSizeMax, endValue: 0, duration: duration, onValueChange: val => text.fontSizeMax = val);
    }
    //Tween que cambia el valor numerico de un texto del marcador
    private Tween TweenTextNumberChange(TMP_Text text, float startValue, float endValue, float duration, string stringEnd)
    {
        return Tween.Custom(startValue: startValue, endValue: endValue, duration: duration, onValueChange:
            val => text.text = ((int) val).ToString() + stringEnd);
    }

    //Restaura el estado original del marcador despues de jugar una baza
    private void RestoreScoreboard()
    {
        scoreEquation.enabled = false;
        scoreEx.text = "x";
        scoreMiddle.alpha = 1;
        scorePlus.text = "+";

        scoreLeft.fontSizeMax = scoreboardOriginalMaxSize;
        scoreEx.fontSizeMax = scoreboardOriginalMaxSize;
        scoreMiddle.fontSizeMax = scoreboardOriginalMaxSize;
        scorePlus.fontSizeMax = scoreboardOriginalMaxSize;
        scoreRight.fontSizeMax = scoreboardOriginalMaxSize;
    }
    private void UpdatePseudoScore(int pseudoScore)
    {
        scoreEx.text = ct.Color(pseudoScore.ToString(), "bonus");
        scoreMiddle.text = "+";
        scorePlus.text = ct.Color(scoreManager.bonusJugada.ToString(), "bonus");

        scoreEx.fontSizeMax = scoreboardOriginalMaxSize;
        scoreMiddle.fontSizeMax = scoreboardOriginalMaxSize;
        scorePlus.fontSizeMax = scoreboardOriginalMaxSize;
    }

    private void UpdateFinalScore(int score)
    {
        scoreMiddle.text = score.ToString();
        scoreMiddle.fontSizeMax = scoreboardOriginalMaxSize;
    }

    //Secuencia de tweens que se lanzan cuando se confirma la baza
    private async Task OnScoreEndCalculate()
    {
        int pseudoScore = scoreManager.puntosJugada * scoreManager.valorJugada;
        int score = pseudoScore + scoreManager.bonusJugada;
        int previousTotalScore = scoreManager.totalScore - score;
        effectDuration = initialEffectDuration;


        await PrimeTween.Sequence.Create().
            Group(TweenVanishScoreboardFont(scoreLeft, effectDuration)).
            Group(TweenVanishScoreboardFont(scoreEx, effectDuration)).
            Group(TweenVanishScoreboardFont(scoreMiddle, effectDuration)).
            Group(TweenVanishScoreboardFont(scorePlus, effectDuration)).
            Group(TweenVanishScoreboardFont(scoreRight, effectDuration)).
            ChainDelay(initialEffectDuration).

            ChainCallback(() => UpdatePseudoScore(pseudoScore)).
            Group(Tween.ShakeLocalPosition(scoreEx.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.ShakeLocalPosition(scoreMiddle.transform, strength: scoreboardShake, duration: effectDuration)).
            Group(Tween.ShakeLocalPosition(scorePlus.transform, strength: scoreboardShake, duration: effectDuration)).
            ChainDelay(initialEffectDuration).

            Group(TweenVanishScoreboardFont(scoreEx, effectDuration)).
            Group(TweenVanishScoreboardFont(scoreMiddle, effectDuration)).
            Group(TweenVanishScoreboardFont(scorePlus, effectDuration)).
            ChainDelay(initialEffectDuration).

            ChainCallback(() => UpdateFinalScore(score)).
            Group(Tween.ShakeLocalRotation(scoreMiddle.transform, strength: popupShake, duration: effectDuration)).
            ChainDelay(initialEffectDuration).

            Group(TweenTextNumberChange(scoreMiddle, score, 0, effectDuration, "")).
            Group(TweenTextNumberChange(totalScore, previousTotalScore, scoreManager.totalScore, effectDuration, " / " + scoreManager.targetScore.ToString())).
            Chain(Tween.Alpha(target: scoreMiddle, endValue: 0f, duration: effectDuration)).
            ChainCallback(() => RestoreScoreboard()).
            ChainCallback(() => deckManager.PrepareNextHand());
    }
}
