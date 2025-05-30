using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;
    [Header("Element")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    // 防御相关
    private VisualElement defenseElement;
    private Label defenseAmountLabel;

    // 力量相关
    private VisualElement strengthElement;
    private Label strengthRound;
    [Header("Strength")]
    public Sprite buffSprite;
    public Sprite debuffSprite;

    private EnemyBase enemy;
    private VisualElement intentSprite;
    private Label intentAmount;


    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();

        enemy = GetComponent<EnemyBase>();
    }

    private void OnEnable()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect
            (element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }

    /// <summary>
    /// 初始化血条UI
    /// </summary>
    [ContextMenu("Test HealthBar")]
    public void InitHealthBar()
    {
        // 根节点
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        // 血条最大值刷新
        healthBar.highValue = currentCharacter.MaxHp;
        // 血条位置
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);

        // 防御值相关
        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
        defenseElement.style.display = DisplayStyle.None;

        // buff相关
        strengthElement = healthBar.Q<VisualElement>("Strength");
        strengthRound = strengthElement.Q<Label>("StrengthRound");
        strengthElement.style.display = DisplayStyle.None;

        // 敌人意图相关
        intentSprite = healthBar.Q<VisualElement>("Intent");
        intentAmount = intentSprite.Q<Label>("IntentAmount");
        intentSprite.style.display = DisplayStyle.None;

    }

    /// <summary>
    /// TODO: 测试血条变化，后续移除
    /// </summary>
    private void Update()
    {
        UpdateHealthBar();
    }

    /// <summary>
    /// 更新血条UI
    /// </summary>
    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            // 生命值信息更新
            healthBar.title = $"{currentCharacter.CurrentHp}/{currentCharacter.MaxHp}";
            healthBar.value = currentCharacter.CurrentHp;
            healthBar.highValue = currentCharacter.MaxHp;

            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            float percentage = (float)currentCharacter.CurrentHp / currentCharacter.MaxHp;

            if (percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealth");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
            }

            // 更新防御值UI
            defenseElement.style.display =
                currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();

            // 力量回合更新
            strengthElement.style.display = currentCharacter.buffRound.currentValue > 0 ?
                DisplayStyle.Flex : DisplayStyle.None;
            strengthElement.style.backgroundImage =
                currentCharacter.baseStrength > 1 ?
                    new StyleBackground(buffSprite) : new StyleBackground(debuffSprite);
            strengthRound.text = currentCharacter.buffRound.currentValue.ToString();
        }
    }

    public void SetIntent()
    {
        intentSprite.style.display = DisplayStyle.Flex;
        intentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);
        int value = enemy.currentAction.effect.value;

        // 判断是否为攻击
        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            // 结果四舍五入
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }

        intentAmount.text = value.ToString();
    }

    public void HideIntent()
    {
        intentSprite.style.display = DisplayStyle.None;
    }
}
