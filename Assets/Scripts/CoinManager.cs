using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private int coinAmount;
    
    private void OnEnable()
    {
        Coin.OnCoinCollected += AddCoin;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= AddCoin;
    }

    private void AddCoin()
    {
        coinAmount++;
        coinText.text = coinAmount.ToString();
    }
}
