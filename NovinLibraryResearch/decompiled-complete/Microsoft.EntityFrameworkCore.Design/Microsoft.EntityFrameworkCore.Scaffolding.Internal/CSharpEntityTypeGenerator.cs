using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpEntityTypeGenerator : ICSharpEntityTypeGenerator
{
	private class AttributeWriter
	{
		private readonly string _attributeName;

		private readonly List<string> _parameters = new List<string>();

		public AttributeWriter([JetBrains.Annotations.NotNull] string attributeName)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(attributeName, "attributeName");
			_attributeName = attributeName;
		}

		public void AddParameter([JetBrains.Annotations.NotNull] string parameter)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameter, "parameter");
			_parameters.Add(parameter);
		}

		public override string ToString()
		{
			return "[" + ((_parameters.Count == 0) ? StripAttribute(_attributeName) : (StripAttribute(_attributeName) + "(" + string.Join(", ", _parameters) + ")")) + "]";
		}

		private static string StripAttribute([JetBrains.Annotations.NotNull] string attributeName)
		{
			if (!attributeName.EndsWith("Attribute", StringComparison.Ordinal))
			{
				return attributeName;
			}
			return attributeName.Substring(0, attributeName.Length - 9);
		}
	}

	private readonly ICSharpHelper _code;

	private IndentedStringBuilder _sb;

	private bool _useDataAnnotations;

	public CSharpEntityTypeGenerator([JetBrains.Annotations.NotNull] ICSharpHelper cSharpHelper)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpHelper, "cSharpHelper");
		_code = cSharpHelper;
	}

	public virtual string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(@namespace, "namespace");
		_sb = new IndentedStringBuilder();
		_useDataAnnotations = useDataAnnotations;
		_sb.AppendLine("using System;");
		_sb.AppendLine("using System.Collections.Generic;");
		if (_useDataAnnotations)
		{
			_sb.AppendLine("using System.ComponentModel.DataAnnotations;");
			_sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
		}
		foreach (string item in (from ns in entityType.GetProperties().SelectMany((IProperty p) => p.ClrType.GetNamespaces())
			where ns != "System" && ns != "System.Collections.Generic"
			select ns).Distinct().OrderBy((string x) => x, new NamespaceComparer()))
		{
			_sb.AppendLine("using " + item + ";");
		}
		_sb.AppendLine();
		_sb.AppendLine("// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.");
		_sb.AppendLine("// If you have enabled NRTs for your project, then un-comment the following line:");
		_sb.AppendLine("// #nullable disable");
		_sb.AppendLine();
		_sb.AppendLine("namespace " + @namespace);
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			GenerateClass(entityType);
		}
		_sb.AppendLine("}");
		return _sb.ToString();
	}

	protected virtual void GenerateClass([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		if (_useDataAnnotations)
		{
			GenerateEntityTypeDataAnnotations(entityType);
		}
		_sb.AppendLine("public partial class " + entityType.Name);
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			GenerateConstructor(entityType);
			GenerateProperties(entityType);
			GenerateNavigationProperties(entityType);
		}
		_sb.AppendLine("}");
	}

	protected virtual void GenerateEntityTypeDataAnnotations([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		GenerateTableAttribute(entityType);
	}

	private void GenerateTableAttribute(IEntityType entityType)
	{
		string tableName = entityType.GetTableName();
		string schema = entityType.GetSchema();
		string defaultSchema = entityType.Model.GetDefaultSchema();
		bool flag = schema != null && schema != defaultSchema;
		if (entityType.FindAnnotation("Relational:ViewDefinition") == null && (flag || (tableName != null && tableName != entityType.GetDbSetName())))
		{
			AttributeWriter attributeWriter = new AttributeWriter("TableAttribute");
			attributeWriter.AddParameter(_code.Literal(tableName));
			if (flag)
			{
				attributeWriter.AddParameter("Schema = " + _code.Literal(schema));
			}
			_sb.AppendLine(attributeWriter.ToString());
		}
	}

	protected virtual void GenerateConstructor([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		List<INavigation> list = (from n in entityType.GetNavigations()
			where n.IsCollection()
			select n).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		_sb.AppendLine("public " + entityType.Name + "()");
		_sb.AppendLine("{");
		using (_sb.Indent())
		{
			foreach (INavigation item in list)
			{
				_sb.AppendLine(item.Name + " = new HashSet<" + item.GetTargetType().Name + ">();");
			}
		}
		_sb.AppendLine("}");
		_sb.AppendLine();
	}

	protected virtual void GenerateProperties([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		foreach (IProperty item in from p in entityType.GetProperties()
			orderby p.GetColumnOrdinal()
			select p)
		{
			if (_useDataAnnotations)
			{
				GeneratePropertyDataAnnotations(item);
			}
			_sb.AppendLine("public " + _code.Reference(item.ClrType) + " " + item.Name + " { get; set; }");
		}
	}

	protected virtual void GeneratePropertyDataAnnotations([JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		GenerateKeyAttribute(property);
		GenerateRequiredAttribute(property);
		GenerateColumnAttribute(property);
		GenerateMaxLengthAttribute(property);
	}

	private void GenerateKeyAttribute(IProperty property)
	{
		if (property.FindContainingPrimaryKey() != null)
		{
			_sb.AppendLine(new AttributeWriter("KeyAttribute"));
		}
	}

	private void GenerateColumnAttribute(IProperty property)
	{
		string columnName = property.GetColumnName();
		string configuredColumnType = property.GetConfiguredColumnType();
		string text = ((columnName != null && columnName != property.Name) ? _code.Literal(columnName) : null);
		string text2 = ((configuredColumnType != null) ? _code.Literal(configuredColumnType) : null);
		if ((text ?? text2) != null)
		{
			AttributeWriter attributeWriter = new AttributeWriter("ColumnAttribute");
			if (text != null)
			{
				attributeWriter.AddParameter(text);
			}
			if (text2 != null)
			{
				attributeWriter.AddParameter("TypeName = " + text2);
			}
			_sb.AppendLine(attributeWriter);
		}
	}

	private void GenerateMaxLengthAttribute(IProperty property)
	{
		int? maxLength = property.GetMaxLength();
		if (maxLength.HasValue)
		{
			AttributeWriter attributeWriter = new AttributeWriter((property.ClrType == typeof(string)) ? "StringLengthAttribute" : "MaxLengthAttribute");
			attributeWriter.AddParameter(_code.Literal(maxLength.Value));
			_sb.AppendLine(attributeWriter.ToString());
		}
	}

	private void GenerateRequiredAttribute(IProperty property)
	{
		if (!property.IsNullable && property.ClrType.IsNullableType() && !property.IsPrimaryKey())
		{
			_sb.AppendLine(new AttributeWriter("RequiredAttribute").ToString());
		}
	}

	protected virtual void GenerateNavigationProperties([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		List<INavigation> list = (from n in entityType.GetNavigations()
			orderby (!n.IsDependentToPrincipal()) ? 1 : 0, n.IsCollection() ? 1 : 0
			select n).ToList();
		if (!list.Any())
		{
			return;
		}
		_sb.AppendLine();
		foreach (INavigation item in list)
		{
			if (_useDataAnnotations)
			{
				GenerateNavigationDataAnnotations(item);
			}
			string name = item.GetTargetType().Name;
			string text = (item.IsCollection() ? ("ICollection<" + name + ">") : name);
			_sb.AppendLine("public virtual " + text + " " + item.Name + " { get; set; }");
		}
	}

	private void GenerateNavigationDataAnnotations(INavigation navigation)
	{
		GenerateForeignKeyAttribute(navigation);
		GenerateInversePropertyAttribute(navigation);
	}

	private void GenerateForeignKeyAttribute(INavigation navigation)
	{
		if (!navigation.IsDependentToPrincipal() || !navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
		{
			return;
		}
		AttributeWriter attributeWriter = new AttributeWriter("ForeignKeyAttribute");
		if (navigation.ForeignKey.Properties.Count > 1)
		{
			attributeWriter.AddParameter(_code.Literal(string.Join(",", navigation.ForeignKey.Properties.Select((IProperty p) => p.Name))));
		}
		else
		{
			attributeWriter.AddParameter("nameof(" + navigation.ForeignKey.Properties.First().Name + ")");
		}
		_sb.AppendLine(attributeWriter.ToString());
	}

	private void GenerateInversePropertyAttribute(INavigation navigation)
	{
		if (!navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
		{
			return;
		}
		INavigation inverseNavigation = navigation.FindInverse();
		if (inverseNavigation != null)
		{
			AttributeWriter attributeWriter = new AttributeWriter("InversePropertyAttribute");
			attributeWriter.AddParameter((!navigation.DeclaringEntityType.GetPropertiesAndNavigations().Any((IPropertyBase m) => m.Name == inverseNavigation.DeclaringEntityType.Name)) ? ("nameof(" + inverseNavigation.DeclaringEntityType.Name + "." + inverseNavigation.Name + ")") : _code.Literal(inverseNavigation.Name));
			_sb.AppendLine(attributeWriter.ToString());
		}
	}
}
