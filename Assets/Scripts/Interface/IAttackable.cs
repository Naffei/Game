using UnityEngine;

public interface IAttackable
{
    public void Hit(float damageTaken, Vector2 knockback);
    public void Hit(float damageTaken);
}
