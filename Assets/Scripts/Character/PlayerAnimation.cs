using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player player;
    private Animator animator;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        animator.Play("sleep");
        animator.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
    }

    public void PlayerTurnEndAnimation()
    {
        if (player.defense.currentValue > 0)
        {
            animator.SetBool("isParry", true);
        }
        else
        {
            animator.SetBool("isSleep", true);
            animator.SetBool("isParry", false);
        }
    }

    public void OnPlayCardEvent(object obj)
    {
        Card card = (Card)obj;

        switch (card.cardData.cardType)
        {
            case CardType.Attack:
                animator.SetTrigger("attack");
                break;
            case CardType.Defense:
                break;
            case CardType.Abilities:
                animator.SetTrigger("skill");
                break;
        }
    }

}
