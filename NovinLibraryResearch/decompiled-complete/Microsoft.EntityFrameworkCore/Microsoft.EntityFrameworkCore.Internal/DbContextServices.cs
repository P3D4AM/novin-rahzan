using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbContextServices : IDbContextServices
{
	private IServiceProvider _scopedProvider;

	private IDbContextOptions _contextOptions;

	private ICurrentDbContext _currentContext;

	private IModel _modelFromSource;

	private bool _inOnModelCreating;

	public virtual ICurrentDbContext CurrentContext => _currentContext;

	public virtual IModel Model => CoreOptions?.Model ?? _modelFromSource ?? (_modelFromSource = CreateModel());

	private CoreOptionsExtension CoreOptions => _contextOptions?.FindExtension<CoreOptionsExtension>();

	public virtual IDbContextOptions ContextOptions => _contextOptions;

	public virtual IServiceProvider InternalServiceProvider => _scopedProvider;

	public virtual IDbContextServices Initialize(IServiceProvider scopedProvider, IDbContextOptions contextOptions, DbContext context)
	{
		_scopedProvider = scopedProvider;
		_contextOptions = contextOptions;
		_currentContext = new CurrentDbContext(context);
		List<IDatabaseProvider> list = ServiceProviderServiceExtensions.GetService<IEnumerable<IDatabaseProvider>>(_scopedProvider)?.ToList();
		int num = list?.Count ?? 0;
		if (num > 1)
		{
			throw new InvalidOperationException(CoreStrings.MultipleProvidersConfigured(BuildDatabaseNamesString(list)));
		}
		if (num == 0 || !list[0].IsConfigured(contextOptions))
		{
			throw new InvalidOperationException(CoreStrings.NoProviderConfigured);
		}
		return this;
	}

	private static string BuildDatabaseNamesString(IEnumerable<IDatabaseProvider> available)
	{
		return string.Join(", ", available.Select((IDatabaseProvider e) => "'" + e.Name + "'"));
	}

	private IModel CreateModel()
	{
		if (_inOnModelCreating)
		{
			throw new InvalidOperationException(CoreStrings.RecursiveOnModelCreating);
		}
		try
		{
			_inOnModelCreating = true;
			return ServiceProviderServiceExtensions.GetService<IModelSource>(_scopedProvider).GetModel(_currentContext.Context, ServiceProviderServiceExtensions.GetService<IConventionSetBuilder>(_scopedProvider));
		}
		finally
		{
			_inOnModelCreating = false;
		}
	}
}
