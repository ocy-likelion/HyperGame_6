using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct ItemData
{
    public string headName;
    public int itemValue;
}

public class ResultItem : MonoBehaviour
{
    public TMP_Text headText;
    public TMP_Text valueText;

    public void SetItem(ItemData itemData)
    {
        headText.text = itemData.headName;
        valueText.text = itemData.itemValue.ToString();
    }
}
