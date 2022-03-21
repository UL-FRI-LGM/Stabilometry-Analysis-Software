using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalysisMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject stabilometryImagePrefab = null;

    private GameObject imageInstance = null;

    #endregion

    private void OnEnable()
    {
        imageInstance = Instantiate(stabilometryImagePrefab, this.transform);

        //imageInstance.GetComponent<StabilometryImageScript>().
    }

    private void OnDisable()
    {
        Destroy(imageInstance);
    }
}
