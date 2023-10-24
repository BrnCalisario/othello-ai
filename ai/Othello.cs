using System.Collections.Generic;
using System.Data;

public struct Othello
{
    private const ulong u = 1;

    private const byte line = 8;
    private const byte collums = 8;

    public static Othello New()
    {
        return new Othello
        {
            whiteInfo = (u << 27) + (u << 36),
            blackInfo = (u << 28) + (u << 35),
            whiteCount = 2,
            blackCount = 2,
            whitePlays = 0
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
            whiteInfo = white,
            blackInfo = black,
            whiteCount = wCount,
            blackCount = bCount,
            whitePlays = whitePlays
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

            blackInfo++;
        }

        Pass();
    }

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


    public IEnumerable<(int x, int y)> PossibleMoves()
    {
        int enemy = WhitePlays ? 2 : 1;
        int player = WhitePlays ? 1 : 2;

        for (int y = 0; y < line; y++)
        {
            for (int x = 0; x < collums; x++)
            {
                var piece = this[x, y];

                if (piece != enemy)
                    continue;

                for (int j = -1; j < 2; j++)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        var adjX = x + i;
                        var adjY = y + j;

                        if(adjX < 0 || adjX > collums)
                            continue;
                        
                        if(adjY < 0 || adjY > line)
                            continue;

                        var space=  this[adjX, adjY];

                        if (space != player)
                            continue;

                        while(true)
                        {
                            adjX += i * -1; // -1 0 1
                            adjY += j * -1; // -1 0 1

                            if(adjX < 0 || adjX > collums)
                                break;

                            if(adjY < 0 || adjY > line)
                                break;

                            space = this[adjX, adjY];

                            if(space != 0)
                                continue;

                            yield return (adjX, adjY);
                            break;
                        }

                    }
                }

            }
        }
    }
}


