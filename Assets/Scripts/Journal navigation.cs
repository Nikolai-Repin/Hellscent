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
    GameObject textLeft;
    GameObject textRight;

    private int page = 0;
    //half the value of the length of texts due to every increase will display 2 pages at once
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
            Debug.Log("book is opened");
        }

        if(isOpen){
            

            if(Input.GetKeyDown(KeyCode.RightArrow)){ 
                Debug.Log("page:" + page + "\ntexts " + text + ", " + (text + 1));
                if(page == 1 && text < texts.Length / 2){
                    ChangeText();
                    text = text + 2;
                }


                else if(page < pages.Length - 1){
                    page++;
                    ChangePage();
                    ChangeText();
                    if(page == pages.Length - 1){
                        RemoveText();
                    }
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
        Destroy(textLeft);
        Destroy(textRight);
        textLeft = Instantiate(texts[text], new Vector2(0, 0), transform.rotation);
        textRight = Instantiate(texts[text + 1], new Vector2(0, 0), transform.rotation);
    }

    private void RemoveText(){
        Destroy(textLeft);
        Destroy(textRight);
    }

}








/*




if(Input.GetKeyDown(KeyCode.RightArrow)){    
                if(page == 1 && text < texts.Length - 1){
                    ChangeText();
                    text = text+2;
                }      
                else if(page < pages.Length - 1){
                    page++;
                    ChangePage();
                    RemoveText();
                }
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                if(page == 1 && text > 1){
                    text = text - 2;
                    ChangeText();
                }   
                else if(page > 0){
                    page--;
                    ChangePage();
                    RemoveText();
                }
            }*/