using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Button TouchButton;
    public Text StageText;
    public Text Info;

    public void Awake()
    {
        TouchButton.onClick.AddListener(OnClickTouchButton);
    }

    public void Start()
    {
        GamePlayManager.Instance.Initialize(this);
    }

    public void ResetUI()
    {
    }

    public void GameMain()
    {
        ResetUI();
    }

    public void GameReady()
    {
        
    }

    public void GamePlay()
    {

    }

    public void GameEnd()
    {

    }

    public void GamePause()
    {

    }

    public void GameContinue()
    {

    }

    public void UpdateUI()
    {
        switch (GamePlayManager.Instance.CurrGameState)
        {
            case GamePlayManager.GAME_STATE.MAIN:
                StageText.text = string.Format("메인 화면 피 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.READY:
                StageText.text = string.Format("레디 화면 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.PLAY:
                StageText.text = string.Format("플레이 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.END:
                StageText.text = string.Format("종료 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.PAUSE:
                break;
            default:
                break;
        }
    }
   
    private void OnClickTouchButton()
    {
        GamePlayManager.Instance.ChangeGameState();
    }

    public void Update()
    {
        StringBuilder info = new StringBuilder();
        info.AppendLine(string.Format("현재 스테이지 : {0}", GamePlayManager.Instance.CurrStageData.id));
        info.AppendLine(string.Format("현재 공 속도 : {0:f2}", GamePlayManager.Instance.CurrStageData.start_speed));
        info.AppendLine(string.Format("현재 공 방향 : {0}", GamePlayManager.Instance.CurrStageData.start_rightdir ? "오른쪽" : "왼쪽"));
        info.AppendLine(string.Format("현재 체력 : {0} / {1}", GamePlayManager.Instance.HealthPoint, GamePlayManager.Instance.MaxHealthPoint));
        info.AppendLine(string.Format("현재 바퀴수 : {0}", GamePlayManager.Instance.TurnCount));
        info.AppendLine(string.Format("현재 점수 : {0}", GamePlayManager.Instance.Score));
        if (GamePlayManager.Instance.PlayBall.CurrCollisionObject == null)
            info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "없음"));
        else
        {
            var type = GamePlayManager.Instance.PlayBall.CurrCollisionObject.Type;
            switch (type)
            {
                case CollisionObject.OBJECT_TYPE.NONE:
                    break;
                case CollisionObject.OBJECT_TYPE.ITEM:
                    {
                        var itemObj = GamePlayManager.Instance.PlayBall.CurrCollisionObject.GetComponent<Item>();
                        switch (itemObj.ItemType)
                        {
                            case CommonData.ITEM_TYPE.NONE:
                                break;
                            case CommonData.ITEM_TYPE.STAR:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "스타 아이템"));
                                break;
                            case CommonData.ITEM_TYPE.SPEED_UP:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "스피드업 아이템"));
                                break;
                            case CommonData.ITEM_TYPE.SPEED_DOWN:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "스피드다운 아이템"));
                                break;
                            case CommonData.ITEM_TYPE.BOMB:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "폭탄 아이템"));
                                break;
                            case CommonData.ITEM_TYPE.COIN:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "코인 아이템"));
                                break;
                            case CommonData.ITEM_TYPE.POTION:
                                info.AppendLine(string.Format("현재 공이랑 부딪친 아이템 이름 : {0}", "포션 아이템"));
                                break;
                            default:
                                break;
                        }
                    }
                    
                    break;
                case CollisionObject.OBJECT_TYPE.STAGE_END_LEFT:
                    info.AppendLine(string.Format("현재 공이랑 부딪친 이름 : {0}", "왼쪽 클리어 체크"));
                    break;
                case CollisionObject.OBJECT_TYPE.STAGE_END_RIGHT:
                    info.AppendLine(string.Format("현재 공이랑 부딪친 이름 : {0}", "오른쪽 클리어 체크"));
                    break;
                case CollisionObject.OBJECT_TYPE.STAGE_START:
                    info.AppendLine(string.Format("현재 공이랑 부딪친 이름 : {0}", "시작지점 체크"));
                    break;
                default:
                    break;
            }
        }
        Info.text = info.ToString();
    }
}
