using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class CallSiteChain
{
	private readonly struct ChainItemInfo
	{
		public int Order { get; }

		public Type ImplementationType { get; }

		public ChainItemInfo(int order, Type implementationType)
		{
			Order = order;
			ImplementationType = implementationType;
		}
	}

	private readonly Dictionary<Type, ChainItemInfo> _callSiteChain;

	public CallSiteChain()
	{
		_callSiteChain = new Dictionary<Type, ChainItemInfo>();
	}

	public void CheckCircularDependency(Type serviceType)
	{
		if (_callSiteChain.ContainsKey(serviceType))
		{
			throw new InvalidOperationException(CreateCircularDependencyExceptionMessage(serviceType));
		}
	}

	public void Remove(Type serviceType)
	{
		_callSiteChain.Remove(serviceType);
	}

	public void Add(Type serviceType, Type? implementationType = null)
	{
		_callSiteChain[serviceType] = new ChainItemInfo(_callSiteChain.Count, implementationType);
	}

	private string CreateCircularDependencyExceptionMessage(Type type)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(System.SR.Format(System.SR.CircularDependencyException, TypeNameHelper.GetTypeDisplayName(type)));
		stringBuilder.AppendLine();
		AppendResolutionPath(stringBuilder, type);
		return stringBuilder.ToString();
	}

	private void AppendResolutionPath(StringBuilder builder, Type currentlyResolving)
	{
		List<KeyValuePair<Type, ChainItemInfo>> list = new List<KeyValuePair<Type, ChainItemInfo>>(_callSiteChain);
		list.Sort((KeyValuePair<Type, ChainItemInfo> a, KeyValuePair<Type, ChainItemInfo> b) => a.Value.Order.CompareTo(b.Value.Order));
		foreach (KeyValuePair<Type, ChainItemInfo> item in list)
		{
			Type key = item.Key;
			Type implementationType = item.Value.ImplementationType;
			if (implementationType == null || key == implementationType)
			{
				builder.Append(TypeNameHelper.GetTypeDisplayName(key));
			}
			else
			{
				builder.AppendFormat("{0}({1})", TypeNameHelper.GetTypeDisplayName(key), TypeNameHelper.GetTypeDisplayName(implementationType));
			}
			builder.Append(" -> ");
		}
		builder.Append(TypeNameHelper.GetTypeDisplayName(currentlyResolving));
	}
}
