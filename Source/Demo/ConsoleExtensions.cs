namespace Demo;

public static class ConsoleExtensions
{
    public static void PrintGrid(this IEnumerable<int> values, int columnWidth, int columnsCount) => values
        .Select(value => value.ToString("#,##0").PadLeft(columnWidth))
        .PrintGrid(columnWidth, columnsCount);

    public static void PrintGrid<T>(this IEnumerable<T> values, int columnWidth, int columnsCount)
    {
        List<string> fields = values
            .Select(value => value?.ToString() ?? string.Empty)
            .Select(field => field.PadRight(columnWidth))
            .ToList();

        int rowsCount = (fields.Count + columnsCount - 1) / columnsCount;

        while (fields.Count < rowsCount * columnsCount)
        {
            fields.Add(string.Empty);
        }

        var rows = Enumerable.Range(0, rowsCount)
            .Select(rowIndex => string.Join(" " , fields.Skip(rowIndex * columnsCount).Take(columnsCount)).TrimEnd())
            .ToArray();

        foreach (var row in rows)
        {
            Console.WriteLine(row);
        }
    }
}