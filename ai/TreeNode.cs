using System.Collections.Generic;

public class TreeNode
{
    public Othello State { get; set; }
    public float Score { get; set; } = 0;

    public List<TreeNode> Children { get; set; } = new();

    public bool Expanded { get; set; } = false;
    public bool IsM1 { get; set; } = true;


    
}