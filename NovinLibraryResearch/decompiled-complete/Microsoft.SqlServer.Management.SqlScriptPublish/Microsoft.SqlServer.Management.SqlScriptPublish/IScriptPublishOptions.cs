using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public interface IScriptPublishOptions
{
	bool ConvertUddtToBaseType { get; }

	bool DelimitStatements { get; }

	bool GenerateAnsiPadding { get; }

	bool IncludeCollation { get; }

	bool IncludeScriptingParametersHeader { get; }

	bool IncludeHeaders { get; }

	bool IncludeIdentity { get; }

	bool IncludeIfNotExists { get; }

	bool IncludeVarDecimal { get; }

	bool SchemaQualify { get; }

	bool SchemaQualifyForeignKeys { get; }

	bool ScriptBinding { get; }

	bool ScriptChangeTracking { get; }

	bool ScriptCheckConstraints { get; }

	bool ScriptDataCompressionOptions { get; }

	bool ScriptDefaults { get; }

	bool ScriptDependentObjects { get; }

	bool ScriptExtendedProperties { get; }

	bool ScriptFileGroups { get; }

	bool ScriptForeignKeys { get; }

	bool ScriptFullTextCatalogs { get; }

	bool ScriptFullTextIndexes { get; }

	bool ScriptIndexes { get; }

	bool ScriptPartitionSchemes { get; }

	bool ScriptPermissions { get; }

	bool ScriptOwner { get; }

	bool ScriptPrimaryKeys { get; }

	bool ScriptStatistics { get; }

	bool ScriptDriIncludeSystemNames { get; }

	bool ScriptTriggers { get; }

	bool ScriptUniqueKeys { get; }

	bool ScriptUseDatabase { get; }

	bool ScriptViewColumns { get; }

	string TargetVersion { get; }

	string BatchSeparator { get; }

	string TargetDatabaseEngineType { get; }

	string TargetDatabaseEngineEdition { get; }

	bool TargetSourceServer { get; }

	ScriptingOptions GetSmoScriptingOptions();

	ScriptingOptions GetSmoScriptingOptions(object sourceObject);
}
