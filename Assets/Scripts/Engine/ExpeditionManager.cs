using System.Xml.Schema;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class ExpeditionManager : MonoBehaviour
{
    public List<Effect> effectList;

    public Dictionary<string, int> effectDictionary;

    public GameAssetList gameAssetList;

    UIMonitor uiMonitor;

    public float waitTime;

    float curTime = 0;

    bool timerOn = false;

    public bool hasPendingResult { get; set; } = false;

    bool hasBossEncounterRune = false;

    bool hasBossFightRune = false;

    bool bossDefeated = false;

    public bool allowMapEffects = false;

    public bool isMouseHovering = false;

    RuneItem runeSelected = null;

    public string hoveredArea = "";

    int pendingCoins = 0;

    MapArea selectedArea = null;

    public HashSet<InteractableItem> pendingInteractable;
    public HashSet<ConsumableItem> pendingConsumable;

    public Player player;

    DialogueBox dialogueBox;

    [SerializeField] GameObject expeditionButton;

    [SerializeField] CanvasGroup mapAreas;

    [SerializeField] GameObject raycSelection;

    List<Rayc> prevRaycs = new List<Rayc>();

    void Awake()
    {
        // attributes initializations
        pendingCoins = 0;
        pendingInteractable = new HashSet<InteractableItem>();
        pendingConsumable = new HashSet<ConsumableItem>();
        effectList = new List<Effect>();
        effectDictionary = new Dictionary<string, int>();

        // getting components
        uiMonitor = GameObject.Find("UIMonitor").GetComponent<UIMonitor>();
        dialogueBox = uiMonitor.dialogueBox;
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (gameAssetList.gameObject.activeSelf)
        {
            PromptForRuneInput();
        }
        else
        {
            LoadingExpeditionResult();
        }
        UpdateMapUIInteractability();
    }

    public void SetSelectedArea(MapArea area)
    {
        selectedArea = area;
    }

    public void ResetSelectedArea()
    {
        selectedArea = null;
    }

    void PromptForRuneInput()
    {
        bool done = false;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos2D), -Vector2.up);
            if (hit.collider != null)
            {
                if (gameAssetList.gameObject.activeSelf && gameAssetList.listType == ListType.Rune)
                {
                    done = WaitForRuneInput(hit);
                }
            }
        }

        if (done)
        {
            if (gameAssetList.gameObject.activeSelf) gameAssetList.gameObject.SetActive(false);
            if (runeSelected != null)
            {
                if (player.assetDict[runeSelected] == 1) FindObjectOfType<Inventory>().RemoveConsumableFromInventory(runeSelected);
                player.RemoveAsset(runeSelected);
                runeSelected = null;
            }
            StartExpedition();
        }
    }

    bool WaitForRuneInput(RaycastHit2D hit)
    {
        ListItem listItem = hit.collider.gameObject.GetComponent<ListItem>();
        if (listItem != null && listItem.rune.gameObject.CompareTag("RuneItem"))
        {
            switch (listItem.rune.gameObject.name)
            {
                case "Fate Rune":
                    UnityEngine.Debug.Log("Fate rune was used...");
                    hasBossEncounterRune = true;
                    runeSelected = listItem.rune;
                    break;

                // adding more runes later on

                default:
                    break;
            }
            return true;
        }
        else if (hit.collider.gameObject.name == "CloseGameAssetList")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateMapUIInteractability()
    {
        if (dialogueBox.gameObject.activeSelf || gameAssetList.gameObject.activeSelf)
        {
            allowMapEffects = false;
            mapAreas.interactable = false;
        }
        else
        {
            allowMapEffects = true;
            mapAreas.interactable = true;
        }
    }

    public bool CheckValidExpedition()
    {
        return effectList.Count > 0;
    }

    void LoadingExpeditionResult()
    {
        if (timerOn)
        {
            expeditionButton.GetComponent<Button>().enabled = false;
            Image imageComponent = expeditionButton.GetComponent<Image>();
            imageComponent.color = new Color(1f, 1f, 1f, 0.5f);
            imageComponent.fillAmount = 0;
            if (curTime < waitTime)
            {
                curTime += Time.deltaTime;
                imageComponent.fillAmount = curTime / waitTime;
            }
            else
            {
                timerOn = false;
                curTime = 0;
                imageComponent.color = new Color(1f, 1f, 1f, 1f);
                imageComponent.fillAmount = 1;
                ClearRaycSelection();
                expeditionButton.GetComponent<Button>().enabled = true;
            }
        }
    }

    void DecreaseRaycSelectionFullness()
    {
        foreach (Transform child in raycSelection.transform)
        {
            SelectionSpot selectionSpot = child.gameObject.GetComponent<SelectionSpot>();
            if (selectionSpot.draggedObject != null)
            {
                Rayc rayc = selectionSpot.draggedObject.GetComponent<Rayc>();
                rayc.fullness -= 1;
            }
        }
    }

    void ClearRaycSelection()
    {
        prevRaycs.Clear();
        foreach (Transform child in raycSelection.transform)
        {
            SelectionSpot selectionSpot = child.GetComponent<SelectionSpot>();
            if (selectionSpot.isOccupied)
            {
                GameObject item = selectionSpot.draggedObject;
                Rayc rayc = item.GetComponent<Rayc>();
                GameObject raycObj = Resources.Load("Prefabs/RaycPrefabs/" + rayc.prefabName) as GameObject;
                prevRaycs.Add(raycObj.GetComponent<Rayc>());
                selectionSpot.RemoveSelection();
            }
        }
    }

    // determines if boss appears and is defeated or not
    public void EncounterBoss()
    {
        float avgDiscovery = 0;
        int raycCount = 0;
        foreach (Transform child in raycSelection.transform)
        {
            if (child.GetComponent<SelectionSpot>().draggedObject != null)
            {
                raycCount += 1;
                avgDiscovery += child.GetComponent<SelectionSpot>().draggedObject.GetComponent<Rayc>().discovery;
            }
        }
        avgDiscovery /= (float)raycCount;

        // float appearProb = (float)System.Math.Exp(avgDiscovery / 10f * 2f) / 150f * (hasBossEncounterRune ? 2f : 1f);

        float appearProb = 1;

        UnityEngine.Debug.Log("The boss appear probability is: " + appearProb);

        if (Random.Range(1, 100) <= appearProb * 100f)
        {
            UnityEngine.Debug.Log("The boss appears!");

            float avgStrength = 0;
            foreach (Transform child in raycSelection.transform)
            {
                if (child.GetComponent<SelectionSpot>().draggedObject != null)
                {
                    avgStrength += child.GetComponent<SelectionSpot>().draggedObject.GetComponent<Rayc>().strength;
                }
            }
            avgStrength /= (float)raycCount;
            Boss boss = selectedArea.boss;
            if (avgStrength > boss.strength)
            {
                // 75% chance to defeat boss without rune, 100% chance to defeat boss with rune
                if (Random.Range(1, 100) <= (hasBossEncounterRune ? 100 : 75))
                {
                    bossDefeated = true;
                }
            }
            else
            {
                // 30% chance to defeat boss without rune, 60% with rune
                if (Random.Range(1, 100) <= (hasBossFightRune ? 60 : 30))
                {
                    bossDefeated = true;
                }
            }
        }
    }

    // generates coins and item rewards
    public void CalculateRewards()
    {
        if (!CheckValidExpedition() || selectedArea == null)
        {
            return;
        }

        float coins = bossDefeated ? selectedArea.baseCoins * 10f : selectedArea.baseCoins;
        foreach (Effect effect in effectList)
        {
            switch (effect.name)
            {
                case "Serendipity":
                    if (Random.Range(1, 100) <= effect.chance * 100f)
                    {
                        coins *= (1 + effect.effectPercent);
                    }
                    break;

                // adding more effects later on

                default:
                    break;
            }
        }

        List<GameAsset> treasureItems = selectedArea.treasureItems;
        foreach (GameAsset item in treasureItems)
        {
            TryObtainItem(item);
        }

        if (bossDefeated)
        {
            // give additional interactable item rewards from bosses
            List<GameAsset> bossItems = selectedArea.boss.bossItems;
            foreach (GameAsset item in bossItems)
            {
                TryObtainItem(item);
            }
        }

        hasPendingResult = true;
        pendingCoins = (int)coins;
        DecreaseRaycSelectionFullness();
    }

    void TryObtainItem(GameAsset _item)
    {
        if (ReachMaxPendingItemLength())
        {
            return;
        }

        Rarity rarity = _item.rarity;

        switch (rarity)
        {
            case Rarity.Common:
                if (Random.Range(1, 100) <= 50)
                {
                    if (!AlreadyObtained(_item))
                    {
                        AddToRewards(_item);
                    }
                }
                break;

            case Rarity.Rare:
                if (Random.Range(1, 100) <= 10)
                {
                    if (!AlreadyObtained(_item))
                    {
                        AddToRewards(_item);
                    }
                }
                break;

            case Rarity.Epic:
                if (Random.Range(1, 200) <= 5)
                {
                    if (!AlreadyObtained(_item))
                    {
                        AddToRewards(_item);
                    }
                }
                break;

            case Rarity.Legendary:
                if (Random.Range(1, 200) <= 1)
                {
                    if (!AlreadyObtained(_item))
                    {
                        AddToRewards(_item);
                    }
                }
                break;

            default:
                break;
        }
    }

    void AddToRewards(GameAsset item)
    {
        if (item.gameObject.CompareTag("InteractableItem"))
        {
            pendingInteractable.Add((InteractableItem)item);
        }
        else
        {
            pendingConsumable.Add((ConsumableItem)item);
        }
    }

    bool AlreadyObtained(GameAsset asset)
    {
        return player.assets.Any(cur => cur.gameObject.name == asset.gameObject.name);
    }

    bool ReachMaxPendingItemLength()
    {
        return pendingInteractable.Count + pendingConsumable.Count >= 5;
    }

    public void GrantPlayerRewards()
    {
        string dialogueText = "You got " + pendingCoins + " coins from last expedition!\n";
        if (pendingConsumable.Count != 0 || pendingInteractable.Count != 0)
        {
            dialogueText += "Also, the following items are obtained:\n";
        }

        foreach (InteractableItem item in pendingInteractable)
        {
            dialogueText += item.name + "\n";
            player.PutToInventory((GameAsset)item, true);
        }
        foreach (ConsumableItem item in pendingConsumable)
        {
            dialogueText += item.name + "\n";
            player.PutToInventory((GameAsset)item, true);
        }

        Inventory inventory = FindObjectOfType<UIMonitor>().inventory;

        inventory.UpdateContentMode(ContentMode.RaycOnly);

        // reset expedition data
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
        ShowExpeditionDialogue(dialogueText);
        player.AddCoins(pendingCoins);
        pendingCoins = 0;
        pendingInteractable.Clear();
        pendingConsumable.Clear();
        hasPendingResult = false;
        hasBossEncounterRune = false;
        hasBossFightRune = false;
        bossDefeated = false;
        selectedArea = null;
    }

    public void ShowExpeditionDialogue(string text)
    {
        dialogueBox.ShowDialogue("", text, false);
    }

    public void OnStartButtonPressed()
    {
        if (CheckValidExpedition())
        {
            uiMonitor.ShiftCamera(CameraDisplacement.MAP, 0);
        }
        else
        {
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            ShowExpeditionDialogue("You must put at least 1 Rayc to start expedition!");
        }
    }

    public void OnBackButtonPressed()
    {
        if (CheckValidExpedition())
        {
            dialogueBox.SetFunctionToYesButton(OnConfirmationYesButtonPressed);
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Back to Room", "You sure you want to go back? You will lose your current Rayc selection", true);
        }
        else
        {
            FindObjectOfType<ButtonHandler>().OnExpeditionButtonPressed();
        }

    }

    public void OnConfirmationYesButtonPressed()
    {
        ClearRaycSelection();
        dialogueBox.HideDialogue();
        FindObjectOfType<ButtonHandler>().OnExpeditionButtonPressed();
    }

    public void StartExpedition()
    {
        dialogueBox.HideDialogue();
        FindObjectOfType<StatusSummary>().ClearStatusList();
        timerOn = true;
        uiMonitor.ShiftCamera(0, 0);
        EncounterBoss();
        CalculateRewards();
        FindObjectOfType<ButtonHandler>().OnExpeditionButtonPressed();
    }

    public void StartRuneSelection()
    {
        gameAssetList.listType = ListType.Rune;
        gameAssetList.gameObject.SetActive(true);
        if (gameAssetList.list.transform.childCount == 0)
        {
            gameAssetList.gameObject.SetActive(false);
            StartExpedition();
        }
    }

    public void PlayBossFight()
    {
        string bossName = selectedArea.GetComponentInChildren<Boss>().bossName;
        BossFight bossFight = GameObject.Find(bossName).GetComponent<BossFight>();
        if (bossFight == null) return;
        bossFight.AssignRaycs(prevRaycs);
        bossFight.PlayFight();
    }
}
