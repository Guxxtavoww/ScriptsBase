using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRb;
    public Transform groundCheck;
    private SpriteRenderer srPlayer;
    public GameObject playerDie;
    public Color hitColor;
    public Color noHitColor;
    private bool isGrounded;
    public bool facingRight = true;
    private float touchRun = 0.0f;
    private bool playerInvencivel;
    public float speed;
    public IASlug iaSlug;
    //Pulo.
    public int jumpForce;
    private int numeroJumps = 0;
    private int maximoJumps = 1;
    public bool jump = false;

    private GameController gameController;


    //Audio.
    public AudioSource fxGame;
    public AudioClip fxPulo;
    public AudioClip fxCenouraColetada;
    public AudioClip fxMorteInimigo;
    public AudioClip fxDie;
    public int vidas = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        srPlayer = GetComponent<SpriteRenderer>();

        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        iaSlug = FindObjectOfType(typeof(IASlug)) as IASlug;
    }

    // Update is called once per frame
    void Update()
    {
        touchRun = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        playerAnimator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        MovePLayer(touchRun);
        SetaMovimento();

        if (jump)
        {
            Jump();
        }
    }

    void MovePLayer(float movimentoH)
    {
        playerRb.velocity = new Vector2(movimentoH * speed, playerRb.velocity.y);
        if (movimentoH < 0 && facingRight || (movimentoH > 0 && !facingRight))
        {
            Flip();
        }
    }

    void SetaMovimento()
    {
        playerAnimator.SetBool("Walk", playerRb.velocity.x != 0 && isGrounded);

        playerAnimator.SetBool("Jump", !isGrounded);

    }

    void Jump()
    {
        

        if (isGrounded)
        {
            numeroJumps = 0;
        }

        if (isGrounded && numeroJumps < maximoJumps)
        {
            fxGame.PlayOneShot(fxPulo);
            playerRb.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
            numeroJumps++;
        }


        

        jump = false;
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

       

       
        switch (collision.gameObject.tag)
        {
            case "Coletaveis":

                gameController.Pontuacao(1);
                fxGame.PlayOneShot(fxCenouraColetada);
                
                Destroy(collision.gameObject); break;

            case "Inimigo":

                //instanciar a anima~]ao de explosão;
                GameObject tempExplosion = Instantiate(gameController.hitPrefab, transform.position, transform.localRotation);
                Destroy(tempExplosion, 0.5f);


                //adicionando força ao pulo.
                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, 500));
                //audio.
                fxGame.PlayOneShot(fxMorteInimigo);
                Destroy(collision.gameObject);
                iaSlug.enemie = null;
                
                break;

            case "Moerte":
                SceneManager.LoadScene("Inicio");
                break;
                

        }
        


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Inimigo":
                GameObject tempExplosion = Instantiate(gameController.hitPrefab, transform.position, transform.localRotation);
                Destroy(tempExplosion, 0.5f);
                
                Hurt(); 
                break;

            case "Plataforma":
                this.transform.parent = collision.transform;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {

            case "Plataforma":
                this.transform.parent = null;
                break;
        }
    }

    void Hurt()
    {

        if (!playerInvencivel)
        {
            playerInvencivel = true;

            StartCoroutine("Dano");
            vidas--;
            Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, 450));
           
           
            gameController.BarraVida(vidas);
            
            if(vidas < 1)
            {
                gameObject.SetActive(false);

                GameObject pDie = Instantiate(playerDie, transform.position, Quaternion.identity);
                Rigidbody2D rbDie = pDie.GetComponent<Rigidbody2D>();
                rbDie.AddForce(new Vector2(150f, 500f));
                fxGame.PlayOneShot(fxDie);

                Invoke("CarregaJogo", 3f);
            }
        }


        
    }

    void CarregaJogo()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    IEnumerator Dano()
    {
        srPlayer.color = noHitColor;
        yield return new WaitForSeconds(0.1f);

        for (float i = 0; i<1; i += 0.2f)
        {
            srPlayer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            srPlayer.enabled = true;
            yield return new WaitForSeconds(0.1f); 
        }

        srPlayer.color = Color.white;
        playerInvencivel = false;
    }
    
   
}
