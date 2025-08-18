using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "SO/FX/VFXData")]
public class VFXDataSO : ScriptableObject
{
    [Tooltip("등록할 프리팹은 EditCanvas를 통해 생성해주세요. VFX는 UI로 취급됩니다.")]
    public GameObject vfxPrefab; 
    
    [Tooltip("VFX가 나타나고 지속될 시간을 지정해주세요.")]
    public float duration;
    
    [Tooltip("풀링할 갯수를 지정해주세요. 자주 생성된다면 여유롭게 만들어주세요.")]
    public int poolSize;
}