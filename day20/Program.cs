const int Border = 52;
var input = File.ReadAllLines(Environment.GetCommandLineArgs()[^1]);

var filter = new bool[512];

int line = 0;
var filterString = "";
while (input[line] != "")
{
    filterString += input[line];
    ++line;
}

for (int i = 0; i < filterString.Length; ++i)
{
    filter[i] = filterString[i] == '#';
}

++line;
var originalWidth = input[line].Length;
var width = originalWidth + 2 * Border;
IList<IList<bool>> image = new List<IList<bool>>();

for (int i = 0; i < Border; ++i) image.Add(new bool[width]);
while (line < input.Length)
{
    var imageLine = (new bool[Border])
        .Concat(input[line].Select(c => c == '#'))
        .Concat(new bool[Border])
        .ToArray();
    image.Add(imageLine);
    ++line;
}
for (int i = 0; i < Border; ++i) image.Add(new bool[width]);

void PrintImage(IEnumerable<IEnumerable<bool>> image)
{
    foreach (var row in image)
    {
        foreach (var pixel in row)
        {
            Console.Write(pixel ? '#' : '.');
        }
        Console.WriteLine();
    }
}

IList<IList<bool>> FilterImage(IList<IList<bool>> image)
{
    var newImage = new bool[image.Count][];
    for (int y = 0; y < image.Count; ++y)
    {
        newImage[y] = new bool[image[y].Count];

        for (int x = 0; x < image[y].Count; ++x)
        {
            if (y == 0 || y + 1 == image.Count ||
                x == 0 || x + 1 == image[y].Count)
            {
                newImage[y][x] = (filter[0] && !image[y][x]);
                continue;
            }

            int index = 0;
            for (int dy = -1; dy <= 1; ++dy)
            {
                for (int dx = -1; dx <= 1; ++dx)
                {
                    index <<= 1;
                    index |= image[y + dy][x + dx] ? 1 : 0;
                }
            }
            newImage[y][x] = filter[index];
        }
    }
    return newImage;
}

int CountLights(IList<IList<bool>> image) =>
    image.ToArray()[2..^2].Sum(row => row.ToArray()[2..^2].Sum(pixel => pixel ? 1 : 0));

var verbose = false;

if (verbose) PrintImage(image);
if (verbose) Console.WriteLine();
image = FilterImage(image);
if (verbose) PrintImage(image);
if (verbose) Console.WriteLine();
image = FilterImage(image);
if (verbose) PrintImage(image);

var task1 = CountLights(image);
Console.WriteLine($"Task 1: {task1}");

for (int i = 2; i < 50; ++i)
{
    image = FilterImage(image);
}

var task2 = CountLights(image);
Console.WriteLine($"Task 2: {task2}");
