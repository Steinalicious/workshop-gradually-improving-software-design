namespace Bookstore.Common;

public static class AsyncOptionExtensions
{
    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> map)
        where T : class
        where TResult : class
    {
        Task<TResult>? task = option.Map(map);
        return task is null ? Option<TResult>.None() : Option<TResult>.Some(await task);
    }

    public static async Task<Option<TResult>> MapToObjectAsync<T, TResult>(this ValueOption<T> option, Func<T, Task<TResult>> map)
        where T : struct
        where TResult : class
    {
        Task<TResult>? task = option.MapToObject(map);
        return task is null ? Option<TResult>.None() : Option<TResult>.Some(await task);
    }

    public static async Task<Option<T>> AuditAsync<T>(this Option<T> option, Func<T, Task> auditAsync) where T : class
    {
        T? value = option;
        if (value is not null) await auditAsync(value);
        return option;
    }

    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, TResult> map)
        where T : class
        where TResult : class =>
        (await option).Map(map);

    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, Task<TResult>> map)
        where T : class
        where TResult : class =>
        await (await option).MapAsync(map);

    public static async Task<Option<T>> AuditAsync<T>(this Task<Option<T>> option, Func<T, Task> auditAsync)
        where T : class =>
        await (await option).AuditAsync(auditAsync);

    public static async Task<Option<TResult>> Map<T, TResult>(this Task<Option<T>> option, Func<T, TResult> map)
        where T : class
        where TResult : class =>
        (await option).Map(map);

    public static async Task<T> Reduce<T>(this Task<Option<T>> option, T whenNone) where T : class =>
        (await option).Reduce(whenNone);

    public static async Task<T> Reduce<T>(this Task<Option<T>> option, Func<T> whenNone) where T : class =>
        (await option).Reduce(whenNone);

    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> option, Func<Task<T>> whenNone) where T : class =>
        ((T?)await option) ?? await whenNone();
}