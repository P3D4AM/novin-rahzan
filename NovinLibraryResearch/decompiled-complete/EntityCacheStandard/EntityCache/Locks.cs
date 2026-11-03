using System.Threading;
using PacketParser.Assistance;

namespace EntityCache;

public class Locks : BaseLocks
{
	private class NestedLocks
	{
		internal static readonly Locks Instance;

		static NestedLocks()
		{
			Instance = new Locks();
		}
	}

	public static SemaphoreSlim FullTextWordSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static SemaphoreSlim DbSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static SemaphoreSlim DbReadSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static SemaphoreSlim DbReadUserInfoSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static SemaphoreSlim TicketSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static SemaphoreSlim OrderSaveSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static Locks LocksInstance => NestedLocks.Instance;
}
