var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var length = input.First().Length;
var a = input.ToArray();
var b = input.ToArray();

for (int i = 0; i < length; ++i) {
    var (zeros, ones) = a.Aggregate((0, 0),
        (pair, line) => line[i] == '1' ? (pair.Item1, pair.Item2 + 1) : (pair.Item1 + 1, pair.Item2));
    a = a.Where(line => ones >= zeros ? line[i] == '1' : line[i] == '0').ToArray();

    if (a.Length == 1) break;
}
int generator = Convert.ToInt32(a[0], 2);

for (int i = 0; i < length; ++i) {
    var (zeros, ones) = b.Aggregate((0, 0),
        (pair, line) => line[i] == '1' ? (pair.Item1, pair.Item2 + 1) : (pair.Item1 + 1, pair.Item2));
    b = b.Where(line => zeros <= ones ? line[i] == '0' : line[i] == '1').ToArray();

    if (b.Length == 1) break;
}
int scrubber = Convert.ToInt32(b[0], 2);

Console.WriteLine($"{generator * scrubber}");
