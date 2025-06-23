using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
   
    public static AudioManager Instance;
    [SerializeField] AudioSource audiobackground;
    [SerializeField] AudioClip audiobackgroundclip;
    [SerializeField] AudioSource EffectAudio;
    [SerializeField] AudioClip Coinclip;
    [SerializeField] AudioClip Jumpclip;
    [SerializeField] AudioClip Hurt;
    [SerializeField] AudioClip pickmagnet;
    [SerializeField] AudioClip buyitemsclip;
    [SerializeField] AudioClip touchClip;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayAudioBackGround();
    }
    public void PlayAudioBackGround()
    {
        if (audiobackground != null && audiobackgroundclip != null)
        {
            audiobackground.clip = audiobackgroundclip;
            audiobackground.loop = true;
            audiobackground.Play();
        }
    }
    public void CoinClip() => EffectAudio.PlayOneShot(Coinclip);
    public void JumpAudio() => EffectAudio.PlayOneShot(Jumpclip);
    public void HurtClip() => EffectAudio.PlayOneShot(Hurt);
    public void PickMagnet() => EffectAudio.PlayOneShot(pickmagnet);
    public void BuyItems() => EffectAudio.PlayOneShot(buyitemsclip, 2f);
    public void Touch() => EffectAudio.PlayOneShot(touchClip);

}