using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class AccordionHeaderComponent : MonoBehaviour
    {
        #region Variables
        public RectTransform ElementTransform { get; set; } = null;

        public bool open { set; get; } = false;

        [SerializeField]
        private Image blocker = null;

        private Image elementHolder = null;

        private RectTransform buttonTransform = null;
        private RectTransform contentTransform = null;
        private MyAccordionScript parentScript = null;

        private int index = -1;

        private float duration = 0.5f;

        public float GetSize
        {
            get
            {
                if (open)
                    return 2 * buttonTransform.sizeDelta.y + contentTransform.sizeDelta.y;
                else
                    return buttonTransform.sizeDelta.y;

            }
        }

        #endregion

        private void Awake()
        {
            ElementTransform = GetComponent<RectTransform>();
            buttonTransform = transform.GetChild(0).GetComponent<RectTransform>();
            contentTransform = transform.GetChild(1).GetComponent<RectTransform>();

            elementHolder = contentTransform.GetComponent<Image>();
        }

        public void SetParentScript(MyAccordionScript parentScript, int indeks)
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
                CollapseSpace();
            else
                ExpandSpace();

            open = !open;
        }

        public void Close()
        {
            CollapseSpace();

            open = false;
        }

        private void ExpandSpace()
        {
            blocker.gameObject.SetActive(false);
            elementHolder.DOFillAmount(1, duration);
        }

        private void CollapseSpace()
        {
            elementHolder.DOFillAmount(0, duration);
            blocker.gameObject.SetActive(true);
        }

        public void SetNewPosition(float yPosition)
        {
            ElementTransform.DOAnchorPosY(yPosition, duration);
            //transform.DOMoveY(yPosition, duration);
        }
    }
}