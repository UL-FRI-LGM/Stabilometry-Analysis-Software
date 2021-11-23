using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotesComponent : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TMP_InputField NoteText = null;
    #endregion

    public void SetText(string text)
    {
        NoteText.text = text;
    }
}
