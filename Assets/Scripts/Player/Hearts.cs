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

    public void SetHearts(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImg.sprite = emptyHeart;
                Debug.Log("Setting empty heart sprite");
                break;
            case HeartStatus.Half:
                heartImg.sprite = halfHeart;
                Debug.Log("Setting half heart sprite");
                break;
            case HeartStatus.Full:
                heartImg.sprite = fullHeart;
                Debug.Log("Setting full heart sprite");
                break;
        }
    }
}

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}