using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
