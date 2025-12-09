# ⚽ Soccer Stars Clone (Unity 2D)

![Görsel](g1.png)

## 📝 Proje Hakkında
Bu proje, popüler **Soccer Stars** oyununun mekaniklerini ve oynanış mantığını Unity oyun motoru ile yeniden oluşturduğum bir **fizik tabanlı 2D futbol** oyunudur.

Amaç; sıra tabanlı (turn-based) bir sistemde, oyuncu taşlarını fırlatarak topu rakip kaleye göndermektir. Proje, Unity'nin 2D fizik motoru ve vektör hesaplamaları üzerine pratik yapmak amacıyla geliştirilmiştir.

### ✨ Temel Özellikler
* **Çek-Bırak Mekaniği (Drag & Shoot):** Fare ile güç ve yön belirleyerek taşları fırlatma.
* **Fizik Tabanlı Çarpışmalar:** Unity Rigidbody2D kullanılarak gerçekçi top ve oyuncu sekmeleri.
* **Gol Tespiti:** Topun kale çizgisiyle etkileşime girmesi ve skor kontrolü.

## 🎮 Nasıl Oynanır?
Oyun tamamen **Fare (Mouse)** ile oynanır:
1.  Kendi takımındaki bir taşa tıkla ve basılı tut.
2.  Farenin imlecini ters yöne doğru çekerek (okçuluk gibi) fırlatma gücünü ve yönünü ayarla.
3.  Fareyi bıraktığında taş fırlatılır.

## 🛠️ Teknik Detaylar
Bu projede Unity'nin fizik bileşenleri yoğun olarak kullanılmıştır.

* **Motor:** Unity(2D)
* **Dil:** C#
* **Fizik Bileşenleri:** `Rigidbody2D`, `CircleCollider2D`, `Physics Material 2D` (Sürtünme ve Sekme ayarları için).
* **Önemli Algoritmalar:** * Vektör kuvveti uygulama (`AddForce`).
    * Hız sönümlemesi (Drag) ile taşların durması.

---
*Geliştirici: [Esin Tekin]*
