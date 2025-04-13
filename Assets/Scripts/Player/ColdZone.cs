using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.inColdZone = true;
            CyanTintEffect.Instance.GetComponent<CyanTintEffect>().ToggleTint();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.inColdZone = false;
            CyanTintEffect.Instance.GetComponent<CyanTintEffect>().ToggleTint();
        }
    }
}
