using UnityEngine;
using System.Collections;

public class CController : MonoBehaviour
{
    public enum CharacterState { Idle, Walk, Fly, Talk } // Talk eklendi

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float flySpeed = 4f;

    [Header("Audio (Talk System)")]
    [SerializeField] private AudioSource audioSource; // Ses çalacak bileþen
    [SerializeField] private AudioClip[] talkSounds;   // Rastgele çalýnacak konuþma sesleri

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Coroutine autoRoutine;
    private Coroutine groundCheckRoutine;
    private bool manualControl;
    private bool facingRight = true;
    private bool isTalking = false; // Konuþma durumunu kilitlemek için kontrol

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float rayDistance = 0.4f;
    [SerializeField] private LayerMask excludeLayers;

    [SerializeField] private bool isGrounded;
    private float groundCheckInterval = 0.1f;

    private WaitForSeconds groundCheckWait;
    private WaitForFixedUpdate waitForFixedUpdate;

    // Animator Hashes
    private int walkHash;
    private int flyHash;
    private int idleHash;
    private int talkHash; // Talk animasyonu için hash
    private int lastTriggerHash;

    protected void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (groundCheckPoint == null) groundCheckPoint = transform;

        // Eðer AudioSource atanmadýysa nesne üzerindekini otomatik al
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        groundCheckWait = new WaitForSeconds(groundCheckInterval);
        waitForFixedUpdate = new WaitForFixedUpdate();

        walkHash = Animator.StringToHash("walk");
        flyHash = Animator.StringToHash("fly");
        idleHash = Animator.StringToHash("idle");
        talkHash = Animator.StringToHash("talk"); // Kayýt yapýldý

        groundCheckRoutine = StartCoroutine(GroundCheckRoutine());
    }

    protected void Start()
    {
        StartAuto();
    }

    protected void Update()
    {
        HandleInput();
        UpdateAnimation();
    }

    protected void FixedUpdate()
    {
        Move();
    }

    protected void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftShift))
        {
            manualControl = !manualControl;

            if (manualControl)
            {
                if (autoRoutine != null) StopCoroutine(autoRoutine);
                if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
                isTalking = false;
                movement = Vector2.zero;
                rb.velocity = Vector2.zero;
            }
            else
            {
                StartAuto();
            }
        }
    }

    private IEnumerator GroundCheckRoutine()
    {
        int layerMask = ~excludeLayers;
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, rayDistance, layerMask);
            isGrounded = (hit.collider != null && hit.collider.CompareTag("wall"));
            yield return groundCheckWait;
        }
    }

    protected void Move()
    {
        // Eðer konuþuyorsa hareket etmesini tamamen engelliyoruz
        if (isTalking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!isGrounded && movement == Vector2.zero)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = movement;
        }

        HandleFlip();
    }

    protected void UpdateAnimation()
    {
        // Eðer konuþuyorsa animasyon sisteminin diðer kurallarý ezmesini engelle
        if (isTalking)
        {
            PlayAnimation(talkHash);
            return;
        }

        if (isGrounded)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                PlayAnimation(walkHash);
            }
            else
            {
                PlayAnimation(idleHash);
            }
        }
        else
        {
            PlayAnimation(flyHash);
        }
    }

    private void PlayAnimation(int triggerHash)
    {
        if (lastTriggerHash == triggerHash) return;

        animator.ResetTrigger(walkHash);
        animator.ResetTrigger(flyHash);
        animator.ResetTrigger(idleHash);
        animator.ResetTrigger(talkHash); // Talk temizliði eklendi

        animator.SetTrigger(triggerHash);
        lastTriggerHash = triggerHash;
    }

    protected void HandleFlip()
    {
        if (isTalking) return; // Konuþurken yön deðiþtirmesin diyorsan

        if (movement.x > 0.1f && !facingRight)
        {
            Flip(true);
        }
        else if (movement.x < -0.1f && facingRight)
        {
            Flip(false);
        }
    }

    protected void Flip(bool faceRight)
    {
        facingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    protected void StartAuto()
    {
        if (autoRoutine != null) StopCoroutine(autoRoutine);
        autoRoutine = StartCoroutine(AutoMove());
    }

    protected IEnumerator AutoMove()
    {
        while (!manualControl)
        {
            if (isGrounded)
            {
                float decision = Random.value;

                // YERDEYKEN YENÝ KARAR ÞEMASI
                if (decision < 0.15f && talkSounds != null && talkSounds.Length > 0)
                {
                    // %15 Ýhtimalle Konuþma Komutu (Sadece Yerdeyken tetiklenebilir)
                    yield return StartCoroutine(TalkRoutine());
                }
                else if (decision < 0.85f) // Geri kalan yüzdede yürümeye öncelik
                {
                    // Yerde Yatay Hareket (Yürüme)
                    float dir = Random.value > 0.5f ? 1f : -1f;
                    movement = new Vector2(dir * moveSpeed, 0f);
                    yield return new WaitForSeconds(Random.Range(1.5f, 3f));
                }
                else
                {
                    // Havaya Kalkýþ (Uçma kararý)
                    movement = new Vector2(Random.Range(-1f, 1f) * moveSpeed, flySpeed);
                    yield return new WaitForSeconds(0.8f);
                }
            }
            else
            {
                // HAVADAYKEN KARAR VERME (Deðiþmedi - Havada asla konuþamaz)
                if (Random.value < 0.9f)
                {
                    float dirX = Random.value > 0.5f ? 1f : -1f;
                    float dirY = Random.Range(-0.3f, 0.8f);
                    movement = new Vector2(dirX * moveSpeed, dirY * flySpeed);
                    yield return new WaitForSeconds(Random.Range(1f, 2.5f));
                }
                else
                {
                    movement = Vector2.zero;
                    while (!isGrounded)
                    {
                        yield return waitForFixedUpdate;
                    }
                }
            }

            if (isGrounded && !isTalking && Random.value > 0.5f)
            {
                movement = Vector2.zero;
                yield return new WaitForSeconds(Random.Range(1f, 2f));
            }
        }
    }

    private IEnumerator TalkRoutine()
    {
        isTalking = true;
        movement = Vector2.zero; // Hareketi sýfýrla
        rb.velocity = Vector2.zero; // Fizik hýzýný tamamen durdur

        // Listeden rastgele bir ses dosyasý seçelim
        AudioClip chosenSound = talkSounds[Random.Range(0, talkSounds.Length)];

        if (chosenSound != null && audioSource != null)
        {
            audioSource.clip = chosenSound;
            audioSource.Play();

            // Ses dosyasýnýn tam süresi kadar (Örn: 2.3 saniye) bu rutini ve animasyonu kilitle
            yield return new WaitForSeconds(chosenSound.length);
        }
        else
        {
            // Eðer ses dosyasý bir sebepten okunamazsa bug'da kalmasýn diye kýsa bir süre verelim
            yield return new WaitForSeconds(1.5f);
        }

        isTalking = false; // Kilit açýldý, karakter normal hareketine dönebilir
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * rayDistance);
        }
    }

    private void OnDestroy()
    {
        if (groundCheckRoutine != null) StopCoroutine(groundCheckRoutine);
        if (autoRoutine != null) StopCoroutine(autoRoutine);
    }
}