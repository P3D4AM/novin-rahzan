using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone)]
public sealed class ExternalLanguage : NamedSmoObject, ICreatable, IAlterable, IDroppable, IDropIfExists, IExtendedProperties, IScriptable, ISfcSupportsDesignMode, IObjectPermission
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[3]
		{
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsSystemObject", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("Owner", expensive: false, readOnly: false, typeof(string))
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
				_ = base.DatabaseEngineEdition;
				_ = 6;
				return -1;
			}
			return propertyName switch
			{
				"ID" => 0, 
				"IsSystemObject" => 1, 
				"Owner" => 2, 
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

	private HashSet<ExternalLanguageFilePlatform> toBeRemovedPlatforms = new HashSet<ExternalLanguageFilePlatform>();

	private ExternalLanguageFileCollection m_ExternalLanguageFiles;

	public static string UrnSuffix => "ExternalLanguage";

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ExtendedProperty))]
	public ExtendedPropertyCollection ExtendedProperties
	{
		get
		{
			CheckObjectState();
			if (m_ExtendedProperties == null)
			{
				m_ExtendedProperties = new ExtendedPropertyCollection(this);
			}
			return m_ExtendedProperties;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ExternalLanguageFile))]
	public ExternalLanguageFileCollection ExternalLanguageFiles
	{
		get
		{
			CheckObjectState();
			if (m_ExternalLanguageFiles == null)
			{
				m_ExternalLanguageFiles = new ExternalLanguageFileCollection(this);
			}
			return m_ExternalLanguageFiles;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Database Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as Database;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	internal override UserPermissionCollection Permissions => GetUserPermissions();

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone, "false")]
	public bool IsSystemObject => (bool)base.Properties.GetValueWithNullReplacement("IsSystemObject");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(User), "Server[@Name = '{0}']/Database[@Name = '{1}']/User[@Name = '{2}']", new string[] { "Parent.Parent.ConnectionContext.TrueName", "Parent.Name", "Owner" })]
	[CLSCompliant(false)]
	public string Owner
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("Owner");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Owner", value);
		}
	}

	internal ExternalLanguage(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
		m_ExternalLanguageFiles = null;
	}

	internal override PropagateInfo[] GetPropagateInfo(PropagateAction action)
	{
		return new PropagateInfo[1]
		{
			new PropagateInfo(this.IsSupportedObject<ExternalLanguage>() ? null : ExtendedProperties, bWithScript: true, ExtendedProperty.UrnSuffix)
		};
	}

	public void AddFile(string fileName, byte[] contentFromBinary, ExternalLanguageFilePlatform platform = ExternalLanguageFilePlatform.Default)
	{
		AddFile(fileName, platform, contentFromBinary);
	}

	public void AddFile(string fileName, string contentFromFile, ExternalLanguageFilePlatform platform = ExternalLanguageFilePlatform.Default)
	{
		AddFile(fileName, platform, null, contentFromFile);
	}

	public void RemoveFile(ExternalLanguageFilePlatform platform)
	{
		toBeRemovedPlatforms.Add(platform);
	}

	public void Create()
	{
		CreateImpl();
	}

	protected override void PostCreate()
	{
		base.PostCreate();
		ExternalLanguageFiles.Clear();
		ExternalLanguageFiles.Refresh();
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		this.ThrowIfNotSupported(GetType(), sp);
		if (ExternalLanguageFiles.Count == 0)
		{
			throw new SmoException(ExceptionTemplatesImpl.MissingExternalLanguageFileSpec);
		}
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		new StringCollection();
		string text = FormatFullNameForScripting(sp);
		if (sp.IncludeScripts.Header)
		{
			stringBuilder.Append(ExceptionTemplates.IncludeHeader(UrnSuffix, text, DateTime.Now.ToString(GetDbCulture())));
			stringBuilder.Append(sp.NewLine);
		}
		StringBuilder stringBuilder2 = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		stringBuilder2.AppendFormat("CREATE EXTERNAL LANGUAGE {0}", text);
		stringBuilder2.Append(sp.NewLine);
		Property property;
		if (sp.IncludeScripts.Owner && (property = base.Properties.Get("Owner")).Value != null)
		{
			stringBuilder2.AppendFormat(" AUTHORIZATION [{0}]", SqlSmoObject.SqlBraket(property.Value.ToString()));
			stringBuilder2.Append(sp.NewLine);
		}
		stringBuilder2.Append(" FROM ");
		List<string> list = new List<string>();
		foreach (ExternalLanguageFile externalLanguageFile in ExternalLanguageFiles)
		{
			string text2 = externalLanguageFile.GetPropValueOptional("FileName", string.Empty);
			ExternalLanguageFilePlatform propValueOptional = externalLanguageFile.GetPropValueOptional("Platform", ExternalLanguageFilePlatform.Default);
			Property property2 = externalLanguageFile.Properties.Get("ContentFromFile");
			Property property3 = externalLanguageFile.Properties.Get("ContentFromBinary");
			string text3 = null;
			string text4 = null;
			if (base.State == SqlSmoState.Creating)
			{
				text3 = (string)property2.Value;
				text4 = ConvertBinaryToString((byte[])property3.Value);
			}
			else
			{
				text4 = externalLanguageFile.GetFileText();
				if (text2 == null)
				{
					text2 = externalLanguageFile.GetFileName();
				}
			}
			if (text3 == null && text4 == null)
			{
				throw new PropertyNotSetException("Content");
			}
			if (text2 == null)
			{
				throw new PropertyNotSetException("FileName");
			}
			list.Add(GenerateContentAndFileNameString(text3, text4, text2, propValueOptional));
		}
		stringBuilder2.Append(string.Join(", " + sp.NewLine, list.ToArray()));
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, IncludeExistsExternalLanguage(exists: false, SqlSmoObject.SqlString(GetName(sp))));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "EXEC sys.sp_executesql N'{0}'", SqlSmoObject.SqlString(stringBuilder2.ToString()));
			stringBuilder.Append(sp.NewLine);
			queries.Add(stringBuilder.ToString());
		}
		else
		{
			queries.Add(stringBuilder2.ToString());
		}
	}

	public void Drop()
	{
		DropImpl();
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	internal override void ScriptDrop(StringCollection dropQuery, ScriptingPreferences sp)
	{
		this.ThrowIfNotSupported(GetType(), sp);
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		string text = FormatFullNameForScripting(sp);
		if (sp.IncludeScripts.Header)
		{
			stringBuilder.Append(ExceptionTemplates.IncludeHeader(UrnSuffix, text, DateTime.Now.ToString(GetDbCulture())));
			stringBuilder.Append(sp.NewLine);
		}
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, IncludeExistsExternalLanguage(exists: true, GetName(sp)));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(" DROP EXTERNAL LANGUAGE {0} ", text);
		stringBuilder.Append(sp.NewLine);
		dropQuery.Add(stringBuilder.ToString());
	}

	public void Alter()
	{
		AlterImpl();
	}

	internal override void ScriptAlter(StringCollection alterQuery, ScriptingPreferences sp)
	{
		this.ThrowIfNotSupported(GetType(), sp);
		CheckObjectState();
		foreach (ExternalLanguageFilePlatform toBeRemovedPlatform in toBeRemovedPlatforms)
		{
			StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			stringBuilder.AppendFormat("ALTER EXTERNAL LANGUAGE {0}", FullQualifiedName);
			stringBuilder.Append(sp.NewLine);
			stringBuilder.AppendFormat(" REMOVE PLATFORM {0}", GetPlatformDescription(toBeRemovedPlatform));
			stringBuilder.Append(sp.NewLine);
			alterQuery.Add(stringBuilder.ToString());
		}
		foreach (ExternalLanguageFile externalLanguageFile in ExternalLanguageFiles)
		{
			if (externalLanguageFile.State != SqlSmoState.Existing || externalLanguageFile.InternalIsObjectDirty)
			{
				StringBuilder stringBuilder2 = new StringBuilder(Globals.INIT_BUFFER_SIZE);
				stringBuilder2.AppendFormat("ALTER EXTERNAL LANGUAGE {0}", FullQualifiedName);
				stringBuilder2.Append(sp.NewLine);
				Property property = externalLanguageFile.Properties.Get("ContentFromFile");
				Property property2 = externalLanguageFile.Properties.Get("ContentFromBinary");
				string propValueOptional = externalLanguageFile.GetPropValueOptional("FileName", string.Empty);
				ExternalLanguageFilePlatform propValueOptional2 = externalLanguageFile.GetPropValueOptional("Platform", ExternalLanguageFilePlatform.Default);
				string contentFromFile = null;
				string contentFromBinary = null;
				if (property.Dirty && property.Value != null && property2.Dirty && property2.Value != null)
				{
					throw new SmoException(ExceptionTemplatesImpl.BadPropertiesCombination);
				}
				if (property.Dirty && property.Value != null)
				{
					contentFromFile = (string)property.Value;
				}
				else if (property2.Dirty && property2.Value != null)
				{
					contentFromBinary = ConvertBinaryToString((byte[])property2.Value);
				}
				if (externalLanguageFile.State == SqlSmoState.Existing)
				{
					stringBuilder2.AppendFormat(" SET {0}", GenerateContentAndFileNameString(contentFromFile, contentFromBinary, propValueOptional, propValueOptional2));
				}
				else if (externalLanguageFile.State == SqlSmoState.Creating)
				{
					stringBuilder2.AppendFormat(" ADD {0}", GenerateContentAndFileNameString(contentFromFile, contentFromBinary, propValueOptional, propValueOptional2));
				}
				stringBuilder2.Append(sp.NewLine);
				alterQuery.Add(stringBuilder2.ToString());
			}
		}
	}

	protected override void PostAlter()
	{
		base.PostAlter();
		ExternalLanguageFiles.Clear();
		ExternalLanguageFiles.Refresh();
		toBeRemovedPlatforms.Clear();
	}

	private void AddFile(string fileName, ExternalLanguageFilePlatform platform, byte[] contentFromBinary = null, string contentFromFile = null)
	{
		ExternalLanguageFile externalLanguageFile = new ExternalLanguageFile(this, fileName)
		{
			FileName = fileName,
			Platform = platform
		};
		if (contentFromBinary != null)
		{
			externalLanguageFile.ContentFromBinary = contentFromBinary;
		}
		if (contentFromFile != null)
		{
			externalLanguageFile.ContentFromFile = contentFromFile;
		}
		ExternalLanguageFiles.Add(externalLanguageFile);
	}

	private static string GenerateContentAndFileNameString(string contentFromFile, string contentFromBinary, string fileName, ExternalLanguageFilePlatform platform)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		if (string.IsNullOrEmpty(contentFromFile) && contentFromBinary == null)
		{
			throw new SmoException("A file spec needs either a path to the language file or language content bits");
		}
		if (string.IsNullOrEmpty(fileName))
		{
			throw new SmoException("In a file spec, file name must be specified");
		}
		if (!string.IsNullOrEmpty(contentFromFile))
		{
			stringBuilder.AppendFormat("(content = '" + SqlSmoObject.SqlString(contentFromFile) + "', file_name = '" + SqlSmoObject.SqlString(fileName) + "'");
		}
		else if (contentFromBinary != null)
		{
			stringBuilder.AppendFormat("(content = " + contentFromBinary + ", file_name = '" + SqlSmoObject.SqlString(fileName) + "'");
		}
		if (platform != 0)
		{
			stringBuilder.AppendFormat(", platform = " + GetPlatformDescription(platform) + ")");
		}
		else
		{
			stringBuilder.AppendFormat(")");
		}
		return stringBuilder.ToString();
	}

	private static string IncludeExistsExternalLanguage(bool exists, string name)
	{
		return "IF " + (exists ? "" : "NOT") + " EXISTS (SELECT * from sys.external_languages langs WHERE langs.language = '" + SqlSmoObject.SqlString(name) + "')";
	}

	private static string GetPlatformDescription(ExternalLanguageFilePlatform platform)
	{
		return TypeDescriptor.GetConverter(typeof(ExternalLanguageFilePlatformConverter)).ConvertToInvariantString(platform);
	}

	private static string ConvertBinaryToString(byte[] fileBytes)
	{
		if (fileBytes == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(fileBytes.Length + 2);
		stringBuilder.Append("0x");
		foreach (byte b in fileBytes)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	protected override void MarkDropped()
	{
		base.MarkDropped();
		if (m_ExternalLanguageFiles != null)
		{
			m_ExternalLanguageFiles.MarkAllDropped();
		}
	}

	public override void Refresh()
	{
		ExternalLanguageFiles.Refresh();
		base.Refresh();
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		return new string[3] { "ID", "Owner", "IsSystemObject" };
	}

	public ExternalLanguage()
	{
	}

	public ExternalLanguage(Database database, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = database;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}

	internal override object GetPropertyDefaultValue(string propname)
	{
		if (propname == "IsSystemObject")
		{
			return false;
		}
		return base.GetPropertyDefaultValue(propname);
	}

	public void Deny(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Deny(ObjectPermissionSet permission, string[] granteeNames, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, granteeNames, null, grantGrant: false, cascade, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames, bool grantGrant)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames, bool grantGrant, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant, cascade: false, asRole);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames, bool revokeGrant, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, revokeGrant, cascade, null);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames, bool revokeGrant, bool cascade, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, revokeGrant, cascade, asRole);
	}

	public void Deny(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Deny(ObjectPermissionSet permission, string granteeName, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName, bool grantGrant)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName, bool grantGrant, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant, cascade: false, asRole);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName, bool revokeGrant, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, revokeGrant, cascade, null);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName, bool revokeGrant, bool cascade, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, revokeGrant, cascade, asRole);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions()
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, null, null);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(string granteeName)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, granteeName, null);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(ObjectPermissionSet permissions)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, null, permissions);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(string granteeName, ObjectPermissionSet permissions)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, granteeName, permissions);
	}
}
