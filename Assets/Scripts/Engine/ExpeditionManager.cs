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

    bool timerOn { get; set; } = false;

    public bool hasPendingResult { get; set; } = false;

    bool hasBossEncounterRune = false;

    bool hasBossFightRune = false;

    bool bossDefeated = false;

    public bool allowMapEffects = false;

    public bool isMouseHovering = false;

    RuneItem runeSelected = null;

    public string hoveredArea = "";

    int pendingCoins = 0;

    public Boss boss;

    public HashSet<InteractableItem> pendingInteractable;
    public HashSet<ConsumableItem> pendingConsumable;

    public Player player;

    List<GameAsset> treasureItems;

    DialogueBox dialogueBox;

    GameObject confirmationDialogue;

    [SerializeField] GameObject expeditionButton;

    [SerializeField] CanvasGroup mapAreas;

    [SerializeField] GameObject raycSelection;

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

    public void SetTreasureItems(List<GameAsset> newList)
    {
        treasureItems = newList;
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
                player.RemoveAsset(runeSelected);
                FindObjectOfType<Inventory>().RemoveConsumableFromInventory(runeSelected);
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
                case "Boss Rune":
                    UnityEngine.Debug.Log("Boss encounter rune was used...");
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

    void ClearRaycSelection()
    {
        foreach (Transform child in raycSelection.transform)
        {
            SelectionSpot selectionSpot = child.GetComponent<SelectionSpot>();
            if (selectionSpot.isOccupied)
            {
                GameObject item = selectionSpot.draggedObject;
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

        float appearProb = (float)System.Math.Exp(avgDiscovery / 10f * 2f) / 150f * (hasBossEncounterRune ? 2f : 1f);

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
        if (!CheckValidExpedition())
        {
            return;
        }

        float coins = bossDefeated ? 1000f : 100f;
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

        foreach (GameAsset item in treasureItems)
        {
            TryObtainItem(item);
        }

        if (bossDefeated)
        {
            // give additional interactable item rewards from bosses
            List<GameAsset> bossItems = boss.bossItems;
            foreach (GameAsset item in bossItems)
            {
                TryObtainItem(item);
            }
        }

        hasPendingResult = true;
        pendingCoins = (int)coins;
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

        // reset expedition data
        ShowExpeditionDialogue(dialogueText);
        player.AddCoins(pendingCoins);
        pendingCoins = 0;
        pendingInteractable.Clear();
        pendingConsumable.Clear();
        hasPendingResult = false;
        hasBossEncounterRune = false;
        hasBossFightRune = false;
        bossDefeated = false;
        boss = null;
        treasureItems = null;
    }

    public void ShowExpeditionDialogue(string text)
    {
        // uiMonitor.ToggleExpeditionUI(false);
        dialogueBox.ShowDialogue("", text, false);
    }

    public void HideExpeditionDialogue()
    {
        // uiMonitor.ToggleExpeditionUI(true);
        dialogueBox.HideDialogue();
    }

    public void OnStartButtonPressed()
    {
        if (CheckValidExpedition())
        {
            uiMonitor.ShiftCamera(CameraDisplacement.MAP, 0);
        }
        else
        {
            ShowExpeditionDialogue("You must put at least 1 Rayc to start expedition!");
        }
    }

    public void OnMapBackButtonPressed()
    {
        uiMonitor.ShiftCamera(CameraDisplacement.EXPEDITION, 0);
    }

    public void OnBackButtonPressed()
    {
        if (CheckValidExpedition())
        {
            // uiMonitor.toggleExpeditionUI(false);
            UnityAction confirmYesAction = null;
            confirmYesAction += OnConfirmationYesButtonPressed;
            dialogueBox.yesButton.onClick.AddListener(confirmYesAction);
            dialogueBox.ShowDialogue("Back to Room", "You sure you want to go back? You will lose your current Rayc selection", true);
        }
        else
        {
            uiMonitor.ShiftCamera(0, 0);
        }

    }

    public void OnConfirmationYesButtonPressed()
    {
        ClearRaycSelection();
        dialogueBox.gameObject.SetActive(false);
        // uiMonitor.toggleExpeditionUI(true);
        uiMonitor.ShiftCamera(0, 0);
    }

    public void OnConfirmationNoButtonPressed()
    {
        // uiMonitor.toggleExpeditionUI(true);
        dialogueBox.gameObject.SetActive(false);
    }

    public void StartExpedition()
    {
        UnityEngine.Debug.Log(boss.bossName);
        dialogueBox.HideDialogue();
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
    }
}
