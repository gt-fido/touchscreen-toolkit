using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogSlider : Slider {
    public float handle_mass = 1.0f;

    public void Update() {
        float rotation = this.transform.eulerAngles.z;
    }

    public void RotateSlider(float degree) {
        // If the absolute difference is less than 0.1 do nothing.
        if(Mathf.Abs(this.transform.eulerAngles.z - degree) <= 0.1)
            return;
        
        // Otherwise rotate to the new degree
        this.transform.localEulerAngles = new Vector3(0.0f,0.0f,degree);

        // TODO: Intelligently change scale to keep in frame
   }

   public void ScaleHandle(float scale) {
        // If the absolute difference is less than 0.01 do nothing.
        if(Mathf.Abs(this.handleRect.transform.localScale.x - scale) <= 0.01)
            return;
        
        // Otherwise scale to new scale
        this.handleRect.transform.localScale = new Vector3(scale, scale, 1.0f);
   }

   public void SetWidth(float width) {
        // If the absolute difference is less than 0.01 do nothing.
        RectTransform rt = this.transform as RectTransform;

        // Otherwise scale to new scale
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
   }

    public void SetHeight(float height) {
        // If the absolute difference is less than 0.01 do nothing.
        RectTransform rt = this.transform as RectTransform;

        // Otherwise scale to new scale
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
   }
}
