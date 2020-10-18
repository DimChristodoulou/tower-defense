// Decompiled with JetBrains decompiler
// Type: UpgradeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
    public GameObject tooltipPanel;
    private Text name;
    private TextMeshProUGUI flavor;
    private TextMeshProUGUI description;
    public UpgradeScriptableObject upgrade;

    private void Awake()
    {
        tooltipPanel = GameObject.Find("UpgradeTooltip");
        name = GameObject.Find("NameText").GetComponent<Text>();
        flavor = GameObject.Find("FlavourText").GetComponent<TextMeshProUGUI>();
        description = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
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
        tooltipPanel.transform.position = Input.mousePosition + new Vector3(175f, 175f, 0.0f);
        name.text = upgrade.name;
        flavor.text = upgrade.flavourDescription;
        description.text = upgrade.description;
    }
}