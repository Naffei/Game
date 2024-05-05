using UnityEngine;

// Send damage and knockback values to receiver
public interface IAttackable
{
    void Hit(float damageTaken, Vector2 knockback);
    void Hit(float damageTaken);
}
