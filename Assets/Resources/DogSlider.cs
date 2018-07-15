using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogSlider : Slider {
    public void Update() {

        // TODO: Based on handle position, set value
        // TODO: Turn off physics if selected

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

   public void ScaleTrackXSize(float scale) {
        // If the absolute difference is less than 0.01 do nothing.
        if(Mathf.Abs(this.transform.localScale.x - scale) <= 0.01)
            return;

        // Otherwise scale to new scale
        this.transform.localScale = new Vector3(scale, 
                                                this.transform.localScale.y,
                                                this.transform.localScale.z);
   }

    public void ScaleTrackYSize(float scale) {
        // If the absolute difference is less than 0.01 do nothing.
        if(Mathf.Abs(this.transform.localScale.y - scale) <= 0.01)
            return;

        // Otherwise scale to new scale
        this.transform.localScale = new Vector3(this.transform.localScale.x, 
                                                scale,
                                                this.transform.localScale.z);
   }
}
