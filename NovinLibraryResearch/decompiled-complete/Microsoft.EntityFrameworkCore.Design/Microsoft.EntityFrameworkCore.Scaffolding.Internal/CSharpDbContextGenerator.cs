using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpDbContextGenerator : ICSharpDbContextGenerator
{
	private const string EntityLambdaIdentifier = "entity";

	private const string Language = "CSharp";

	private readonly ICSharpHelper _code;

	private readonly IProviderConfigurationCodeGenerator _providerConfigurationCodeGenerator;

	private readonly IAnnotationCodeGenerator _annotationCodeGenerator;

	private IndentedStringBuilder _sb;

	private bool _entityTypeBuilderInitialized;

	public CSharpDbContextGenerator([JetBrains.Annotations.NotNull] IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator, [JetBrains.Annotations.NotNull] IAnnotationCodeGenerator annotationCodeGenerator, [JetBrains.Annotations.NotNull] ICSharpHelper cSharpHelper)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(providerConfigurationCodeGenerator, "providerConfigurationCodeGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotationCodeGenerator, "annotationCodeGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpHelper, "cSharpHelper");
		_providerConfigurationCodeGenerator = providerConfigurationCodeGenerator;
		_annotationCodeGenerator = annotationCodeGenerator;
		_code = cSharpHelper;
	}

	public virtual string WriteCode(IModel model, string contextName, string connectionString, string contextNamespace, string modelNamespace, bool useDataAnnotations, bool suppressConnectionStringWarning)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		_sb = new IndentedStringBuilder();
		_sb.AppendLine("using System;");
		_sb.AppendLine("using Microsoft.EntityFrameworkCore;");
		_sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata;");
		string text = contextNamespace ?? modelNamespace;
		if (text != modelNamespace)
		{
			_sb.AppendLine("using " + modelNamespace + ";");
		}
		_sb.AppendLine();
		_sb.AppendLine("// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.");
		_sb.AppendLine("// If you have enabled NRTs for your project, then un-comment the following line:");
		_sb.AppendLine("// #nullable disable");
		_sb.AppendLine();
		_sb.AppendLine("namespace " + text);
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			GenerateClass(model, contextName, connectionString, useDataAnnotations, suppressConnectionStringWarning);
		}
		_sb.AppendLine("}");
		return _sb.ToString();
	}

	protected virtual void GenerateClass([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string contextName, [JetBrains.Annotations.NotNull] string connectionString, bool useDataAnnotations, bool suppressConnectionStringWarning)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextName, "contextName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connectionString, "connectionString");
		_sb.AppendLine("public partial class " + contextName + " : DbContext");
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			GenerateConstructors(contextName);
			GenerateDbSets(model);
			GenerateEntityTypeErrors(model);
			GenerateOnConfiguring(connectionString, suppressConnectionStringWarning);
			GenerateOnModelCreating(model, useDataAnnotations);
		}
		_sb.AppendLine();
		using (_sb.Indent())
		{
			_sb.AppendLine("partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");
		}
		_sb.AppendLine("}");
	}

	private void GenerateConstructors(string contextName)
	{
		_sb.AppendLine("public " + contextName + "()").AppendLine("{").AppendLine("}")
			.AppendLine();
		_sb.AppendLine("public " + contextName + "(DbContextOptions<" + contextName + "> options)").IncrementIndent().AppendLine(": base(options)")
			.DecrementIndent()
			.AppendLine("{")
			.AppendLine("}")
			.AppendLine();
	}

	private void GenerateDbSets(IModel model)
	{
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			_sb.AppendLine("public virtual DbSet<" + entityType.Name + "> " + entityType.GetDbSetName() + " { get; set; }");
		}
		if (model.GetEntityTypes().Any())
		{
			_sb.AppendLine();
		}
	}

	private void GenerateEntityTypeErrors(IModel model)
	{
		foreach (KeyValuePair<string, string> entityTypeError in model.GetEntityTypeErrors())
		{
			_sb.AppendLine("// " + entityTypeError.Value + " Please see the warning messages.");
		}
		if (model.GetEntityTypeErrors().Count > 0)
		{
			_sb.AppendLine();
		}
	}

	protected virtual void GenerateOnConfiguring([JetBrains.Annotations.NotNull] string connectionString, bool suppressConnectionStringWarning)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connectionString, "connectionString");
		_sb.AppendLine("protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			_sb.AppendLine("if (!optionsBuilder.IsConfigured)");
			_sb.AppendLine("{");
			using (_sb.Indent())
			{
				if (!suppressConnectionStringWarning)
				{
					_sb.DecrementIndent().DecrementIndent().DecrementIndent()
						.DecrementIndent()
						.AppendLine("#warning " + DesignStrings.SensitiveInformationWarning)
						.IncrementIndent()
						.IncrementIndent()
						.IncrementIndent()
						.IncrementIndent();
				}
				_sb.Append("optionsBuilder");
				MethodCallCodeFragment methodCallCodeFragment = _providerConfigurationCodeGenerator.GenerateUseProvider(connectionString, _providerConfigurationCodeGenerator.GenerateProviderOptions());
				MethodCallCodeFragment methodCallCodeFragment2 = _providerConfigurationCodeGenerator.GenerateContextOptions();
				if (methodCallCodeFragment2 != null)
				{
					methodCallCodeFragment = methodCallCodeFragment.Chain(methodCallCodeFragment2);
				}
				_sb.Append(_code.Fragment(methodCallCodeFragment)).AppendLine(";");
			}
			_sb.AppendLine("}");
		}
		_sb.AppendLine("}");
		_sb.AppendLine();
	}

	protected virtual void GenerateOnModelCreating([JetBrains.Annotations.NotNull] IModel model, bool useDataAnnotations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		_sb.AppendLine("protected override void OnModelCreating(ModelBuilder modelBuilder)");
		_sb.Append("{");
		List<IAnnotation> annotations = model.GetAnnotations().ToList();
		RemoveAnnotation(ref annotations, "ProductVersion");
		RemoveAnnotation(ref annotations, "ChangeTrackingStrategy");
		RemoveAnnotation(ref annotations, "OwnedTypes");
		RemoveAnnotation(ref annotations, "ChangeDetector.SkipDetectChanges");
		RemoveAnnotation(ref annotations, "Relational:MaxIdentifierLength");
		RemoveAnnotation(ref annotations, "Relational:CheckConstraints");
		RemoveAnnotation(ref annotations, "Scaffolding:DatabaseName");
		RemoveAnnotation(ref annotations, "Scaffolding:EntityTypeErrors");
		List<IAnnotation> list = new List<IAnnotation>();
		list.AddRange(annotations.Where((IAnnotation a) => a.Name.StartsWith("Relational:Sequence:", StringComparison.Ordinal)));
		List<string> list2 = new List<string>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(model, item))
			{
				list.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(model, item);
			if (methodCallCodeFragment != null)
			{
				list2.Add(_code.Fragment(methodCallCodeFragment));
				list.Add(item);
			}
		}
		list2.AddRange(GenerateAnnotations(annotations.Except(list)));
		if (list2.Count > 0)
		{
			using (_sb.Indent())
			{
				_sb.AppendLine();
				_sb.Append("modelBuilder" + list2[0]);
				using (_sb.Indent())
				{
					foreach (string item2 in list2.Skip(1))
					{
						_sb.AppendLine();
						_sb.Append(item2);
					}
				}
				_sb.AppendLine(";");
			}
		}
		using (_sb.Indent())
		{
			foreach (IEntityType entityType in model.GetEntityTypes())
			{
				_entityTypeBuilderInitialized = false;
				GenerateEntityType(entityType, useDataAnnotations);
				if (_entityTypeBuilderInitialized)
				{
					_sb.AppendLine("});");
				}
			}
			foreach (ISequence sequence in model.GetSequences())
			{
				GenerateSequence(sequence);
			}
		}
		_sb.AppendLine();
		using (_sb.Indent())
		{
			_sb.AppendLine("OnModelCreatingPartial(modelBuilder);");
		}
		_sb.AppendLine("}");
	}

	private void InitializeEntityTypeBuilder(IEntityType entityType)
	{
		if (!_entityTypeBuilderInitialized)
		{
			_sb.AppendLine();
			_sb.AppendLine("modelBuilder.Entity<" + entityType.Name + ">(entity =>");
			_sb.Append("{");
		}
		_entityTypeBuilderInitialized = true;
	}

	private void GenerateEntityType(IEntityType entityType, bool useDataAnnotations)
	{
		GenerateKey(entityType.FindPrimaryKey(), entityType, useDataAnnotations);
		List<IAnnotation> annotations = entityType.GetAnnotations().ToList();
		RemoveAnnotation(ref annotations, "ConstructorBinding");
		RemoveAnnotation(ref annotations, "Relational:TableName");
		RemoveAnnotation(ref annotations, "Relational:Comment");
		RemoveAnnotation(ref annotations, "Relational:Schema");
		RemoveAnnotation(ref annotations, "Scaffolding:DbSetName");
		RemoveAnnotation(ref annotations, "Relational:ViewDefinition");
		bool flag = entityType.FindAnnotation("Relational:ViewDefinition") != null;
		if (!useDataAnnotations || flag)
		{
			GenerateTableName(entityType);
		}
		List<IAnnotation> list = new List<IAnnotation>();
		List<string> list2 = new List<string>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(entityType, item))
			{
				list.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(entityType, item);
			if (methodCallCodeFragment != null)
			{
				list2.Add(_code.Fragment(methodCallCodeFragment));
				list.Add(item);
			}
		}
		list2.AddRange(GenerateAnnotations(annotations.Except(list)));
		if (entityType.GetComment() != null)
		{
			list2.Add(".HasComment(" + _code.Literal(entityType.GetComment()) + ")");
		}
		AppendMultiLineFluentApi(entityType, list2);
		foreach (IIndex index in entityType.GetIndexes())
		{
			GenerateIndex(index);
		}
		foreach (IProperty property in entityType.GetProperties())
		{
			GenerateProperty(property, useDataAnnotations);
		}
		foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
		{
			GenerateRelationship(foreignKey, useDataAnnotations);
		}
	}

	private void AppendMultiLineFluentApi(IEntityType entityType, IList<string> lines)
	{
		if (lines.Count <= 0)
		{
			return;
		}
		InitializeEntityTypeBuilder(entityType);
		using (_sb.Indent())
		{
			_sb.AppendLine();
			_sb.Append("entity" + lines[0]);
			using (_sb.Indent())
			{
				foreach (string item in lines.Skip(1))
				{
					_sb.AppendLine();
					_sb.Append(item);
				}
			}
			_sb.AppendLine(";");
		}
	}

	private void GenerateKey(IKey key, IEntityType entityType, bool useDataAnnotations)
	{
		if (key == null)
		{
			List<string> lines = new List<string> { ".HasNoKey()" };
			AppendMultiLineFluentApi(entityType, lines);
			return;
		}
		List<IAnnotation> annotations = key.GetAnnotations().ToList();
		bool flag = key.GetName() != key.GetDefaultName();
		RemoveAnnotation(ref annotations, "Relational:Name");
		if (key.Properties.Count == 1 && annotations.Count == 0 && ((key is Key key2 && key.Properties.SequenceEqual(KeyDiscoveryConvention.DiscoverKeyProperties(key2.DeclaringEntityType, key2.DeclaringEntityType.GetProperties()))) || (!flag && useDataAnnotations)))
		{
			return;
		}
		List<string> list = new List<string> { ".HasKey(e => " + GenerateLambdaToKey(key.Properties, "e") + ")" };
		if (flag)
		{
			list.Add(".HasName(" + _code.Literal(key.GetName()) + ")");
		}
		List<IAnnotation> list2 = new List<IAnnotation>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(key, item))
			{
				list2.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(key, item);
			if (methodCallCodeFragment != null)
			{
				list.Add(_code.Fragment(methodCallCodeFragment));
				list2.Add(item);
			}
		}
		list.AddRange(GenerateAnnotations(annotations.Except(list2)));
		AppendMultiLineFluentApi(key.DeclaringEntityType, list);
	}

	private void GenerateTableName(IEntityType entityType)
	{
		string tableName = entityType.GetTableName();
		string schema = entityType.GetSchema();
		string defaultSchema = entityType.Model.GetDefaultSchema();
		bool flag = schema != null && schema != defaultSchema;
		bool num = flag || (tableName != null && tableName != entityType.GetDbSetName());
		bool flag2 = entityType.FindAnnotation("Relational:ViewDefinition") != null;
		if (num || flag2)
		{
			string text = _code.Literal(tableName);
			if (flag)
			{
				text = text + ", " + _code.Literal(schema);
			}
			List<string> list = new List<string>();
			list.Add("." + (flag2 ? "ToView" : "ToTable") + "(" + text + ")");
			List<string> lines = list;
			AppendMultiLineFluentApi(entityType, lines);
		}
	}

	private void GenerateIndex(IIndex index)
	{
		List<string> list = new List<string> { ".HasIndex(e => " + GenerateLambdaToKey(index.Properties, "e") + ")" };
		List<IAnnotation> annotations = index.GetAnnotations().ToList();
		if (!string.IsNullOrEmpty((string)index["Relational:Name"]))
		{
			list.Add(".HasName(" + _code.Literal(index.GetName()) + ")");
			RemoveAnnotation(ref annotations, "Relational:Name");
		}
		if (index.IsUnique)
		{
			list.Add(".IsUnique()");
		}
		if (index.GetFilter() != null)
		{
			list.Add(".HasFilter(" + _code.Literal(index.GetFilter()) + ")");
			RemoveAnnotation(ref annotations, "Relational:Filter");
		}
		List<IAnnotation> list2 = new List<IAnnotation>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(index, item))
			{
				list2.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(index, item);
			if (methodCallCodeFragment != null)
			{
				list.Add(_code.Fragment(methodCallCodeFragment));
				list2.Add(item);
			}
		}
		list.AddRange(GenerateAnnotations(annotations.Except(list2)));
		AppendMultiLineFluentApi(index.DeclaringEntityType, list);
	}

	private void GenerateProperty(IProperty property, bool useDataAnnotations)
	{
		List<string> list = new List<string> { ".Property(e => e." + property.Name + ")" };
		List<IAnnotation> annotations = property.GetAnnotations().ToList();
		RemoveAnnotation(ref annotations, "Relational:ColumnName");
		RemoveAnnotation(ref annotations, "Relational:ColumnType");
		RemoveAnnotation(ref annotations, "MaxLength");
		RemoveAnnotation(ref annotations, "TypeMapping");
		RemoveAnnotation(ref annotations, "Unicode");
		RemoveAnnotation(ref annotations, "Relational:DefaultValue");
		RemoveAnnotation(ref annotations, "Relational:DefaultValueSql");
		RemoveAnnotation(ref annotations, "Relational:Comment");
		RemoveAnnotation(ref annotations, "Relational:ComputedColumnSql");
		RemoveAnnotation(ref annotations, "Relational:IsFixedLength");
		RemoveAnnotation(ref annotations, "Scaffolding:ColumnOrdinal");
		if (!useDataAnnotations)
		{
			if (!property.IsNullable && property.ClrType.IsNullableType() && !property.IsPrimaryKey())
			{
				list.Add(".IsRequired()");
			}
			string columnName = property.GetColumnName();
			if (columnName != null && columnName != property.Name)
			{
				list.Add(".HasColumnName(" + _code.Literal(columnName) + ")");
			}
			string configuredColumnType = property.GetConfiguredColumnType();
			if (configuredColumnType != null)
			{
				list.Add(".HasColumnType(" + _code.Literal(configuredColumnType) + ")");
			}
			int? maxLength = property.GetMaxLength();
			if (maxLength.HasValue)
			{
				list.Add(".HasMaxLength(" + _code.Literal(maxLength.Value) + ")");
			}
		}
		if (property.IsUnicode().HasValue)
		{
			list.Add(".IsUnicode(" + ((property.IsUnicode() == false) ? "false" : "") + ")");
		}
		if (property.IsFixedLength())
		{
			list.Add(".IsFixedLength()");
		}
		if (property.GetDefaultValue() != null)
		{
			list.Add(".HasDefaultValue(" + _code.UnknownLiteral(property.GetDefaultValue()) + ")");
		}
		if (property.GetDefaultValueSql() != null)
		{
			list.Add(".HasDefaultValueSql(" + _code.Literal(property.GetDefaultValueSql()) + ")");
		}
		if (property.GetComment() != null)
		{
			list.Add(".HasComment(" + _code.Literal(property.GetComment()) + ")");
		}
		if (property.GetComputedColumnSql() != null)
		{
			list.Add(".HasComputedColumnSql(" + _code.Literal(property.GetComputedColumnSql()) + ")");
		}
		ValueGenerated valueGenerated = property.ValueGenerated;
		bool flag = false;
		if (((IConventionProperty)property).GetValueGeneratedConfigurationSource().HasValue && RelationalValueGenerationConvention.GetValueGenerated(property) != valueGenerated)
		{
			list.Add("." + valueGenerated switch
			{
				ValueGenerated.OnAdd => "ValueGeneratedOnAdd", 
				ValueGenerated.OnAddOrUpdate => property.IsConcurrencyToken ? "IsRowVersion" : "ValueGeneratedOnAddOrUpdate", 
				ValueGenerated.OnUpdate => "ValueGeneratedOnUpdate", 
				ValueGenerated.Never => "ValueGeneratedNever", 
				_ => throw new InvalidOperationException(string.Format("Unhandled enum value '{0}.{1}'", "ValueGenerated", valueGenerated)), 
			} + "()");
		}
		if (property.IsConcurrencyToken && !flag)
		{
			list.Add(".IsConcurrencyToken()");
		}
		List<IAnnotation> list2 = new List<IAnnotation>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(property, item))
			{
				list2.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(property, item);
			if (methodCallCodeFragment != null)
			{
				list.Add(_code.Fragment(methodCallCodeFragment));
				list2.Add(item);
			}
		}
		list.AddRange(GenerateAnnotations(annotations.Except(list2)));
		switch (list.Count)
		{
		case 1:
			return;
		case 2:
			list = new List<string> { list[0] + list[1] };
			break;
		}
		AppendMultiLineFluentApi(property.DeclaringEntityType, list);
	}

	private void GenerateRelationship(IForeignKey foreignKey, bool useDataAnnotations)
	{
		bool flag = true;
		List<IAnnotation> annotations = foreignKey.GetAnnotations().ToList();
		List<string> list = new List<string>();
		list.Add(".HasOne(" + ((foreignKey.DependentToPrincipal != null) ? ("d => d." + foreignKey.DependentToPrincipal.Name) : null) + ")");
		list.Add("." + (foreignKey.IsUnique ? "WithOne" : "WithMany") + "(" + ((foreignKey.PrincipalToDependent != null) ? ("p => p." + foreignKey.PrincipalToDependent.Name) : null) + ")");
		List<string> list2 = list;
		if (!foreignKey.PrincipalKey.IsPrimaryKey())
		{
			flag = false;
			list2.Add(".HasPrincipalKey" + (foreignKey.IsUnique ? ("<" + foreignKey.PrincipalEntityType.DisplayName() + ">") : "") + "(p => " + GenerateLambdaToKey(foreignKey.PrincipalKey.Properties, "p") + ")");
		}
		list2.Add(".HasForeignKey" + (foreignKey.IsUnique ? ("<" + foreignKey.DeclaringEntityType.DisplayName() + ">") : "") + "(d => " + GenerateLambdaToKey(foreignKey.Properties, "d") + ")");
		DeleteBehavior deleteBehavior = (foreignKey.IsRequired ? DeleteBehavior.Cascade : DeleteBehavior.ClientSetNull);
		if (foreignKey.DeleteBehavior != deleteBehavior)
		{
			flag = false;
			list2.Add(".OnDelete(" + _code.Literal(foreignKey.DeleteBehavior) + ")");
		}
		if (!string.IsNullOrEmpty((string)foreignKey["Relational:Name"]))
		{
			flag = false;
			list2.Add(".HasConstraintName(" + _code.Literal(foreignKey.GetConstraintName()) + ")");
			RemoveAnnotation(ref annotations, "Relational:Name");
		}
		List<IAnnotation> list3 = new List<IAnnotation>();
		foreach (IAnnotation item in annotations)
		{
			if (item.Value == null || _annotationCodeGenerator.IsHandledByConvention(foreignKey, item))
			{
				list3.Add(item);
				continue;
			}
			MethodCallCodeFragment methodCallCodeFragment = _annotationCodeGenerator.GenerateFluentApi(foreignKey, item);
			if (methodCallCodeFragment != null)
			{
				flag = false;
				list2.Add(_code.Fragment(methodCallCodeFragment));
				list3.Add(item);
			}
		}
		list2.AddRange(GenerateAnnotations(annotations.Except(list3)));
		if (!useDataAnnotations || !flag)
		{
			AppendMultiLineFluentApi(foreignKey.DeclaringEntityType, list2);
		}
	}

	private void GenerateSequence(ISequence sequence)
	{
		string text = "HasSequence";
		if (sequence.ClrType != Sequence.DefaultClrType)
		{
			text = text + "<" + _code.Reference(sequence.ClrType) + ">";
		}
		string text2 = _code.Literal(sequence.Name);
		if (!string.IsNullOrEmpty(sequence.Schema) && sequence.Model.GetDefaultSchema() != sequence.Schema)
		{
			text2 = text2 + ", " + _code.Literal(sequence.Schema);
		}
		List<string> list = new List<string>();
		list.Add("modelBuilder." + text + "(" + text2 + ")");
		List<string> list2 = list;
		if (sequence.StartValue != 1)
		{
			list2.Add(string.Format(".{0}({1})", "StartsAt", sequence.StartValue));
		}
		if (sequence.IncrementBy != 1)
		{
			list2.Add(string.Format(".{0}({1})", "IncrementsBy", sequence.IncrementBy));
		}
		if (sequence.MinValue != Sequence.DefaultMinValue)
		{
			list2.Add(string.Format(".{0}({1})", "HasMin", sequence.MinValue));
		}
		if (sequence.MaxValue != Sequence.DefaultMaxValue)
		{
			list2.Add(string.Format(".{0}({1})", "HasMax", sequence.MaxValue));
		}
		if (sequence.IsCyclic != Sequence.DefaultIsCyclic)
		{
			list2.Add(".IsCyclic()");
		}
		if (list2.Count == 2)
		{
			list2 = new List<string> { list2[0] + list2[1] };
		}
		_sb.AppendLine();
		_sb.Append(list2[0]);
		using (_sb.Indent())
		{
			foreach (string item in list2.Skip(1))
			{
				_sb.AppendLine();
				_sb.Append(item);
			}
		}
		_sb.AppendLine(";");
	}

	private static string GenerateLambdaToKey(IReadOnlyList<IProperty> properties, string lambdaIdentifier)
	{
		if (properties.Count > 0)
		{
			if (properties.Count != 1)
			{
				return "new { " + string.Join(", ", properties.Select((IProperty p) => lambdaIdentifier + "." + p.Name)) + " }";
			}
			return lambdaIdentifier + "." + properties[0].Name;
		}
		return "";
	}

	private static void RemoveAnnotation(ref List<IAnnotation> annotations, string annotationName)
	{
		annotations.Remove(annotations.SingleOrDefault((IAnnotation a) => a.Name == annotationName));
	}

	private IList<string> GenerateAnnotations(IEnumerable<IAnnotation> annotations)
	{
		return annotations.Select(GenerateAnnotation).ToList();
	}

	private string GenerateAnnotation(IAnnotation annotation)
	{
		return ".HasAnnotation(" + _code.Literal(annotation.Name) + ", " + _code.UnknownLiteral(annotation.Value) + ")";
	}
}
