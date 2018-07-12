// Draws 2 toggle controls, one with a text, the other with an image.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSelector : ToggleGroup
{
    public Texture aTexture;

    private bool toggleTxt = false;
    private bool toggleImg = false;
}