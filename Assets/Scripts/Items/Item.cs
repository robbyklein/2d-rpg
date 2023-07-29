public abstract class Item {
    public string Name { get; protected set; }

    public abstract void Use(PlayerStatsSO playerStats);
}