using UnityEngine;

public interface IAttackable
{
    void Hit(float damageTaken, Vector2 knockback);
    void Hit(float damageTaken);
}
