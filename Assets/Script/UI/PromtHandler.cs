using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using UnityEngine.UI;

// 綁在 Prompt Object 上
public class PromtHandler : MonoBehaviour
{
    // DialogueTreeObject 是 ScriptableObject
    [SerializeField] private DialogueTreeObject promtTree;
    // PromptUI 用來 處理提示
    [SerializeField] private PromptUI promptUI;

    private void OnEnable() {
        EventManager.StartPromptEvent += HandlePrompt;
    }
    private void OnDisable() {
        EventManager.StartPromptEvent -= HandlePrompt;
    }

    // 連結特定提示框 並設定欲顯示的提示
    private void HandlePrompt(int state)
    {
        promptUI.SetSentences(promtTree.dialogueUnits[state].sentences, state);
    }
}
