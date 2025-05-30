using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    
    
    [SerializeField] private int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;
    public int coins;
    public bool isLoadGame;
    public int CurrentHp
    {
        get => hp.currentValue;
        set => hp.SetValue(value);
    }
    public int MaxHp { get => hp.maxValue; }

    protected Animator animator;

    public bool isDead;

    public VFXController VFX;

    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;

    [Header("广播事件")]
    public ObjectEventSO characterDeadEvent;
    public ObjectEventSO addCoinsEvent;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        hp.maxValue = maxHp;
        if (!isLoadGame)
        {
            CurrentHp = MaxHp;
        }

        ResetDefense();
        buffRound.SetValue(0);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(int damage)
    {
        // 伤害与防御的结算
        var currentDamage = (damage - defense.currentValue) > 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue) > 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);

        if (CurrentHp > currentDamage)
        {
            CurrentHp -= currentDamage;
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHp = 0;
            // 死亡
            isDead = true;
            animator.SetBool("isDead", true);
            characterDeadEvent.RaiseEvent(this, this);
            
            addCoinsEvent.RaiseEvent(this,this);
        }
    }

    /// <summary>
    /// 更新防御值
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    /// <summary>
    /// 重置防御值
    /// </summary>
    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    /// <summary>
    /// 治疗生命值
    /// </summary>
    /// <param name="amount"></param>
    public void HealHealth(int amount)
    {
        CurrentHp += amount;
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);
        VFX.buff.SetActive(true);
    }

    /// <summary>
    /// 设置力量效果
    /// </summary>
    /// <param name="round"></param>
    /// <param name="isPositive"></param>
    public void SetupStrength(int round, bool isPositive)
    {
        // buff
        if (isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            VFX.buff.SetActive(true);
        }
        // debuff
        else
        {
            baseStrength = 1 - strengthEffect;
            VFX.debuff.SetActive(true);
        }

        var currentRound = buffRound.currentValue + round;

        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(currentRound);
        }
    }

    /// <summary>
    /// 回合转换事件函数，更新力量回合数
    /// </summary>
    public void UpdateStrengthRound()
    {
        if (buffRound.currentValue < 1)
            return;
        buffRound.SetValue(buffRound.currentValue - 1);
    }

}
