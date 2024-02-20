using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public VisualEffect attackVFX;
    public ParticleSystem beingHitVFX;
    public VisualEffect beingHitSplashVFX;
    public void playAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }
    public void burstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }
    public void playBeingHitVFX(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;
        beingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
        beingHitVFX.Play();
        Vector3 splashPos = transform.position;
        splashPos.y += 2f;
        VisualEffect newSplashVFX = Instantiate(beingHitSplashVFX, splashPos, Quaternion.identity);
        newSplashVFX.SendEvent("OnPlay");
        Destroy(newSplashVFX.gameObject, 10f);
    }
}
