using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journalnavigation : MonoBehaviour
{
    private bool journalOpen = false;
    private bool isOpen = false;

    [SerializeField] private GameObject[] pages;
    [SerializeField] private GameObject[] texts;
    GameObject book;
    private int page = 0;
    private int text = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Theres this one thing that got me trippen");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)){
            journalOpen = !journalOpen;
            OpenBook();
        }

        if(isOpen){
            if(Input.GetKeyDown(KeyCode.RightArrow)){          
                if(page < pages.Length - 1){
                    page++;
                    ChangePage();
                }
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                if(page > 0){
                    page--;
                    ChangePage();
                }
            }
        }
    }

    private void OpenBook(){
        if(journalOpen && !isOpen){
            isOpen = !isOpen;
            book = Instantiate(pages[page], new Vector2(0, 0), transform.rotation);
        }
        else if(!journalOpen && isOpen){
            isOpen = !isOpen;
            Destroy(book);
        }

    }

    private void ChangePage(){
        Destroy(book);
        book = Instantiate(pages[page], new Vector2(0, 0), transform.rotation);
    }

    private void ChangeText(){
        
    }

}
