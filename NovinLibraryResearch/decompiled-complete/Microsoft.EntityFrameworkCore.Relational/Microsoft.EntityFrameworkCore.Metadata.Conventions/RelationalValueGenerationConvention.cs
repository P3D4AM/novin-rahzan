using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalValueGenerationConvention : ValueGenerationConvention, IPropertyAnnotationChangedConvention, IConvention, IEntityTypeAnnotationChangedConvention
{
	public RelationalValueGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies)
	{
	}

	public virtual void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		IConventionProperty metadata = propertyBuilder.Metadata;
		switch (name)
		{
		case "Relational:DefaultValue":
		case "Relational:DefaultValueSql":
		case "Relational:ComputedColumnSql":
			propertyBuilder.ValueGenerated(GetValueGenerated(metadata));
			break;
		}
	}

	public virtual void ProcessEntityTypeAnnotationChanged(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (name == "Relational:TableName")
		{
			ProcessTableChanged(entityTypeBuilder, ((string)oldAnnotation?.Value) ?? entityTypeBuilder.Metadata.GetDefaultTableName(), entityTypeBuilder.Metadata.GetSchema());
		}
		else if (name == "Relational:Schema")
		{
			ProcessTableChanged(entityTypeBuilder, entityTypeBuilder.Metadata.GetTableName(), ((string)oldAnnotation?.Value) ?? entityTypeBuilder.Metadata.GetDefaultSchema());
		}
	}

	private void ProcessTableChanged(IConventionEntityTypeBuilder entityTypeBuilder, string oldTable, string oldSchema)
	{
		IConventionKey conventionKey = entityTypeBuilder.Metadata.FindPrimaryKey();
		if (conventionKey == null)
		{
			return;
		}
		IForeignKey foreignKey = conventionKey.Properties.First().FindSharedTableLink(oldTable, oldSchema);
		IForeignKey foreignKey2 = conventionKey.Properties.First().FindSharedTableLink();
		if (foreignKey == null && foreignKey2 == null)
		{
			return;
		}
		foreach (IConventionProperty property in conventionKey.Properties)
		{
			property.Builder.ValueGenerated(GetValueGenerated(property));
		}
	}

	protected override ValueGenerated? GetValueGenerated(IConventionProperty property)
	{
		return GetValueGenerated(property);
	}

	public new static ValueGenerated? GetValueGenerated([JetBrains.Annotations.NotNull] IProperty property)
	{
		ValueGenerated? valueGenerated = ValueGenerationConvention.GetValueGenerated(property);
		if (valueGenerated.HasValue)
		{
			return valueGenerated;
		}
		if (property.GetComputedColumnSql() == null)
		{
			if (property.GetDefaultValue() == null && property.GetDefaultValueSql() == null)
			{
				return null;
			}
			return ValueGenerated.OnAdd;
		}
		return ValueGenerated.OnAddOrUpdate;
	}
}
