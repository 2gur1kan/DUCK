# Ördek 🦆 | Local AI-Powered Desktop Pet & NPC

**Ördek** (Duck) is an open-source, locally-run desktop pet and AI-powered NPC prototype developed using the Unity game engine. 

Moving away from generic, corporate AI assistant behaviors, Ördek is designed with a unique, sassy, and playful personality. It engages with the player in natural Turkish dialogue while strictly adhering to system prompts and custom runtime behavioral constraints. If you want to talk to him, simply press the Shift+I key combination.

---

⚠️ CRITICAL STEP: Git LFS (Large File Storage) Setup
Why Git LFS is Mandatory
GitHub enforces a strict 100 MB file size limit per file. Because this project includes massive binary assets—specifically the core local language model (*.gguf) and native Android libraries (*.so files)—they cannot be uploaded via standard Git workflows.

To solve this, the project uses Git LFS. It replaces heavy binaries with lightweight text pointers inside the repository. If you skip the LFS configuration, your cloned project will contain missing files, leading to unresolvable Unity compilation errors.

Quick Installation Steps
Before opening the project in Unity, open your terminal (or Git Bash) and run the following commands to install the extension and fetch the actual files:

Bash
1. Download and install Git LFS on your system (if not already done)
Windows: git-lfs.com | macOS: brew install git-lfs | Ubuntu: sudo apt-get install git-lfs

2. Register the LFS filters in your global Git configuration
git lfs install

3. Navigate into your cloned project directory
cd ordek

4. Download the actual heavy model and library binaries
git lfs pull
Once the download finishes, verify that your .gguf model file and .so plugins are fully materialized with their actual file sizes inside your Unity assets folder.
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

### 🦆 Try The Project
You can directly play, test, and download the compiled, ready-to-run release of Ördek via the official itch.io platform:  
👉 [Play Ördek on itch.io](https://systembug.itch.io/duck)
