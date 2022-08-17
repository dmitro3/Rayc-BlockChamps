using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExpeditionManager : MonoBehaviour
{
    public List<Effect> effectList;

    public Dictionary<string, int> effectDictionary;

    public GameAssetList gameAssetList;

    UIMonitor uiMonitor;

    GameObject raycSelection;

    public float waitTime;

    float curTime = 0;

    public bool timerOn = false;

    public bool hasPendingResult = false;

    public bool hasBossEncounterRune = false;

    public bool hasBossFightRune = false;

    public bool bossDefeated = false;

    public bool allowMapEffects = false;

    public bool isMouseHovering = false;

    RuneItem runeSelected = null;

    public string hoveredArea = "";

    public int pendingCoins;

    public HashSet<GameAsset> pendingItems;

    public Player player;

    DialogueBox dialogueBox;

    GameObject confirmationDialogue;

    GameObject expeditionButton;

    CanvasGroup mapAreas;

    // ExpeditionButtonScript expeditionButtonScript;

    MapConfirmationDialogue mapConfirmationDialogue;

    DialogueBox mapDialogueBox;

    void Awake()
    {
        // attributes initializations
        pendingCoins = 0;
        pendingItems = new HashSet<GameAsset>(new GameAssetComparator());
        effectList = new List<Effect>();
        effectDictionary = new Dictionary<string, int>();

        // getting components
        uiMonitor = GameObject.Find("UIMonitor").GetComponent<UIMonitor>();
        raycSelection = GameObject.Find("RaycSelection");
        expeditionButton = GameObject.Find("ExpeditionButton");
        player = GameObject.Find("Player").GetComponent<Player>();
        dialogueBox = GameObject.Find("DialogueBox").GetComponent<DialogueBox>();
        confirmationDialogue = GameObject.Find("ConfirmationDialogue");
        mapConfirmationDialogue = GameObject.Find("MapConfirmationDialogue").GetComponent<MapConfirmationDialogue>();
        mapDialogueBox = GameObject.Find("MapDialogueBox").GetComponent<DialogueBox>();
        // expeditionButtonScript = GameObject.Find("ExpeditionButton").GetComponent<ExpeditionButtonScript>();
        mapAreas = GameObject.Find("MapAreas").GetComponent<CanvasGroup>();

        // component initializations
        confirmationDialogue.SetActive(false);
    }

    void Update()
    {
        if (gameAssetList.gameObject.activeSelf)
        {
            PromptForRuneInput();
        }
        else
        {
            loadingExpeditionResult();
        }
        updateMapUIInteractability();
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
                player.removeAsset(runeSelected);
                FindObjectOfType<Inventory>().RemoveFromInventory(runeSelected);
                runeSelected = null;
            }
            startExpedition();
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
                    Debug.Log("Boss encounter rune was used...");
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

    void updateMapUIInteractability()
    {
        if (mapDialogueBox.gameObject.activeSelf || mapConfirmationDialogue.gameObject.activeSelf || gameAssetList.gameObject.activeSelf)
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

    public bool checkValidExpedition()
    {
        return effectList.Count > 0;
    }

    void loadingExpeditionResult()
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
                clearRaycSelection();
                expeditionButton.GetComponent<Button>().enabled = true;
            }
        }
    }

    void clearRaycSelection()
    {
        foreach (Transform child in raycSelection.transform)
        {
            SelectionSpot selectionSpot = child.GetComponent<SelectionSpot>();
            if (selectionSpot.isOccupied)
            {
                GameObject item = selectionSpot.draggedObject;
                selectionSpot.removeSelection();
            }
        }
    }

    // determines if boss appears and is defeated or not
    public void encounterBoss()
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

        Debug.Log("The boss appear probability is: " + appearProb);

        if (Random.Range(1, 100) <= appearProb * 100f)
        {
            Debug.Log("The boss appears!");

            Boss boss = mapConfirmationDialogue.mapArea.boss;
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
    public void calculateRewards()
    {
        if (!checkValidExpedition())
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

        List<Item> treasureItems = mapConfirmationDialogue.mapArea.treasureItems;
        foreach (Item item in treasureItems)
        {
            tryObtainItem(item);
        }

        if (bossDefeated)
        {
            // give additional interactable item rewards from bosses
            List<Item> bossItems = mapConfirmationDialogue.mapArea.boss.bossItems;
            foreach (Item item in bossItems)
            {
                tryObtainItem(item);
            }
        }

        hasPendingResult = true;
        pendingCoins = (int)coins;
    }

    void tryObtainItem(Item _item)
    {
        if (reachMaxPendingItemLength())
        {
            return;
        }

        Rarity rarity = _item.rarity;
        GameAsset item = (GameAsset)_item;
        switch (rarity)
        {
            case Rarity.Common:
                if (Random.Range(1, 100) <= 50)
                {
                    if (!alreadyObtained(item))
                    {
                        pendingItems.Add(item);
                    }
                }
                break;

            case Rarity.Rare:
                if (Random.Range(1, 100) <= 10)
                {
                    if (!alreadyObtained(item))
                    {
                        pendingItems.Add(item);
                    }
                }
                break;

            case Rarity.Epic:
                if (Random.Range(1, 200) <= 5)
                {
                    if (!alreadyObtained(item))
                    {
                        pendingItems.Add(item);
                    }
                }
                break;

            case Rarity.Legendary:
                if (Random.Range(1, 200) <= 1)
                {
                    if (!alreadyObtained(item))
                    {
                        pendingItems.Add(item);
                    }
                }
                break;

            default:
                break;
        }
    }

    bool alreadyObtained(GameAsset asset)
    {
        return player.assets.Any(cur => cur.gameObject.name == asset.gameObject.name);
    }

    bool reachMaxPendingItemLength()
    {
        return pendingItems.Count >= 4;
    }

    public void grantPlayerRewards()
    {
        string dialogueText = "You got " + pendingCoins + " coins from last expedition!\n";
        if (pendingItems.Count != 0)
        {
            dialogueText += "Also, the following items are obtained:\n";
        }
        foreach (Item item in pendingItems)
        {
            dialogueText += item.name + "\n";
            player.putToInventory(item, true);
        }
        showExpeditionDialogue(dialogueText);
        player.addCoins(pendingCoins);
        pendingCoins = 0;
        pendingItems.Clear();
        hasPendingResult = false;
        hasBossEncounterRune = false;
        hasBossFightRune = false;
        bossDefeated = false;
    }

    public void showExpeditionDialogue(string text)
    {
        uiMonitor.toggleExpeditionUI(false);
        dialogueBox.showDialogue(text);
    }

    public void hideExpeditionDialogue()
    {
        uiMonitor.toggleExpeditionUI(true);
        dialogueBox.hideDialogue();
    }

    public void OnStartButtonPressed()
    {
        if (checkValidExpedition())
        {
            uiMonitor.shiftCamera(CameraDisplacement.MAP);
        }
        else
        {
            showExpeditionDialogue("You must put at least 1 Rayc to start expedition!");
        }
    }

    public void OnMapBackButtonPressed()
    {
        mapConfirmationDialogue.toggleConfirmationDialogue(false);
        mapDialogueBox.gameObject.SetActive(true);
        uiMonitor.shiftCamera(CameraDisplacement.EXPEDITION);
    }

    public void OnBackButtonPressed()
    {
        if (checkValidExpedition())
        {
            uiMonitor.toggleExpeditionUI(false);
            confirmationDialogue.SetActive(true);
        }
        else
        {
            // expeditionButtonScript.OnButtonPressed();
        }

    }

    public void OnConfirmationYesButtonPressed()
    {
        clearRaycSelection();
        confirmationDialogue.SetActive(false);
        uiMonitor.toggleExpeditionUI(true);
        // expeditionButtonScript.OnButtonPressed();
    }

    public void OnConfirmationNoButtonPressed()
    {
        uiMonitor.toggleExpeditionUI(true);
        confirmationDialogue.SetActive(false);
    }

    public void startExpedition()
    {
        timerOn = true;
        mapConfirmationDialogue.toggleConfirmationDialogue(false);
        mapDialogueBox.gameObject.SetActive(true);
        // expeditionButtonScript.OnButtonPressed();
        encounterBoss();
        calculateRewards();
    }

    public void StartRuneSelection()
    {
        gameAssetList.listType = ListType.Rune;
        gameAssetList.gameObject.SetActive(true);
    }
}
