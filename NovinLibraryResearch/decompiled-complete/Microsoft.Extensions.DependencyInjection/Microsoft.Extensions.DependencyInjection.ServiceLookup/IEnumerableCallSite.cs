using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
internal sealed class IEnumerableCallSite : ServiceCallSite
{
	internal Type ItemType { get; }

	internal ServiceCallSite[] ServiceCallSites { get; }

	public override Type ServiceType => typeof(IEnumerable<>).MakeGenericType(ItemType);

	public override Type ImplementationType => ItemType.MakeArrayType();

	public override CallSiteKind Kind { get; } = CallSiteKind.IEnumerable;


	public IEnumerableCallSite(ResultCache cache, Type itemType, ServiceCallSite[] serviceCallSites)
		: base(cache)
	{
		ItemType = itemType;
		ServiceCallSites = serviceCallSites;
	}
}
