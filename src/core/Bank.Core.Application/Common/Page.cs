using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Common;

/// <summary>
/// Результат при постраничном запросе
/// </summary>
/// <typeparam name="T"></typeparam>
public class Page<T>
{
    [Required]
    public T[] Items { get; }

    [Required]
    public int Total { get; }

    public static Page<T> Empty = new(Array.Empty<T>(), 0);
    public static Page<T> From(T[] items, int total) => new(items, total);

    /// <param name="items">Полученные элементы</param>
    /// <param name="total">Общее кол-во элементов</param>
    private Page(T[] items, int total)
    {
        Items = items;
        Total = total;
    }
}
