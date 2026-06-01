# Ördek 🦆 | Local AI-Powered Desktop Pet & NPC

**Ördek** (Duck) is an open-source, locally-run desktop pet and AI-powered NPC prototype developed using the Unity game engine. 

Moving away from generic, corporate AI assistant behaviors, Ördek is designed with a unique, sassy, and playful personality. It engages with the player in natural Turkish dialogue while strictly adhering to system prompts and custom runtime behavioral constraints. If you want to talk to him, simply press the Shift+I key combination.

---

## ⚠️ CRITICAL STEP: AI Model Download Instruction
> **IMPORTANT:** Due to GitHub's file size limitations (files exceeding 100 MB cannot be uploaded directly to the repository), the required language model is **NOT included** in the source files. You **MUST** download the model manually before running or building the project.

### How to set up the model:
1. Go to the official repository: [HuggingFace - Qwen2.5-0.5B-Instruct-GGUF](https://huggingface.co/Qwen/Qwen2.5-0.5B-Instruct-GGUF/tree/main)
2. Download the model file (Recommended: `qwen2.5-0_5b-instruct-q4_k_m.gguf` or any preferred quantization flavor).
3. Import the downloaded `.gguf` file into your Unity project.
4. Assign it to your `LLMCharacter` inspector component.

---

## 🚀 Key Features

- **100% Local & Offline:** Operates entirely on the user's hardware without relying on any external APIs (e.g., OpenAI) or internet connectivity.
- **Robust Text Stream Filtering:** Includes a custom C# buffering engine built specifically for small language models to eliminate recursive phrasing, text-doubling loops, and accidental system tag leakages during live text streaming.
- **Strict Persona Alignment:** Guided by advanced roleplay rules that force the model to behave purely as an interactive game character rather than a chatbot.
- **Contextual Memory Retention:** Successfully remembers and follows dynamic player-defined rules and custom constraints within the chat flow.

---

## 🛠️ Tech Stack & Open-Source Dependencies

This project leverages cutting-edge lightweight language models and open-source packages from the Unity development community:

### 🧠 Language Model: Qwen 2.5 (0.5B Instruct GGUF)
The AI cognitive engine is powered by Alibaba's state-of-the-art **Qwen2.5-0.5B-Instruct**.
- **Format:** The **GGUF** format is used to ensure high performance on consumer-grade CPUs and GPUs with minimal hardware resource allocation.
- **Capability:** Despite its compact size (0.5 Billion parameters), it demonstrates exceptional comprehension of Turkish syntax and instructions.

### 🎮 Unity Integration: LLMUnity
Manages the offline deployment of the LLM right within the Unity environment. It handles asynchronous inference pipelines and real-time word-by-word text streaming (token handling).
- Repository: [GitHub - undreamai/LLMUnity](https://github.com/undreamai/LLMUnity)

### 🖥️ Desktop Pet Architecture: NikoDesktopPet
The application's graphical overlay, screen boundary collisions, and aesthetic desk-companion movement patterns are inherited from the foundation of the open-source **NikoDesktopPet** project.
- Repository: [GitHub - omotamiadev/NikoDesktopPet](https://github.com/omotamiadev/NikoDesktopPet)
