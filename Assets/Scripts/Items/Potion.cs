public class Potion : Item {
    private int healAmount = 50;

    public Potion() {
        Name = "Potion";
    }

    public override void Use(PlayerStatsSO playerStats) {
        playerStats.Heal(healAmount);
    }
}