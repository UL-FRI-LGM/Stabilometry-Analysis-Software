using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarScript : MonoBehaviour
{
    #region Variables
    private Scrollbar scrollbar = null;
    #endregion

    private void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    public void SetSize(float fullSize, float shownSize)
    {
        if (fullSize <= shownSize)
            scrollbar.size = 1;
        else
            scrollbar.size = shownSize / fullSize;
    }
}
