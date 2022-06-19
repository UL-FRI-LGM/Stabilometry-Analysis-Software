using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccordionHeaderComponent : MonoBehaviour
{
    #region Variables
    public bool open { set; get; } = false;

    [SerializeField]
    private Image elementHolder = null;

    private RectTransform buttonTransform = null;
    private RectTransform contentTransform = null;
    private AccordionComponent parentScript = null;

    private int index = -1;


    private float duration = 0.5f;

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
    #endregion

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
        parentScript.ElementClicked(index);
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
        elementHolder.DOFillAmount(0, duration);
    }

    private void CollapseSpace()
    {
        elementHolder.DOFillAmount(1, duration);
    }

    public void SetNewPosition(float yPosition)
    {
        buttonTransform.DOMoveY(yPosition, duration);
    }
}
