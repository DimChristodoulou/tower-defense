// Decompiled with JetBrains decompiler
// Type: UpgradeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
    private GameObject tooltipPanel;
    private TextMeshProUGUI name, flavor, description, costText;
    public UpgradeScriptableObject upgrade;

    private void Awake()
    {
        tooltipPanel = GameObject.Find("UpgradeTooltip");
        name = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        flavor = GameObject.Find("FlavourText").GetComponent<TextMeshProUGUI>();
        description = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        costText = GameObject.Find("CostsText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() => tooltipPanel.SetActive(false);

    private void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData) => showUpgradeTooltip();

    public void OnPointerExit(PointerEventData eventData) => tooltipPanel.SetActive(false);

    public void showUpgradeTooltip()
    {
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = Input.mousePosition + new Vector3(150f, 150f, 0.0f);
        name.text = upgrade.name;
        flavor.text = upgrade.flavourDescription;
        description.text = upgrade.description;
        costText.text = "";
        
        foreach (KeyValuePair<StringLiterals.singularItemDrops, int> cost in upgrade.costs){
            costText.text += cost.Value + " " + StringLiterals.monsterDropToString(cost.Key) + "\n";
        }
    }

    public void UnlockUpgrade(){
        GlobalPlayerProgress.UnlockedUpgrades.Add(upgrade.id);
        PlayerProgress currentPlayerProgress = new PlayerProgress();
        currentPlayerProgress.saveProgress();
    }
}