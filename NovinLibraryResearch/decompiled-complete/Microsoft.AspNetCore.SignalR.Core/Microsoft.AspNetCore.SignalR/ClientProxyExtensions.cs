using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR;

public static class ClientProxyExtensions
{
	public static Task SendAsync(this IClientProxy clientProxy, string method, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, Array.Empty<object>(), cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[1] { arg1 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[2] { arg1, arg2 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[3] { arg1, arg2, arg3 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
	}

	public static Task SendAsync(this IClientProxy clientProxy, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, CancellationToken cancellationToken = default(CancellationToken))
	{
		return clientProxy.SendCoreAsync(method, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
	}
}
