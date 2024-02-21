using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;
    [SerializeField] private float speed = 0.4f;
    [SerializeField] private int spriteIndex = 0;

    void Start()
    {
        StartCoroutine(playAnimation());
    }

    IEnumerator playAnimation(){
        while(true){
            yield return new WaitForSeconds(speed);

            if(spriteIndex >= sprites.Length) spriteIndex = 0;

            image.sprite = sprites[spriteIndex];
            spriteIndex++;
        }
    }
}
