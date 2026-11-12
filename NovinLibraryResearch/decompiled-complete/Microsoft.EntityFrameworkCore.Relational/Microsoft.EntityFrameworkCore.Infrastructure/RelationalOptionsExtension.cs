using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public abstract class RelationalOptionsExtension : IDbContextOptionsExtension
{
	protected abstract class RelationalExtensionInfo : DbContextOptionsExtensionInfo
	{
		private string _logFragment;

		public new virtual RelationalOptionsExtension Extension => (RelationalOptionsExtension)base.Extension;

		public override bool IsDatabaseProvider => true;

		public override string LogFragment
		{
			get
			{
				if (_logFragment == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (Extension._commandTimeout.HasValue)
					{
						stringBuilder.Append("CommandTimeout=").Append(Extension._commandTimeout).Append(' ');
					}
					if (Extension._maxBatchSize.HasValue)
					{
						stringBuilder.Append("MaxBatchSize=").Append(Extension._maxBatchSize).Append(' ');
					}
					if (Extension._useRelationalNulls)
					{
						stringBuilder.Append("UseRelationalNulls ");
					}
					if (Extension._migrationsAssembly != null)
					{
						stringBuilder.Append("MigrationsAssembly=").Append(Extension._migrationsAssembly).Append(' ');
					}
					if (Extension._migrationsHistoryTableName != null || Extension._migrationsHistoryTableSchema != null)
					{
						stringBuilder.Append("MigrationsHistoryTable=");
						if (Extension._migrationsHistoryTableSchema != null)
						{
							stringBuilder.Append(Extension._migrationsHistoryTableSchema).Append('.');
						}
						stringBuilder.Append(Extension._migrationsHistoryTableName ?? "__EFMigrationsHistory").Append(' ');
					}
					_logFragment = stringBuilder.ToString();
				}
				return _logFragment;
			}
		}

		protected RelationalExtensionInfo(IDbContextOptionsExtension extension)
			: base(extension)
		{
		}

		public override long GetServiceProviderHashCode()
		{
			return 0L;
		}
	}

	private string _connectionString;

	private DbConnection _connection;

	private int? _commandTimeout;

	private int? _maxBatchSize;

	private int? _minBatchSize;

	private bool _useRelationalNulls;

	private string _migrationsAssembly;

	private string _migrationsHistoryTableName;

	private string _migrationsHistoryTableSchema;

	private Func<ExecutionStrategyDependencies, IExecutionStrategy> _executionStrategyFactory;

	public abstract DbContextOptionsExtensionInfo Info { get; }

	public virtual string ConnectionString => _connectionString;

	public virtual DbConnection Connection => _connection;

	public virtual int? CommandTimeout => _commandTimeout;

	public virtual int? MaxBatchSize => _maxBatchSize;

	public virtual int? MinBatchSize => _minBatchSize;

	public virtual bool UseRelationalNulls => _useRelationalNulls;

	public virtual string MigrationsAssembly => _migrationsAssembly;

	public virtual string MigrationsHistoryTableName => _migrationsHistoryTableName;

	public virtual string MigrationsHistoryTableSchema => _migrationsHistoryTableSchema;

	public virtual Func<ExecutionStrategyDependencies, IExecutionStrategy> ExecutionStrategyFactory => _executionStrategyFactory;

	protected RelationalOptionsExtension()
	{
	}

	protected RelationalOptionsExtension([JetBrains.Annotations.NotNull] RelationalOptionsExtension copyFrom)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(copyFrom, "copyFrom");
		_connectionString = copyFrom._connectionString;
		_connection = copyFrom._connection;
		_commandTimeout = copyFrom._commandTimeout;
		_maxBatchSize = copyFrom._maxBatchSize;
		_minBatchSize = copyFrom._minBatchSize;
		_useRelationalNulls = copyFrom._useRelationalNulls;
		_migrationsAssembly = copyFrom._migrationsAssembly;
		_migrationsHistoryTableName = copyFrom._migrationsHistoryTableName;
		_migrationsHistoryTableSchema = copyFrom._migrationsHistoryTableSchema;
		_executionStrategyFactory = copyFrom._executionStrategyFactory;
	}

	protected abstract RelationalOptionsExtension Clone();

	public virtual RelationalOptionsExtension WithConnectionString([JetBrains.Annotations.NotNull] string connectionString)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._connectionString = connectionString;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithConnection([JetBrains.Annotations.NotNull] DbConnection connection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._connection = connection;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithCommandTimeout(int? commandTimeout)
	{
		if (commandTimeout.HasValue && commandTimeout <= 0)
		{
			throw new InvalidOperationException(RelationalStrings.InvalidCommandTimeout);
		}
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._commandTimeout = commandTimeout;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithMaxBatchSize(int? maxBatchSize)
	{
		if (maxBatchSize.HasValue && maxBatchSize <= 0)
		{
			throw new InvalidOperationException(RelationalStrings.InvalidMaxBatchSize);
		}
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._maxBatchSize = maxBatchSize;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithMinBatchSize(int? minBatchSize)
	{
		if (minBatchSize.HasValue && minBatchSize <= 0)
		{
			throw new InvalidOperationException(RelationalStrings.InvalidMinBatchSize);
		}
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._minBatchSize = minBatchSize;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithUseRelationalNulls(bool useRelationalNulls)
	{
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._useRelationalNulls = useRelationalNulls;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithMigrationsAssembly([JetBrains.Annotations.CanBeNull] string migrationsAssembly)
	{
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._migrationsAssembly = migrationsAssembly;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithMigrationsHistoryTableName([JetBrains.Annotations.CanBeNull] string migrationsHistoryTableName)
	{
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._migrationsHistoryTableName = migrationsHistoryTableName;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithMigrationsHistoryTableSchema([JetBrains.Annotations.CanBeNull] string migrationsHistoryTableSchema)
	{
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._migrationsHistoryTableSchema = migrationsHistoryTableSchema;
		return relationalOptionsExtension;
	}

	public virtual RelationalOptionsExtension WithExecutionStrategyFactory([JetBrains.Annotations.CanBeNull] Func<ExecutionStrategyDependencies, IExecutionStrategy> executionStrategyFactory)
	{
		RelationalOptionsExtension relationalOptionsExtension = Clone();
		relationalOptionsExtension._executionStrategyFactory = executionStrategyFactory;
		return relationalOptionsExtension;
	}

	public static RelationalOptionsExtension Extract([JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		List<RelationalOptionsExtension> list = options.Extensions.OfType<RelationalOptionsExtension>().ToList();
		if (list.Count == 0)
		{
			throw new InvalidOperationException(RelationalStrings.NoProviderConfigured);
		}
		if (list.Count > 1)
		{
			throw new InvalidOperationException(RelationalStrings.MultipleProvidersConfigured);
		}
		return list[0];
	}

	public abstract void ApplyServices(IServiceCollection services);

	public virtual void Validate(IDbContextOptions options)
	{
	}
}
