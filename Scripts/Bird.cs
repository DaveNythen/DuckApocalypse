using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public float speed;
    private float aliveSpeed;
    public bool isAlive;

    [Header("Sprites")]
    [SerializeField] Sprite hitSprite;
    [SerializeField] Sprite deadSprite;

    [Header("Points")]
    [SerializeField] GameObject pointsCanvas;
    private Text pointsText;
    public int scorePoints = 100;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rgbd2D;
    private UIManager uiMan;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rgbd2D = GetComponent<Rigidbody2D>();
        uiMan = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        isAlive = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rgbd2D.velocity = new Vector2(speed, 0);
        anim.SetBool("isFlying", true);
        aliveSpeed = speed;
    }

    public void Reverse()
    {
        //For birds spawned form the right
        spriteRenderer.flipX = true;
        speed = -speed;
    }

    public void Hit()
    {
        SoundManager.PlaySound(SoundManager.soundList.hit);

        ShowPoints();
        Die();
    }

    private void ShowPoints()
    {
        GameObject points = null;
        if (scorePoints != 0)
        {
            uiMan.IncreaseScore(scorePoints);

            points = Instantiate(pointsCanvas, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            pointsText = points.transform.Find("Points").GetComponent<Text>();
            pointsText.text = "+ " + scorePoints.ToString();
        }

        Destroy(points, 0.25f);
    }

    private void Die()
    {
        speed = 0;
        anim.enabled = false;
        spriteRenderer.sprite = hitSprite;
        rgbd2D.gravityScale = 10;
        isAlive = false;

        SoundManager.PlaySound(SoundManager.soundList.fall);

        StartCoroutine(WaitToMoveToPool());
    }

    IEnumerator WaitToMoveToPool()
    {
        yield return new WaitForSeconds(1f);
        RestoreBird();
        BirdPool.ReturnToPoolPos(transform);
    }

    private void RestoreBird()
    {
        speed = Mathf.Abs(aliveSpeed);
        anim.enabled = true;
        rgbd2D.gravityScale = 0;
        spriteRenderer.flipX = false;
        isAlive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) //Only collides with the Floor layer
    {
        spriteRenderer.sprite = deadSprite;
    }
}
