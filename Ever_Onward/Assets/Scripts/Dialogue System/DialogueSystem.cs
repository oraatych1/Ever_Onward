using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{


    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    public PlayerMovement pc;

    public static bool inConversation = false;



    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        //print(inConversation);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        inConversation = true;

        if (pc.lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pc.lockCursor = false;
        }
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
    }
    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);

        if (!pc.lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pc.lockCursor = true;
        }
        inConversation = false;
    }
}