using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StabilometryAnalysis
{
    public class ScrollbarScript : MonoBehaviour
    {
        #region Variables
        public float valuePositon
        {
            get { return scrollbar.value; }
            set { scrollbar.value = value; }
        }
        private Scrollbar scrollbar = null;

        private static string mouseScrollWheelAxis = "Mouse ScrollWheel";
        private bool swallowMouseWheelScrolls = true;
        private bool isEnabled = false;
        private float scrollSensitivity = 0.12f;
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

        private void OnEnable()
        {
            isEnabled = true;
        }

        private void OnDisable()
        {
            isEnabled = false;
        }

        private void Update()
        {
            if (!isEnabled || LocationPointer.mainScript.backgroundBlocker.activeSelf)
                return;

            var delta = UnityEngine.Input.GetAxis(mouseScrollWheelAxis);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.scrollDelta = new Vector2(0f, delta);

            swallowMouseWheelScrolls = false;
            Scroll(pointerData);
            swallowMouseWheelScrolls = true;
        }

        private void Scroll(PointerEventData data)
        {

            if (IsMouseWheelRolling() && swallowMouseWheelScrolls)
            {
                // Eat the scroll so that we don't get a double scroll when the mouse is over an image
            }
            else
            {
                // Amplify the mousewheel so that it matches the scroll sensitivity.
                if (data.scrollDelta.y < -Mathf.Epsilon)
                {
                    float newValue = scrollbar.value + scrollSensitivity;
                    if (newValue > 1)
                        newValue = 1;

                    scrollbar.value = newValue;
                }
                else if (data.scrollDelta.y > Mathf.Epsilon)
                {
                    float newValue = scrollbar.value - scrollSensitivity;
                    if (newValue < 0)
                        newValue = 0;

                    scrollbar.value = newValue;
                }
            }
        }

        private static bool IsMouseWheelRolling()
        {
            return UnityEngine.Input.GetAxis(mouseScrollWheelAxis) != 0;
        }
    }
}