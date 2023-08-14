using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideBetPlayerCell : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] Image betColor;
    [SerializeField] TMP_Text betAmount;

    public void SetPlayerData(string _playerName, Color _betColor, string _betAmount)
    {
        playerName.text = _playerName;
        betColor.color = _betColor;
        betAmount.text = _betAmount;
    }
}
