using UnityEngine;

public class arrowscript : MonoBehaviour
{
    [Header("Uzatma Ayarlari")] //okun uzatma ayarlari
    public Transform arrowobject;
    public float maxmouselength = 5f;
    public float minlength = 1f;
    public float maxlength = 5f;

    [Header("Player Firlatma")] //player firlatma ayarlari
    public float forceMultiplier = 15f;
    public float maxForce = 30f;

    private Vector3 startposition; //oku cekmeye basladigimiz nokta
    private Vector3 originscale; //ok nesnesinin orijinal boyutu
    private bool isstretching = false; //ok cekilme durumu- false
    private Camera playerCamera;
    private Transform selectedPlayer; //secili oyuncu

    void Start()
    {
        playerCamera = Camera.main;
        if (arrowobject != null)
        {
            originscale = arrowobject.localScale;
        }
        arrowobject.gameObject.SetActive(false);//oyun baslangicinda ok gorunmez olur
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))//mouse sol tiklandiginda
        {
            TrySelectPlayer();//mouse pozisyonunda oyuncu ara
            if (selectedPlayer != null)//oyuncu bulunduysa 
            {
                StartStretching();//oku cekmeye basla
            }
        }

        if (Input.GetMouseButton(0) && isstretching)//mouse basili tutuluyorsa ve ok cekiliyorsa
        {
            UpdateStretch();//oku guncelle
        }

        if (Input.GetMouseButtonUp(0) && isstretching)//mouse sol tiklama birakildiginda ve ok cekiliyorsa
        {
            StopStretching();//ok cekmeyi birak
        }
    }

    void TrySelectPlayer()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();//mouse'un dunya pozisyonunu al
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Player"))//tagi player olan bir nesneye carptiysa
        {
            selectedPlayer = hit.collider.transform;//oyuncuyu sec
        }
        else//degilse
        {
            selectedPlayer = null;//oyuncu secilmedi
        }
    }

    void StartStretching()
    {
        isstretching = true; //ok cekilme durumu- true
        startposition = selectedPlayer.position; //oyuncu pozisyonunu, baslangic pozisyonu olarak al/kaydet
        arrowobject.position = startposition;// ok nesnesini oyuncu pozisyonuna koy
    }

    void UpdateStretch()
    {
        Vector3 currentMousePos = GetMouseWorldPosition();//mouse'un su anki pozisyonu
        Vector3 direction = currentMousePos - startposition;//yon hesapla
        float distance = direction.magnitude;

        if (!arrowobject.gameObject.activeSelf)
        {
            arrowobject.gameObject.SetActive(true);
        }

        distance = Mathf.Clamp(distance, 0f, maxmouselength);

        float stretchRatio = distance / maxmouselength;
        float newLength = originscale.x + (originscale.x * stretchRatio);

        Vector3 newScale = originscale; //okun boyutunu guncelle
        newScale.x = newLength;
        arrowobject.localScale = newScale;

        if (direction != Vector3.zero)//okun yonunu ayarla
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowobject.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void StopStretching()
    {
        isstretching = false;//ok cekme durumunu pasiflestir

        if (selectedPlayer != null)//eger oyuncu seciliyse
        {
            FirePlayer();//oyuncuyu firlat
        }

        ResetArrow();//oku sifirla
        arrowobject.gameObject.SetActive(false);//oku gizle
        selectedPlayer = null;//oyuncu secimini kaldir
    }

    void FirePlayer()//oyuncu firlatma metodu
    {
        Rigidbody2D playerRb = selectedPlayer.GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogWarning("Player'da Rigidbody2D yok!");
            return;
        }

        Vector3 currentMousePos = GetMouseWorldPosition();//su anki mouse pozisyonunu al
        Vector3 direction = currentMousePos - startposition;//firlatma yonu

        if (direction.magnitude < 1f) return;//mesafe kisa ise ok firlatilmasin

        // Ok uzunluguna gore kuvvet hesapla
        float currentArrowLength = arrowobject.localScale.x;
        float forceRatio = (currentArrowLength - originscale.x) / originscale.x;
        float force = Mathf.Clamp(forceRatio * forceMultiplier, 0f, maxForce);

        // Player'i firlat
        Vector2 forceVector = direction.normalized * force;
        playerRb.AddForce(forceVector, ForceMode2D.Impulse);

        Debug.Log($"Player firlatildi! Kuvvet: {force}");
    }

    void ResetArrow()//oku baslangic haline dondur
    {
        arrowobject.localScale = originscale;//boyutu sifirla
        arrowobject.rotation = Quaternion.identity;//rotasyonu sifirla
    }

    Vector3 GetMouseWorldPosition()//mouse'un dunya pozisyonunu al
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = playerCamera.WorldToScreenPoint(selectedPlayer != null ? selectedPlayer.position : arrowobject.position).z;
        return playerCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ball"))//carpilan nesne ball tagina sahipse
        {
            Debug.Log("Player topa carpti!");
            // Carpýsma kuvvetini artir (opsiyonel)
            Rigidbody2D topRb = collision.gameObject.GetComponent<Rigidbody2D>();//topa kuvvet uygula
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            topRb.AddForce(pushDirection * 5f, ForceMode2D.Impulse);
        }
    }
}
