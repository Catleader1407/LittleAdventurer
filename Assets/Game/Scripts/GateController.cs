using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GameObject gateVisual;
    private Collider gateCollider;
    public float openDuration = 2f;
    public float openTargetY = -3f;

    private void Awake()
    {
        gateCollider = GetComponent<Collider>();
    }

    IEnumerator openGateAnimation()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.gateOpen,0.5f);
        float currentOpenDuration = 0f;
        Vector3 startPos = gateVisual.transform.position;
        Vector3 targetPos = startPos + Vector3.up * openTargetY;

        while (currentOpenDuration < openDuration)
        {
            currentOpenDuration += Time.deltaTime;
            gateVisual.transform.position = Vector3.Lerp(startPos, targetPos, currentOpenDuration/ openDuration);
            yield return null;
        }
        gateCollider.enabled = false;
    }

    public void openGate()
    {
        StartCoroutine(openGateAnimation());
    }
}
