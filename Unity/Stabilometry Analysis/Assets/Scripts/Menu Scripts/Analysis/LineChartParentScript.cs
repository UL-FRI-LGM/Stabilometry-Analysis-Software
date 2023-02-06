using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public abstract class LineChartParentScript : MonoBehaviour
    {
        public abstract void OpenAnalysisMenu(int index);

        /// <summary>
        /// Get the size of the area for currently spawned charts.
        /// </summary>
        /// <param name="allInstances"></param>
        /// <param name="maskSize"></param>
        /// <returns></returns>
        protected static float GetCurrentChartAreaSize(List<GameObject> allInstances, float maskSize)
        {
            if (allInstances.Count < 2)
                return maskSize;

            // else
            RectTransform firstRect = (RectTransform)allInstances[0].transform;
            RectTransform lastRect = (RectTransform)allInstances[allInstances.Count - 1].transform;

            float result = Mathf.Abs((firstRect.localPosition.y + firstRect.rect.height / 2f) - (lastRect.localPosition.y - lastRect.rect.height / 2f));

            return result;
        }

    }
}