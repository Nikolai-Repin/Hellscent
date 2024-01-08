using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journalnavigation : MonoBehaviour
{
    private bool journalOpen = false;
    private bool isOpen = false;

    [SerializeField] private GameObject[] pages;
    [SerializeField] public GameObject[] texts;
    public int lastPage;

    GameObject book;
    GameObject textLeft;
    GameObject textRight;

    private int page = 0;
    //half the value of the length of texts due to every increase will display 2 pages at once
    private int text = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J) && false){
            journalOpen = !journalOpen;
            OpenBook();
            Debug.Log("book is opened");
        }

        if(isOpen){
            if(Input.GetKeyDown(KeyCode.RightArrow)){ 
                text++;
                if (text > texts.Length / 2) {
                    text = texts.Length / 2;
                }
                ChangePage();
                ChangeText();
                Debug.Log("page: " + page + ", text: " + text);
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)){ 
                text--;
                if (text < 0) {
                    text = 0;
                } 
                ChangePage();
                ChangeText();
                Debug.Log("page: " + page + ", text: " + text);
            }
        }
        else if(!isOpen){
            Destroy(book);
            RemoveText();
            text = 0;
            page = 0;
        }
    }

    private void OpenBook(){
        if(journalOpen && !isOpen){
            isOpen = !isOpen;
            book = Instantiate(pages[page], new Vector2(0, 0), transform.rotation);
            book.transform.SetParent(transform, false);
            book.transform.Translate(new Vector3(0, 0, 1));
            GameObject[] players = GameObject.FindGameObjectsWithTag("player");
			foreach (GameObject p in players) {
				p.GetComponent<PlayerController>().SetControl(false);
			}
        }
        else if(!journalOpen && isOpen){
            isOpen = !isOpen;
            Destroy(book);
            GameObject[] players = GameObject.FindGameObjectsWithTag("player");
			foreach (GameObject p in players) {
				p.GetComponent<PlayerController>().SetControl(true);
			}
        }

    }

    private void ChangePage(){
        Destroy(book);
        RemoveText();
        if (text == 0) {
            page = 0;
        } else if (text == texts.Length / 2) {
            page = 2;
        } else {
            page = 1;
        }
        book = Instantiate(pages[page], new Vector2(0, 0), transform.rotation);
        book.transform.SetParent(transform, false);
        book.transform.Translate(new Vector3(0, 0, 1));
    }

    private void ChangeText(){
        RemoveText();
        textLeft = Instantiate(texts[text * 2], new Vector2(0, 0), transform.rotation);
        textLeft.transform.SetParent(transform, false);
        textLeft.transform.Translate(new Vector3(0, 0, 1));
        textRight = Instantiate(texts[text * 2 + 1], new Vector2(0, 0), transform.rotation);
        textRight.transform.SetParent(transform, false);
        textRight.transform.Translate(new Vector3(0, 0, 1));
    }

    private void RemoveText(){
        Destroy(textLeft);
        Destroy(textRight);
    }

}
