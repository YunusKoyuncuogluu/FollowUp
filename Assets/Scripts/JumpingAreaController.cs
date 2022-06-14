using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAreaController : MonoBehaviour
{
    [SerializeField] internal protected GameObject TextCountObject;
    [SerializeField] internal protected byte CountFactor;
    private byte TempCountFactor;
    [SerializeField] internal protected bool IsFactor = false;

    void Start()
    {
        DefaultValue();
    }

    internal void UpdateCountFactorText()
    {
        TempCountFactor = CountFactor;
        TextCountObject.GetComponent<TextMesh>().text = TempCountFactor.ToString();
    }

    internal void DefaultValue()
    {
        UpdateCountFactorText();
        TextCountObject.SetActive(IsFactor);
    }
}
