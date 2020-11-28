// Decompiled with JetBrains decompiler
// Type: TowerConstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TowerConstruction : MonoBehaviour
{
    private GameObject[] _constructionPoints;
    private GameManager _manager;
    private GameObject _gridTilemap;

    private void Start()
    {
        _constructionPoints = GameObject.FindGameObjectsWithTag("Construction Points");
        _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gridTilemap = GameObject.Find("GridTilemap");
        _gridTilemap.SetActive(false);
    }

    private void Update()
    {
        if (_manager.IsTowerSelected)
            _gridTilemap.SetActive(true);
        else
            _gridTilemap.SetActive(false);
    }

    public void chooseTower(int chosentower)
    {
        _manager.SelectedTowerIndex = chosentower;
        if (_manager.gold < _manager.TowerInformation[chosentower].cost)
            return;
        _manager.IsTowerSelected = true;
    }
}