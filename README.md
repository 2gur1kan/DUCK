# Ördek 🦆 | Local AI-Powered Desktop Pet & NPC

**Ördek**, Unity oyun motoru kullanılarak geliştirilmiş, tamamen yerel (local) olarak çalışan ve özgün bir kişiliğe sahip yapay zeka tabanlı bir masaüstü evcil hayvan (desktop pet) ve NPC projesidir. 

Sıradan, kurumsal ve asistan yapay zeka kalıplarından uzaklaşarak; oyuncuyla samimi, esprili ve talimatlara sadık şekilde Türkçe diyalog kurabilen akıllı bir oyun karakteri prototipidir.

---

## 🚀 Öne Çıkan Özellikler

- **%100 Yerel (Offline) Çalışma:** Hiçbir API anahtarına (OpenAI vb.) veya internet bağlantısına ihtiyaç duymadan doğrudan kullanıcının donanımını kullanır.
- **Akıllı Cümle ve Bağlam Yönetimi:** Geliştirilen özel filtreleme motoru sayesinde, küçük dil modellerinde sıkça yaşanan döngüsel tekrarlar, kelime katlanmaları ve LLM sistem etiketlerinin sızması tamamen engellenmiştir.
- **Gelişmiş Persona Sadakati:** Rol yapma (roleplay) mekaniği sayesinde Ördek, kurumsal bir asistan gibi davranmayı reddeder ve tamamen bir oyun karakteri gibi yanıtlar verir.
- **Hafıza ve Talimat Testi:** Kullanıcı tarafından verilen anlık kuralları ve hafıza testlerini akış boyunca başarıyla korur.

---

## 🛠️ Teknolojik Altyapı & Açık Kaynak Bağımlılıkları

Bu proje, gücünü açık kaynak dünyasının başarılı kütüphanelerinden ve hafif dil modellerinden almaktadır:

### 🧠 Yapay Zeka Modeli: Qwen 2.5 (0.5B Instruct GGUF)
Ördek'in zihin mimarisi, Alibaba tarafından geliştirilen ve son teknoloji hafif dil modellerinden biri olan **Qwen2.5-0.5B-Instruct** üzerine kurulmuştur.
- **Format:** Cihaz kaynaklarını minimum düzeyde tüketen ve CPU/GPU üzerinde jet hızında çalışan **GGUF** formatı tercih edilmiştir.
- **Yetenek:** 0.5 milyar parametreye sahip olmasına rağmen Türkçe talimatları anlama ve bağlamı sürdürme konusunda yüksek performansa sahiptir.
- Model Sayfası: [HuggingFace - Qwen2.5-0.5B-Instruct-GGUF](https://huggingface.co/Qwen/Qwen2.5-0.5B-Instruct-GGUF)

### 🎮 Unity Entegrasyonu: LLMUnity
Yapay zeka modelinin Unity içinde hiçbir bulut servisine bağımlı olmadan gömülü olarak çalıştırılmasını sağlar. Projede canlı metin akışı (streaming) ve asenkron yanıt tamamlama süreçleri bu kütüphane üzerinden yönetilmektedir.
- Depo: [GitHub - undreamai/LLMUnity](https://github.com/undreamai/LLMUnity)

### 🖥️ Masaüstü Mimarisi: NikoDesktopPet
Ördek'in ekrandaki fiziksel varlığı, masaüstü etkileşimleri ve ekran sınırları içerisindeki estetik hareket mimarisi, açık kaynak kodlu **NikoDesktopPet** projesinin esnek altyapısı temel alınarak geliştirilmiştir.
- Depo: [GitHub - omotamiadev/NikoDesktopPet](https://github.com/omotamiadev/NikoDesktopPet)

---

## ⚙️ Kurulum ve Çalıştırma

1. Bu depoyu klonlayın:
   ```bash
   git clone [https://github.com/kullanici-adiniz/ordek.git](https://github.com/kullanici-adiniz/ordek.git)
