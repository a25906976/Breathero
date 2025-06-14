using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    // 綁在 ActionForMagicBook 上 (Canvas的parent)
    // 用來裝填 dialogueTree 裡的 對話
    public class DialogueHandler : MonoBehaviour
    {
        // DialogueTreeObject 是 ScriptableObject
        [SerializeField] private DialogueTreeObject dialogueTree;
        // DialogueUI 用來 處理對話
        [SerializeField] private DialogueUI dialogueUI;
        // Start is called before the first frame update
        [SerializeField] GameObject Little_DialogueUI;

        private void OnEnable() {
            EventManager.StartDialogueEvent += HandleDialogue;
        }
        private void OnDisable() {
            EventManager.StartDialogueEvent -= HandleDialogue;
        }


        // 連結特定對話框 並裝填欲顯示的對話
        private void HandleDialogue(int state)
        {
            dialogueUI.SetSentences(dialogueTree.dialogueUnits[state].sentences, state);

            // 根據目前的Dialogue(State)來開關UI圖片
            for (int i = 3; i < Little_DialogueUI.transform.childCount; i++)
            {
                //先全關掉
                Little_DialogueUI.transform.GetChild(i).gameObject.SetActive(false);
            }
            // 指定打開(Note: UI照片們在hirechay那邊要擺好順序呦~)
            Little_DialogueUI.transform.GetChild((state + 3)).gameObject.SetActive(true);
        }
    }
}    