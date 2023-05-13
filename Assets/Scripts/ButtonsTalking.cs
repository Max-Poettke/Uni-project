using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsTalking : MonoBehaviour
{

    public GameObject conversationUI;

    public void activateConversationUI()
    {
        conversationUI.SetActive(!conversationUI.activeSelf);
    }
}
