using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class StabilometryImageElementScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject imagePrefab = null;
        [SerializeField] private RectTransform[] imageHolders = null;

        private List<GameObject> images = null;
        private StabilometryImagesMenuScript parentScript = null;

        // Todo: Probably remove this
        private int index = -1;

        private StabilometryMeasurement data = null;

        private Vector2 newPosition = new Vector2();

        #endregion

        private void Awake()
        {
            float halfSize = imageHolders[0].rect.width/ 2f;
            newPosition = new Vector2(-halfSize, halfSize);
        }

        public void SetData(int index, StabilometryMeasurement data, StabilometryImagesMenuScript parentScript)
        {
            this.index = index;
            this.data = data;
            this.parentScript = parentScript;
        }

        public void SetVisible(bool isVisible)
        {
            bool chartsSpawned = AreImagesCreated();

            if (!isVisible && chartsSpawned)
                DestroyImages();
            else if (isVisible && !chartsSpawned)
                CreateImages();
        }

        private bool AreImagesCreated()
        {
            return false;
        }

        private void DestroyImages()
        {
            foreach (GameObject image in images)
                Destroy(image);

            images = null;
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
        }

        private GameObject InstantiatePrefab(StabilometryTask task, RectTransform parent)
        {
            GameObject instance = Instantiate(imagePrefab, parent.transform);
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.sizeDelta = parent.sizeDelta;
            rect.localPosition = newPosition;

            return instance;
        }

        public void buttonclicked()
        {
            parentScript.OpenAnalysisMenu(data);
        }
    }
}
