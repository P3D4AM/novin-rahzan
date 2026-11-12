using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal interface IAsyncEnumerator<out T>
{
	T Current { get; }

	Task<bool> MoveNextAsync();
}
