// Decompiled with JetBrains decompiler
// Type: zoomScrollRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class zoomScrollRect : MonoBehaviour, IScrollHandler, IEventSystemHandler{
    private Vector3 initialScale;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float minZoom = 0.6f;

    private void Awake() => this.initialScale = this.transform.localScale;

    public void OnScroll(PointerEventData eventData){
        Vector3 vector3_1 = Vector3.one * (eventData.scrollDelta.y * this.zoomSpeed);
        Vector3 vector3_2 = this.ClampDesiredScale(this.transform.localScale + vector3_1);
        Debug.Log((object) ("desired: " + (object) vector3_2 + " delta:" + (object) vector3_1));
        this.transform.localScale = vector3_2;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale){
        Debug.Log((object) ("MAXZOOM: " + (object) this.maxZoom));
        Debug.Log((object) ("INIT: " + (object) (this.initialScale * this.maxZoom)));
        desiredScale = Vector3.Max(this.initialScale * this.minZoom, desiredScale);
        Debug.Log((object) ("desiredMAX: " + (object) (this.initialScale * this.minZoom) + " scale: " + (object) desiredScale));
        desiredScale = Vector3.Min(this.initialScale * this.maxZoom, desiredScale);
        Debug.Log((object) ("desiredMIN: " + (object) (this.initialScale * this.maxZoom) + " scale: " + (object) desiredScale));
        return desiredScale;
    }
}