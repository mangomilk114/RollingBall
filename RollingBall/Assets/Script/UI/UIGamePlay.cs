using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Button TouchButton;
    public Text Info;
    public GameObject Main;
    public Button RankButton;
    public Button SoundButton;
    public Image SoundButtonImg;
    public GameObject Ready;
    public GameObject ReadyMsg;
    public GameObject GoMag;
    public GameObject Play;
    public UICountImgFont StageCount;
    public GameObject End;

    private bool NextUIEnable = false;

    public void Awake()
    {
        TouchButton.onClick.AddListener(OnClickTouchButton);
        RankButton.onClick.AddListener(OnClickRank);
        SoundButton.onClick.AddListener(OnClickSound);
    }

    public void Start()
    {
        GamePlayManager.Instance.Initialize(this);
    }

    public void ResetUI()
    {
        NextUIEnable = true;
        Main.gameObject.SetActive(false);
        Ready.gameObject.SetActive(false);
        Play.gameObject.SetActive(false);
        End.gameObject.SetActive(false);
        StageCount.gameObject.SetActive(false);
    }

    public void GameMain()
    {
        ResetUI();
        Main.gameObject.SetActive(true);
        RefrehUI();
    }

    public void GameReady()
    {
        ResetUI();
        Ready.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CoGameReady());
    }

    IEnumerator CoGameReady()
    {
        NextUIEnable = false;
        ReadyMsg.gameObject.SetActive(true);
        GoMag.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        ReadyMsg.gameObject.SetActive(false);
        GoMag.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        NextUIEnable = true;
        OnClickTouchButton();
    }

    public void GamePlay()
    {
        ResetUI();
        Play.gameObject.SetActive(true);
        ChangeStageCount();
    }

    public void GameEnd()
    {
        ResetUI();
        ChangeStageCount();
        End.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CoGameEnd());
    }

    IEnumerator CoGameEnd()
    {
        NextUIEnable = false;
        yield return new WaitForSeconds(1f);
        NextUIEnable = true;
    }

    public void GamePause()
    {

    }

    public void GameContinue()
    {

    }
    private void OnClickTouchButton()
    {
        if (NextUIEnable == false)
            return;

        GamePlayManager.Instance.ChangeGameState();
    }
    private void OnClickRank()
    {

    }

    private void OnClickSound()
    {
        PlayerData.Instance.SetSoundEnable(!PlayerData.Instance.SoundEnable);
        RefrehUI();
    }

    private void RefrehUI()
    {
        var fileName = PlayerData.Instance.SoundEnable ? "ui_btn_sound_on" : "ui_btn_sound_off";
        SoundButtonImg.sprite = (Sprite)Resources.Load(fileName, typeof(Sprite));
    }

    public void ChangeStageCount()
    {
        StageCount.gameObject.SetActive(true);
        StageCount.SetValue(string.Format("s{0}", GamePlayManager.Instance.StageIndex + 1), UICountImgFont.IMG_RANGE.CENTER);
    }

    public bool IsNextUIEnable()
    {
        return NextUIEnable;
    }

    public void Update()
    {
        StringBuilder info = new StringBuilder();
        info.AppendLine(string.Format("현재 스테이지 : {0}", GamePlayManager.Instance.StageIndex + 1));
        info.AppendLine(string.Format("현재 공 속도 : {0:f2}", GamePlayManager.Instance.PlayJellyHero.MoveSpeed));
        info.AppendLine(string.Format("현재 공 방향 : {0}", GamePlayManager.Instance.CurrStageData.start_rightdir ? "오른쪽" : "왼쪽"));
        info.AppendLine(string.Format("현재 체력 : {0} / {1}", GamePlayManager.Instance.HealthPoint, GamePlayManager.Instance.MaxHealthPoint));
        info.AppendLine(string.Format("현재 바퀴수 : {0}", GamePlayManager.Instance.TurnCount));
        info.AppendLine(string.Format("현재 점수 : {0}", PlayerData.Instance.Score));
        Info.text = info.ToString();
    }

    public void SetNextUIEnable(bool bNextUIEnable)
    {
        NextUIEnable = bNextUIEnable;
    }

    public bool GetNextUIEnable()
    {
        return NextUIEnable;
    }

}
