using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{

    [SerializeField]
    private Sprite spriteBack;
    [SerializeField]
    private Sprite spriteFront;

    private bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RectTransform>().localRotation.y > 0.7f && !flipped)
        {
            GetComponent<Image>().sprite = spriteFront;
            flipped = true;
        }
        else if(GetComponent<RectTransform>().localRotation.y < 0.7f && flipped)
        {
            GetComponent<Image>().sprite = spriteBack;
            flipped = false;
        }
    }
}
