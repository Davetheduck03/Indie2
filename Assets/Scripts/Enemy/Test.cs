using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Action onSpaceEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onSpaceEvent?.Invoke();
        }
    }
}
