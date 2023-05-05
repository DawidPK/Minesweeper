using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


public enum difficulty
{
    easy,
    medium, 
    hard
}

public class GameHandler : MonoBehaviour
{
    
    [SerializeField] static GameObject Menu, UI;
    [SerializeField] static TextMeshProUGUI text;
    [SerializeField] BoardGen board;
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume=0.5f;
    public static bool isPaused = true;
    public difficulty Difficulty = difficulty.medium;
    private void Awake()
    {
        Menu = GameObject.Find("MainMenu");
        text = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        board = GameObject.Find("Board").GetComponent<BoardGen>();
        UI = GameObject.Find("GameUI");
        UI.SetActive(false);
    }
    public void check()
    {
        //Debug.Log(BoardGen.invisibleFieldsCords.Count);
        if(BoardGen.invisibleFieldsCords.Count != 0){return;}
        
        int flaggedProperly = 0;
        for (int i = 0; i < BoardGen.flaggedFieldsCords.Count; i++)
        {
            FieldScript g = GameObject.Find(BoardGen.flaggedFieldsCords.ElementAt(i)).GetComponent<FieldScript>();
            if(g.Typ == type.bomb){flaggedProperly += 1;}
        }
        if(flaggedProperly == BoardGen.flaggedFieldsCords.Count){gameWon();}
        
    }
    public IEnumerator gameOver()
    {
        audioSource.PlayOneShot(clip, volume);
        foreach (var item in board.bombCords)
        {
            FieldScript f = GameObject.Find(item).GetComponent<FieldScript>();
            f.setVisible();
            yield return new WaitForSeconds(0.1f);
        }
        isPaused = true;
        Menu.SetActive(true);
        text.text = "Game Over";
        UI.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        //animation here
    }
    public void buttonStart()
    {
        StartCoroutine(startGame());
    }
    public IEnumerator startGame()
    {
        board.Clear();
        yield return new WaitForSeconds(0.3f);
        board.GenFields(Difficulty);
        Menu.SetActive(false);
        UI.SetActive(true);
        Camera cum = Camera.main;
        switch(Difficulty)
        {
            case difficulty.easy:
                cum.orthographicSize = 6;
                break;
            case difficulty.medium:
                cum.transform.position = new Vector3(0, 0.5f, -10f);
                cum.orthographicSize = 10;
                break;
            case difficulty.hard:
                cum.transform.position = new Vector3(-0.5f, 0, -10f);
                cum.orthographicSize = 10;
                break;
        } 
        isPaused = false;
    }
    public void difficultyChange(TMP_Dropdown change)
    {
        Difficulty = (difficulty)Enum.ToObject(typeof(difficulty), change.value);
    }
    public void gameWon(){
        Debug.Log("Wygrana");
        isPaused = true;
        Menu.SetActive(true);
        text.text = "You Won!";
    }

}
