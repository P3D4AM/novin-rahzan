namespace System.Threading.Tasks;

internal static class TaskExtensions
{
	public static async Task NoThrow(this Task task)
	{
		await new NoThrowAwaiter(task);
	}
}
