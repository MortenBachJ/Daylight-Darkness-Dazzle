using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask platformLayerMask;
    Rigidbody2D rigidbody;
    CircleCollider2D circleCollider2d;
    public GameObject dash;
    public GameObject scoreController;
    public Sprite explosion;
    public Text BonusText;

    SpriteRenderer enemySprite;

    public float movementSpeed = 10;
    float extraSpeed;
    public float jumpForce = 15;
    public float dashForce = 20;
    public float dashDistance = 15f;
    bool isDashing;
    int enemiesKilled;
    int killBonus;


    public const string JUMP = "jump";
    public const string DASH = "dash";

    string buttonPressed;
    bool constantForward = false;
    float dashCooldown;
    float jumpCooldown;
    float jumpCount;
    float extraJump;
    bool isGrounded;
    bool alive;
    float deathDelay;
    GameObject currentEnemy;
    public UnityEngine.Experimental.Rendering.Universal.Light2D environmentLight;
    Color dashColor;
    Color defaultColor;
    static public float finalScore;
    scoreCounter ScoreCounter;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
        circleCollider2d = transform.GetComponent<CircleCollider2D>();
        ScoreCounter = scoreController.GetComponent<scoreCounter>();

        constantForward = true;
        BonusText.text = "";
        isGrounded = false;
        dashCooldown = Time.time;
        isDashing = false;
        extraJump = 1;
        enemiesKilled = 0;
        alive = true;
        extraSpeed = 0;
        
        // colors for dash effect on environment
        defaultColor = environmentLight.color;
        dashColor = new Color(environmentLight.color.r, 0.9607f, 0.8627f, environmentLight.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive == true){

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            else if (Input.GetButtonDown("Dash"))
            {
                StartCoroutine(Dash());
            }
            else{ 
                constantForward = true;
                extraSpeed += Time.deltaTime/5;
                Debug.Log(extraSpeed);
            }

            CheckPlayerPos();
        }
        
    }

    private void FixedUpdate() 
    {
        if (constantForward == true && !isDashing)
        {
            rigidbody.velocity = new Vector2(movementSpeed+extraSpeed, rigidbody.velocity.y); //constant movement
        } 
    }


    private void Jump()
    {
        if (isGrounded || jumpCount < extraJump)
        {
            constantForward = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    IEnumerator Dash(){
        //Debug.Log("Time is" + Time.time + " and dashCooldown is " + dashCooldown + " isDashing is " + isDashing);
        if(isDashing == false && Time.time > dashCooldown)
        {
            //Debug.Log("if-statement works");
            constantForward = false;
            isDashing = true;
            dash.SetActive(true);
            environmentLight.color = dashColor;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
            rigidbody.AddForce(new Vector2(dashDistance, 0), ForceMode2D.Impulse);
            float gravity = rigidbody.gravityScale;
            rigidbody.gravityScale = 0;
            yield return new WaitForSeconds(0.4f);
            environmentLight.color = defaultColor;
            dash.SetActive(false);
            dashCooldown = Time.time + 0.5f;
            isDashing = false;
            rigidbody.gravityScale = gravity;
        }
    }
    
    private void CheckPlayerPos()
    {
        //check if the player is grounded
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(circleCollider2d.bounds.center, circleCollider2d.bounds.size, 0f, Vector2.down, .1f, platformLayerMask);

        if(transform.position.y < -4.7){ //if the player falls out of the map
            //die
            StartCoroutine(onDeath());
        }
        else if(raycastHit2d == true)
        {
            isGrounded = true;
            jumpCount = 0;
            jumpCooldown = Time.time + 0.2f;
        } 
        else if (Time.time < jumpCooldown){ //allowing double jump
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9) //if the collision object is an enemy
        {
            Debug.Log("Hit Enemy?");
            if (isDashing==true)
            {
                currentEnemy = col.gameObject;
                
                //kill enemy
                StartCoroutine(killEnemy(currentEnemy));
            }
            else if (isDashing == false)
            {
                //die
                StartCoroutine(onDeath());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.layer == 8) //if the collision object is an obstacle
        {
            //die
            StartCoroutine(onDeath());
        }
    }

    IEnumerator onDeath()
    {
        alive = false;
        constantForward = false;
        deathDelay = Time.time+3f;
        //animation here
        yield return new WaitForSeconds(0.5f);
        finalScore = ScoreCounter.score;
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

    IEnumerator killEnemy(GameObject currentEnemy)
    {
        enemiesKilled += 1;
        killBonus = 1000*enemiesKilled;
        //bonus score pop-up
        BonusText.text = "+" + killBonus.ToString();
        //get 1000 points
        ScoreCounter.score += killBonus;
        //change sprite to explosion
        enemySprite = currentEnemy.GetComponent<SpriteRenderer>();
        enemySprite.sprite = explosion; 
        //wait before destroying object
        yield return new WaitForSeconds(0.2f);
        //destroy the enemy object
        Destroy(currentEnemy);
        yield return new WaitForSeconds(0.2f);
        //remove bonus score pop-up
        BonusText.text = "";
    }
}

