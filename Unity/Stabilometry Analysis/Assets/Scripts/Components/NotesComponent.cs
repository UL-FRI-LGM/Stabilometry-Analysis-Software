using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class NotesComponent : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private TMP_InputField NoteText = null;

        public string text { get { return NoteText.text; } }
        #endregion

        /// <summary>
        /// Fill notes input field.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            NoteText.text = text;
        }

        public void MakeTextEditable(bool textEditable)
        {
            NoteText.interactable = textEditable;
        }
    }
}