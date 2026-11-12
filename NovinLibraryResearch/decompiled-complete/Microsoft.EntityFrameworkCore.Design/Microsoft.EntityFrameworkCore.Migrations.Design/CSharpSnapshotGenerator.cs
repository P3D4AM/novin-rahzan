using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public class CSharpSnapshotGenerator : ICSharpSnapshotGenerator
{
	protected virtual CSharpSnapshotGeneratorDependencies Dependencies { get; }

	private ICSharpHelper Code => Dependencies.CSharpHelper;

	public CSharpSnapshotGenerator([JetBrains.Annotations.NotNull] CSharpSnapshotGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual void Generate(string builderName, IModel model, IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		List<IAnnotation> annotations = model.GetAnnotations().ToList();
		if (annotations.Count > 0)
		{
			stringBuilder.Append(builderName);
			using (stringBuilder.Indent())
			{
				GenerateFluentApiForAnnotation(ref annotations, "Relational:DefaultSchema", "HasDefaultSchema", stringBuilder);
				IgnoreAnnotationTypes(annotations, "Relational:DbFunction");
				IgnoreAnnotations(annotations, "ChangeDetector.SkipDetectChanges", "ChangeTrackingStrategy", "OwnedTypes", "Relational:CheckConstraints");
				GenerateAnnotations(annotations, stringBuilder);
			}
			stringBuilder.AppendLine(";");
		}
		GenerateEntityTypes(builderName, Sort((from et in model.GetEntityTypes()
			where !et.IsIgnoredByMigrations()
			select et).ToList()), stringBuilder);
	}

	private static IReadOnlyList<IEntityType> Sort(IReadOnlyList<IEntityType> entityTypes)
	{
		Multigraph<IEntityType, int> multigraph = new Multigraph<IEntityType, int>();
		multigraph.AddVertices(entityTypes);
		foreach (IEntityType item in entityTypes.Where((IEntityType et) => et.BaseType != null))
		{
			multigraph.AddEdge(item.BaseType, item, 0);
		}
		return multigraph.TopologicalSort();
	}

	protected virtual void GenerateEntityTypes([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> entityTypes, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypes, "entityTypes");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IEntityType item in entityTypes.Where((IEntityType e) => !e.HasDefiningNavigation() && e.FindOwnership() == null))
		{
			stringBuilder.AppendLine();
			GenerateEntityType(builderName, item, stringBuilder);
		}
		foreach (IEntityType item2 in entityTypes.Where((IEntityType e) => !e.HasDefiningNavigation() && e.FindOwnership() == null && (e.GetDeclaredForeignKeys().Any() || e.GetDeclaredReferencingForeignKeys().Any((IForeignKey fk) => fk.IsOwnership))))
		{
			stringBuilder.AppendLine();
			GenerateEntityTypeRelationships(builderName, item2, stringBuilder);
		}
	}

	protected virtual void GenerateEntityType([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		IForeignKey foreignKey = entityType.FindOwnership();
		string text = foreignKey?.PrincipalToDependent.Name;
		stringBuilder.Append(builderName).Append((text == null) ? ".Entity(" : (foreignKey.IsUnique ? ".OwnsOne(" : ".OwnsMany(")).Append(Code.Literal(entityType.Name));
		if (text != null)
		{
			stringBuilder.Append(", ").Append(Code.Literal(text));
		}
		if (builderName.StartsWith("b", StringComparison.Ordinal))
		{
			int result = 1;
			if (builderName.Length > 1 && int.TryParse(builderName.Substring(1), out result))
			{
				result++;
			}
			builderName = "b" + ((result == 0) ? "" : result.ToString());
		}
		else
		{
			builderName = "b";
		}
		stringBuilder.Append(", ").Append(builderName).AppendLine(" =>");
		using (stringBuilder.Indent())
		{
			stringBuilder.Append("{");
			using (stringBuilder.Indent())
			{
				GenerateBaseType(builderName, entityType.BaseType, stringBuilder);
				GenerateProperties(builderName, entityType.GetDeclaredProperties(), stringBuilder);
				GenerateKeys(builderName, entityType.GetDeclaredKeys(), entityType.FindDeclaredPrimaryKey(), stringBuilder);
				GenerateIndexes(builderName, entityType.GetDeclaredIndexes(), stringBuilder);
				GenerateEntityTypeAnnotations(builderName, entityType, stringBuilder);
				GenerateCheckConstraints(builderName, entityType, stringBuilder);
				if (text != null)
				{
					GenerateRelationships(builderName, entityType, stringBuilder);
				}
				GenerateData(builderName, entityType.GetProperties(), entityType.GetSeedData(providerValues: true), stringBuilder);
			}
			stringBuilder.AppendLine("});");
		}
	}

	protected virtual void GenerateOwnedTypes([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IForeignKey> ownerships, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownerships, "ownerships");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IForeignKey ownership in ownerships)
		{
			stringBuilder.AppendLine();
			GenerateOwnedType(builderName, ownership, stringBuilder);
		}
	}

	protected virtual void GenerateOwnedType([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IForeignKey ownership, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownership, "ownership");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		GenerateEntityType(builderName, ownership.DeclaringEntityType, stringBuilder);
	}

	protected virtual void GenerateEntityTypeRelationships([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		stringBuilder.Append(builderName).Append(".Entity(").Append(Code.Literal(entityType.Name))
			.AppendLine(", b =>");
		using (stringBuilder.Indent())
		{
			stringBuilder.Append("{");
			using (stringBuilder.Indent())
			{
				GenerateRelationships("b", entityType, stringBuilder);
			}
			stringBuilder.AppendLine("});");
		}
	}

	protected virtual void GenerateRelationships([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		GenerateForeignKeys(builderName, entityType.GetDeclaredForeignKeys(), stringBuilder);
		GenerateOwnedTypes(builderName, from fk in entityType.GetDeclaredReferencingForeignKeys()
			where fk.IsOwnership
			select fk, stringBuilder);
	}

	protected virtual void GenerateBaseType([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.CanBeNull] IEntityType baseType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		if (baseType != null)
		{
			stringBuilder.AppendLine().Append(builderName).Append(".HasBaseType(")
				.Append(Code.Literal(baseType.Name))
				.AppendLine(");");
		}
	}

	protected virtual void GenerateProperties([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IProperty> properties, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IProperty property in properties)
		{
			GenerateProperty(builderName, property, stringBuilder);
		}
	}

	protected virtual void GenerateProperty([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		Type type = FindValueConverter(property)?.ProviderClrType.MakeNullable(property.IsNullable) ?? property.ClrType;
		stringBuilder.AppendLine().Append(builderName).Append(".Property<")
			.Append(Code.Reference(type))
			.Append(">(")
			.Append(Code.Literal(property.Name))
			.Append(")");
		using (stringBuilder.Indent())
		{
			if (property.IsConcurrencyToken)
			{
				stringBuilder.AppendLine().Append(".IsConcurrencyToken()");
			}
			if (property.IsNullable != (type.IsNullableType() && !property.IsPrimaryKey()))
			{
				stringBuilder.AppendLine().Append(".IsRequired()");
			}
			if (property.ValueGenerated != 0)
			{
				stringBuilder.AppendLine().Append((property.ValueGenerated == ValueGenerated.OnAdd) ? ".ValueGeneratedOnAdd()" : ((property.ValueGenerated == ValueGenerated.OnUpdate) ? ".ValueGeneratedOnUpdate()" : ".ValueGeneratedOnAddOrUpdate()"));
			}
			GeneratePropertyAnnotations(property, stringBuilder);
		}
		stringBuilder.AppendLine(";");
	}

	protected virtual void GeneratePropertyAnnotations([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		List<IAnnotation> annotations = property.GetAnnotations().ToList();
		GenerateFluentApiForAnnotation(ref annotations, "Relational:ColumnName", "HasColumnName", stringBuilder);
		stringBuilder.AppendLine().Append(".").Append("HasColumnType")
			.Append("(")
			.Append(Code.Literal(property.GetColumnType() ?? Dependencies.RelationalTypeMappingSource.GetMapping(property).StoreType))
			.Append(")");
		GenerateFluentApiForAnnotation(ref annotations, "Relational:DefaultValueSql", "HasDefaultValueSql", stringBuilder);
		GenerateFluentApiForAnnotation(ref annotations, "Relational:ComputedColumnSql", "HasComputedColumnSql", stringBuilder);
		GenerateFluentApiForAnnotation(ref annotations, "Relational:IsFixedLength", "IsFixedLength", stringBuilder);
		GenerateFluentApiForAnnotation(ref annotations, "Relational:Comment", "HasComment", stringBuilder);
		GenerateFluentApiForAnnotation(ref annotations, "MaxLength", "HasMaxLength", stringBuilder);
		GenerateFluentApiForAnnotation(ref annotations, "Unicode", "IsUnicode", stringBuilder);
		ValueConverter valueConverter = FindValueConverter(property);
		GenerateFluentApiForAnnotation(ref annotations, "Relational:DefaultValue", (IAnnotation a) => (valueConverter != null) ? valueConverter.ConvertToProvider(a?.Value) : a?.Value, "HasDefaultValue", stringBuilder);
		IgnoreAnnotations(annotations, "Relational:ColumnType", "ValueGeneratorFactory", "PropertyAccessMode", "ChangeTrackingStrategy", "BeforeSaveBehavior", "AfterSaveBehavior", "TypeMapping", "ValueComparer", "KeyValueComparer", "StructuralValueComparer", "ValueConverter", "ProviderClrType");
		GenerateAnnotations(annotations, stringBuilder);
	}

	private ValueConverter FindValueConverter(IProperty property)
	{
		return (property.FindTypeMapping() ?? Dependencies.RelationalTypeMappingSource.FindMapping(property))?.Converter;
	}

	protected virtual void GenerateKeys([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IKey> keys, [JetBrains.Annotations.CanBeNull] IKey primaryKey, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keys, "keys");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		if (primaryKey != null)
		{
			GenerateKey(builderName, primaryKey, stringBuilder, primary: true);
		}
		IKey key2 = primaryKey;
		if (key2 != null && key2.DeclaringEntityType.IsOwned())
		{
			return;
		}
		foreach (IKey item in keys.Where((IKey key) => key != primaryKey && (!key.GetReferencingForeignKeys().Any() || key.GetAnnotations().Any())))
		{
			GenerateKey(builderName, item, stringBuilder);
		}
	}

	protected virtual void GenerateKey([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder, bool primary = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(key, "key");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		stringBuilder.AppendLine().Append(builderName).Append(primary ? ".HasKey(" : ".HasAlternateKey(")
			.Append(string.Join(", ", key.Properties.Select((IProperty p) => Code.Literal(p.Name))))
			.Append(")");
		using (stringBuilder.Indent())
		{
			List<IAnnotation> annotations = key.GetAnnotations().ToList();
			GenerateFluentApiForAnnotation(ref annotations, "Relational:Name", "HasName", stringBuilder);
			GenerateAnnotations(annotations, stringBuilder);
		}
		stringBuilder.AppendLine(";");
	}

	protected virtual void GenerateIndexes([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IIndex> indexes, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexes, "indexes");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IIndex index in indexes)
		{
			GenerateIndex(builderName, index, stringBuilder);
		}
	}

	protected virtual void GenerateIndex([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IIndex index, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(index, "index");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		stringBuilder.AppendLine().Append(builderName).Append(".HasIndex(")
			.Append(string.Join(", ", index.Properties.Select((IProperty p) => Code.Literal(p.Name))))
			.Append(")");
		using (stringBuilder.Indent())
		{
			if (index.IsUnique)
			{
				stringBuilder.AppendLine().Append(".IsUnique()");
			}
			List<IAnnotation> annotations = index.GetAnnotations().ToList();
			GenerateFluentApiForAnnotation(ref annotations, "Relational:Name", "HasName", stringBuilder);
			GenerateFluentApiForAnnotation(ref annotations, "Relational:Filter", "HasFilter", stringBuilder);
			GenerateAnnotations(annotations, stringBuilder);
		}
		stringBuilder.AppendLine(";");
	}

	protected virtual void GenerateEntityTypeAnnotations([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		List<IAnnotation> list = entityType.GetAnnotations().ToList();
		IAnnotation annotation = list.FirstOrDefault((IAnnotation a) => a.Name == "Relational:TableName");
		IAnnotation annotation2 = list.FirstOrDefault((IAnnotation a) => a.Name == "Relational:Schema");
		bool flag = false;
		if ((annotation != null && annotation.Value != null) || entityType.BaseType == null)
		{
			stringBuilder.AppendLine().Append(builderName).Append(".")
				.Append("ToTable")
				.Append("(")
				.Append(Code.Literal(((string)annotation?.Value) ?? entityType.GetTableName()));
			list.Remove(annotation);
			flag = true;
		}
		if (annotation2 != null && annotation2.Value != null)
		{
			stringBuilder.Append(",").Append(Code.Literal((string)annotation2.Value));
			list.Remove(annotation2);
			flag = true;
		}
		if (flag)
		{
			stringBuilder.AppendLine(");");
		}
		IAnnotation annotation3 = list.FirstOrDefault((IAnnotation a) => a.Name == "DiscriminatorProperty");
		IAnnotation annotation4 = list.FirstOrDefault((IAnnotation a) => a.Name == "DiscriminatorValue");
		if ((annotation3 ?? annotation4) != null)
		{
			stringBuilder.AppendLine().Append(builderName).Append(".")
				.Append("HasDiscriminator");
			if (annotation3 != null && annotation3.Value != null)
			{
				IProperty property = entityType.FindProperty((string)annotation3.Value);
				Type type = FindValueConverter(property)?.ProviderClrType.MakeNullable(property.IsNullable) ?? property.ClrType;
				stringBuilder.Append("<").Append(Code.Reference(type)).Append(">(")
					.Append(Code.Literal((string)annotation3.Value))
					.Append(")");
			}
			else
			{
				stringBuilder.Append("()");
			}
			if (annotation4 != null && annotation4.Value != null)
			{
				object obj = annotation4.Value;
				IProperty discriminatorProperty = entityType.GetRootType().GetDiscriminatorProperty();
				if (discriminatorProperty != null)
				{
					ValueConverter valueConverter = FindValueConverter(discriminatorProperty);
					if (valueConverter != null)
					{
						obj = valueConverter.ConvertToProvider(obj);
					}
				}
				stringBuilder.Append(".").Append("HasValue").Append("(")
					.Append(Code.UnknownLiteral(obj))
					.Append(")");
			}
			stringBuilder.AppendLine(";");
			list.Remove(annotation3);
			list.Remove(annotation4);
		}
		IAnnotation annotation5 = list.FirstOrDefault((IAnnotation a) => a.Name == "Relational:Comment");
		if (annotation5 != null)
		{
			stringBuilder.AppendLine().Append(builderName).Append(".")
				.Append("HasComment")
				.Append("(")
				.Append(Code.UnknownLiteral(annotation5.Value))
				.AppendLine(");");
			list.Remove(annotation5);
		}
		IgnoreAnnotations(list, "RelationshipDiscoveryConvention:NavigationCandidates", "RelationshipDiscoveryConvention:AmbiguousNavigations", "InversePropertyAttributeConvention:InverseNavigations", "NavigationAccessMode", "PropertyAccessMode", "ChangeTrackingStrategy", "ConstructorBinding", "DefiningQuery", "QueryFilter", "Relational:CheckConstraints");
		if (list.Count <= 0)
		{
			return;
		}
		foreach (IAnnotation item in list)
		{
			if (item.Value != null)
			{
				stringBuilder.AppendLine().Append(builderName);
				GenerateAnnotation(item, stringBuilder);
				stringBuilder.AppendLine(";");
			}
		}
	}

	protected virtual void GenerateCheckConstraints([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (ICheckConstraint checkConstraint in entityType.GetCheckConstraints())
		{
			stringBuilder.AppendLine();
			GenerateCheckConstraint(builderName, checkConstraint, stringBuilder);
		}
	}

	protected virtual void GenerateCheckConstraint([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] ICheckConstraint checkConstraint, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(checkConstraint, "checkConstraint");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		stringBuilder.Append(builderName).Append(".HasCheckConstraint(").Append(Code.Literal(checkConstraint.Name))
			.Append(", ")
			.Append(Code.Literal(checkConstraint.Sql))
			.AppendLine(");");
	}

	protected virtual void GenerateForeignKeys([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IForeignKey> foreignKeys, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeys, "foreignKeys");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IForeignKey foreignKey in foreignKeys)
		{
			stringBuilder.AppendLine();
			GenerateForeignKey(builderName, foreignKey, stringBuilder);
		}
	}

	protected virtual void GenerateForeignKey([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		if (!foreignKey.IsOwnership)
		{
			stringBuilder.Append(builderName).Append(".HasOne(").Append(Code.Literal(foreignKey.PrincipalEntityType.Name))
				.Append(", ")
				.Append((foreignKey.DependentToPrincipal == null) ? Code.UnknownLiteral(null) : Code.Literal(foreignKey.DependentToPrincipal.Name));
		}
		else
		{
			stringBuilder.Append(builderName).Append(".WithOwner(");
			if (foreignKey.DependentToPrincipal != null)
			{
				stringBuilder.Append(Code.Literal(foreignKey.DependentToPrincipal.Name));
			}
		}
		stringBuilder.Append(")").AppendLine();
		using (stringBuilder.Indent())
		{
			if (foreignKey.IsUnique && !foreignKey.IsOwnership)
			{
				stringBuilder.Append(".WithOne(");
				if (foreignKey.PrincipalToDependent != null)
				{
					stringBuilder.Append(Code.Literal(foreignKey.PrincipalToDependent.Name));
				}
				stringBuilder.AppendLine(")").Append(".HasForeignKey(").Append(Code.Literal(foreignKey.DeclaringEntityType.Name))
					.Append(", ")
					.Append(string.Join(", ", foreignKey.Properties.Select((IProperty p) => Code.Literal(p.Name))))
					.Append(")");
				GenerateForeignKeyAnnotations(foreignKey, stringBuilder);
				if (foreignKey.PrincipalKey != foreignKey.PrincipalEntityType.FindPrimaryKey())
				{
					stringBuilder.AppendLine().Append(".HasPrincipalKey(").Append(Code.Literal(foreignKey.PrincipalEntityType.Name))
						.Append(", ")
						.Append(string.Join(", ", foreignKey.PrincipalKey.Properties.Select((IProperty p) => Code.Literal(p.Name))))
						.Append(")");
				}
			}
			else
			{
				if (!foreignKey.IsOwnership)
				{
					stringBuilder.Append(".WithMany(");
					if (foreignKey.PrincipalToDependent != null)
					{
						stringBuilder.Append(Code.Literal(foreignKey.PrincipalToDependent.Name));
					}
					stringBuilder.AppendLine(")");
				}
				stringBuilder.Append(".HasForeignKey(").Append(string.Join(", ", foreignKey.Properties.Select((IProperty p) => Code.Literal(p.Name)))).Append(")");
				GenerateForeignKeyAnnotations(foreignKey, stringBuilder);
				if (foreignKey.PrincipalKey != foreignKey.PrincipalEntityType.FindPrimaryKey())
				{
					stringBuilder.AppendLine().Append(".HasPrincipalKey(").Append(string.Join(", ", foreignKey.PrincipalKey.Properties.Select((IProperty p) => Code.Literal(p.Name))))
						.Append(")");
				}
			}
			if (!foreignKey.IsOwnership)
			{
				if (foreignKey.DeleteBehavior != 0)
				{
					stringBuilder.AppendLine().Append(".OnDelete(").Append(Code.Literal(foreignKey.DeleteBehavior))
						.Append(")");
				}
				if (foreignKey.IsRequired)
				{
					stringBuilder.AppendLine().Append(".IsRequired()");
				}
			}
		}
		stringBuilder.AppendLine(";");
	}

	protected virtual void GenerateForeignKeyAnnotations([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		List<IAnnotation> annotations = foreignKey.GetAnnotations().ToList();
		GenerateFluentApiForAnnotation(ref annotations, "Relational:Name", "HasConstraintName", stringBuilder);
		GenerateAnnotations(annotations, stringBuilder);
	}

	protected virtual void IgnoreAnnotations([JetBrains.Annotations.NotNull] IList<IAnnotation> annotations, [JetBrains.Annotations.NotNull] params string[] annotationNames)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotations, "annotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotationNames, "annotationNames");
		foreach (string annotationName in annotationNames)
		{
			IAnnotation annotation = annotations.FirstOrDefault((IAnnotation a) => a.Name == annotationName);
			if (annotation != null)
			{
				annotations.Remove(annotation);
			}
		}
	}

	protected virtual void IgnoreAnnotationTypes([JetBrains.Annotations.NotNull] IList<IAnnotation> annotations, [JetBrains.Annotations.NotNull] params string[] annotationPrefixes)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotations, "annotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotationPrefixes, "annotationPrefixes");
		foreach (IAnnotation item in annotations.Where((IAnnotation a) => annotationPrefixes.Any((string pre) => a.Name.StartsWith(pre, StringComparison.OrdinalIgnoreCase))).ToList())
		{
			annotations.Remove(item);
		}
	}

	protected virtual void GenerateAnnotations([JetBrains.Annotations.NotNull] IReadOnlyList<IAnnotation> annotations, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotations, "annotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		foreach (IAnnotation annotation in annotations)
		{
			if (annotation.Value != null)
			{
				stringBuilder.AppendLine();
				GenerateAnnotation(annotation, stringBuilder);
			}
		}
	}

	protected virtual void GenerateFluentApiForAnnotation([JetBrains.Annotations.NotNull] ref List<IAnnotation> annotations, [JetBrains.Annotations.NotNull] string annotationName, [JetBrains.Annotations.NotNull] string fluentApiMethodName, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		GenerateFluentApiForAnnotation(ref annotations, annotationName, (IAnnotation a) => a?.Value, fluentApiMethodName, stringBuilder);
	}

	protected virtual void GenerateFluentApiForAnnotation([JetBrains.Annotations.NotNull] ref List<IAnnotation> annotations, [JetBrains.Annotations.NotNull] string annotationName, [JetBrains.Annotations.CanBeNull] Func<IAnnotation, object> annotationValueFunc, [JetBrains.Annotations.NotNull] string fluentApiMethodName, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		IAnnotation annotation = annotations.FirstOrDefault((IAnnotation a) => a.Name == annotationName);
		object obj = annotationValueFunc?.Invoke(annotation);
		if (obj != null)
		{
			stringBuilder.AppendLine().Append(".").Append(fluentApiMethodName)
				.Append("(")
				.Append(Code.UnknownLiteral(obj))
				.Append(")");
			annotations.Remove(annotation);
		}
	}

	protected virtual void GenerateAnnotation([JetBrains.Annotations.NotNull] IAnnotation annotation, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		stringBuilder.Append(".HasAnnotation(").Append(Code.Literal(annotation.Name)).Append(", ")
			.Append(Code.UnknownLiteral(annotation.Value))
			.Append(")");
	}

	protected virtual void GenerateData([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IEnumerable<IProperty> properties, [JetBrains.Annotations.NotNull] IEnumerable<IDictionary<string, object>> data, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(data, "data");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stringBuilder, "stringBuilder");
		List<IDictionary<string, object>> list = data.ToList();
		if (list.Count == 0)
		{
			return;
		}
		List<IProperty> list2 = properties.ToList();
		stringBuilder.AppendLine().Append(builderName).Append(".")
			.Append("HasData")
			.AppendLine("(");
		using (stringBuilder.Indent())
		{
			bool flag = true;
			foreach (IDictionary<string, object> item in list)
			{
				if (!flag)
				{
					stringBuilder.AppendLine(",");
				}
				else
				{
					flag = false;
				}
				stringBuilder.AppendLine("new").AppendLine("{");
				using (stringBuilder.Indent())
				{
					bool flag2 = true;
					foreach (IProperty item2 in list2)
					{
						if (item.TryGetValue(item2.Name, out var value) && value != null)
						{
							if (!flag2)
							{
								stringBuilder.AppendLine(",");
							}
							else
							{
								flag2 = false;
							}
							stringBuilder.Append(Code.Identifier(item2.Name)).Append(" = ").Append(Code.UnknownLiteral(value));
						}
					}
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("}");
			}
		}
		stringBuilder.AppendLine(");");
	}
}
