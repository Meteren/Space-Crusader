public interface IDamageable<T>
{
    void OnDamage(T damageSource, IEffect<IDamageable<T>> effectResolver = null);
}