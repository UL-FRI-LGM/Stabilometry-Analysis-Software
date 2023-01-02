using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class ScrollbarScript : MonoBehaviour
    {
        #region Variables
        public float valuePositon { 
            get { return scrollbar.value; } 
            set { scrollbar.value = value; } }
        private Scrollbar scrollbar = null;
        #endregion

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
        }

        public void SetSize(float fullSize, float shownSize)
        {
            if (scrollbar == null)
                scrollbar = GetComponent<Scrollbar>();

            if (fullSize <= shownSize)
                scrollbar.size = 1;
            else
                scrollbar.size = shownSize / fullSize;
        }
    }
}