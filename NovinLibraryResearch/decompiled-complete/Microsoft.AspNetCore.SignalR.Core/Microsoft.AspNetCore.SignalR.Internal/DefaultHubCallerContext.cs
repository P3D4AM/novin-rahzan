using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.SignalR.Internal;

public class DefaultHubCallerContext : HubCallerContext
{
	private readonly HubConnectionContext _connection;

	public override string ConnectionId => _connection.ConnectionId;

	public override string UserIdentifier => _connection.UserIdentifier;

	public override ClaimsPrincipal User => _connection.User;

	public override IDictionary<object, object> Items => _connection.Items;

	public override IFeatureCollection Features => _connection.Features;

	public override CancellationToken ConnectionAborted => _connection.ConnectionAborted;

	public DefaultHubCallerContext(HubConnectionContext connection)
	{
		_connection = connection;
	}

	public override void Abort()
	{
		_connection.Abort();
	}
}
