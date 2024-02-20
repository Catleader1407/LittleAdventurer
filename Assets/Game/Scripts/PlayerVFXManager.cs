using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public ParticleSystem blade01;
    public VisualEffect slash;
    public VisualEffect heal;
    public ParticleSystem blade02;
    public ParticleSystem blade03;
    public void Update_FootStep(bool state)
    {
        if (state)
        {
            footStep.Play();            
        } 
        else
        {
            footStep.Stop();
        }
    }
    public void playBlade01()
    {
        blade01.Play();
        AudioManager.instance.PlaySound(AudioManager.instance.combo01Swing, 0.3f);
    }
    public void playBlade02()
    {
        blade02.Play();
        AudioManager.instance.PlaySound(AudioManager.instance.combo02Swing, 0.3f);
    }
    public void playBlade03()
    {
        blade03.Play();
        AudioManager.instance.PlaySound(AudioManager.instance.combo03Swing, 0.3f);
    }
    public void stopBlade()
    {
        blade01.Simulate(0);
        blade01.Stop();

        blade02.Simulate(0);
        blade02.Stop();

        blade03.Simulate(0);
        blade03.Stop();
    }
    public void playSlash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }
    public void playHeal()
    {
        heal.Play();
    }
}
