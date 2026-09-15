using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    Minigame,
    Shop,
    Qustion
}

public enum SFXType
{
    UI_Button_Click,
    UI_Item_Buy,
    Wardrobe,
    Pet_Caress,
    Pet_Caress_Success
}

[System.Serializable]
public class SFXData
{
    public AudioClip clip;
    public SFXType type;
}

[System.Serializable]
public class MusicData
{
    public AudioClip clip;
    public MusicType type;
}

[CreateAssetMenu(fileName = "SFXListSO", menuName = "Scriptable Objects/Audio/SFX List")]
public class SFXListSO : ScriptableObject
{
    public List<SFXData> sfxList;
}

[CreateAssetMenu(fileName = "MusicListSO", menuName = "Scriptable Objects/Audio/Music List")]
public class MusicListSO : ScriptableObject
{
    public List<MusicData> musicList;
}
