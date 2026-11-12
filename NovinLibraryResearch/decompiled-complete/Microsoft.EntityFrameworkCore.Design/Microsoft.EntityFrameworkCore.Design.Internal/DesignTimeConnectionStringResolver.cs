using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class DesignTimeConnectionStringResolver : NamedConnectionStringResolverBase
{
	private readonly Func<IServiceProvider> _applicationServiceProviderAccessor;

	protected override IServiceProvider ApplicationServiceProvider => _applicationServiceProviderAccessor?.Invoke();

	public DesignTimeConnectionStringResolver([JetBrains.Annotations.CanBeNull] Func<IServiceProvider> applicationServiceProviderAccessor)
	{
		_applicationServiceProviderAccessor = applicationServiceProviderAccessor;
	}
}
