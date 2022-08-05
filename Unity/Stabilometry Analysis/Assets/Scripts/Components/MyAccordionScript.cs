using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAccordionScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private AccordionHeaderComponent[] headers = null;

    private bool updating = false;
    private RectTransform rectTransform = null;
    #endregion

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < headers.Length; i++)
            headers[i].SetParentScript(this, i);
    }

    public void ElementClicked(int index)
    {
        if (updating)
            return;
        else
            updating = true;

        float newPosition = 0;

        for (int i = 0; i < headers.Length; i++)
        {
            if (i == index)
                headers[i].OpenClose();
            else 
                headers[i].Close();

            headers[i].SetNewPosition(newPosition);

            newPosition -= headers[i].GetSize;
        }

        updating = false;
    }
}
