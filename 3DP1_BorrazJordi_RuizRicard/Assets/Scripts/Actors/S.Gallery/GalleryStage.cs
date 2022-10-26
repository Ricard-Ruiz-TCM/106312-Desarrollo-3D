using System;
using System.Collections.Generic;
using UnityEngine;

// clase que determina un stage completo
[Serializable]
public class GalleryStage {
    public int StageID;
    public float StageDuration;
    public SGallery Gallery;
    public GameObject StageObj;
    public List<GalleryItem> Items;

    public GalleryStage(SGallery galery, int id, GameObject go, float duration) {
        Gallery = galery; StageID = id; StageObj = go; StageDuration = duration;
    }

    public void FillItems() {
        Items = new List<GalleryItem>();
        foreach (Transform c in StageObj.transform) {
            GalleryItem gi = c.gameObject.GetComponentInChildren<GalleryItem>();
            if (gi != null) {
                Items.Add(gi);
                Items[Items.Count - 1].SetGallery(Gallery);
            }
        }
    }

    public void ShowRandomItem() {
        Items[UnityEngine.Random.Range(0, Items.Count - 1)].Show();
    }

    public void ActiveItems() {
        foreach (GalleryItem i in Items) {
            if (!i.transform.parent.gameObject.activeSelf)
                i.transform.parent.gameObject.SetActive(true);
        }
    }

};