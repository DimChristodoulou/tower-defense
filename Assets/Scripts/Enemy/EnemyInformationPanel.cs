using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyInformationPanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerExitHandler{
    
    public GameObject informationPanel;
    public GameObject enemyNameText;
    public GameObject enemyDescriptionText;
    public GameObject enemyStatistics;
    public Enemy enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        informationPanel = GameObject.Find("GameplayInformation");
        enemyNameText = GameObject.Find("EnemyName");
        enemyDescriptionText = GameObject.Find("EnemyDescription");
        enemyStatistics = GameObject.Find("EnemyStatistics");
    }
    

    public void OnPointerClick(PointerEventData eventData){
        foreach (Transform child in enemyStatistics.transform){
            Destroy(child.gameObject);
        }
        
        enemyNameText.GetComponent<TextMeshProUGUI>().text = enemy.enemyData.name;
        enemyDescriptionText.GetComponent<TextMeshProUGUI>().text = enemy.enemyData.description;
        List<string> enemyDataToDisplay = new List<string>();
        enemyDataToDisplay = enemy.parseEnemyDataToString();

        foreach (string enemyString in enemyDataToDisplay){
            string buffOrDebuff = enemyString.Substring(0, 3);
            string stringPayload = enemyString.Substring(3, enemyString.Length-3);
            
            GameObject statText = new GameObject();
            statText.AddComponent<TextMeshProUGUI>();
            statText.transform.SetParent(enemyStatistics.transform);
            
            if (buffOrDebuff == "[B]"){
                //This is a buff so text color should be green
                FormatEnemyAttributeText(statText, stringPayload, Color.green);
            }
            else if (buffOrDebuff == "[D]"){
                //This is a debuff so text color should be red
                FormatEnemyAttributeText(statText, stringPayload, Color.red);
            }
            else{
                //Black text color on everything else
                FormatEnemyAttributeText(statText, enemyString, Color.black);
            }
        }
    }

    private static void FormatEnemyAttributeText(GameObject statText, string stringPayload, Color textColor){
        statText.GetComponent<TextMeshProUGUI>().color = textColor;
        statText.GetComponent<TextMeshProUGUI>().fontSize = 16;
        statText.GetComponent<TextMeshProUGUI>().text = stringPayload;
    }

    public void OnPointerExit(PointerEventData eventData){
        /*enemyNameText.GetComponent<TextMeshProUGUI>().text = "";
        enemyDescriptionText.GetComponent<TextMeshProUGUI>().text = "";
        
        foreach (Transform child in enemyStatistics.transform) {
            GameObject.Destroy(child.gameObject);
        }*/
    }
}
