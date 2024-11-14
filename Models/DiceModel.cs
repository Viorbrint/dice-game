namespace Dice.Models;

public class DiceModel
{
    public int[] Sides { get; }

    public DiceModel(int[] sides)
    {
        Sides = sides;
    }

    public override string ToString() => string.Join(",", Sides);
}
