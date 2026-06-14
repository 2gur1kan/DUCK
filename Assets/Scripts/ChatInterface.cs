using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LLMUnity;

public class ChatInterface : MonoBehaviour
{
    [Header("LLM Character Configuration")]
    [SerializeField] private LLMCharacter activeLLMCharacter;

    [Header("UI Elements")]
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private TextMeshProUGUI chatHistory;

    private bool isChatOpen = false;
    private bool isWaitingForResponse = false;

    // Gelen ham cevabý tek parça yönetmek için string builder veya düz string kullanacaðýz
    private string lastBotResponse = "";

    private const string BotPrefix = "<color=#FFCC00><b>Ördek:</b></color>";
    private const string PlayerPrefix = "<color=#00FF00><b>Siz:</b></color>";

    void Start()
    {
        if (playerInput != null)
        {
            playerInput.onEndEdit.AddListener(OnInputFieldSubmit);
        }

        if (activeLLMCharacter == null)
        {
            activeLLMCharacter = FindFirstObjectByType<LLMCharacter>();
        }

        if (activeLLMCharacter != null)
        {
            // Modelin gereksiz yere uzayýp döngüye girmemesi için çýktýyý sýnýrlýyoruz
            activeLLMCharacter.numPredict = 40;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftShift))
        {
            ToggleChat();
        }
    }

    void ToggleChat()
    {
        isChatOpen = !isChatOpen;
        chatPanel.SetActive(isChatOpen);
        Time.timeScale = isChatOpen ? 0f : 1f;
        if (isChatOpen) playerInput.ActivateInputField();
    }

    private void OnInputFieldSubmit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessageToAI();
        }
    }

    public void SendMessageToAI()
    {
        if (isWaitingForResponse || activeLLMCharacter == null) return;

        string text = playerInput.text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        playerInput.text = "";
        lastBotResponse = ""; // Yeni mesaj öncesi eski bot hafýzasýný temizle
        isWaitingForResponse = true;

        chatHistory.text += $"\n{PlayerPrefix} {text}";
        chatHistory.text += $"\n{BotPrefix} Düþünüyor...";

        activeLLMCharacter.Chat(text, HandleReply, ReplyCompleted);
        playerInput.ActivateInputField();
    }

    // Modelden her veri parçasý tetiklendiðinde çalýþan alan
    void HandleReply(string incomingText)
    {
        // LLMUnity yapýsýna göre: Eðer incomingText her seferinde tüm metni içeriyorsa direkt eþitliyoruz,
        // eðer sadece tek bir kelime/token parça geliyorsa birbiri ardýna ekleme (append) mantýðý kuruyoruz.
        // Tekrarlama krizini önlemek için en güvenli yol gelen metnin uzunluk kontrolünü yapmaktýr:

        if (incomingText.Length >= lastBotResponse.Length)
        {
            // LLMUnity tüm metni kümülatif gönderiyor demektir, üst üste bindirmeyi engellemek için direkt eþitliyoruz
            lastBotResponse = incomingText;
        }
        else
        {
            // Eðer sadece yeni token parçasý geliyorsa biriktiriyoruz
            lastBotResponse += incomingText;
        }

        // Sistem etiketlerini (tagleri) temizle
        string streamingClean = CleanSystemTags(lastBotResponse);

        if (!string.IsNullOrEmpty(streamingClean))
        {
            UpdateLastHistoryLine(streamingClean);
        }
    }

    // Konuþma tamamen sonlandýðýnda çalýþan alan
    void ReplyCompleted()
    {
        // Model sustu, gelen tüm birikmiþ metni nihai filtreye sokuyoruz
        string finalCleanReply = FilterFinalResponse(lastBotResponse);

        if (string.IsNullOrEmpty(finalCleanReply))
        {
            finalCleanReply = "Anlayamadým vak!";
        }

        UpdateLastHistoryLine(finalCleanReply);
        isWaitingForResponse = false;
    }

    /// <summary>
    /// Hazýr çöp kalýplarý ("Size nasýl yardýmcý olabilirim" vb.) metinden ayýran akýllý filtre
    /// </summary>
    private string FilterFinalResponse(string input)
    {
        string cleaned = CleanSystemTags(input);
        if (string.IsNullOrEmpty(cleaned)) return "";

        string[] sentences = Regex.Split(cleaned, @"(?<=[.?!])");
        List<string> validList = new List<string>();

        string[] blacklistedPatterns = { "yardýmcý olabilirim", "yardým edebilirim", "neler yardým", "size nasýl" };

        foreach (string s in sentences)
        {
            string trimmed = s.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.Length < 2) continue;

            bool hasPattern = false;
            string lowerSentence = trimmed.ToLower();
            foreach (string pattern in blacklistedPatterns)
            {
                if (lowerSentence.Contains(pattern))
                {
                    hasPattern = true;
                    break;
                }
            }

            if (!hasPattern)
            {
                validList.Add(trimmed);
            }
        }

        if (validList.Count == 0) return cleaned;

        return string.Join(" ", validList);
    }

    private string CleanSystemTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return input.Replace("<|im_end|>", "").Replace("<|im_start|>", "")
                    .Replace("<|user|>", "").Replace("<|assistant|>", "")
                    .Replace("assistant", "").Replace("user", "")
                    .Replace("system", "").Trim();
    }

    private void UpdateLastHistoryLine(string newContent)
    {
        string currentText = chatHistory.text;
        int lastIndex = currentText.LastIndexOf(BotPrefix);

        if (lastIndex >= 0)
        {
            chatHistory.text = currentText.Substring(0, lastIndex) + $"{BotPrefix} {newContent}";
        }
    }

    private void OnDestroy()
    {
        if (playerInput != null) playerInput.onEndEdit.RemoveListener(OnInputFieldSubmit);
    }
}