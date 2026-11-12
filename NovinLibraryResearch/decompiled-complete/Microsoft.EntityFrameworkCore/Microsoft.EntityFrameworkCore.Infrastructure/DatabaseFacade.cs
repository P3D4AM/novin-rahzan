using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class DatabaseFacade : IInfrastructure<IServiceProvider>, IDatabaseFacadeDependenciesAccessor
{
	private readonly DbContext _context;

	private IDatabaseFacadeDependencies _dependencies;

	private IDatabaseFacadeDependencies Dependencies => _dependencies ?? (_dependencies = _context.GetService<IDatabaseFacadeDependencies>());

	public virtual IDbContextTransaction CurrentTransaction => Dependencies.TransactionManager.CurrentTransaction;

	public virtual bool AutoTransactionsEnabled { get; set; } = true;


	public virtual string ProviderName => _context.GetService<IEnumerable<IDatabaseProvider>>()?.Select((IDatabaseProvider p) => p.Name).FirstOrDefault();

	IServiceProvider IInfrastructure<IServiceProvider>.Instance => ((IInfrastructure<IServiceProvider>)_context).Instance;

	IDatabaseFacadeDependencies IDatabaseFacadeDependenciesAccessor.Dependencies => Dependencies;

	DbContext IDatabaseFacadeDependenciesAccessor.Context => _context;

	public DatabaseFacade([JetBrains.Annotations.NotNull] DbContext context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		_context = context;
	}

	public virtual bool EnsureCreated()
	{
		return Dependencies.DatabaseCreator.EnsureCreated();
	}

	public virtual Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.DatabaseCreator.EnsureCreatedAsync(cancellationToken);
	}

	public virtual bool EnsureDeleted()
	{
		return Dependencies.DatabaseCreator.EnsureDeleted();
	}

	public virtual Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.DatabaseCreator.EnsureDeletedAsync(cancellationToken);
	}

	public virtual bool CanConnect()
	{
		return Dependencies.DatabaseCreator.CanConnect();
	}

	public virtual Task<bool> CanConnectAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.DatabaseCreator.CanConnectAsync(cancellationToken);
	}

	public virtual IDbContextTransaction BeginTransaction()
	{
		return Dependencies.TransactionManager.BeginTransaction();
	}

	public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Dependencies.TransactionManager.BeginTransactionAsync(cancellationToken);
	}

	public virtual void CommitTransaction()
	{
		Dependencies.TransactionManager.CommitTransaction();
	}

	public virtual void RollbackTransaction()
	{
		Dependencies.TransactionManager.RollbackTransaction();
	}

	public virtual IExecutionStrategy CreateExecutionStrategy()
	{
		return Dependencies.ExecutionStrategyFactory.Create();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
