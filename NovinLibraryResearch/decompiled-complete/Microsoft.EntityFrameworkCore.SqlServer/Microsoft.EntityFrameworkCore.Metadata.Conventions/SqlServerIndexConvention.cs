using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SqlServerIndexConvention : IEntityTypeBaseTypeChangedConvention, IConvention, IIndexAddedConvention, IIndexUniquenessChangedConvention, IIndexAnnotationChangedConvention, IPropertyNullabilityChangedConvention, IPropertyAnnotationChangedConvention
{
	private readonly ISqlGenerationHelper _sqlGenerationHelper;

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public SqlServerIndexConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		_sqlGenerationHelper = sqlGenerationHelper;
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (oldBaseType != null && newBaseType != null)
		{
			return;
		}
		foreach (IConventionIndex declaredIndex in entityTypeBuilder.Metadata.GetDeclaredIndexes())
		{
			SetIndexFilter(declaredIndex.Builder);
		}
	}

	public virtual void ProcessIndexAdded(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
	{
		SetIndexFilter(indexBuilder);
	}

	public virtual void ProcessIndexUniquenessChanged(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
	{
		SetIndexFilter(indexBuilder);
	}

	public virtual void ProcessPropertyNullabilityChanged(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		foreach (IConventionIndex containingIndex in propertyBuilder.Metadata.GetContainingIndexes())
		{
			SetIndexFilter(containingIndex.Builder);
		}
	}

	public virtual void ProcessIndexAnnotationChanged(IConventionIndexBuilder indexBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (name == "SqlServer:Clustered")
		{
			SetIndexFilter(indexBuilder);
		}
	}

	public virtual void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (!(name == "Relational:ColumnName"))
		{
			return;
		}
		foreach (IConventionIndex containingIndex in propertyBuilder.Metadata.GetContainingIndexes())
		{
			SetIndexFilter(containingIndex.Builder, columnNameChanged: true);
		}
	}

	private IConventionIndexBuilder SetIndexFilter(IConventionIndexBuilder indexBuilder, bool columnNameChanged = false)
	{
		IConventionIndex metadata = indexBuilder.Metadata;
		if (metadata.IsUnique && metadata.IsClustered() != true && metadata.Properties.Any((IConventionProperty property) => property.IsColumnNullable()))
		{
			if (columnNameChanged || metadata.GetFilter() == null)
			{
				indexBuilder.HasFilter(CreateIndexFilter(metadata));
			}
		}
		else if (metadata.GetFilter() != null)
		{
			indexBuilder.HasFilter(null);
		}
		return indexBuilder;
	}

	private string CreateIndexFilter(IIndex index)
	{
		List<string> list = (from property in index.Properties
			where property.IsColumnNullable()
			select property.GetColumnName()).ToList();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < list.Count; i++)
		{
			if (i != 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder.Append(_sqlGenerationHelper.DelimitIdentifier(list[i])).Append(" IS NOT NULL");
		}
		return stringBuilder.ToString();
	}
}
