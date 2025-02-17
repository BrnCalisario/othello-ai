using System;
using System.Collections.Generic;
using System.Linq;

public class TreeNode
{
    public Othello State { get; set; }
    public float Score { get; set; } = 0;

    public List<TreeNode> Children { get; set; } = new();

    public bool Expanded { get; set; } = false;
    public bool YourTurn { get; set; } = true;
    public bool YourColor { get; set; } = false;

    
    public TreeNode Play(Othello game)
    {
        foreach(var child in Children)
            if(child.State == game)
                return child;
    
        return null;
    }

    public void Expand(int deep)
    {
        if (deep == 0)
            return;
        if(!this.Expanded)
        {
            var possibleMoves = State.NextMoves();

            // foreach(var p in possibleMoves)
            // {
            //     System.Console.Write($"x: {p.x}, y: {p.y}\n");
            // }

            foreach(var move in possibleMoves)
            {
                var clone = State.Clone();
                clone.Play(move.x, move.y);

                var node = new TreeNode
                {
                    State = clone,
                    YourTurn = !this.YourTurn
                };

                this.Children.Add(node);
                this.Expanded = true;
            }
        }
        
        foreach(var child in this.Children)
            child.Expand(deep - 1);
    }

    public TreeNode PlayBest()
    {
        return Children.MaxBy(n => 
            YourTurn ? n.Score : -n.Score);
    }

    public float AlphaBeta() => this.AlphaBetaPruning(float.NegativeInfinity, float.PositiveInfinity);
    
    float AlphaBetaPruning(float alpha, float beta)
    {
        if(this.Children.Count == 0)
        {
            this.Score = aval();
            return this.Score;
        }

        float value;

        if(YourTurn)
        {
            value = float.NegativeInfinity;
            foreach(var child in Children)
            {
                value = MathF.Max(value, child.AlphaBetaPruning(alpha, beta));

                if(value > beta)
                    break;

                beta = MathF.Max(beta, value);                
            }
        }
        else
        {
            value = float.PositiveInfinity;

            foreach(var child in Children)
            {
                value = MathF.Min(value, child.AlphaBetaPruning(alpha, beta));

                if(value < alpha)
                    break;
                
                alpha = MathF.Max(alpha, value);
            }
        }
        this.Score = value;
        return value;
    }


    private float aval()
    {
        if(State.GameOver())
            return YourColor == State.WhiteWon() ? float.PositiveInfinity : float.NegativeInfinity;

        return Random.Shared.NextSingle();
    }

}