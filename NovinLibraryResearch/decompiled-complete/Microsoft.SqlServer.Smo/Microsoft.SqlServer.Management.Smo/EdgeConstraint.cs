using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone | SfcElementFlags.SqlAzureDatabase)]
public sealed class EdgeConstraint : ScriptNameObjectBase, ICreatable, IDroppable, IDropIfExists, IRenamable, IMarkForDrop, IScriptable, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 };

		private static int[] cloudVersionCount = new int[3] { 0, 0, 5 };

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[5]
		{
			new StaticMetadata("CreateDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DateLastModified", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DeleteAction", expensive: false, readOnly: false, typeof(EdgeConstraintDeleteAction)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsSystemNamed", expensive: false, readOnly: true, typeof(bool))
		};

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[5]
		{
			new StaticMetadata("CreateDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DateLastModified", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DeleteAction", expensive: false, readOnly: false, typeof(EdgeConstraintDeleteAction)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsSystemNamed", expensive: false, readOnly: true, typeof(bool))
		};

		public override int Count
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return sqlDwPropertyCount;
					}
					int num = ((currentVersionIndex < cloudVersionCount.Length) ? currentVersionIndex : (cloudVersionCount.Length - 1));
					return cloudVersionCount[num];
				}
				int num2 = ((currentVersionIndex < versionCount.Length) ? currentVersionIndex : (versionCount.Length - 1));
				return versionCount[num2];
			}
		}

		protected override int[] VersionCount
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return new int[1] { sqlDwPropertyCount };
					}
					return cloudVersionCount;
				}
				return versionCount;
			}
		}

		internal PropertyMetadataProvider(ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
			: base(version, databaseEngineType, databaseEngineEdition)
		{
		}

		public override int PropertyNameToIDLookup(string propertyName)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return -1;
				}
				return propertyName switch
				{
					"CreateDate" => 0, 
					"DateLastModified" => 1, 
					"DeleteAction" => 2, 
					"ID" => 3, 
					"IsSystemNamed" => 4, 
					_ => -1, 
				};
			}
			return propertyName switch
			{
				"CreateDate" => 0, 
				"DateLastModified" => 1, 
				"DeleteAction" => 2, 
				"ID" => 3, 
				"IsSystemNamed" => 4, 
				_ => -1, 
			};
		}

		internal new static int[] GetVersionArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return new int[1] { sqlDwPropertyCount };
				}
				return cloudVersionCount;
			}
			return versionCount;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata[id];
				}
				return cloudStaticMetadata[id];
			}
			return staticMetadata[id];
		}

		internal new static StaticMetadata[] GetStaticMetadataArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata;
				}
				return cloudStaticMetadata;
			}
			return staticMetadata;
		}
	}

	private const string EDGE_CONSTRAINT_URN = "EdgeConstraint";

	private const string DELETE_ACTION = "DeleteAction";

	private EdgeConstraintClauseCollection m_EdgeConstraintClauses;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(EdgeConstraintClause), SfcObjectFlags.Design)]
	public EdgeConstraintClauseCollection EdgeConstraintClauses
	{
		get
		{
			CheckObjectState();
			if (m_EdgeConstraintClauses == null)
			{
				m_EdgeConstraintClauses = new EdgeConstraintClauseCollection(this);
			}
			return m_EdgeConstraintClauses;
		}
	}

	public static string UrnSuffix => "EdgeConstraint";

	[SfcKey(0)]
	[SfcProperty(SfcPropertyFlags.ReadOnlyAfterCreation | SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Design)]
	public override string Name
	{
		get
		{
			if (!base.IsDesignMode || !GetIsSystemNamed() || base.State != SqlSmoState.Creating)
			{
				return base.Name;
			}
			return null;
		}
		set
		{
			base.Name = value;
			if (base.ParentColl != null)
			{
				SetIsSystemNamed(flag: false);
			}
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public bool IsSystemNamed
	{
		get
		{
			if (base.ParentColl != null && base.IsDesignMode && base.State != SqlSmoState.Existing)
			{
				throw new PropertyNotSetException("IsSystemNamed");
			}
			return (bool)base.Properties.GetValueWithNullReplacement("IsSystemNamed");
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectFlags.Design)]
	[SfcParent("Table")]
	[SfcParent("UserDefinedTableType")]
	public SqlSmoObject Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public DateTime CreateDate => (DateTime)base.Properties.GetValueWithNullReplacement("CreateDate");

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public DateTime DateLastModified => (DateTime)base.Properties.GetValueWithNullReplacement("DateLastModified");

	[SfcProperty(SfcPropertyFlags.ReadOnlyAfterCreation | SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Design)]
	public EdgeConstraintDeleteAction DeleteAction
	{
		get
		{
			return (EdgeConstraintDeleteAction)base.Properties.GetValueWithNullReplacement("DeleteAction");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("DeleteAction", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	internal EdgeConstraint(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
		m_EdgeConstraintClauses = null;
	}

	internal override void UpdateObjectState()
	{
		if (base.State == SqlSmoState.Pending && base.ParentColl != null && (!key.IsNull || base.IsDesignMode))
		{
			SetState(SqlSmoState.Creating);
			if (key.IsNull)
			{
				AutoGenerateName();
			}
			else
			{
				SetIsSystemNamed(flag: false);
			}
		}
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	public void Create()
	{
		ThrowIfParentIsNotEdgeTable();
		CreateImpl();
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		ThrowIfParentIsNotEdgeTable();
		this.ThrowIfNotSupported(GetType(), sp);
		queries.Add(ScriptWithOption(sp, ScriptFlag.CREATE));
	}

	internal string ScriptWithOption(ScriptingPreferences sp, ScriptFlag option)
	{
		if (option == ScriptFlag.DROP)
		{
			return string.Format(SmoApplication.DefaultCulture, "ALTER TABLE {0} DROP CONSTRAINT {1}", Parent.FullQualifiedName, FullQualifiedName);
		}
		return string.Format(SmoApplication.DefaultCulture, "ALTER TABLE {0} ADD{1}", Parent.FullQualifiedName, ScriptDdlBodyWorker(sp));
	}

	internal string ScriptDdlBody(ScriptingPreferences sp)
	{
		ThrowIfParentIsNotEdgeTable();
		return ScriptDdlBodyWorker(sp);
	}

	private string ScriptDeleteReferentialAction()
	{
		EdgeConstraintDeleteAction propValueOptional = GetPropValueOptional("DeleteAction", EdgeConstraintDeleteAction.NoAction);
		return propValueOptional switch
		{
			EdgeConstraintDeleteAction.NoAction => string.Empty, 
			EdgeConstraintDeleteAction.Cascade => " ON DELETE CASCADE", 
			_ => throw new InvalidSmoOperationException(ExceptionTemplatesImpl.InvalidEdgeConstraintReferentialAction(propValueOptional.ToString())), 
		};
	}

	private string ScriptDdlBodyWorker(ScriptingPreferences sp, bool withConstraintName = true)
	{
		CheckObjectState();
		if (EdgeConstraintClauses.Count < 1)
		{
			throw new InvalidSmoOperationException(ExceptionTemplatesImpl.InvalidEdgeConstraintDefinition);
		}
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		AddConstraintName(stringBuilder, sp);
		string arg = ScriptEdgeConstraintClauses();
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "CONNECTION ({0})", arg);
		stringBuilder.Append(ScriptDeleteReferentialAction());
		return stringBuilder.ToString();
	}

	private string ScriptEdgeConstraintClauses()
	{
		return string.Join(",", from EdgeConstraintClause clause in EdgeConstraintClauses
			select $"{SqlSmoObject.MakeSqlBraket(clause.From)} To {SqlSmoObject.MakeSqlBraket(clause.To)}");
	}

	public void Rename(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			throw new InvalidSmoOperationException(ExceptionTemplatesImpl.EmptyConstraintName);
		}
		RenameImpl(newName);
	}

	internal override void ScriptRename(StringCollection renameQuery, ScriptingPreferences sp, string newName)
	{
		this.ThrowIfNotSupported(GetType(), sp);
		renameQuery.Add(string.Format(SmoApplication.DefaultCulture, "EXEC {0}.dbo.sp_rename @objname = N'{1}.{2}', @newname = N'{3}', @objtype = N'OBJECT'", SqlSmoObject.MakeSqlBraket(GetDBName()), SqlSmoObject.SqlString(SqlSmoObject.MakeSqlBraket(((ScriptSchemaObjectBase)base.ParentColl.ParentInstance).Schema)), SqlSmoObject.SqlString(FullQualifiedName), SqlSmoObject.SqlString(newName)));
	}

	public void Drop()
	{
		DropImpl();
	}

	private void ThrowIfParentIsNotEdgeTable()
	{
		if (!(Parent is Table { IsEdge: not false }))
		{
			throw new InvalidSmoOperationException(ExceptionTemplatesImpl.InvalidEdgeConstraintParent);
		}
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		this.ThrowIfNotSupported(GetType(), sp);
		CheckObjectState();
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		VersionUtils.IsTargetServerVersionSQl13OrLater(sp.TargetServerVersionInternal);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			string s = ((TableViewBase)base.ParentColl.ParentInstance).FormatFullNameForScripting(sp);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_TABLE90, string.Empty, SqlSmoObject.SqlString(s));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER TABLE {0} DROP CONSTRAINT {1}{2}", ((ScriptSchemaObjectBase)base.ParentColl.ParentInstance).FormatFullNameForScripting(sp), sp.IncludeScripts.ExistenceCheck ? "IF EXISTS " : string.Empty, FormatFullNameForScripting(sp));
		queries.Add(stringBuilder.ToString());
	}

	public void MarkForDrop(bool dropOnAlter)
	{
		MarkForDropImpl(dropOnAlter);
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		string[] fields = new string[2] { "DeleteAction", "IsSystemNamed" };
		return SqlSmoObject.GetSupportedScriptFields(typeof(PropertyMetadataProvider), fields, version, databaseEngineType, databaseEngineEdition).ToArray();
	}

	public EdgeConstraint()
	{
	}

	public EdgeConstraint(SqlSmoObject parent, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = parent;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}

	internal override string[] GetNonAlterableProperties()
	{
		return new string[1] { "DeleteAction" };
	}
}
