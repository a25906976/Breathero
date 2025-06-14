using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DialogueUnit
    {
        // StateIndex沒有用到 (目前是直接看List Index)
        public string StateIndex;
        
        // 要顯示的句子
        [TextArea(2,5)] // 參數是 min and max line
        public string[] sentences;
    }
}
