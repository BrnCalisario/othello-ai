using System.Linq;

Othello o = Othello.New();


foreach(var m in o.PossibleMoves().ToList())
{
    System.Console.WriteLine($"x:{m.x}, y:{m.y}");
}