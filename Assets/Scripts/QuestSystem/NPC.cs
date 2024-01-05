using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NPC : MonoBehaviour
{
    public static NPC Instance { get; set; }
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    public string npcName;
    private int index;

    public GameObject continueButton;
    public GameObject acceptButton;
    public float wordSpeed;
    public bool playerIsClose;
    public bool accepted = false;

    void Update()
    {
        // Press E to Interact with NPC
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose) {
            if (dialoguePanel.activeInHierarchy) {
                zeroText();
            }

            else {
                zeroText();
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        // Press F to continue to next page of dialogue
        if(Input.GetKeyDown(KeyCode.F) && playerIsClose && 
        (continueButton.activeInHierarchy || acceptButton.activeInHierarchy)) {
            if(dialoguePanel.activeInHierarchy) {
                NextLine();
            }
        }

        // Last page of dialogue has accept button
        if(index == dialogue.Length - 1) {
            acceptButton.SetActive(true);
            continueButton.SetActive(false);
            accepted = true;
        }

        if(dialogueText.text == dialogue[index] && index < dialogue.Length - 1) {
            continueButton.SetActive(true);
        }
    }

    public void AcceptQuest() {
        QuestGiver.Instance.AssignQuest();
        zeroText();
    }

    // Goes to next line of dialogue
    public void NextLine() {
        acceptButton.SetActive(false);
        continueButton.SetActive(false);

        if(index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }

        else {
            zeroText();
        }
    }

    public void AddNewDialogue(string[] lines, string npcName) {

        index = 0;
        dialogue = lines;
        this.npcName = npcName;

        StartCoroutine(Typing());
    }

    public void zeroText() {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() {
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    // Can only talk to NPC when player is close
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("player")) {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("player")) {
            playerIsClose = false;
            accepted = false;
            zeroText();
        }
    }
}
