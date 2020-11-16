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
    private TextMeshProUGUI name, flavor, description, costText, prerequisitesText, mutuallyExclusiveText;
    public UpgradeScriptableObject upgrade;

    private void Awake()
    {
        tooltipPanel = GameObject.Find("UpgradeTooltip");
        name = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        flavor = GameObject.Find("FlavourText").GetComponent<TextMeshProUGUI>();
        description = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        prerequisitesText = GameObject.Find("PrerequisitesText").GetComponent<TextMeshProUGUI>();
        mutuallyExclusiveText = GameObject.Find("MutuallyExclusiveText").GetComponent<TextMeshProUGUI>();
        costText = GameObject.Find("CostsText").GetComponent<TextMeshProUGUI>();
        
        GetComponent<Button>().onClick.AddListener(CheckAndUnlockUpgrade);
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
        name.text = upgrade.name;
        flavor.text = upgrade.flavourDescription;
        description.text = upgrade.description;
        costText.text = "";
        
        foreach (KeyValuePair<StringLiterals.singularItemDrops, int> cost in upgrade.costs){
            costText.text += cost.Value + " " + StringLiterals.monsterDropToString(cost.Key) + "\n";
        }
        
        bool canPurchase = isUpgradePurchasable();

        if (canPurchase){
            costText.color = new Color(0.02f, 0.41f, 0.17f);
        }
        
        if (upgrade.hasPrerequisites){
            string prereqText = "";
            
            foreach (UpgradeScriptableObject prerequisite in upgrade.prerequisites){
                prereqText += prerequisite.name + " ";
            }
            
            prerequisitesText.text = prereqText + " must be unlocked first";
        }
        else{
            prerequisitesText.text = "";
        }

        if (upgrade.isMutuallyExclusiveWithOtherSkills){
            string mutuallyExclusiveStr = "";
            
            foreach (UpgradeScriptableObject exclusiveSkill in upgrade.mutuallyExclusiveSkills){
                mutuallyExclusiveStr += exclusiveSkill.name + " ";
            }
            
            mutuallyExclusiveText.text = "You cannot have " + upgrade.name + " together with " + mutuallyExclusiveStr;
        }
        else{
            mutuallyExclusiveText.text = "";
        }
    }

    public void CheckAndUnlockUpgrade(){
        bool areAllPrerequisitesUnlocked = true;
        
        if (upgrade.hasPrerequisites){

            foreach (UpgradeScriptableObject prerequisite in upgrade.prerequisites){
                if (!prerequisite.isUnlocked){
                    areAllPrerequisitesUnlocked = false;
                }
            }
        }
        
        if (areAllPrerequisitesUnlocked){
            UnlockUpgradeIfNotUnlocked();
        }
    }

    private void UnlockUpgradeIfNotUnlocked(){
        if (!GlobalPlayerProgress.UnlockedUpgrades.Contains(upgrade)){
            bool canPurchase = isUpgradePurchasable();
            
            if (canPurchase){
                foreach (KeyValuePair<StringLiterals.singularItemDrops, int> drops in upgrade.costs){
                    GlobalPlayerProgress.playerDrops[drops.Key] -= drops.Value;
                }

                upgrade.isUnlocked = true;
                gameObject.GetComponent<Image>().sprite = upgrade.UpgradeIcon;
                GlobalPlayerProgress.UnlockedUpgrades.Add(upgrade);
                PlayerProgress currentPlayerProgress = new PlayerProgress();
                currentPlayerProgress.saveProgress();
                
                GlobalPlayerProgress.UpdatePlayerWallet(currentPlayerProgress, GameObject.Find("PlayerWallet"));
            }
        }
    }

    private bool isUpgradePurchasable(){
        bool canPurchase = true;
        foreach (KeyValuePair<StringLiterals.singularItemDrops, int> drops in upgrade.costs){
            if (!GlobalPlayerProgress.playerDrops.ContainsKey(drops.Key) || GlobalPlayerProgress.playerDrops[drops.Key] < drops.Value){
                //Player hasn't collected all the necessary resources so he can't purchase the upgrade
                canPurchase = false;
            }
        }

        return canPurchase;
    }
}