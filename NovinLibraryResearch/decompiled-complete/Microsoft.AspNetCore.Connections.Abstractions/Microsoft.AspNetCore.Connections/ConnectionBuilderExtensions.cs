using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Connections;

public static class ConnectionBuilderExtensions
{
	public static IConnectionBuilder UseConnectionHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConnectionHandler>(this IConnectionBuilder connectionBuilder) where TConnectionHandler : ConnectionHandler
	{
		TConnectionHandler handler = ActivatorUtilities.GetServiceOrCreateInstance<TConnectionHandler>(connectionBuilder.ApplicationServices);
		return connectionBuilder.Run((ConnectionContext connection) => handler.OnConnectedAsync(connection));
	}

	public static IConnectionBuilder Use(this IConnectionBuilder connectionBuilder, Func<ConnectionContext, Func<Task>, Task> middleware)
	{
		Func<ConnectionContext, Func<Task>, Task> middleware2 = middleware;
		return connectionBuilder.Use((ConnectionDelegate next) => delegate(ConnectionContext context)
		{
			Func<Task> arg = () => next(context);
			return middleware2(context, arg);
		});
	}

	public static IConnectionBuilder Run(this IConnectionBuilder connectionBuilder, Func<ConnectionContext, Task> middleware)
	{
		Func<ConnectionContext, Task> middleware2 = middleware;
		return connectionBuilder.Use((ConnectionDelegate next) => (ConnectionContext context) => middleware2(context));
	}
}
