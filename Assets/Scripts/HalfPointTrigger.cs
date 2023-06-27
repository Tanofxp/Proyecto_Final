using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject lapCompleteTrigger;
    public GameObject halfLapTrigger;

    private void OnTriggerEnter()
    {
        lapCompleteTrigger.SetActive(true);
        halfLapTrigger.SetActive(false);
    }
}
