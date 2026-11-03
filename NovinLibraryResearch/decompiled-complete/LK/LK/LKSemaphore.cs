using System.Threading;

namespace LK;

public class LKSemaphore
{
	private static readonly SemaphoreSlim _sema = new SemaphoreSlim(1, 1);

	public static SemaphoreSlim LKSemaphoreUnit => _sema;
}
