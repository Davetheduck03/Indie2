using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [SerializeField] private Test test;

    private void Start()
    {
        test.onSpaceEvent += OnSpaceListener;
    }

    private void OnSpaceListener()
    {
        Debug.Log("Test2 listen");
    }
}
