using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class MeasurementImageScript : MonoBehaviour
    {
        [SerializeField] private GameObject stabilometryImagePrefab = null;
        [SerializeField] private TextMeshProUGUI durationText = null;

        private Vector2 localPosition = new Vector2(0, 0);
        private Vector2 localSize = new Vector2(253, 253);

        private GameObject instance = null;

        public void SetData(StabilometryTask task)
        {
            if (task != null)
                durationText.text = $"{task.GetDuration()} s";
            else
                durationText.text = $"";

            HandleImage(task);
        }

        private void HandleImage(StabilometryTask task)
        {
            if (instance != null)
                Destroy(instance);

            instance = Instantiate(stabilometryImagePrefab, this.transform);

            RectTransform rect = instance.GetComponent<RectTransform>();

            rect.sizeDelta = localSize;
            rect.localPosition = localPosition;

            bool highPrecision = true;

            if (task != null)
                instance.transform.GetChild(0).GetComponent<StabilometryImageScript>().DrawImage(task, highPrecision);
        }

        private void OnDisable()
        {
            Destroy(instance);
        }
    }
}