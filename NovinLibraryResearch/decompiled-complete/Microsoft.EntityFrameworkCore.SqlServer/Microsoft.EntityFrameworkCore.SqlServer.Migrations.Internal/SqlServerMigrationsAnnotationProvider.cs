using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

public class SqlServerMigrationsAnnotationProvider : MigrationsAnnotationProvider
{
	public SqlServerMigrationsAnnotationProvider([JetBrains.Annotations.NotNull] MigrationsAnnotationProviderDependencies dependencies)
		: base(dependencies)
	{
	}

	public override IEnumerable<IAnnotation> For(IModel model)
	{
		string databaseMaxSize = model.GetDatabaseMaxSize();
		string serviceTierSql = model.GetServiceTierSql();
		string performanceLevelSql = model.GetPerformanceLevelSql();
		if (databaseMaxSize != null || serviceTierSql != null || performanceLevelSql != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (databaseMaxSize != null)
			{
				stringBuilder.Append("MAXSIZE = ");
				stringBuilder.Append(databaseMaxSize);
				stringBuilder.Append(", ");
			}
			if (serviceTierSql != null)
			{
				stringBuilder.Append("EDITION = ");
				stringBuilder.Append(serviceTierSql);
				stringBuilder.Append(", ");
			}
			if (performanceLevelSql != null)
			{
				stringBuilder.Append("SERVICE_OBJECTIVE = ");
				stringBuilder.Append(performanceLevelSql);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			yield return new Annotation("SqlServer:EditionOptions", stringBuilder.ToString());
		}
		foreach (IAnnotation item in ForRemove(model))
		{
			yield return item;
		}
	}

	public override IEnumerable<IAnnotation> For(IEntityType entityType)
	{
		return ForRemove(entityType);
	}

	public override IEnumerable<IAnnotation> For(IKey key)
	{
		bool? flag = key.IsClustered();
		if (flag.HasValue)
		{
			yield return new Annotation("SqlServer:Clustered", flag.Value);
		}
	}

	public override IEnumerable<IAnnotation> For(IIndex index)
	{
		bool? flag = index.IsClustered();
		if (flag.HasValue)
		{
			yield return new Annotation("SqlServer:Clustered", flag.Value);
		}
		IReadOnlyList<string> includeProperties = index.GetIncludeProperties();
		if (includeProperties != null)
		{
			IReadOnlyList<string> value = includeProperties.Select((string p) => index.DeclaringEntityType.FindProperty(p).GetColumnName()).ToArray();
			yield return new Annotation("SqlServer:Include", value);
		}
		bool? flag2 = index.IsCreatedOnline();
		if (flag2.HasValue)
		{
			yield return new Annotation("SqlServer:Online", flag2.Value);
		}
	}

	public override IEnumerable<IAnnotation> For(IProperty property)
	{
		if (property.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn)
		{
			int? identitySeed = property.GetIdentitySeed();
			int? identityIncrement = property.GetIdentityIncrement();
			yield return new Annotation("SqlServer:Identity", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", identitySeed ?? 1, identityIncrement ?? 1));
		}
	}

	public override IEnumerable<IAnnotation> ForRemove(IModel model)
	{
		if (model.GetEntityTypes().Any((IEntityType e) => e.BaseType == null && e.IsMemoryOptimized()))
		{
			yield return new Annotation("SqlServer:MemoryOptimized", true);
		}
	}

	public override IEnumerable<IAnnotation> ForRemove(IEntityType entityType)
	{
		if (IsMemoryOptimized(entityType))
		{
			yield return new Annotation("SqlServer:MemoryOptimized", true);
		}
	}

	private static bool IsMemoryOptimized(IEntityType entityType)
	{
		return entityType.GetAllBaseTypesInclusive().Any((IEntityType t) => t.IsMemoryOptimized());
	}
}
