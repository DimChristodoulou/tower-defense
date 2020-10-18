// Decompiled with JetBrains decompiler
// Type: ConstructionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ConstructionPoint : MonoBehaviour
{
    private bool _isOccupied;
    private GameManager _manager;

    public bool IsOccupied
    {
        get => _isOccupied;
        set => _isOccupied = value;
    }

    private void Start() => _manager = GameObject.Find("GameManager").GetComponent<GameManager>();

    public void OnMouseDown()
    {
        if (!_manager.IsTowerSelected || _isOccupied)
            return;
        ConstructTower();
    }

    private void ConstructTower()
    {
        GameObject original = Resources.Load(_manager.TowerInformation[_manager.SelectedTowerIndex].pathToResource) as GameObject;
        if (original != null && original.GetComponent<Tower>().towerData.cost > _manager.gold)
            return;
        _manager.RemoveGold(original.GetComponent<Tower>().towerData.cost);
        Instantiate(original, transform.position, Quaternion.identity);
        _manager.IsTowerSelected = false;
        _manager.SelectedTowerIndex = -1;
        _isOccupied = true;
    }
}