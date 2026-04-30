using UnityEngine;

/// <summary>
/// Противник: здоровье, получение урона, взаимодействие с менеджером и движением.
/// Enemy: health, damage handling, interaction with manager and movement.
/// </summary>
public class Enemy : MonoBehaviour
{
    // Ссылки, устанавливаемые при инициализации / References set during initialization
    private EnemyManager manager;
    private Transform pathRoot;

    /// <summary> Жив ли враг? / Is enemy alive? </summary>
    public bool IsAlive { get; private set; } = true;

    /// <summary> Позиция врага (для оптимизации) / Enemy position (optimized access) </summary>
    public Vector2 Position => transform.position;

    /// <summary>
    /// Инициализация после создания (вызывается EnemyManager).
    /// Initialization after spawn (called by EnemyManager).
    /// </summary>
    /// <param name="manager">Менеджер врагов / Enemy manager</param>
    /// <param name="pathRoot">Корневой объект пути / Path root</param>
    public void Init(EnemyManager manager, Transform pathRoot)
    {
        this.manager = manager;
        this.pathRoot = pathRoot;

        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.InitPath(pathRoot);
        }
        else
        {
            Debug.LogWarning($"Enemy {name}: компонент EnemyMovement отсутствует!");
        }
    }

    /// <summary>
    /// Получить урон от турели. При смерти уведомляет менеджер.
    /// Take damage from turret. Notifies manager on death.
    /// </summary>
    /// <param name="amount">Количество урона / Damage amount</param>
    /// <param name="source">Турель, нанесшая урон (может быть null) / Turret that dealt damage</param>
    public void TakeDamage(int amount, Turret source = null)
    {
        if (!IsAlive) return;

        // Здесь будет настоящая логика здоровья. Пока враг погибает от одного удара.
        // Later: implement actual health; currently one-shot kill.
        IsAlive = false;

        // Награда турели за убийство / Reward turret for kill
        source?.AddKillReward();

        // Сообщаем менеджеру о гибели врага (чтобы создать нового)
        manager?.OnEnemyDeath(this, pathRoot);

        // Удаляем объект со сцены
        Destroy(gameObject);
    }

    /// <summary>
    /// Враг достиг конца пути (не убит, а просто ушёл).
    /// Enemy reached end of path (not killed, just walked off).
    /// </summary>
    public void ReachEnd()
    {
        if (!IsAlive) return;
        IsAlive = false;

        manager?.OnEnemyDeath(this, pathRoot);

        Destroy(gameObject);
    }
}