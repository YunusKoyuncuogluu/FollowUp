using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("TAKIP EDILECEK HEDEF OBJE")]
    [SerializeField] private Transform Target;
    [Header("TAKIP YUMUSAKLIGININ HIZI")]
    [SerializeField] private float LerpSpeed;

    private Vector3 Diff = Vector3.zero;

    internal bool Active = false;


    void Start()
    {
        Diff = transform.position - Target.position;
        Active = true;
    }


    void Update()
    {
        if (!Active)
            return;
        transform.position = Vector3.Lerp(transform.position, (Target.position + Diff), LerpSpeed * Time.deltaTime);
    }
}
