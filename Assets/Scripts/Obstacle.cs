using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] internal protected GameObject TextObject;
    [SerializeField] internal byte ObstacleFactor;
    [SerializeField] internal protected bool IsSuccess = false;
    private byte TempObstacleFactor;
    void Start()
    {
        Defaultvalue();
    }

    internal protected void UpdateObstacleFactor()
    {
        if (TempObstacleFactor > 0)
            TempObstacleFactor--;

        TextObject.GetComponent<TextMesh>().text = "X" + TempObstacleFactor.ToString();
        if (TempObstacleFactor <= 0)
            gameObject.SetActive(false);
    }

    internal protected void Defaultvalue()
    {
        TempObstacleFactor = ObstacleFactor;
        TextObject.GetComponent<TextMesh>().text = "X" + TempObstacleFactor.ToString();
    }
}
