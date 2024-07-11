using UnityEngine;

[System.Serializable]
public class ParticleColorData
{
    public string prefabId;
    public Color32 particleColor;

}
[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public ParticleColorData[] particleColorData;

    public Color32 GetParticleColorFromItemType(string prefabId)
    {
        foreach (var item in particleColorData)
        {
            if (item.prefabId==prefabId)
                return item.particleColor;
        }
        return Color.white;
    }
}