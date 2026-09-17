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

    private Dictionary<string, PetPoseSO> poseDictionary;

    public void InitializePoseDictionary()
    {
        poseDictionary = new Dictionary<string, PetPoseSO>();
        foreach (var pose in poses)
        {
            poseDictionary.Add(pose.poseName, pose);
        }
    }

    public PetPoseSO GetPoseByName(string poseName)
    {
        if (poseDictionary == null)
        {
            InitializePoseDictionary();
        }

        if (poseDictionary.TryGetValue(poseName, out PetPoseSO poseData))
        {
            return poseData;
        }
        else
        {
            Debug.LogWarning($"Pose '{poseName}' não encontrada na Lista de Poses, entregando pose base.");
            
            // Retorna o objeto base completo
            return poseDictionary[PetPoses.Neutral]; 
        }
    }
}
