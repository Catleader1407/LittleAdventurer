using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Define a public AudioClip for each action
    public AudioClip appear;
    public AudioClip dead;
    public AudioClip npc2Attack;
    public AudioClip npc1AttackLanding;
    public AudioClip combo01Impact;
    public AudioClip combo02Impact;
    public AudioClip combo03Impact;
    public AudioClip combo01Swing;
    public AudioClip combo02Swing;
    public AudioClip combo03Swing;
    public AudioClip playerHitRange;
    public AudioClip pickUpCoin;
    public AudioClip pickUpHeal;
    public AudioClip gateOpen;
    [SerializeField] private AudioSource audioSource;

    public AudioClip nonCombatBGM;
    public AudioClip CombatBGM;
    [SerializeField] private AudioSource audioSourceBGM;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSourceBGM = GetComponent<AudioSource>();
        playNonCombatBGM();
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.volume = volume; // Set the desired volume
            audioSource.PlayOneShot(clip);
        }
        else
        {
            if (clip == null)
                Debug.LogWarning("Attempted to play a null AudioClip.");
            if (audioSource == null)
                Debug.LogError("AudioSource component is null.");
        }
    }
    public void PlayBGM(AudioClip clip, float volume)
    {
        if (audioSourceBGM != null && clip != null)
        {
            audioSourceBGM.clip = clip; // Assign the BGM clip
            audioSourceBGM.volume = volume; // Set the desired volume
            audioSourceBGM.loop = true; // Enable looping
            audioSourceBGM.Stop(); // Stop current audio to ensure a clean start
            audioSourceBGM.Play(); // Start playing the new BGM clip
        }
        else
        {
            if (clip == null)
                Debug.LogWarning("Attempted to play a null AudioClip for BGM.");
            if (audioSourceBGM == null)
                Debug.LogError("AudioSource component is null in PlayBGM.");
        }
    }
    
    public void playNonCombatBGM()
    {
        PlayBGM(nonCombatBGM, 0.5f);
    }

    public void playCombatBGM()
    {
        PlayBGM(CombatBGM, 0.5f);
    }

    public void playSoundNPC01()
    {
        PlaySound(npc1AttackLanding,0.3f);
    }
    public void playSoundNPC02()
    {
        PlaySound(npc2Attack,0.5f);
    }

}
