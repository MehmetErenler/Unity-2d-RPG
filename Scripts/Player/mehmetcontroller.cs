using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mehmetcontroller : MonoBehaviour
{
    private float moveDirection;
    public float speed = 5f;
    public float jumpPower=5f;
    public float groundRadius;

    public GameObject checkGround;

    public Transform attackPoint;
    public float attackDistance;
    public LayerMask enemyLayers;

    public float attackRate=2f;
    float nextAttack = 0; 

    bool isFacingRight = true;
    bool isGrounded;

    Rigidbody2D rb;
    Animator anim;

    public int damage=20;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
        Jump();
        CheckSurface();
        CheckAnimation();
        

        if (Time.time > nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Attack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }
    }
    private void FixedUpdate()
    {
        Movement();
       
    }

    void Movement()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        anim.SetFloat("runSpeed", Mathf.Abs(moveDirection * speed));
        
    }

    void CheckSurface()
    {
        isGrounded = Physics2D.OverlapCircle(checkGround.transform.position, groundRadius, whatIsGround);
    }

    void CheckRotation()
    {
        if (isFacingRight && moveDirection <0)
        {
            Flip();
        }
        else if (!isFacingRight && moveDirection > 0)
        {
            Flip();
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *=-1;
        transform.localScale = theScale;
    }
    void CheckAnimation()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVel", rb.velocity.y);


    }
    void Jump()
    {
       if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);

            }
        }
       

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.transform.position, groundRadius);
    }

    void AttackInput()
    {
       
    }

    public void Attack()
    {

        float numb = Random.Range(0, 2);

        if (numb == 0)
        {
            anim.SetTrigger("Attack1");
        }
        else if(numb==1)
        {
            anim.SetTrigger("Attack2");
        }
        Collider2D[] hitEnemies= Physics2D.OverlapCircleAll(attackPoint.position,attackDistance,enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemilerce>().TakeDamage(damage);
        }
    }
}
