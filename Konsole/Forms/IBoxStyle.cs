namespace Konsole.Forms
{
    public interface IBoxStyle
    {
        char TL { get; }
        char T { get; }
        char TR { get; }
        char L { get; }
        char R { get; }
        char BL { get; }
        char BR { get; }
        char B { get; }
        char LJ { get; } // left line join
        char RJ { get; } // right line join
    }
}