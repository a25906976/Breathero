using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName="DialogueTree", menuName="ScriptableObjects/Dialogue Tree")]
    public class DialogueTreeObject : ScriptableObject
    {
        // npcName並沒有用
        public string npcName;
        
        public DialogueUnit[] dialogueUnits;

    }
}  