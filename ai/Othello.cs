using System.Collections.Generic;

public struct Othello
{
    private const ulong u = 1;
    private const byte line = 8;
    private const byte columns = 8;

    public readonly bool GameOver() => whiteCount + blackCount == 64;
    public readonly bool WhiteWon() => whiteCount > blackCount;

    public static bool operator ==(Othello a, Othello b)
    {
        return (
            a.WhitePlays == b.WhitePlays &&
            a.whiteInfo == b.whiteInfo &&
            a.whiteCount == b.whiteCount &&
            a.blackInfo == b.blackInfo &&
            a.blackCount == b.blackCount
        );
    }

    public static bool operator !=(Othello a, Othello b) => !(a == b);

    public byte lastX;
    public byte lastY;

    public readonly (int x, int y) GetLast() => (lastX, lastY);

    public static Othello New()
    {
        return new Othello
        {
            whiteInfo = (u << 27) + (u << 36),
            blackInfo = (u << 28) + (u << 35),
            whiteCount = 2,
            blackCount = 2,
            whitePlays = 1
        };
    }

    public static Othello New(
        byte whitePlays,
        ulong white, ulong black,
        byte wCount, byte bCount
    )
    {
        return new Othello
        {
            whitePlays = whitePlays,
            whiteInfo = white,
            blackInfo = black,
            whiteCount = wCount,
            blackCount = bCount
        };
    }

    public readonly bool WhitePlays
        => whitePlays == 1;

    public readonly byte WhitePoints
        => whiteCount;

    public readonly byte BlackPoints
        => blackCount;

    public void Pass()
        => whitePlays = (byte)(1 - whitePlays);

    public void Play(int i, int j)
    {
        int index = i + j * 8;

        ulong play = u << index;
        ulong temp;

        if (WhitePlays)
        {
            temp = whiteInfo;

            whiteInfo |= play;

            if (whiteInfo == temp)
                return;

            whiteCount++;
        }
        else
        {
            temp = blackInfo;

            blackInfo |= play;

            if (blackInfo == temp)
                return;

            blackCount++;
        }

        lastY = (byte)j;
        lastX = (byte)i;

        Intersect(i, j);

        Pass();
    }


