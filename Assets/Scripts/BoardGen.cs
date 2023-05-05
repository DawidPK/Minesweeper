using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;

public class BoardGen : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] int BombCount;
    public GameObject Field;
    List<string> fieldCords = new List<string>();
    public List<string> bombCords = new List<string>();
    public static List<string> visibleFieldsCords = new List<string>();
    public static List<string> invisibleFieldsCords = new List<string>();
    public static List<string> flaggedFieldsCords = new List<string>();
    public static int flags = 0;
    public event EventHandler<int> uiUpdated;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GenFields(difficulty dif)
    {
        switch (dif)
        {
            case difficulty.easy:
                size = new Vector2(10f, 10f);
                BombCount = 10;
                break;
            case difficulty.medium:
                size = new Vector2(16f, 16f);
                BombCount = 40;
                break;
            case difficulty.hard:
                size = new Vector2(30f, 16f);
                BombCount = 99;
                break;
        }
        for (int i = (int)-size.y/2; i < (int)size.y/2; i++) 
        {
            for(int j = (int)-size.x/2; j < (int)size.x/2; j++) 
            {
                var C = Instantiate(Field, Vector3.zero, Quaternion.identity);
                C.transform.SetParent(this.transform);
                C.transform.position = new Vector3(j,i,0);
                string name = $"Cell{j}{i}";
                fieldCords.Add($"{j}{i}");
                invisibleFieldsCords.Add(name);
                C.transform.name = name; 
            }
            
        }
        GenBombs(BombCount);
        flags = BombCount;
        Debug.Log("Dotarliśmy");
        updateFlags(flags);
    }
    public void GenBombs(int BombsCount) 
    {
        for(int i = 0; i < BombsCount; i++) 
        {
            //Debug.Log("Bomba");
            int x = UnityEngine.Random.Range(0, fieldCords.Count);
            string name = "Cell" + fieldCords.ElementAt(x);
            
            FieldScript f = transform.Find(name).gameObject.GetComponent<FieldScript>();
            f.changeToBomb();
            bombCords.Add(name);
            fieldCords.RemoveAt(x);
        }
    }
    public void Clear()
    {
        foreach (string item in bombCords)
        {
            string name = item;
            //Debug.Log("Bomba delete");
            GameObject g = GameObject.Find(name);
            Destroy(g);
        }
        foreach (string item in fieldCords)
        {
            string name = "Cell" + item;
            
            GameObject g = GameObject.Find(name);
            Destroy(g);
        }
        
        fieldCords.Clear();
        bombCords.Clear();
        visibleFieldsCords.Clear();
        invisibleFieldsCords.Clear();
        flaggedFieldsCords.Clear();

    }
    public void updateFlags(int e)
    {
        Debug.Log(e);

        uiUpdated?.Invoke(this, e);
    }
}
