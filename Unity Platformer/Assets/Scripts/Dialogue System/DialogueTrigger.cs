﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue _dialogue;
	public void TriggerDialogue ()
    {
        // Start dialogue, parsing in dialogue to be displayed
        DialogueManager.Instance.StartDialogue(_dialogue);
	}
}
