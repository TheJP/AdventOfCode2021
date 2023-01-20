var input = File.ReadAllLines(Environment.GetCommandLineArgs()[^1]);

var playerStart = new[] {
    int.Parse(input[0].Split(' ')[^1]),
    int.Parse(input[1].Split(' ')[^1]),
};
var player = new[] { playerStart[0] - 1, playerStart[1] - 1 };

int nextDice = 1;
int rolls = 0;
var score = new int[2];

int turn = 1;
while (score[turn] < 1000)
{
    turn = 1 - turn;
    for (int i = 0; i < 3; ++i)
    {
        player[turn] += nextDice;
        nextDice = (nextDice + 1) % 100;
        ++rolls;
    }
    player[turn] %= 10;
    score[turn] += player[turn] + 1;
}

Console.WriteLine($"{score[1 - turn]} * {rolls}");
Console.WriteLine($"{score[1 - turn] * rolls}");
