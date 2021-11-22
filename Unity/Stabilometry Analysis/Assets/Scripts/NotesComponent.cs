using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotesComponent : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TextMeshProUGUI NoteText = null;
    #endregion

    public void SetText(string text)
    {
        NoteText.text = text;
    }
}
