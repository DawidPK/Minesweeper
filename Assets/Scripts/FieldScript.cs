using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.InputSystem;

public enum type 
    {
        field,
        bomb
    }
    public enum look 
    {
        invisible,
        number,
        flagged
    }
public class FieldScript : MonoBehaviour ,IPointerClickHandler
{
    SpriteRenderer render;
    [SerializeField] Sprite bombSprite, visibleSprite, flagSprite;
    [SerializeField] TextMeshProUGUI text;
    GameHandler game;
    int bombsAround = 0;
    public type Typ = type.field;
    public look Wyg = look.invisible;
    BoardGen board;
    public void changeToBomb() 
    {
        Typ = type.bomb;
        Destroy(text.gameObject);
        //render.color = Color.black;
    }
    public void setInvisible() 
    {
        Wyg = look.invisible;
        text.enabled = false;
        render.color = Color.gray;
        string name = $"Cell{transform.position.x}{transform.position.y}";
    }
    public void setVisible() 
    {
        switch (Typ) 
        {
            case type.field:
                render.color = Color.white;
                text.enabled = true;
                render.sprite = visibleSprite;
                if(bombsAround == 0){text.text = "";}
                else
                {
                    text.text = $"{bombsAround}";
                    text.color = getColor(bombsAround);
                }
                Wyg = look.number;
                BoardGen.invisibleFieldsCords.Remove(name);
                for (int i = 0; i < BoardGen.invisibleFieldsCords.Count; i++)
                {
            
                    BoardGen.invisibleFieldsCords[i] = BoardGen.invisibleFieldsCords[i].Trim();
            
                }
                BoardGen.visibleFieldsCords.Add(name);
                game.check();
                break;
            case type.bomb:
                render.sprite = bombSprite;
                break;
        }
    }
    private void Awake()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        setInvisible();
        game = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        board = GameObject.Find("Board").GetComponent<BoardGen>();
    }
    private void Start()
    {
        bombsAround = lookForBombs(new Vector2(transform.position.x, transform.position.y));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameHandler.isPaused == true){return;}
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(Wyg == look.flagged){return;}
            setVisible();
            if(Typ == type.bomb){StartCoroutine(game.gameOver()); return;}
            if(bombsAround == 0){
                lookAround(new Vector2(transform.position.x, transform.position.y));
            }
        }else if (eventData.button == PointerEventData.InputButton.Right)
        {
            switch (Wyg)
            {
                case look.invisible:
                    placeFlag();
                    break;
                case look.number:
                    break;
                case look.flagged:
                    removeFlag();
                    break;
            }
            
        }
        
    }
    void lookAround(Vector2 pos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0){continue;}
                int x = (int)pos.x + j;
                int y = (int)pos.y + i;
                // Debug.Log("X: " + x + "Y: " + y);
                
                string name = $"Cell{x}{y}";
                GameObject nCell = GameObject.Find(name);
                if(BoardGen.visibleFieldsCords.Contains($"Cell{x}{y}")){continue;}
                if(nCell == null){continue;}
                FieldScript nField = nCell.GetComponent<FieldScript>();
                if(nField.Wyg == look.number){continue;}
                if(nField.Typ == type.field)
                {
                    nField.setVisible();
                    if(nField.bombsAround != 0){continue;}
                    nField.lookAround(new Vector2(nField.transform.position.x,nField.transform.position.y));
                }
            }
        }
    }
    int lookForBombs(Vector2 pos)
    {
        int Bombs = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // if(i ==0 && j == 0){continue;}
                int x = (int)pos.x + j;
                int y = (int)pos.y + i;
                // Debug.Log("X: " + x + "Y: " + y);
                string name = $"Cell{x}{y}";
                GameObject nCell = GameObject.Find(name);
                if(nCell == null){continue;}
                FieldScript nField = nCell.GetComponent<FieldScript>();
                if(nField.Typ == type.bomb)
                {
                    Bombs+= 1;
                }
            }
        }
        return Bombs;
    }
    void placeFlag()
    {
        if(BoardGen.flags <= 0){return;}
        //zmiana wyglądu
        // render.color = Color.red;
        render.sprite = flagSprite;
        //zmiana tagu
        Wyg = look.flagged;
        //zmień listę
        BoardGen.invisibleFieldsCords.Remove(this.transform.name);
        BoardGen.flaggedFieldsCords.Add(this.transform.name);
        //odjęcie z liczby flag
        BoardGen.flags -= 1;
        board.updateFlags(BoardGen.flags);
    }
    void removeFlag()
    {
        //zmiana wyglądu
        render.color = Color.gray;
        //zmiana tagu
        Wyg = look.invisible;
        //zmiana list
        BoardGen.invisibleFieldsCords.Add(this.transform.name);
        BoardGen.flaggedFieldsCords.Remove(this.transform.name);
        //odjęcie z liczby flag
        BoardGen.flags += 1;
        board.updateFlags(BoardGen.flags);
    }
    private Color getColor(int bombs)
    {
        switch (bombs)
        {
            case 1:
                return Color.blue;
            case 2:
                return Color.green;
            case 3: 
                return Color.red;
            case 4:
                return Color.yellow;
            case 5:
                return Color.cyan;
            case 6:
                return Color.magenta;
            case 7:
                return Color.black;
            case 8:
                return Color.gray;
            default:
                return Color.blue;
        }
    }

}
