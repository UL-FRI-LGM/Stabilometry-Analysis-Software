using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAccordionScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private AccordionHeaderComponent[] headers = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < headers.Length; i++)
            headers[i].SetParentScript(this, i);
    }

    public void ElementClicked(int index)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            if (i == index)
                headers[i].OpenClose();
            else if (headers[i].open)
                headers[i].OpenClose();

            headers[i].SetNewPosition(0);
        }

    }
}
