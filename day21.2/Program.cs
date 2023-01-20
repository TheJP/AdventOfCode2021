var input = File.ReadAllLines(Environment.GetCommandLineArgs()[^1]);

var playerStart = new[] {
    int.Parse(input[0].Split(' ')[^1]),
    int.Parse(input[1].Split(' ')[^1]),
};
var player = new[] { playerStart[0] - 1, playerStart[1] - 1 };

var wins = new long[2, 2, 31, 31, 10, 10]; // dp: wins[player][turn][score player 0][score player 1][wheel player 0][wheel player 1]

var histogram = new int[10];
for (int a = 1; a <= 3; ++a)
    for (int b = 1; b <= 3; ++b)
        for (int c = 1; c <= 3; ++c)
            ++histogram[a + b + c];

(long p0, long p1) ComputeWins(int turn, int score0, int score1, int player0, int player1)
{
    // Console.WriteLine($"{turn}, {score0}, {score1}, {player0}, {player1}");
    var p0 = wins[0, turn, score0, score1, player0, player1];
    var p1 = wins[1, turn, score0, score1, player0, player1];
    if (p0 > 0 || p1 > 0) return (p0, p1);

    for (int i = 3; i <= 9; ++i)
    {
        var newPlayer0 = turn == 0 ? (player0 + i) % 10 : player0;
        var newPlayer1 = turn == 1 ? (player1 + i) % 10 : player1;
        var newScore0 = score0 + (turn == 0 ? newPlayer0 + 1 : 0);
        var newScore1 = score1 + (turn == 1 ? newPlayer1 + 1 : 0);
        if (newScore0 >= 21)
        {
            p0 += histogram[i];
        }
        else if (newScore1 >= 21)
        {
            p1 += histogram[i];
        }
        else
        {
            var w = ComputeWins(1 - turn, newScore0, newScore1, newPlayer0, newPlayer1);
            p0 += histogram[i] * w.p0;
            p1 += histogram[i] * w.p1;
        }
    }

    wins[0, turn, score0, score1, player0, player1] = p0;
    wins[1, turn, score0, score1, player0, player1] = p1;
    return (p0, p1);
}

var result = ComputeWins(0, 0, 0, player[0], player[1]);
Console.WriteLine(Math.Max(result.p0, result.p1));
