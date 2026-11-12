using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR;

public class DefaultHubLifetimeManager<THub> : HubLifetimeManager<THub> where THub : Hub
{
	private readonly HubConnectionStore _connections = new HubConnectionStore();

	private readonly HubGroupList _groups = new HubGroupList();

	private readonly ILogger _logger;

	public DefaultHubLifetimeManager(ILogger<DefaultHubLifetimeManager<THub>> logger)
	{
		_logger = logger;
	}

	public override Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (connectionId == null)
		{
			throw new ArgumentNullException("connectionId");
		}
		if (groupName == null)
		{
			throw new ArgumentNullException("groupName");
		}
		HubConnectionContext hubConnectionContext = _connections[connectionId];
		if (hubConnectionContext == null)
		{
			return Task.CompletedTask;
		}
		_groups.Add(hubConnectionContext, groupName);
		return Task.CompletedTask;
	}

	public override Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (connectionId == null)
		{
			throw new ArgumentNullException("connectionId");
		}
		if (groupName == null)
		{
			throw new ArgumentNullException("groupName");
		}
		if (_connections[connectionId] == null)
		{
			return Task.CompletedTask;
		}
		_groups.Remove(connectionId, groupName);
		return Task.CompletedTask;
	}

	public override Task SendAllAsync(string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendToAllConnections(methodName, args, null);
	}

	private Task SendToAllConnections(string methodName, object[] args, Func<HubConnectionContext, bool> include)
	{
		List<Task> list = null;
		SerializedHubMessage serializedHubMessage = null;
		foreach (HubConnectionContext connection in _connections)
		{
			if (include != null && !include(connection))
			{
				continue;
			}
			if (serializedHubMessage == null)
			{
				serializedHubMessage = CreateSerializedInvocationMessage(methodName, args);
			}
			ValueTask valueTask = connection.WriteAsync(serializedHubMessage);
			if (!valueTask.IsCompletedSuccessfully)
			{
				if (list == null)
				{
					list = new List<Task>();
				}
				list.Add(valueTask.AsTask());
			}
		}
		if (list == null)
		{
			return Task.CompletedTask;
		}
		return Task.WhenAll(list);
	}

	private void SendToGroupConnections(string methodName, object[] args, ConcurrentDictionary<string, HubConnectionContext> connections, Func<HubConnectionContext, bool> include, ref List<Task> tasks, ref SerializedHubMessage message)
	{
		foreach (KeyValuePair<string, HubConnectionContext> connection in connections)
		{
			if (include != null && !include(connection.Value))
			{
				continue;
			}
			if (message == null)
			{
				message = CreateSerializedInvocationMessage(methodName, args);
			}
			ValueTask valueTask = connection.Value.WriteAsync(message);
			if (!valueTask.IsCompletedSuccessfully)
			{
				if (tasks == null)
				{
					tasks = new List<Task>();
				}
				tasks.Add(valueTask.AsTask());
			}
		}
	}

	public override Task SendConnectionAsync(string connectionId, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (connectionId == null)
		{
			throw new ArgumentNullException("connectionId");
		}
		HubConnectionContext hubConnectionContext = _connections[connectionId];
		if (hubConnectionContext == null)
		{
			return Task.CompletedTask;
		}
		HubMessage message = CreateInvocationMessage(methodName, args);
		return hubConnectionContext.WriteAsync(message).AsTask();
	}

	public override Task SendGroupAsync(string groupName, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (groupName == null)
		{
			throw new ArgumentNullException("groupName");
		}
		ConcurrentDictionary<string, HubConnectionContext> concurrentDictionary = _groups[groupName];
		if (concurrentDictionary != null)
		{
			List<Task> tasks = null;
			SerializedHubMessage message = null;
			SendToGroupConnections(methodName, args, concurrentDictionary, null, ref tasks, ref message);
			if (tasks != null)
			{
				return Task.WhenAll(tasks);
			}
		}
		return Task.CompletedTask;
	}

	public override Task SendGroupsAsync(IReadOnlyList<string> groupNames, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<Task> tasks = null;
		SerializedHubMessage message = null;
		foreach (string groupName in groupNames)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				throw new InvalidOperationException("Cannot send to an empty group name.");
			}
			ConcurrentDictionary<string, HubConnectionContext> concurrentDictionary = _groups[groupName];
			if (concurrentDictionary != null)
			{
				SendToGroupConnections(methodName, args, concurrentDictionary, null, ref tasks, ref message);
			}
		}
		if (tasks != null)
		{
			return Task.WhenAll(tasks);
		}
		return Task.CompletedTask;
	}

	public override Task SendGroupExceptAsync(string groupName, string methodName, object[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (groupName == null)
		{
			throw new ArgumentNullException("groupName");
		}
		ConcurrentDictionary<string, HubConnectionContext> concurrentDictionary = _groups[groupName];
		if (concurrentDictionary != null)
		{
			List<Task> tasks = null;
			SerializedHubMessage message = null;
			SendToGroupConnections(methodName, args, concurrentDictionary, (HubConnectionContext connection) => !excludedConnectionIds.Contains(connection.ConnectionId), ref tasks, ref message);
			if (tasks != null)
			{
				return Task.WhenAll(tasks);
			}
		}
		return Task.CompletedTask;
	}

	private SerializedHubMessage CreateSerializedInvocationMessage(string methodName, object[] args)
	{
		return new SerializedHubMessage(CreateInvocationMessage(methodName, args));
	}

	private HubMessage CreateInvocationMessage(string methodName, object[] args)
	{
		return new InvocationMessage(methodName, args);
	}

	public override Task SendUserAsync(string userId, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendToAllConnections(methodName, args, (HubConnectionContext connection) => string.Equals(connection.UserIdentifier, userId, StringComparison.Ordinal));
	}

	public override Task OnConnectedAsync(HubConnectionContext connection)
	{
		_connections.Add(connection);
		return Task.CompletedTask;
	}

	public override Task OnDisconnectedAsync(HubConnectionContext connection)
	{
		_connections.Remove(connection);
		_groups.RemoveDisconnectedConnection(connection.ConnectionId);
		return Task.CompletedTask;
	}

	public override Task SendAllExceptAsync(string methodName, object[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendToAllConnections(methodName, args, (HubConnectionContext connection) => !excludedConnectionIds.Contains(connection.ConnectionId));
	}

	public override Task SendConnectionsAsync(IReadOnlyList<string> connectionIds, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendToAllConnections(methodName, args, (HubConnectionContext connection) => connectionIds.Contains(connection.ConnectionId));
	}

	public override Task SendUsersAsync(IReadOnlyList<string> userIds, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendToAllConnections(methodName, args, (HubConnectionContext connection) => userIds.Contains(connection.UserIdentifier));
	}
}
