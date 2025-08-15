using UnityEngine;

public class arrowscript : MonoBehaviour
{
    [Header("Uzatma Ayarlarý")] //okun uzatma ayarlarý
    public Transform arrowobject;
    public float maxmouselength = 5f;
    public float minlength = 1f;
    public float maxlength = 5f;

    [Header("Player Fýrlatma")] //player fýrlatma ayarlarý
    public float forceMultiplier = 15f;
    public float maxForce = 30f;

    private Vector3 startposition; //oku çekmeye baþladýðýmýz nokta
    private Vector3 originscale; //ok nesnesinin orijinal boyutu
    private bool isstretching = false; //ok çekilme durumu- false
    private Camera playerCamera;
    private Transform selectedPlayer; //seçili oyuncu

    void Start()
    {
        playerCamera = Camera.main;
        if (arrowobject != null)
        {
            originscale = arrowobject.localScale;
        }
        arrowobject.gameObject.SetActive(false);//oyun baþlangýcýnda ok görünmez olur
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))//mouse sol týklandýðýnda
        {
            TrySelectPlayer();//mouse pozisyonunda oyuncu ara
            if (selectedPlayer != null)//oyuncu bulunduysa 
            {
                StartStretching();//oku çekmeye baþla
            }
        }

        if (Input.GetMouseButton(0) && isstretching)//mouse basýlý tutuluyorsa ve ok çekiliyorsa
        {
            UpdateStretch();//oku güncelle
        }

        if (Input.GetMouseButtonUp(0) && isstretching)//mouse sol týklama býrakýldýðýnda ve ok çekiliyorsa
        {
            StopStretching();//ok çekmeyi býrak
        }
    }

    void TrySelectPlayer()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();//mouse'un dünya pozisyonunu al
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Player"))//tagý player olan bir nesneye çarptýysa
        {
            selectedPlayer = hit.collider.transform;//oyuncuyu seç
        }
        else//deðilse
        {
            selectedPlayer = null;//oyuncu seçilmedi
        }
    }

    void StartStretching()
    {
        isstretching = true; //ok çekilme durumu- true
        startposition = selectedPlayer.position; //oyuncu pozisyonunu, baþlangýç pozisyonu olarak al/kaydet
        arrowobject.position = startposition;// ok nesnesini oyuncu pozisyonuna koy
    }

    void UpdateStretch()
    {
        Vector3 currentMousePos = GetMouseWorldPosition();//mouse'un þu anki pozisyonu
        Vector3 direction = currentMousePos - startposition;//yön hesapla
        float distance = direction.magnitude;

        if (!arrowobject.gameObject.activeSelf)
        {
            arrowobject.gameObject.SetActive(true);
        }

        distance = Mathf.Clamp(distance, 0f, maxmouselength);

        float stretchRatio = distance / maxmouselength;
        float newLength = originscale.x + (originscale.x * stretchRatio);

        Vector3 newScale = originscale; //okun boyutunu güncelle
        newScale.x = newLength;
        arrowobject.localScale = newScale;

        if (direction != Vector3.zero)//okun yönünü ayarla
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowobject.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void StopStretching()
    {
        isstretching = false;//ok çekme durumunu pasifleþtir

        if (selectedPlayer != null)//eðer oyuncu seçiliyse
        {
            FirePlayer();//oyuncuyu fýrlat
        }

        ResetArrow();//oku sýfýrla
        arrowobject.gameObject.SetActive(false);//oku gizle
        selectedPlayer = null;//oyuncu seçimini kaldýr
    }

    void FirePlayer()//oyuncu fýrlatma metodu
    {
        Rigidbody2D playerRb = selectedPlayer.GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogWarning("Player'da Rigidbody2D yok!");
            return;
        }

        Vector3 currentMousePos = GetMouseWorldPosition();//þu anki mouse pozisyonunu al
        Vector3 direction = currentMousePos - startposition;//fýrlatma yönü

        if (direction.magnitude < 1f) return;//mesafe kýsa ise ok fýrlatýlmasýn

        // Ok uzunluðuna göre kuvvet hesapla
        float currentArrowLength = arrowobject.localScale.x;
        float forceRatio = (currentArrowLength - originscale.x) / originscale.x;
        float force = Mathf.Clamp(forceRatio * forceMultiplier, 0f, maxForce);

        // Player'ý fýrlat
        Vector2 forceVector = direction.normalized * force;
        playerRb.AddForce(forceVector, ForceMode2D.Impulse);

        Debug.Log($"Player fýrlatýldý! Kuvvet: {force}");
    }

    void ResetArrow()//oku baþlangýç haline döndür
    {
        arrowobject.localScale = originscale;//boyutu sýfýrla
        arrowobject.rotation = Quaternion.identity;//rotasyonu sýfýrla
    }

    Vector3 GetMouseWorldPosition()//mou'un dünya pozisyonunu al
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = playerCamera.WorldToScreenPoint(selectedPlayer != null ? selectedPlayer.position : arrowobject.position).z;
        return playerCamera.ScreenToWorldPoint(mouseScreenPos);
    }
  
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("ball"))//çarpýlan nesne ball tagýna sahipse
            {
                Debug.Log("Player topa çarptý!");
                // Çarpýþma kuvvetini artýr (opsiyonel)
                Rigidbody2D topRb = collision.gameObject.GetComponent<Rigidbody2D>();//topa kuvvet uygula
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                topRb.AddForce(pushDirection * 5f, ForceMode2D.Impulse);
            }
        }
}
