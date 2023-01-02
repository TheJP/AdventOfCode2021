const int BoardLength = 5;
var lines = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var numbers = lines.First().Split(',').Select(int.Parse).ToArray();

var boards = lines.Skip(2).Chunk(6).Select(board =>
    board.Take(5).Select(line =>
        line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
    ).ToArray()
).ToArray();

var found = new bool[boards.Length, BoardLength, BoardLength];
int victories = 0;
var boardDone = new bool[boards.Length];

foreach (var current in numbers)
{
    for (int i = 0; i < boards.Length; ++i)
    {
        if (boardDone[i]) continue;

        for (int y = 0; y < BoardLength; ++y)
        {
            bool victory = true;
            int addedX = -1;
            for (int x = 0; x < BoardLength; ++x)
            {
                if (boards[i][y][x] == current) { addedX = x; found[i, y, x] = true; }
                else if (!found[i, y, x]) victory = false;
            }
            if (addedX > 0 && !victory)
            {
                victory = true;
                for (int y2 = 0; y2 < BoardLength; ++y2)
                {
                    if (!found[i, y2, addedX]) { victory = false; break; }
                }
            }

            int score = 0;
            if (victory)
            {
                boardDone[i] = true;
                ++victories;
                if (victories != 1 && victories != boards.Length) continue;

                for (int y2 = 0; y2 < BoardLength; ++y2)
                {
                    for (int x = 0; x < BoardLength; ++x)
                    {
                        if (!found[i, y2, x]) score += boards[i][y2][x];
                    }
                }
                // Console.WriteLine($"{i}");
                // Console.WriteLine($"{score} {current}");
                Console.WriteLine($"{score * current}");
            }
        }
    }
}