using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccordionHeaderComponent : MonoBehaviour
{
    private RectTransform buttonTransform = null;
    private RectTransform contentTransform = null;
    private AccordionComponent parentScript = null;

    private int index = -1;

    public bool open { set; get; } = false;


    public float closedSize
    {
        get
        {
            return buttonTransform.sizeDelta.y;
        }
    }

    public float openSize
    {
        get
        {
            return buttonTransform.sizeDelta.y + contentTransform.sizeDelta.y;
        }
    }

    private void Awake()
    {
        buttonTransform = GetComponent<RectTransform>();
        contentTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetParentScript(AccordionComponent parentScript, int indeks)
    {
        this.parentScript = parentScript;
        this.index = indeks;
    }

    public void HeaderClicked()
    {
        parentScript.HeaderClicked(index);
    }
}
