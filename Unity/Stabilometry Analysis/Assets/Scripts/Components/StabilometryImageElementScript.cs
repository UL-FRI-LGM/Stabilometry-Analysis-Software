using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StabilometryAnalysis
{
    public class StabilometryImageElementScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject imagePrefab = null;
        [SerializeField] private RectTransform[] imageHolders = null;
        [SerializeField] private TextMeshProUGUI dateText = null;

        private List<GameObject> images = null;
        private StabilometryImagesMenuScript parentScript = null;

        private StabilometryMeasurement data = null;

        private Vector2 newPosition = new Vector2();
        private bool imagesCreated = false;

        #endregion

        private void Awake()
        {
            float halfSize = imageHolders[0].rect.width/ 2f;
            newPosition = new Vector2(-halfSize, halfSize);
        }

        public void SetData(StabilometryMeasurement data, StabilometryImagesMenuScript parentScript)
        {
            this.data = data;
            this.parentScript = parentScript;
            dateText.text = data.dateTime.ToStringShortNewLine();
        }

        public void SetVisible(bool isVisible)
        {
            if (!isVisible && imagesCreated)
                DestroyImages();
            else if (isVisible && !imagesCreated)
                CreateImages();
        }

        private void DestroyImages()
        {
            foreach (GameObject image in images)
                Destroy(image);

            images = null;

            imagesCreated = false;
        }

        private void CreateImages()
        {
            if (images != null)
                DestroyImages();

            images = new List<GameObject>();

            images.Add(InstantiatePrefab(data.eyesOpenSolidSurface, imageHolders[0]));
            images.Add(InstantiatePrefab(data.eyesClosedSolidSurface, imageHolders[1]));
            images.Add(InstantiatePrefab(data.eyesOpenSoftSurface, imageHolders[2]));
            images.Add(InstantiatePrefab(data.eyesClosedSoftSurface, imageHolders[3]));

            imagesCreated = true;
        }

        private GameObject InstantiatePrefab(StabilometryTask task, RectTransform parent)
        {
            GameObject instance = Instantiate(imagePrefab, parent.transform);
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.sizeDelta = parent.sizeDelta;
            rect.localPosition = newPosition;

            bool lowPrecision = false;

            if (task != null)
                instance.transform.GetChild(0).GetComponent<StabilometryImageScript>().DrawImage(task, lowPrecision);

            return instance;
        }

        public void buttonclicked()
        {
            parentScript.OpenAnalysisMenu(data);
        }
    }
}
