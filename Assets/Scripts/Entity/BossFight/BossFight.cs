using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    private const int ANIMATION_TICKS = 300;
    public SpriteRenderer sprite;
    public RaycSpots raycSpots;
    public FadeEffect shade;

    int cutSceneTicks = 0;

    bool playingScene = false;

    void Update()
    {
        if (!playingScene) return;
        switch (cutSceneTicks--)
        {
            case ANIMATION_TICKS:
                shade.FadeOut();
                break;
            case 250:
                sprite.color = Color.red;
                break;
            case 225:
                sprite.color = Color.white;
                break;
            case 175:
                sprite.color = Color.red;
                break;
            case 150:
                sprite.color = Color.white;
                break;
            case 100:
                sprite.color = Color.red;
                break;
            case 75:
                sprite.color = Color.white;
                break;
            case 50:
                shade.FadeIn();
                break;
            case 0:
                playingScene = false;
                GameObject.Find("UIMonitor").GetComponent<UIMonitor>().ShiftCamera(CameraDisplacement.EXPEDITION, 0);
                GameObject.Find("ExpeditionManager").GetComponent<ExpeditionManager>().GrantPlayerRewards();
                break;
            default:
                break;
        }
    }

    public void AssignRaycs(List<Sprite> raycs)
    {
        raycSpots.AssignRaycs(raycs);
    }

    public void PlayFight()
    {
        cutSceneTicks = ANIMATION_TICKS;
        playingScene = true;
        GameObject.Find("UIMonitor").GetComponent<UIMonitor>().ShiftCamera(CameraDisplacement.MAP, this.transform.position.y);
    }
}
