using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.SqlScriptPublish.SqlScriptOptionsSR")]
internal sealed class SqlTransferOptions : SqlScriptOptions
{
	private BooleanTypeOptions scriptDatabaseCreate = BooleanTypeOptions.False;

	[DisplayNameKey("ScriptDatabaseCreate")]
	[DisplayDescriptionKey("genreatescriptdatabasecreate")]
	[DisplayCategoryKey("General")]
	[Browsable(false)]
	public BooleanTypeOptions ScriptDatabaseCreate
	{
		get
		{
			return scriptDatabaseCreate;
		}
		set
		{
			scriptDatabaseCreate = value;
			if (scriptDatabaseCreate == BooleanTypeOptions.True)
			{
				ScriptUseDatabase = BooleanTypeOptions.True;
			}
		}
	}

	public SqlTransferOptions(Version version)
		: base(version)
	{
		base.GenerateScriptForDependentObjects = BooleanTypeOptions.True;
		base.ScriptIndexes = BooleanTypeOptions.True;
	}
}
