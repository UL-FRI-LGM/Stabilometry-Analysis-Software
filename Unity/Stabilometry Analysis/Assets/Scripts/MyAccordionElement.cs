using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MyAccordionElement : MonoBehaviour
{
    [SerializeField]
    private Image blackImage = null;

    private bool open = true;
    

    private float maxSize = -170;
    private float minSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenClose()
    {
        if (open)
            ExpandSpace();
        else
            CollapseSpace();

        open = !open; 
    }

    private void ExpandSpace()
    {
        //RectTransform transform = blackImage.GetComponent<RectTransform>();
        blackImage.DOFillAmount(0, 0.5f);
        //transform.DOSizeDelta(new Vector2(transform.sizeDelta.x, maxSize), 1);
    }

    private void CollapseSpace()
    {
        //RectTransform transform = blackImage.GetComponent<RectTransform>();
        blackImage.DOFillAmount(1, 0.5f);
        //transform.DOSizeDelta(new Vector2(transform.sizeDelta.x, minSize), 1);
    }
}
