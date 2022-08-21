using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    public SpriteRenderer sprite;
    public RaycSpots raycSpots;

    int cutSceneTicks = 0;

    bool playingScene = false;

    void Update()
    {
        if (!playingScene) return;
        cutSceneTicks--;
        switch (cutSceneTicks)
        {
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
        cutSceneTicks = 300;
        playingScene = true;
        GameObject.Find("UIMonitor").GetComponent<UIMonitor>().ShiftCamera(CameraDisplacement.MAP, this.transform.position.y);
    }
}
