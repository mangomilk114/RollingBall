using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public SpriteRenderer Img;
    
    public void Initialize(string trackImg)
    {
        Img.sprite = (Sprite)Resources.Load(trackImg, typeof(Sprite));
    }
}
