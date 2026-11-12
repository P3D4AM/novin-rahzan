using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class ConstantCallSite : ServiceCallSite
{
	private readonly Type _serviceType;

	internal object? DefaultValue => base.Value;

	public override Type ServiceType => _serviceType;

	public override Type ImplementationType => DefaultValue?.GetType() ?? _serviceType;

	public override CallSiteKind Kind { get; } = CallSiteKind.Constant;


	public ConstantCallSite(Type serviceType, object? defaultValue)
		: base(ResultCache.None)
	{
		_serviceType = serviceType ?? throw new ArgumentNullException("serviceType");
		if (defaultValue != null && !serviceType.IsInstanceOfType(defaultValue))
		{
			throw new ArgumentException(System.SR.Format(System.SR.ConstantCantBeConvertedToServiceType, defaultValue.GetType(), serviceType));
		}
		base.Value = defaultValue;
	}
}
