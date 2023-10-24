// using System.Linq;

// Othello o = Othello.New();

// foreach(var m in o.PossibleMoves().ToList())
// {
//     System.Console.WriteLine($"x:{m.x}, y:{m.y}");
// }

using System.Threading;

int deep = 4;

Othello initial = Othello.New();

TreeNode tree = new TreeNode
{
    State = initial,
    YourTurn = true
};

tree.Expand(deep);

if(tree.YourTurn)
{
    tree.AlphaBeta();

    tree.PlayBest();

    tree.Expand(deep);

    // var last = tree.State.GetLast();


}


while(true)
{

    Thread.Sleep(1000);

    // Arquivo

    // Criar novo othello baseado no arquivo

    


}