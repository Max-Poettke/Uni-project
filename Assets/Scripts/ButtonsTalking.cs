using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsTalking : MonoBehaviour
{
 private WorldSelection _worldSelection;
    public GameObject conversationUI;
    
    public void activateConversationUI()
    {
        conversationUI.SetActive(!conversationUI.activeSelf);
        if (_worldSelection == null)
        {
            _worldSelection = FindObjectOfType<WorldSelection>().GetComponent<WorldSelection>();
        }
        _worldSelection.talking = !_worldSelection.talking;
    }
}
