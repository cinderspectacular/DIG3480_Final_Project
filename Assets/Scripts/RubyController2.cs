using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController2 : MonoBehaviour
{
    Rigidbody2D rb2D;
    float horizontal;
    float vertical;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public int score;
    public Text catText;
    public Text gameOver;
    public GameObject timerMode;
    public Text timerTime;

    public GameObject hitPrefab;
    public GameObject healthPrefab;

    CameraController cam;
    public GameObject mainCamera;

    bool isInvincible;
    float invincibleTimer;
    public GameObject projectilePrefab2;
    public Vector3 rubyInitial;
    public Vector3 catInitial;
    public Vector3 rubySpot;
    public Vector3 catSpot;

    public int catCount;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;
    public AudioSource speaker;
    public AudioClip cogClip;
    public AudioClip bg;
    public AudioClip win;
    public AudioClip lose;
    private float timer;
    bool powerup;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.text = null;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        catCount = 0;
        catText.text = "Cats left: " + catCount;

        cam = mainCamera.GetComponent<CameraController>();
        cam.level2 = true;

        RubyController rc1 = GetComponent<RubyController>();
        Destroy(rc1);

        speaker.clip = bg;
        speaker.loop = true;
        speaker.Play();

        powerup = false;
        timer = 0.0f;
    }
    

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
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

        timer += Time.deltaTime;
        timerTime.text = timer.ToString();

        if(timer > 30.0f)
        {
            speaker.loop = false;
            speaker.clip = lose;
            speaker.Play();
            Destroy(timerMode);
            cam.gameOver = true;
            gameOver.text = "You lose! \nPress R to restart";
        }
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rb2D.MovePosition(position);
    }

    public void SetCatText()
    {
        AudioSource meow = GetComponent<AudioSource>();
        catText.text = "Cats left: " + catCount;
        meow.Play();
        if(catCount == 9)
        {
            speaker.loop = false;
            speaker.clip = win;
            speaker.Play();
            Destroy(timerMode);
            cam.gameOver = true;
            speed = 0.0f;
            gameOver.text = "You win! \nGame made by Jim Elso\nPress R to restart, if you wish";
        }
    }

}