    private void Intersect(int x, int y)
    {
        var enemy = WhitePlays ? 2 : 1;
 
        var playerBoard = WhitePlays ? whiteInfo : blackInfo;
        var enemyBoard = WhitePlays ? blackInfo : whiteInfo;

        var playerCount = WhitePlays ? whiteCount : blackCount;
        var enemyCount = WhitePlays ? blackCount : whiteCount;

        bool changed = false;

        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                int place = this[x + i, y + j];

                if (place != enemy)
                    continue;

                changed = false;

                for (int k = 1; k < 8; k++)
                {

                    var adjX = x + i * k;
                    var adjY = y + j * k;

                    var index = adjX + adjY * 8;

                    place = this[adjX, adjY];

                    if (place == 0)
                    {
                        if (!changed)
                        {
                            playerBoard = WhitePlays ? whiteInfo : blackInfo;
                            enemyBoard = WhitePlays ? blackInfo : whiteInfo;

                            playerCount = WhitePlays ? whiteCount : blackCount;
                            enemyCount = WhitePlays ? blackCount : whiteCount;
                        }

                        break;
                    }

                    if (place == enemy)
                    {
                        playerBoard |= u << index;
                        enemyBoard ^= u << index;

                        playerCount++;
                        enemyCount--;

                        continue;
                    }

                    whiteCount = WhitePlays ? playerCount : enemyCount;
                    blackCount = WhitePlays ? enemyCount : playerCount;

                    whiteInfo = WhitePlays ? playerBoard : enemyBoard;
                    blackInfo = WhitePlays ? enemyBoard : playerBoard;

                    changed = true;
                }
            }
        }
    }
    public IEnumerable<(int x, int y)> NextMoves()
    {
        var player = WhitePlays ? 1 : 2;
        // var enemy = WhitePlays ? 2 : 1;
        var board = WhitePlays ? blackInfo : whiteInfo;

        for (int inc = 0; inc < 64; inc++)
        {
            ulong spot = board & (u << inc);

            if (spot == 0)
                continue;

            var x = inc % 8;
            var y = inc / 8;

            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    int place = this[x + i, y + j];

                    if (i == j || place != player)
                        continue;

                    yield return (x + (i * -1), y + (j * -1));
                }
            }
        }
    }

    // private void PaintIntersections(int x, int y)
    // {
    //     ulong playerBoard = WhitePlays ? whiteInfo : blackInfo;
    //     ulong enemyBoard = WhitePlays ? blackInfo : whiteInfo;

    //     List<(int x, int y)> list = new();

    //     var index = x + y * 8;

    //     bool flag = false;

    //     for (int j = -8; j < 9; j += 8)
    //     {
    //         for (int i = -1; i < 2; i++)
    //         {
    //             if (j == i)
    //                 continue;

    //             var adj = index + j + i;

    //             if (adj < 0 || adj > 64)
    //                 continue;

    //             var place = enemyBoard & (u << adj);

    //             if (place == 0)
    //                 continue;

    //             var tempX = i;
    //             var tempY = j;

    //             while (true)
    //             {
    //                 var tempAdj = index + tempY + tempX;

    //                 if (tempAdj < 0 || tempAdj > 64)
    //                     break;

    //                 var enemyBlock = enemyBoard & (u << tempAdj);
    //                 var playerBlock = playerBoard & (u << tempAdj);

    //                 if (enemyBlock > 0)
    //                 {
    //                     var enemyX = tempAdj % 8;
    //                     var enemyY = tempAdj / 8;

    //                     list.Add((enemyX, enemyY));

    //                     tempX += i;
    //                     tempY += j;

    //                     continue;
    //                 }

    //                 tempX += i;
    //                 tempY += j;

    //                 if (playerBlock > 0)
    //                 {
    //                     flag = true;
    //                     break;
    //                 }
    //             }
    //         }

    //         if(flag)
    //             foreach (var p in list)                
    //                 SwitchColor(p.x, p.y, whitePlays);



    //     }
    // }

    // private void SwitchColor(int x, int y, byte color)
    // {

    //     // Alerta de bug: se trocar valor zerado que já esta zerado irá quebrar tudo
    //     // Sugestão: verificação se valor já está alterado

    //     var index = x + y * 8;

    //     var colorIn = color == 1 ? whiteInfo : blackInfo;
    //     var colorOut = color == 1 ? blackInfo : whiteInfo;

    //     colorOut -= u << index;
    //     colorIn += u << index;

    //     whiteInfo = color == 1 ? colorIn : colorOut;
    //     blackInfo = color == 1 ? colorOut : colorIn;
    // }


    private byte whitePlays;
    private ulong whiteInfo;
    private ulong blackInfo;

    private byte whiteCount;
    private byte blackCount;

    public readonly int this[int i, int j]
    {
        get
        {
            int index = i + j * 8;
            return ((whiteInfo & (u << index)) > 0) ? 1 : ((blackInfo & (u << index)) > 0) ? 2 : 0;
        }
    }

    public override string ToString()
      => $"{whitePlays} {whiteInfo} {whiteCount} {blackInfo} {blackCount}";


    // public readonly IEnumerable<(int x, int y)> PossibleMoves()
    // {
    //     int enemy = WhitePlays ? 2 : 1;
    //     int player = WhitePlays ? 1 : 2;

    //     for (int y = 0; y < 8; y++)
    //     {
    //         for (int x = 0; x < 8; x++)
    //         {
    //             var piece = this[x, y];

    //             if (piece != enemy)
    //                 continue;

    //             for (int j = -1; j < 2; j++)
    //             {
    //                 for (int i = -1; i < 2; i++)
    //                 {
    //                     if (i == 0 && j == 0)
    //                         continue;

    //                     var adjX = x + i;
    //                     var adjY = y + j;

    //                     var index = adjX + adjY * 8;

    //                     if (index >= 64 || index < 0)
    //                         break;

    //                     var space = this[adjX, adjY];

    //                     if (space != player)
    //                         continue;

    //                     while (true)
    //                     {
    //                         adjX += i * -1; // -1 0 1
    //                         adjY += j * -1; // -1 0 1

    //                         index = adjX + adjY * 8;

    //                         if (index >= 64 || index < 0)
    //                             break;

    //                         space = this[adjX, adjY];

    //                         if (space != 0)
    //                             continue;

    //                         yield return (adjX, adjY);
    //                         break;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }


    public Othello Clone()
    {
        return new Othello
        {
            whiteInfo = this.whiteInfo,
            blackInfo = this.blackInfo,
            whiteCount = this.whiteCount,
            blackCount = this.blackCount,
            whitePlays = whitePlays
        };
    }
}


