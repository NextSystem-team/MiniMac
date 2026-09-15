using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SFXListSO sfxListSO;
    [SerializeField] private MusicListSO musicListSO;

    private Dictionary<SFXType, SFXData> sfxList = new();
    private Dictionary<MusicType, MusicData> musicList = new();

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // foreach (var music in musicListSO.musicList)
        //     musicList.Add(music.type, music);

        foreach (var sfx in sfxListSO.sfxList)
            sfxList.Add(sfx.type, sfx);
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxList.TryGetValue(type, out SFXData sfxData))
        {
            AudioSource freeSource = GetFreeSFXSource();
            freeSource.pitch = Random.Range(0.95f, 1.05f);
            freeSource.PlayOneShot(sfxData.clip);
        }
    }

    private AudioSource GetFreeSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying) return source;
        }
        return sfxSources[0];
    }
}
