using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountImgFont : MonoBehaviour
{
    public Transform StartPos;

    private List<Image> ImgFontList = new List<Image>();

    public enum IMG_RANGE
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public void SetValue(string count, IMG_RANGE range)
    {
        string countSrt = count.ToString();
        int countLength = countSrt.Length;
        Image prevImg = null;
        int allWidthSize = 0;

        for (int i = 0; i < ImgFontList.Count; i++)
        {
            ImgFontList[i].gameObject.SetActive(false);
        }

        string formString = "number_{0}";

        for (int i = 0; i < countLength; i++)
        {
            char oneStr = countSrt[i];
            string imgfileName = string.Format(formString, oneStr);

            if (ImgFontList.Count <= i)
            {
                var obj = new GameObject();
                obj.AddComponent<Image>();
                Image tempImage = obj.GetComponent<Image>();
                ImgFontList.Add(tempImage);
                obj.gameObject.transform.SetParent(gameObject.transform);
                obj.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

            Image countImg = ImgFontList[i];
            countImg.gameObject.SetActive(true);
            CommonData.SetImageFile(imgfileName, ref countImg);
            allWidthSize += (int)countImg.sprite.rect.size.x;
        }

        for (int i = 0; i < countLength; i++)
        {
            Image currImg = ImgFontList[i];
            Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;
            Vector3 prevImgLocalPosition = prevImg == null ? Vector3.zero : prevImg.gameObject.transform.localPosition;
            Vector2 currimgSize = currImg.sprite.rect.size;
            Vector2 previmgSize = prevImg == null ? Vector2.zero : prevImg.sprite.rect.size;

            currImgLocalPosition = new Vector3(prevImgLocalPosition.x + previmgSize.x / 2 + currimgSize.x / 2, 0);
            currImg.gameObject.transform.localPosition = currImgLocalPosition;

            prevImg = currImg;
        }

        if (range == IMG_RANGE.CENTER)
        {
            if (countLength == 1)
            {
                Image currImg = ImgFontList[0];
                currImg.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                for (int i = 0; i < countLength; i++)
                {
                    Image currImg = ImgFontList[i];
                    Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;

                    currImgLocalPosition = new Vector3(currImgLocalPosition.x - allWidthSize / 2, 0);
                    currImg.gameObject.transform.localPosition = currImgLocalPosition;
                }
            }

        }
        else if (range == IMG_RANGE.RIGHT)
        {
            for (int i = 0; i < countLength; i++)
            {
                Image currImg = ImgFontList[i];
                Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;

                currImgLocalPosition = new Vector3(currImgLocalPosition.x - allWidthSize + ImgFontList[0].sprite.rect.size.x / 2, 0);
                currImg.gameObject.transform.localPosition = currImgLocalPosition;
            }
        }
    }

}
