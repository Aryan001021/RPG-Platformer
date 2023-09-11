public class PlayerStats : CharacterStats
{
    Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }//this function is called when an opponent hit player it scans it and use this function to do damage to player
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
