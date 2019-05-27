using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Oui : MonoBehaviour
{
    public Animator animator;

    AnimatorController myAnim;

    public float dash;

    public int vie = 6;

    public float TempsDInvincibilite = 2;

    private bool shifttoggle;

    public float shift;

    public float speed;
    //Var de la vitesse de déplacement
    public float jumpForce;
    // Var de la hauteur du saut
    private float moveInput;
    // Si les fleche de gauche ou de droite sont préssées, changera de valeur
    public bool facingRight = true;
    //La variable qui renvoie true quand le perso se déplace et regarde vers la droite et faux pour la gauche
    private Rigidbody2D rb;
    

    //Behold les variables pour le saut
    private bool isGrounded;
    // Variable T/F qui confirme si le joueur est sur une plateforme ou dans les airs
    public Transform groundCheck;

    public float checkRadius;

    public LayerMask whatIsGround;

    private int extraJumps;

    public int extraJumpValue;

    void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();

      

    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        //Crée un cercle de rayon checkRadius autour de la position de groundCheck 
        //Si  cercle touche le sol (whatIsGround=sol) alors retourne True sinon False

        moveInput = Input.GetAxis("Horizontal");
        shifttoggle = Input.GetKeyDown(KeyCode.LeftShift);

        //Assigne la variable moveInput aux fleches directionnelles horizontales (si fleche de gauche: var=-1, si fleche de droite: var= +1)
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Donne a la valeur speed dans l'animator la valeur du vecteur créé au dessus
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        //Ca gere la vitesse de déplacement (x, y)
        if (facingRight == false && moveInput > 0)
        //si tu vas à droite mais que tu regardes à gauche
        {
            Flip();
        } else if (facingRight == true && moveInput < 0)
        //si tu vas à gauche mais que tu regardes à droite
        {
            Flip();
        }
        if (shifttoggle==true)
        {
            Dash();
        }
    }
    void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
        }
        //Si le perso est au sol, alors il a le droit de sauter x fois
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce; //Fait sauter vers le haut
            extraJumps--; // -1 saut
        }
        //Si le joueur appuie sur la fleche du haut et que le perso a encore des sauts, alors celui-ci saute
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        //Si le joueur appuie sur la fleche du haut, que le perso n'a plus de sauts mais qu'il est au sol, celui-ci saute quand même
    }
    void Flip()
    //Fait changer de sens le perso
    {
        facingRight = !facingRight;
        // inverse true et false
        Vector3 Scaler = transform.localScale;
        // Emprunte les scales du personnage. (x and y)
        Scaler.x *= -1;
        // Inverse seulement la coordonnée x de Scaler (si x=7 alors x=-7) 
        transform.localScale = Scaler;
        // L'applique sur le vrai scale du perso.
    }
    void Dash()
    {
        animator.SetBool("Anim_Dash", true);
        if (facingRight == true)
        {
            rb.velocity = Vector2.right * dash;

        }
        else
        {
            rb.velocity = Vector2.left * dash;
        }
        animator.SetBool("Anim_Dash", false);
    }

    public void TriggerHurt(float hurtTime)
    {
        StartCoroutine(HurtBlinker(hurtTime));
    }

    IEnumerator HurtBlinker(float hurtTime)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);

        animator.SetLayerWeight(1, 1);


        yield return new WaitForSeconds(hurtTime);

        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
        animator.SetLayerWeight(1, 0);
    }

        

    void Hurt()
    {
        vie--;
        if (vie <= 0)
            Application.LoadLevel(0);
        else
            myAnim.TriggerHurt();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Picots enemy = collision.collider.GetComponent<Picots>();
        if(enemy != null)
        {
            Hurt();
        }
    }
}