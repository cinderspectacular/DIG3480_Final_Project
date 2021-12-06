using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    Rigidbody2D rb2D;
    float horizontal;
    float vertical;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public int score;
    public Text scoreText;
    public Text cogText;
    public Text gameOver;

    public GameObject projectilePrefab;
    public GameObject hitPrefab;
    public GameObject healthPrefab;

    CameraController cam;

    bool isInvincible;
    float invincibleTimer;
    bool level2;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;
    public int cogCount;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;
    public AudioSource speaker;
    public AudioClip cogClip;
    public AudioClip bg;
    public AudioClip win;
    public AudioClip lose;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.text = null;
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        score = 0;
        cogCount = 4;
        scoreText.text = "Robots Fixed: " + score;
        cogText.text = "Cogs: " + cogCount;

        GameObject camOb = GameObject.FindWithTag("MainCamera");
        if (camOb != null)
        {
            cam = camOb.GetComponent<CameraController>();
        }

        speaker.clip = bg;
        speaker.loop = true;
        speaker.Play();

        level2 = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && (cogCount > 0))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if(level2)
                {
                    this.gameObject.AddComponent<RubyController2>();
                    SceneManager.LoadScene("Level2");
                }
                else
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rb2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {

            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject hitObject = Instantiate(hitPrefab, transform.position, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            speaker.loop = false;
            speaker.clip = lose;
            speaker.Play();
            cam.gameOver = true;
            scoreText.text = null;
            gameOver.text = "You Lost! Press R to Restart!";

            Destroy(this.gameObject);
        }
    }

    public void ChangeScore(int sAmount)
    {
        score += sAmount;
        scoreText.text = "Robots Fixed: " + score;
        if (score > 3)
        {
            level2 = true;
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        audioSource.PlayOneShot(cogClip);
        animator.SetTrigger("Launch");

        cogCount -= 1;
        SetCogText();

    }

    void SetCogText()
    {
        cogText.text = "Cogs: " + cogCount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            Destroy(other.gameObject);
            cogCount += 1;
            SetCogText();
        }
    }
}
