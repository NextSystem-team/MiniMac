using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PetPoseSO
{
    public string poseName;
    public Sprite poseSprite;
}

[CreateAssetMenu(fileName = "PoseListSO", menuName = "Scriptable Objects/Pose List")]
public class PoseListSO : ScriptableObject
{
    public List<PetPoseSO> poses;

    private Dictionary<string, Sprite> poseDictionary;

    public void InitializePoseDictionary()
    {
        poseDictionary = new Dictionary<string, Sprite>();
        foreach (var pose in poses)
        {
            poseDictionary.Add(pose.poseName, pose.poseSprite);
        }
    }

    public Sprite GetPoseByName(string poseName)
    {
        if (poseDictionary == null)
        {
            InitializePoseDictionary();
        }

        if (poseDictionary.TryGetValue(poseName, out Sprite poseSprite))
        {
            return poseSprite;
        }
        else
        {
            Debug.LogWarning($"Pose '{poseName}' não encontrada na Lista de Poses, entregando pose base.");
            return poseDictionary[PetPoses.Neutral];
        }
    }
}
