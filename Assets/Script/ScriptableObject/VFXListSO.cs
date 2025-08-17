using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXList", menuName = "SO/FX/VFXList")]
public class VFXListSO : ScriptableObject
{
    public List<VFXDataSO> VFXList;
}