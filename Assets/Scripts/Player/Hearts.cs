using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts: MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    Image heartImg;


    private void Awake()
    {
        heartImg = GetComponent<Image>();
    }

    //
    public void SetHearts(int status)
    {
        if (status == 0)
        {
            heartImg.sprite = emptyHeart;
            Debug.Log("Setting empty heart sprite");
        }
        else if (status == 1)
        {
            heartImg.sprite = halfHeart;
            Debug.Log("Setting half heart sprite");
        }
        else if (status == 2)
        {
            heartImg.sprite = fullHeart;
            Debug.Log("Setting full heart sprite");
        }
    }
}