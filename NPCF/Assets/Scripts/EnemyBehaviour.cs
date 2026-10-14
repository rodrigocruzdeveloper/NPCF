using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceWakeUp;
    [SerializeField] private float distanceAttack;
    [SerializeField] private float speed;

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
        attacking = false;
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
        attacking = false;

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
        attacking = true;

        // COLOCAR UMA JANELA DE OPORTUNIDADE PARA O JOGADOR ATACAR
        // BATE 3X E FICA CANSADO  
    }


    void AnimationManager()
    {
        animator.SetBool("pWakeUp", wakeUp);
        animator.SetBool("pAttack", attacking);
    }

}
