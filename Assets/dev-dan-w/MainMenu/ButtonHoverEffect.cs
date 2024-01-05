using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    private Sprite buttonOriginalSprite;
    public Sprite buttonHoverSprite;
    void Start()
    {
        buttonOriginalSprite = gameObject.GetComponent<Image>().sprite;
    }

    public void changeOnHover()
    {
        this.gameObject.GetComponent<Image>().sprite = buttonHoverSprite;
    }

    public void changeOnUnhover()
    {
        this.gameObject.GetComponent<Image>().sprite = buttonOriginalSprite;
    }

}
