using NUnit.Compatibility;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceWakeUp;
    [SerializeField] private float distanceAttack;
    [SerializeField] private float speed;
    [SerializeField] private int life;

    private Animator animator;
    private SpriteRenderer spriteRender;

    private bool wakeUp;
    private bool attacking;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player");
        wakeUp = false;
    }
    
    
    void Update()
    {
        if (wakeUp == false)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distanceWakeUp)
            {
                wakeUp = true;
            }
        }
        else 
        {
            if(Vector2.Distance(transform.position, player.transform.position) < distanceAttack)
            {
                Attack();
            }
            else
            {
                Move();
            }            
        }


        AnimationManager();
    }


    void Move()
    {
        if (attacking == true) return;

        transform.position = Vector2.MoveTowards(transform.position, 
                                                 player.transform.position, 
                                                 speed * Time.deltaTime);

        if(player.transform.position.x > transform.position.x)
        {
            spriteRender.flipX = false;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            spriteRender.flipX = true;
        }

    }


    void Attack()
    {
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        attacking = true;
        wakeUp = true;
        yield return new WaitForSeconds(1.5f);
        attacking = false;
       
    }
   
    void AnimationManager()
    {
        animator.SetBool("pWakeUp", wakeUp);
        animator.SetBool("pAttack", attacking);
        animator.SetBool("pDeath", life <= 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {

            StartCoroutine(Damage());
    
        }
    }

    IEnumerator Damage()
    {
        life--;

        if (life > 0)
        {
            spriteRender.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
        
    }


}
