using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Управление параметром Speed для ходьбы и бега
        float moveInput = Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"));
        animator.SetFloat("Speed", moveInput);

        // Управление параметром IsJumping для прыжка
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed",5);
        }
        
        // Управление параметром IsAttackingMelee для атаки ближнего боя
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Левая кнопка мыши
        {
            animator.SetBool("IsAttackingMelee", true);
        }
        else
        {
            animator.SetBool("IsAttackingMelee", false);
        }

        // Управление параметром IsAttackingMagic для атаки магией
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Правая кнопка мыши
        {
            animator.SetBool("IsAttackingMagic", true);
        }
        else
        {
            animator.SetBool("IsAttackingMagic", false);
        }
    }
}
