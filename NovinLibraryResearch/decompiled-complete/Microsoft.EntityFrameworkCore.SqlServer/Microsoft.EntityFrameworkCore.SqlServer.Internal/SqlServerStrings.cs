using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.SqlServer.Internal;

public static class SqlServerStrings
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.SqlServer.Properties.SqlServerStrings", typeof(SqlServerStrings).GetTypeInfo().Assembly);

	public static string IndexTableRequired => GetString("IndexTableRequired");

	public static string AlterMemoryOptimizedTable => GetString("AlterMemoryOptimizedTable");

	public static string AlterIdentityColumn => GetString("AlterIdentityColumn");

	public static string TransientExceptionDetected => GetString("TransientExceptionDetected");

	public static string NoInitialCatalog => GetString("NoInitialCatalog");

	public static string InvalidColumnNameForFreeText => GetString("InvalidColumnNameForFreeText");

	public static string IdentityBadType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("IdentityBadType", "property", "entityType", "propertyType"), property, entityType, propertyType);
	}

	public static string UnqualifiedDataType([JetBrains.Annotations.CanBeNull] object dataType)
	{
		return string.Format(GetString("UnqualifiedDataType", "dataType"), dataType);
	}

	public static string UnqualifiedDataTypeOnProperty([JetBrains.Annotations.CanBeNull] object dataType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("UnqualifiedDataTypeOnProperty", "dataType", "property"), dataType, property);
	}

	public static string SequenceBadType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("SequenceBadType", "property", "entityType", "propertyType"), property, entityType, propertyType);
	}

	public static string NonKeyValueGeneration([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NonKeyValueGeneration", "property", "entityType"), property, entityType);
	}

	public static string MultipleIdentityColumns([JetBrains.Annotations.CanBeNull] object properties, [JetBrains.Annotations.CanBeNull] object table)
	{
		return string.Format(GetString("MultipleIdentityColumns", "properties", "table"), properties, table);
	}

	public static string IncompatibleTableMemoryOptimizedMismatch([JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object memoryOptimizedEntityType, [JetBrains.Annotations.CanBeNull] object nonMemoryOptimizedEntityType)
	{
		return string.Format(GetString("IncompatibleTableMemoryOptimizedMismatch", "table", "entityType", "otherEntityType", "memoryOptimizedEntityType", "nonMemoryOptimizedEntityType"), table, entityType, otherEntityType, memoryOptimizedEntityType, nonMemoryOptimizedEntityType);
	}

	public static string DuplicateColumnNameValueGenerationStrategyMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table)
	{
		return string.Format(GetString("DuplicateColumnNameValueGenerationStrategyMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table"), entityType1, property1, entityType2, property2, columnName, table);
	}

	public static string InvalidTableToIncludeInScaffolding([JetBrains.Annotations.CanBeNull] object table)
	{
		return string.Format(GetString("InvalidTableToIncludeInScaffolding", "table"), table);
	}

	public static string IncludePropertyDuplicated([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("IncludePropertyDuplicated", "entityType", "property"), entityType, property);
	}

	public static string IncludePropertyInIndex([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("IncludePropertyInIndex", "entityType", "property"), entityType, property);
	}

	public static string IncludePropertyNotFound([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("IncludePropertyNotFound", "entityType", "property"), entityType, property);
	}

	public static string DuplicateKeyMismatchedClustering([JetBrains.Annotations.CanBeNull] object key1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object key2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object keyName)
	{
		return string.Format(GetString("DuplicateKeyMismatchedClustering", "key1", "entityType1", "key2", "entityType2", "table", "keyName"), key1, entityType1, key2, entityType2, table, keyName);
	}

	public static string FunctionOnClient([JetBrains.Annotations.CanBeNull] object methodName)
	{
		return string.Format(GetString("FunctionOnClient", "methodName"), methodName);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		for (int i = 0; i < formatterNames.Length; i++)
		{
			text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
		}
		return text;
	}
}
