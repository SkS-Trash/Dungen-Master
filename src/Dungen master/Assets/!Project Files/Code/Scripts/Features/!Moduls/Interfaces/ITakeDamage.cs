public interface ITakeDamage
{
    UnitType Owner { get; }

    void TakeHit(float amount);
}