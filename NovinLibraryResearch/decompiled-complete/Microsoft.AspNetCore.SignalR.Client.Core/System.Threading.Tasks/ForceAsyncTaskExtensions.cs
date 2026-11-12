namespace System.Threading.Tasks;

internal static class ForceAsyncTaskExtensions
{
	public static ForceAsyncAwaiter ForceAsync(this Task task)
	{
		return new ForceAsyncAwaiter(task);
	}

	public static ForceAsyncAwaiter<T> ForceAsync<T>(this Task<T> task)
	{
		return new ForceAsyncAwaiter<T>(task);
	}
}
