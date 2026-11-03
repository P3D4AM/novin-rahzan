using System.Threading;
using PacketParser.Assistance;

namespace AccSqlServerPersistence.AssistanceClasses;

internal class Locks : BaseLocks
{
	private class NestedLocks
	{
		internal static readonly Locks Instance;

		static NestedLocks()
		{
			Instance = new Locks();
		}
	}

	public static SemaphoreSlim KardexRefreshSemaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public static Locks LocksInstance => NestedLocks.Instance;
}
