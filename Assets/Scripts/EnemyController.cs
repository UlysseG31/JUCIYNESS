using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private SimpleFlash flashEffect;
    [SerializeField] private KeyCode flashKey;
    public float Health;
    public float MaxSpeed;
    public float AccelerationRate;
    public ParticleSystem Boom;
    [SerializeField] private AudioSource hit;
    //public Camera_Shake cameraShake;

    private ScoreManager scoremanager;

    // Private Variables
    float Speed;
    float DriftFactor;
    GameObject Player;
    Vector2 PlayerDirection;
    Vector2 PreviousPlayerDirection;
    Rigidbody2D rb;
    BoxCollider2D col;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        Player = GameObject.FindWithTag("Player");
        DriftFactor = 1;
        
        scoremanager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        //Should I rotate towards Player ?
        PlayerDirection = Player.transform.position - transform.position;
        if(Mathf.Sign(PlayerDirection.x) != Mathf.Sign(PreviousPlayerDirection.x))
        {
            RotateTowardsPlayer();
        }
        PreviousPlayerDirection = PlayerDirection;

        //Go towards Player
        rb.velocity = new Vector2(transform.forward.z * DriftFactor * Speed * Time.fixedDeltaTime, rb.velocity.y);

        //Die
        if(Health <= 0)
        {
            scoremanager.AddScore();
            StartCoroutine(CreateBoom());
            CameraShaker.Instance.ShakeOnce(1.2f, 1.2f, .1f, 1f);
            //StartCoroutine(cameraShake.Shake(.15f, .4f));
            Destroy(gameObject);
        }

        if(Speed <= 0)
        {
            StartCoroutine(GetToSpeed(MaxSpeed));
        }
        //Debug.Log(Speed);
    }

    public void GetDamage(float dmg)
    {
        Health -= dmg;
        flashEffect.Flash();
        CameraShaker.Instance.ShakeOnce(0.6f, 0.6f, .1f, 1f);
        hit.Play();
    }

    void RotateTowardsPlayer()
    {
        if (PlayerDirection.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        DriftFactor = -1;
        StartCoroutine(GetToSpeed(0));
    }

    IEnumerator GetToSpeed( float s)
    {
        //Debug.Log(s);
        float baseSpeed = Speed;
        float SignMultiplier = Mathf.Sign(s - Speed);
        for(float f=baseSpeed; f*SignMultiplier<=s; f += AccelerationRate*SignMultiplier)
        {
            Speed = f;
            yield return null;
        }
        DriftFactor = 1;
    }

    IEnumerator CreateBoom()
    {
        ParticleSystem bommDestroy = Instantiate(Boom, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
        Destroy(bommDestroy);
    }
}

