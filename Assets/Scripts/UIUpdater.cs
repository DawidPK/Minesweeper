using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _flagText;
    [SerializeField] BoardGen board;
    private void Awake()
    {
        board = GameObject.Find("Board").GetComponent<BoardGen>();
        
    }
    void Start()
    {
        board.uiUpdated += updateUi;
    }

    void updateUi(object sender, int a)
    {
        Debug.Log("Dupa");
        _flagText.text = a.ToString();
        Debug.Log(a);
    }
}
