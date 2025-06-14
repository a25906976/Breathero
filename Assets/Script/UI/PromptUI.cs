using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 綁在 Prompt Object 上
public class PromptUI : MonoBehaviour
{
    // 是TextMeshProUGUI 非TextMeshPro
    [SerializeField] private TextMeshProUGUI sentenceText;

    private Queue<string> _sentences;

    private void Awake() {
        _sentences = new Queue<string>();
    }

    // 裝填句子
    public void SetSentences(IEnumerable<string> sentences, int state)
    {
        _sentences.Clear();
        foreach(var sentence in sentences)
        {
            _sentences.Enqueue(sentence);
        }
        
        sentenceText.text = _sentences.Dequeue();
    }

}
