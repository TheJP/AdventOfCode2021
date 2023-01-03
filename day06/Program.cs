var numbers = File.ReadLines(Environment.GetCommandLineArgs()[1])
    .First().Split(',').Select(int.Parse);

var buckets = numbers.GroupBy(x => x).ToDictionary(group => group.Key, group => (long)group.Count());

const int Part1 = 80;
const int Part2 = 256;

for (int i = 0; i < Part2; ++i)
{
    if (i == Part1) Console.WriteLine($"Task 1: {buckets.Values.Sum()}");

    var next = new Dictionary<int, long>();
    if (buckets.ContainsKey(0))
    {
        next.Add(8, buckets[0]);
    }
    foreach (var entry in buckets)
    {
        var key = entry.Key == 0 ? 6 : entry.Key - 1;
        if (next.ContainsKey(key)) next[key] += entry.Value;
        else next.Add(key, entry.Value);
    }

    buckets = next;
}

Console.WriteLine($"Task 2: {buckets.Values.Sum()}");
