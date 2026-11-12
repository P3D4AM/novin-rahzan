using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

public class SqlServerOptionsExtension : RelationalOptionsExtension
{
	private sealed class ExtensionInfo : RelationalExtensionInfo
	{
		private long? _serviceProviderHash;

		private string _logFragment;

		private new SqlServerOptionsExtension Extension => (SqlServerOptionsExtension)base.Extension;

		public override bool IsDatabaseProvider => true;

		public override string LogFragment
		{
			get
			{
				if (_logFragment == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(base.LogFragment);
					if (Extension._rowNumberPaging == true)
					{
						stringBuilder.Append("RowNumberPaging ");
					}
					_logFragment = stringBuilder.ToString();
				}
				return _logFragment;
			}
		}

		public ExtensionInfo(IDbContextOptionsExtension extension)
			: base(extension)
		{
		}

		public override long GetServiceProviderHashCode()
		{
			if (!_serviceProviderHash.HasValue)
			{
				_serviceProviderHash = (base.GetServiceProviderHashCode() * 397) ^ (Extension._rowNumberPaging?.GetHashCode() ?? 0);
			}
			return _serviceProviderHash.Value;
		}

		public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
		{
			debugInfo["SqlServer:UseRowNumberForPaging"] = ((long)(Extension._rowNumberPaging?.GetHashCode() ?? 0)).ToString(CultureInfo.InvariantCulture);
		}
	}

	private DbContextOptionsExtensionInfo _info;

	private bool? _rowNumberPaging;

	public override DbContextOptionsExtensionInfo Info => _info ?? (_info = new ExtensionInfo(this));

	public virtual bool? RowNumberPaging => _rowNumberPaging;

	public SqlServerOptionsExtension()
	{
	}

	protected SqlServerOptionsExtension([JetBrains.Annotations.NotNull] SqlServerOptionsExtension copyFrom)
		: base(copyFrom)
	{
		_rowNumberPaging = copyFrom._rowNumberPaging;
	}

	protected override RelationalOptionsExtension Clone()
	{
		return new SqlServerOptionsExtension(this);
	}

	public virtual SqlServerOptionsExtension WithRowNumberPaging(bool rowNumberPaging)
	{
		SqlServerOptionsExtension obj = (SqlServerOptionsExtension)Clone();
		obj._rowNumberPaging = rowNumberPaging;
		return obj;
	}

	public override void ApplyServices(IServiceCollection services)
	{
		services.AddEntityFrameworkSqlServer();
	}
}
