using System.Threading;
using System.IO;

string player = args.Length < 1 ? "m1" : args[0]; ;
string path = @$"..\front\";

int deep = 4;

Othello initial = Othello.New();

TreeNode tree = new()
{
    State = initial,
    YourTurn = player == "m1"
};

tree.Expand(deep);

if (tree.YourTurn)
{
    tree.AlphaBeta();
    tree = tree.PlayBest();
    tree.Expand(deep);

    File.WriteAllText($"{path + player}.txt", $"{tree.State}");

    System.Console.WriteLine(tree.State);
}


while (true)
{
    Thread.Sleep(1000);

    if (!File.Exists($"{path}[OUTPUT]{player}.txt"))
        continue;

    var text = File.ReadAllText($"{path}[OUTPUT]{player}.txt");

    File.Delete($"{path}[OUTPUT]{player}.txt");

    var data = text.Split(" ");
    var whitePlays = byte.Parse(data[0]);
    var whiteInfo = ulong.Parse(data[1]);
    var whiteCount = byte.Parse(data[2]);
    var blackInfo = ulong.Parse(data[3]);
    var blackCount = byte.Parse(data[4]);

    var othello = Othello.New(whitePlays, whiteInfo, blackInfo, whiteCount, blackCount);

    tree = tree.Play(othello);
    tree.Expand(deep);

    tree.AlphaBeta();
    tree = tree.PlayBest();
    tree.Expand(deep);

    File.WriteAllText($"{path + player}.txt", $"{tree.State}");

    System.Console.WriteLine(tree.State);
}