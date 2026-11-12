using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class SerializationVisitor : IMetadataObjectVisitor<SerializationInfo>, IDatabaseObjectVisitor<SerializationInfo>, IServerOwnedObjectVisitor<SerializationInfo>, IDatabaseOwnedObjectVisitor<SerializationInfo>, ISchemaOwnedObjectVisitor<SerializationInfo>
{
	private sealed class ApplicationRoleSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ApplicationRoleSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(15);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IApplicationRole)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IApplicationRole obj)
		{
			return new TId[6]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.DefaultSchema, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.MemberOfRoles, obj, isChild: false),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Permissions, obj, isChild: true)
			};
		}
	}

	private sealed class AsymmetricKeySerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public AsymmetricKeySerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(16);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IAsymmetricKey)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IAsymmetricKey obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class CertificateSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CertificateSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(18);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICertificate)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICertificate obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class CheckConstraintSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CheckConstraintSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(19);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICheckConstraint)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICheckConstraint obj)
		{
			return new TId[7]
			{
				serializer.RegisterValue(obj.IsChecked),
				serializer.RegisterValue(obj.IsEnabled),
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NotForReplication),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.Text)
			};
		}
	}

	private sealed class ClrDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ClrDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(20);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IClrDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IClrDataType obj)
		{
			return new TId[1] { serializer.RegisterValue(obj.Name) };
		}
	}

	private sealed class CollationSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CollationSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(21);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICollation)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICollation obj)
		{
			return new TId[1] { serializer.RegisterValue(obj.Name) };
		}
	}

	private sealed class ColumnSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ColumnSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(22);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IColumn)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IColumn obj)
		{
			return new TId[18]
			{
				serializer.RegisterValue(obj.Collation, obj, isChild: false),
				serializer.RegisterValue(obj.ComputedColumnInfo),
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.DefaultValue, obj, isChild: true),
				serializer.RegisterValue(obj.IdentityColumnInfo),
				serializer.RegisterValue(obj.InPrimaryKey),
				serializer.RegisterValue(obj.IsColumnSet),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsRowEnd),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsRowStart),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsSequenceNumberEnd),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsSequenceNumberStart),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsTransactionIdEnd),
				serializer.RegisterValue(obj.IsGeneratedAlwaysAsTransactionIdStart),
				serializer.RegisterValue(obj.IsSparse),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.RowGuidCol)
			};
		}
	}

	private sealed class CredentialSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CredentialSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(24);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICredential)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICredential obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Server, obj, isChild: false)
			};
		}
	}

	private sealed class CursorDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CursorDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(25);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICursorDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICursorDataType obj)
		{
			return new TId[0];
		}
	}

	private sealed class CursorParameterSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CursorParameterSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(26);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICursorParameter)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICursorParameter obj)
		{
			return new TId[1] { serializer.RegisterValue(obj.Name) };
		}
	}

	private sealed class CursorVariableSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public CursorVariableSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(27);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ICursorVariable)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ICursorVariable obj)
		{
			return new TId[1] { serializer.RegisterValue(obj.Name) };
		}
	}

	private sealed class DatabaseSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DatabaseSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(29);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDatabase)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDatabase obj)
		{
			return new TId[14]
			{
				serializer.RegisterValue(obj.ApplicationRoles, obj, isChild: true),
				serializer.RegisterValue(obj.AsymmetricKeys, obj, isChild: true),
				serializer.RegisterValue(obj.Certificates, obj, isChild: true),
				serializer.RegisterValue(obj.CollationInfo),
				serializer.RegisterValue(obj.CompatibilityLevel),
				serializer.RegisterValue(obj.DefaultSchemaName),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Owner, obj, isChild: false),
				serializer.RegisterValue(obj.Roles, obj, isChild: true),
				serializer.RegisterValue(obj.Schemas, obj, isChild: true),
				serializer.RegisterValue(obj.Server, obj, isChild: false),
				serializer.RegisterValue(obj.Triggers, obj, isChild: true),
				serializer.RegisterValue(obj.Users, obj, isChild: true)
			};
		}
	}

	private sealed class DatabaseDdlTriggerSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DatabaseDdlTriggerSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(30);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDatabaseDdlTrigger)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDatabaseDdlTrigger obj)
		{
			return new TId[10]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.DatabaseDdlEvents),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.IsEnabled),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class DatabasePermissionSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DatabasePermissionSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(33);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDatabasePermission)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDatabasePermission obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.DatabasePrincipal, obj, isChild: false),
				serializer.RegisterValue(obj.Grantor, obj, isChild: false),
				serializer.RegisterValue(obj.PermissionState),
				serializer.RegisterValue(obj.PermissionType),
				serializer.RegisterValue(obj.TargetObject, obj, isChild: false)
			};
		}
	}

	private sealed class DatabaseRoleSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DatabaseRoleSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(35);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDatabaseRole)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDatabaseRole obj)
		{
			return new TId[7]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.IsFixedRole),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.MemberOfRoles, obj, isChild: false),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Owner, obj, isChild: false),
				serializer.RegisterValue(obj.Permissions, obj, isChild: true)
			};
		}
	}

	private sealed class DefaultConstraintSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DefaultConstraintSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(37);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDefaultConstraint)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDefaultConstraint obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.Text)
			};
		}
	}

	private sealed class DmlTriggerSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public DmlTriggerSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(38);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IDmlTrigger)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IDmlTrigger obj)
		{
			return new TId[16]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.Delete),
				serializer.RegisterValue(obj.DeleteActivationOrder),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.Insert),
				serializer.RegisterValue(obj.InsertActivationOrder),
				serializer.RegisterValue(obj.InsteadOf),
				serializer.RegisterValue(obj.IsEnabled),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NotForReplication),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.Update),
				serializer.RegisterValue(obj.UpdateActivationOrder)
			};
		}
	}

	private sealed class ExecutionContextSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ExecutionContextSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(39);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IExecutionContext)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IExecutionContext obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.ContextType),
				serializer.RegisterValue(obj.Login, obj, isChild: false),
				serializer.RegisterValue(obj.User, obj, isChild: false)
			};
		}
	}

	private sealed class ExtendedStoredProcedureSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ExtendedStoredProcedureSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(40);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IExtendedStoredProcedure)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IExtendedStoredProcedure obj)
		{
			return new TId[7]
			{
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.ReturnType, obj, isChild: false),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class FileGroupSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public FileGroupSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(41);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IFileGroup)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IFileGroup obj)
		{
			return new TId[6]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.IsDefault),
				serializer.RegisterValue(obj.IsFileStream),
				serializer.RegisterValue(obj.IsReadOnly),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class ForeignKeyColumnSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ForeignKeyColumnSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(42);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IForeignKeyColumn)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IForeignKeyColumn obj)
		{
			return new TId[2]
			{
				serializer.RegisterValue(obj.ReferencedColumn, obj, isChild: false),
				serializer.RegisterValue(obj.ReferencingColumn, obj, isChild: false)
			};
		}
	}

	private sealed class ForeignKeyConstraintSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ForeignKeyConstraintSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(43);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IForeignKeyConstraint)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IForeignKeyConstraint obj)
		{
			return new TId[10]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: false),
				serializer.RegisterValue(obj.DeleteAction),
				serializer.RegisterValue(obj.IsChecked),
				serializer.RegisterValue(obj.IsEnabled),
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NotForReplication),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.ReferencedTable, obj, isChild: false),
				serializer.RegisterValue(obj.UpdateAction)
			};
		}
	}

	private sealed class IndexedColumnSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public IndexedColumnSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(47);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IIndexedColumn)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IIndexedColumn obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.IsIncluded),
				serializer.RegisterValue(obj.ReferencedColumn, obj, isChild: false),
				serializer.RegisterValue(obj.SortOrder)
			};
		}
	}

	private sealed class LoginSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public LoginSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(49);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ILogin)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ILogin obj)
		{
			return new TId[11]
			{
				serializer.RegisterValue(obj.AsymmetricKey, obj, isChild: false),
				serializer.RegisterValue(obj.Certificate, obj, isChild: false),
				serializer.RegisterValue(obj.Credential, obj, isChild: false),
				serializer.RegisterValue(obj.DefaultDatabase, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Language),
				serializer.RegisterValue(obj.LoginType),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Password),
				serializer.RegisterValue(obj.Server, obj, isChild: false),
				serializer.RegisterValue(obj.Sid)
			};
		}
	}

	private sealed class PartitionSchemeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public PartitionSchemeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(88);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IPartitionScheme)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IPartitionScheme obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.FileGroups, obj, isChild: true),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class PrimaryKeyConstraintSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public PrimaryKeyConstraintSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(90);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IPrimaryKeyConstraint)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IPrimaryKeyConstraint obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.AssociatedIndex, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parent, obj, isChild: false)
			};
		}
	}

	private sealed class RelationalIndexSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public RelationalIndexSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(91);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IRelationalIndex)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IRelationalIndex obj)
		{
			return new TId[20]
			{
				serializer.RegisterValue(obj.CompactLargeObjects),
				serializer.RegisterValue(obj.DisallowPageLocks),
				serializer.RegisterValue(obj.DisallowRowLocks),
				serializer.RegisterValue(obj.FileGroup, obj, isChild: true),
				serializer.RegisterValue(obj.FileStreamFileGroup, obj, isChild: true),
				serializer.RegisterValue(obj.FileStreamPartitionScheme, obj, isChild: true),
				serializer.RegisterValue(obj.FillFactor),
				serializer.RegisterValue(obj.FilterDefinition),
				serializer.RegisterValue(obj.IgnoreDuplicateKeys),
				serializer.RegisterValue(obj.IndexKey, obj, isChild: false),
				serializer.RegisterValue(obj.IndexedColumns, obj, isChild: false),
				serializer.RegisterValue(obj.IsClustered),
				serializer.RegisterValue(obj.IsDisabled),
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.IsUnique),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NoAutomaticRecomputation),
				serializer.RegisterValue(obj.PadIndex),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.PartitionScheme, obj, isChild: true)
			};
		}
	}

	private sealed class ScalarDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ScalarDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(93);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IScalarDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IScalarDataType obj)
		{
			return new TId[1] { serializer.RegisterValue(obj.Name) };
		}
	}

	private sealed class ScalarParameterSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ScalarParameterSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(95);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IScalarParameter)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IScalarParameter obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.DefaultValue),
				serializer.RegisterValue(obj.IsOutput),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable)
			};
		}
	}

	private sealed class ScalarValuedFunctionSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ScalarValuedFunctionSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(96);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IScalarValuedFunction)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IScalarValuedFunction obj)
		{
			return new TId[13]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSchemaBound),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.ReturnsNullOnNullInput),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class ScalarVariableSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ScalarVariableSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(97);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IScalarVariable)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IScalarVariable obj)
		{
			return new TId[3]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable)
			};
		}
	}

	private sealed class SchemaSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public SchemaSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(98);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ISchema)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ISchema obj)
		{
			return new TId[15]
			{
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.ExtendedStoredProcedures, obj, isChild: true),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Owner, obj, isChild: false),
				serializer.RegisterValue(obj.ScalarValuedFunctions, obj, isChild: true),
				serializer.RegisterValue(obj.StoredProcedures, obj, isChild: true),
				serializer.RegisterValue(obj.Synonyms, obj, isChild: true),
				serializer.RegisterValue(obj.TableValuedFunctions, obj, isChild: true),
				serializer.RegisterValue(obj.Tables, obj, isChild: true),
				serializer.RegisterValue(obj.UserDefinedAggregates, obj, isChild: true),
				serializer.RegisterValue(obj.UserDefinedClrTypes, obj, isChild: true),
				serializer.RegisterValue(obj.UserDefinedDataTypes, obj, isChild: true),
				serializer.RegisterValue(obj.UserDefinedTableTypes, obj, isChild: true),
				serializer.RegisterValue(obj.Views, obj, isChild: true)
			};
		}
	}

	private sealed class ServerSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ServerSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(100);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IServer)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IServer obj)
		{
			return new TId[7]
			{
				serializer.RegisterValue(obj.CollationInfo),
				serializer.RegisterValue(obj.Credentials, obj, isChild: true),
				serializer.RegisterValue(obj.Databases, obj, isChild: true),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Logins, obj, isChild: true),
				serializer.RegisterValue(((options & 1) == 1) ? "" : obj.Name),
				serializer.RegisterValue(obj.Triggers, obj, isChild: true)
			};
		}
	}

	private sealed class ServerDdlTriggerSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ServerDdlTriggerSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(101);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IServerDdlTrigger)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IServerDdlTrigger obj)
		{
			return new TId[10]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.IsEnabled),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Server, obj, isChild: false),
				serializer.RegisterValue(obj.ServerDdlEvents)
			};
		}
	}

	private sealed class SpatialIndexSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public SpatialIndexSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(103);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ISpatialIndex)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ISpatialIndex obj)
		{
			return new TId[19]
			{
				serializer.RegisterValue(obj.BoundingBoxXMax),
				serializer.RegisterValue(obj.BoundingBoxXMin),
				serializer.RegisterValue(obj.BoundingBoxYMax),
				serializer.RegisterValue(obj.BoundingBoxYMin),
				serializer.RegisterValue(obj.CellsPerObject),
				serializer.RegisterValue(obj.DisallowPageLocks),
				serializer.RegisterValue(obj.DisallowRowLocks),
				serializer.RegisterValue(obj.FillFactor),
				serializer.RegisterValue(obj.IgnoreDuplicateKeys),
				serializer.RegisterValue(obj.IndexedColumn, obj, isChild: false),
				serializer.RegisterValue(obj.IsDisabled),
				serializer.RegisterValue(obj.Level1Density),
				serializer.RegisterValue(obj.Level2Density),
				serializer.RegisterValue(obj.Level3Density),
				serializer.RegisterValue(obj.Level4Density),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NoAutomaticRecomputation),
				serializer.RegisterValue(obj.PadIndex),
				serializer.RegisterValue(obj.Parent, obj, isChild: false)
			};
		}
	}

	private sealed class StatisticsSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public StatisticsSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(104);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IStatistics)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IStatistics obj)
		{
			return new TId[6]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: false),
				serializer.RegisterValue(obj.FilterDefinition),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.NoAutomaticRecomputation),
				serializer.RegisterValue(obj.Parent, obj, isChild: false),
				serializer.RegisterValue(obj.Type)
			};
		}
	}

	private sealed class StoredProcedureSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public StoredProcedureSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(105);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IStoredProcedure)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IStoredProcedure obj)
		{
			return new TId[12]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.ForReplication),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsRecompiled),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.Schema, obj, isChild: false),
				serializer.RegisterValue(obj.Startup)
			};
		}
	}

	private sealed class SynonymSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public SynonymSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(106);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ISynonym)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ISynonym obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.BaseObjectName),
				serializer.RegisterValue(obj.BaseType),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class SystemClrDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public SystemClrDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(107);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ISystemClrDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ISystemClrDataType obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.Length),
				serializer.RegisterValue(obj.NumericPrecision),
				serializer.RegisterValue(obj.NumericScale),
				serializer.RegisterValue(obj.TypeSpec)
			};
		}
	}

	private sealed class SystemDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public SystemDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(108);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ISystemDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ISystemDataType obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.Length),
				serializer.RegisterValue(obj.NumericPrecision),
				serializer.RegisterValue(obj.NumericScale),
				serializer.RegisterValue(obj.TypeSpec)
			};
		}
	}

	private sealed class TableSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public TableSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(109);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ITable)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ITable obj)
		{
			return new TId[9]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: true),
				serializer.RegisterValue(obj.Constraints, obj, isChild: true),
				serializer.RegisterValue(obj.Indexes, obj, isChild: true),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Schema, obj, isChild: false),
				serializer.RegisterValue(obj.Statistics, obj, isChild: true),
				serializer.RegisterValue(obj.Triggers, obj, isChild: true)
			};
		}
	}

	private sealed class TableDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public TableDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(110);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ITableDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ITableDataType obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: true),
				serializer.RegisterValue(obj.Constraints, obj, isChild: true),
				serializer.RegisterValue(obj.Indexes, obj, isChild: true),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Statistics, obj, isChild: true)
			};
		}
	}

	private sealed class TableParameterSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public TableParameterSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(111);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ITableParameter)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ITableParameter obj)
		{
			return new TId[2]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class TableValuedFunctionSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public TableValuedFunctionSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(112);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ITableValuedFunction)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ITableValuedFunction obj)
		{
			return new TId[16]
			{
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.BodyText),
				serializer.RegisterValue(obj.Columns, obj, isChild: true),
				serializer.RegisterValue(obj.Constraints, obj, isChild: true),
				serializer.RegisterValue(obj.ExecutionContext, obj, isChild: false),
				serializer.RegisterValue(obj.Indexes, obj, isChild: true),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsInline),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSchemaBound),
				serializer.RegisterValue(obj.IsSqlClr),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.Schema, obj, isChild: false),
				serializer.RegisterValue(obj.Statistics, obj, isChild: true),
				serializer.RegisterValue(obj.TableVariableName)
			};
		}
	}

	private sealed class TableVariableSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public TableVariableSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(113);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (ITableVariable)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, ITableVariable obj)
		{
			return new TId[2]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.Name)
			};
		}
	}

	private sealed class UdtDataMemberSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UdtDataMemberSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(118);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUdtDataMember)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUdtDataMember obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.IsStatic),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.UserDefinedType, obj, isChild: false)
			};
		}
	}

	private sealed class UdtMethodSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UdtMethodSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(120);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUdtMethod)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUdtMethod obj)
		{
			return new TId[6]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.IsStatic),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.UserDefinedType, obj, isChild: false)
			};
		}
	}

	private sealed class UniqueConstraintSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UniqueConstraintSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(121);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUniqueConstraint)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUniqueConstraint obj)
		{
			return new TId[4]
			{
				serializer.RegisterValue(obj.AssociatedIndex, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemNamed),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Parent, obj, isChild: false)
			};
		}
	}

	private sealed class UserSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UserSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(123);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUser)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUser obj)
		{
			return new TId[11]
			{
				serializer.RegisterValue(obj.AsymmetricKey, obj, isChild: false),
				serializer.RegisterValue(obj.Certificate, obj, isChild: false),
				serializer.RegisterValue(obj.Database, obj, isChild: false),
				serializer.RegisterValue(obj.DefaultSchema, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Login, obj, isChild: false),
				serializer.RegisterValue(obj.MemberOfRoles, obj, isChild: false),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Password),
				serializer.RegisterValue(obj.Permissions, obj, isChild: true),
				serializer.RegisterValue(obj.UserType)
			};
		}
	}

	private sealed class UserDefinedAggregateSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UserDefinedAggregateSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(124);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUserDefinedAggregate)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUserDefinedAggregate obj)
		{
			return new TId[6]
			{
				serializer.RegisterValue(obj.DataType, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.Parameters, obj, isChild: false),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class UserDefinedClrTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UserDefinedClrTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(125);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUserDefinedClrType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUserDefinedClrType obj)
		{
			return new TId[10]
			{
				serializer.RegisterValue(obj.AssemblyName),
				serializer.RegisterValue(obj.ClassName),
				serializer.RegisterValue(obj.DataMembers, obj, isChild: false),
				serializer.RegisterValue(obj.IsBinaryOrdered),
				serializer.RegisterValue(obj.IsComVisible),
				serializer.RegisterValue(obj.IsNullable),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Methods, obj, isChild: false),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class UserDefinedDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UserDefinedDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(126);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUserDefinedDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUserDefinedDataType obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.BaseSystemDataType, obj, isChild: false),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Nullable),
				serializer.RegisterValue(obj.Schema, obj, isChild: false)
			};
		}
	}

	private sealed class UserDefinedTableTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public UserDefinedTableTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(129);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IUserDefinedTableType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IUserDefinedTableType obj)
		{
			return new TId[7]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: true),
				serializer.RegisterValue(obj.Constraints, obj, isChild: true),
				serializer.RegisterValue(obj.Indexes, obj, isChild: true),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.Schema, obj, isChild: false),
				serializer.RegisterValue(obj.Statistics, obj, isChild: true)
			};
		}
	}

	private sealed class ViewSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public ViewSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(131);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IView)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IView obj)
		{
			return new TId[15]
			{
				serializer.RegisterValue(obj.Columns, obj, isChild: true),
				serializer.RegisterValue(obj.Constraints, obj, isChild: true),
				serializer.RegisterValue(obj.HasCheckOption),
				serializer.RegisterValue(obj.HasColumnSpecification),
				serializer.RegisterValue(obj.Indexes, obj, isChild: true),
				serializer.RegisterValue(obj.IsEncrypted),
				serializer.RegisterValue(obj.IsQuotedIdentifierOn),
				serializer.RegisterValue(obj.IsSchemaBound),
				serializer.RegisterValue(obj.IsSystemObject),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(((options & 2) == 2) ? "" : obj.QueryText),
				serializer.RegisterValue(obj.ReturnsViewMetadata),
				serializer.RegisterValue(obj.Schema, obj, isChild: false),
				serializer.RegisterValue(obj.Statistics, obj, isChild: true),
				serializer.RegisterValue(obj.Triggers, obj, isChild: true)
			};
		}
	}

	private sealed class VoidDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public VoidDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(132);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IVoidDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IVoidDataType obj)
		{
			return new TId[0];
		}
	}

	private sealed class XmlDataTypeSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public XmlDataTypeSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(133);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IXmlDataType)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IXmlDataType obj)
		{
			return new TId[5]
			{
				serializer.RegisterValue(obj.IsXmlNode),
				serializer.RegisterValue(obj.Length),
				serializer.RegisterValue(obj.NumericPrecision),
				serializer.RegisterValue(obj.NumericScale),
				serializer.RegisterValue(obj.TypeSpec)
			};
		}
	}

	private sealed class XmlIndexSerializationInfo : SerializationInfo
	{
		private readonly MetadataTypeInfo typeInfo;

		private readonly int options;

		public override MetadataTypeInfo TypeInfo => typeInfo;

		public XmlIndexSerializationInfo(SerializationOptions options)
		{
			this.options = (int)options;
			typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(136);
		}

		public override TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj)
		{
			return GetObjectData(serializer, (IXmlIndex)obj);
		}

		private TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IXmlIndex obj)
		{
			return new TId[8]
			{
				serializer.RegisterValue(obj.DisallowPageLocks),
				serializer.RegisterValue(obj.DisallowRowLocks),
				serializer.RegisterValue(obj.FillFactor),
				serializer.RegisterValue(obj.IgnoreDuplicateKeys),
				serializer.RegisterValue(obj.IsDisabled),
				serializer.RegisterValue(obj.Name),
				serializer.RegisterValue(obj.PadIndex),
				serializer.RegisterValue(obj.Parent, obj, isChild: false)
			};
		}
	}

	private readonly ApplicationRoleSerializationInfo applicationRoleSerializationInfo;

	private readonly AsymmetricKeySerializationInfo asymmetricKeySerializationInfo;

	private readonly CertificateSerializationInfo certificateSerializationInfo;

	private readonly CheckConstraintSerializationInfo checkConstraintSerializationInfo;

	private readonly ClrDataTypeSerializationInfo clrDataTypeSerializationInfo;

	private readonly CollationSerializationInfo collationSerializationInfo;

	private readonly ColumnSerializationInfo columnSerializationInfo;

	private readonly CredentialSerializationInfo credentialSerializationInfo;

	private readonly CursorDataTypeSerializationInfo cursorDataTypeSerializationInfo;

	private readonly CursorParameterSerializationInfo cursorParameterSerializationInfo;

	private readonly CursorVariableSerializationInfo cursorVariableSerializationInfo;

	private readonly DatabaseSerializationInfo databaseSerializationInfo;

	private readonly DatabaseDdlTriggerSerializationInfo databaseDdlTriggerSerializationInfo;

	private readonly DatabasePermissionSerializationInfo databasePermissionSerializationInfo;

	private readonly DatabaseRoleSerializationInfo databaseRoleSerializationInfo;

	private readonly DefaultConstraintSerializationInfo defaultConstraintSerializationInfo;

	private readonly DmlTriggerSerializationInfo dmlTriggerSerializationInfo;

	private readonly ExecutionContextSerializationInfo executionContextSerializationInfo;

	private readonly ExtendedStoredProcedureSerializationInfo extendedStoredProcedureSerializationInfo;

	private readonly FileGroupSerializationInfo fileGroupSerializationInfo;

	private readonly ForeignKeyColumnSerializationInfo foreignKeyColumnSerializationInfo;

	private readonly ForeignKeyConstraintSerializationInfo foreignKeyConstraintSerializationInfo;

	private readonly IndexedColumnSerializationInfo indexedColumnSerializationInfo;

	private readonly LoginSerializationInfo loginSerializationInfo;

	private readonly PartitionSchemeSerializationInfo partitionSchemeSerializationInfo;

	private readonly PrimaryKeyConstraintSerializationInfo primaryKeyConstraintSerializationInfo;

	private readonly RelationalIndexSerializationInfo relationalIndexSerializationInfo;

	private readonly ScalarDataTypeSerializationInfo scalarDataTypeSerializationInfo;

	private readonly ScalarParameterSerializationInfo scalarParameterSerializationInfo;

	private readonly ScalarValuedFunctionSerializationInfo scalarValuedFunctionSerializationInfo;

	private readonly ScalarVariableSerializationInfo scalarVariableSerializationInfo;

	private readonly SchemaSerializationInfo schemaSerializationInfo;

	private readonly ServerSerializationInfo serverSerializationInfo;

	private readonly ServerDdlTriggerSerializationInfo serverDdlTriggerSerializationInfo;

	private readonly SpatialIndexSerializationInfo spatialIndexSerializationInfo;

	private readonly StatisticsSerializationInfo statisticsSerializationInfo;

	private readonly StoredProcedureSerializationInfo storedProcedureSerializationInfo;

	private readonly SynonymSerializationInfo synonymSerializationInfo;

	private readonly SystemClrDataTypeSerializationInfo systemClrDataTypeSerializationInfo;

	private readonly SystemDataTypeSerializationInfo systemDataTypeSerializationInfo;

	private readonly TableSerializationInfo tableSerializationInfo;

	private readonly TableDataTypeSerializationInfo tableDataTypeSerializationInfo;

	private readonly TableParameterSerializationInfo tableParameterSerializationInfo;

	private readonly TableValuedFunctionSerializationInfo tableValuedFunctionSerializationInfo;

	private readonly TableVariableSerializationInfo tableVariableSerializationInfo;

	private readonly UdtDataMemberSerializationInfo udtDataMemberSerializationInfo;

	private readonly UdtMethodSerializationInfo udtMethodSerializationInfo;

	private readonly UniqueConstraintSerializationInfo uniqueConstraintSerializationInfo;

	private readonly UserSerializationInfo userSerializationInfo;

	private readonly UserDefinedAggregateSerializationInfo userDefinedAggregateSerializationInfo;

	private readonly UserDefinedClrTypeSerializationInfo userDefinedClrTypeSerializationInfo;

	private readonly UserDefinedDataTypeSerializationInfo userDefinedDataTypeSerializationInfo;

	private readonly UserDefinedTableTypeSerializationInfo userDefinedTableTypeSerializationInfo;

	private readonly ViewSerializationInfo viewSerializationInfo;

	private readonly VoidDataTypeSerializationInfo voidDataTypeSerializationInfo;

	private readonly XmlDataTypeSerializationInfo xmlDataTypeSerializationInfo;

	private readonly XmlIndexSerializationInfo xmlIndexSerializationInfo;

	public SerializationVisitor(SerializationOptions options)
	{
		applicationRoleSerializationInfo = new ApplicationRoleSerializationInfo(options);
		asymmetricKeySerializationInfo = new AsymmetricKeySerializationInfo(options);
		certificateSerializationInfo = new CertificateSerializationInfo(options);
		checkConstraintSerializationInfo = new CheckConstraintSerializationInfo(options);
		clrDataTypeSerializationInfo = new ClrDataTypeSerializationInfo(options);
		collationSerializationInfo = new CollationSerializationInfo(options);
		columnSerializationInfo = new ColumnSerializationInfo(options);
		credentialSerializationInfo = new CredentialSerializationInfo(options);
		cursorDataTypeSerializationInfo = new CursorDataTypeSerializationInfo(options);
		cursorParameterSerializationInfo = new CursorParameterSerializationInfo(options);
		cursorVariableSerializationInfo = new CursorVariableSerializationInfo(options);
		databaseSerializationInfo = new DatabaseSerializationInfo(options);
		databaseDdlTriggerSerializationInfo = new DatabaseDdlTriggerSerializationInfo(options);
		databasePermissionSerializationInfo = new DatabasePermissionSerializationInfo(options);
		databaseRoleSerializationInfo = new DatabaseRoleSerializationInfo(options);
		defaultConstraintSerializationInfo = new DefaultConstraintSerializationInfo(options);
		dmlTriggerSerializationInfo = new DmlTriggerSerializationInfo(options);
		executionContextSerializationInfo = new ExecutionContextSerializationInfo(options);
		extendedStoredProcedureSerializationInfo = new ExtendedStoredProcedureSerializationInfo(options);
		fileGroupSerializationInfo = new FileGroupSerializationInfo(options);
		foreignKeyColumnSerializationInfo = new ForeignKeyColumnSerializationInfo(options);
		foreignKeyConstraintSerializationInfo = new ForeignKeyConstraintSerializationInfo(options);
		indexedColumnSerializationInfo = new IndexedColumnSerializationInfo(options);
		loginSerializationInfo = new LoginSerializationInfo(options);
		partitionSchemeSerializationInfo = new PartitionSchemeSerializationInfo(options);
		primaryKeyConstraintSerializationInfo = new PrimaryKeyConstraintSerializationInfo(options);
		relationalIndexSerializationInfo = new RelationalIndexSerializationInfo(options);
		scalarDataTypeSerializationInfo = new ScalarDataTypeSerializationInfo(options);
		scalarParameterSerializationInfo = new ScalarParameterSerializationInfo(options);
		scalarValuedFunctionSerializationInfo = new ScalarValuedFunctionSerializationInfo(options);
		scalarVariableSerializationInfo = new ScalarVariableSerializationInfo(options);
		schemaSerializationInfo = new SchemaSerializationInfo(options);
		serverSerializationInfo = new ServerSerializationInfo(options);
		serverDdlTriggerSerializationInfo = new ServerDdlTriggerSerializationInfo(options);
		spatialIndexSerializationInfo = new SpatialIndexSerializationInfo(options);
		statisticsSerializationInfo = new StatisticsSerializationInfo(options);
		storedProcedureSerializationInfo = new StoredProcedureSerializationInfo(options);
		synonymSerializationInfo = new SynonymSerializationInfo(options);
		systemClrDataTypeSerializationInfo = new SystemClrDataTypeSerializationInfo(options);
		systemDataTypeSerializationInfo = new SystemDataTypeSerializationInfo(options);
		tableSerializationInfo = new TableSerializationInfo(options);
		tableDataTypeSerializationInfo = new TableDataTypeSerializationInfo(options);
		tableParameterSerializationInfo = new TableParameterSerializationInfo(options);
		tableValuedFunctionSerializationInfo = new TableValuedFunctionSerializationInfo(options);
		tableVariableSerializationInfo = new TableVariableSerializationInfo(options);
		udtDataMemberSerializationInfo = new UdtDataMemberSerializationInfo(options);
		udtMethodSerializationInfo = new UdtMethodSerializationInfo(options);
		uniqueConstraintSerializationInfo = new UniqueConstraintSerializationInfo(options);
		userSerializationInfo = new UserSerializationInfo(options);
		userDefinedAggregateSerializationInfo = new UserDefinedAggregateSerializationInfo(options);
		userDefinedClrTypeSerializationInfo = new UserDefinedClrTypeSerializationInfo(options);
		userDefinedDataTypeSerializationInfo = new UserDefinedDataTypeSerializationInfo(options);
		userDefinedTableTypeSerializationInfo = new UserDefinedTableTypeSerializationInfo(options);
		viewSerializationInfo = new ViewSerializationInfo(options);
		voidDataTypeSerializationInfo = new VoidDataTypeSerializationInfo(options);
		xmlDataTypeSerializationInfo = new XmlDataTypeSerializationInfo(options);
		xmlIndexSerializationInfo = new XmlIndexSerializationInfo(options);
	}

	public SerializationInfo Visit(IBuiltInFunction obj)
	{
		return null;
	}

	public SerializationInfo Visit(ICheckConstraint obj)
	{
		return checkConstraintSerializationInfo;
	}

	public SerializationInfo Visit(IClrDataType obj)
	{
		return clrDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(ICollation obj)
	{
		return collationSerializationInfo;
	}

	public SerializationInfo Visit(IColumn obj)
	{
		return columnSerializationInfo;
	}

	public SerializationInfo Visit(ICursorDataType obj)
	{
		return cursorDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(ICursorParameter obj)
	{
		return cursorParameterSerializationInfo;
	}

	public SerializationInfo Visit(ICursorVariable obj)
	{
		return cursorVariableSerializationInfo;
	}

	public SerializationInfo Visit(IDatabasePermission obj)
	{
		return databasePermissionSerializationInfo;
	}

	public SerializationInfo Visit(IDatePart obj)
	{
		return null;
	}

	public SerializationInfo Visit(IDefaultConstraint obj)
	{
		return defaultConstraintSerializationInfo;
	}

	public SerializationInfo Visit(IDmlTrigger obj)
	{
		return dmlTriggerSerializationInfo;
	}

	public SerializationInfo Visit(IExecutionContext obj)
	{
		return executionContextSerializationInfo;
	}

	public SerializationInfo Visit(IForeignKeyColumn obj)
	{
		return foreignKeyColumnSerializationInfo;
	}

	public SerializationInfo Visit(IForeignKeyConstraint obj)
	{
		return foreignKeyConstraintSerializationInfo;
	}

	public SerializationInfo Visit(IIndexedColumn obj)
	{
		return indexedColumnSerializationInfo;
	}

	public SerializationInfo Visit(IPrimaryKeyConstraint obj)
	{
		return primaryKeyConstraintSerializationInfo;
	}

	public SerializationInfo Visit(IRelationalIndex obj)
	{
		return relationalIndexSerializationInfo;
	}

	public SerializationInfo Visit(IScalarExpression obj)
	{
		return null;
	}

	public SerializationInfo Visit(IScalarDataType obj)
	{
		return scalarDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(IScalarParameter obj)
	{
		return scalarParameterSerializationInfo;
	}

	public SerializationInfo Visit(IScalarVariable obj)
	{
		return scalarVariableSerializationInfo;
	}

	public SerializationInfo Visit(ISpatialIndex obj)
	{
		return spatialIndexSerializationInfo;
	}

	public SerializationInfo Visit(IStatistics obj)
	{
		return statisticsSerializationInfo;
	}

	public SerializationInfo Visit(ISystemDataType obj)
	{
		return systemDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(ISystemClrDataType obj)
	{
		return systemClrDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(ITableDataType obj)
	{
		return tableDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(ITableParameter obj)
	{
		return tableParameterSerializationInfo;
	}

	public SerializationInfo Visit(ITableVariable obj)
	{
		return tableVariableSerializationInfo;
	}

	public SerializationInfo Visit(IUdtDataMember obj)
	{
		return udtDataMemberSerializationInfo;
	}

	public SerializationInfo Visit(IUdtMethod obj)
	{
		return udtMethodSerializationInfo;
	}

	public SerializationInfo Visit(IUniqueConstraint obj)
	{
		return uniqueConstraintSerializationInfo;
	}

	public SerializationInfo Visit(IVirtualTable obj)
	{
		return null;
	}

	public SerializationInfo Visit(IVoidDataType obj)
	{
		return voidDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(IXmlDataType obj)
	{
		return xmlDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(IXmlDataTypeMethod obj)
	{
		return null;
	}

	public SerializationInfo Visit(IXmlDataTypeTableMethod obj)
	{
		return null;
	}

	public SerializationInfo Visit(IXmlIndex obj)
	{
		return xmlIndexSerializationInfo;
	}

	public SerializationInfo Visit(IServer obj)
	{
		return serverSerializationInfo;
	}

	public SerializationInfo Visit(ICredential obj)
	{
		return credentialSerializationInfo;
	}

	public SerializationInfo Visit(IDatabase obj)
	{
		return databaseSerializationInfo;
	}

	public SerializationInfo Visit(ILogin obj)
	{
		return loginSerializationInfo;
	}

	public SerializationInfo Visit(IServerDdlTrigger obj)
	{
		return serverDdlTriggerSerializationInfo;
	}

	public SerializationInfo Visit(IApplicationRole obj)
	{
		return applicationRoleSerializationInfo;
	}

	public SerializationInfo Visit(IAsymmetricKey obj)
	{
		return asymmetricKeySerializationInfo;
	}

	public SerializationInfo Visit(ICertificate obj)
	{
		return certificateSerializationInfo;
	}

	public SerializationInfo Visit(IDatabaseDdlTrigger obj)
	{
		return databaseDdlTriggerSerializationInfo;
	}

	public SerializationInfo Visit(IDatabaseRole obj)
	{
		return databaseRoleSerializationInfo;
	}

	public SerializationInfo Visit(IFileGroup obj)
	{
		return fileGroupSerializationInfo;
	}

	public SerializationInfo Visit(IPartitionScheme obj)
	{
		return partitionSchemeSerializationInfo;
	}

	public SerializationInfo Visit(ISchema obj)
	{
		return schemaSerializationInfo;
	}

	public SerializationInfo Visit(IUser obj)
	{
		return userSerializationInfo;
	}

	public SerializationInfo Visit(IExtendedStoredProcedure obj)
	{
		return extendedStoredProcedureSerializationInfo;
	}

	public SerializationInfo Visit(IScalarValuedFunction obj)
	{
		return scalarValuedFunctionSerializationInfo;
	}

	public SerializationInfo Visit(IStoredProcedure obj)
	{
		return storedProcedureSerializationInfo;
	}

	public SerializationInfo Visit(ISynonym obj)
	{
		return synonymSerializationInfo;
	}

	public SerializationInfo Visit(ITable obj)
	{
		return tableSerializationInfo;
	}

	public SerializationInfo Visit(ITableValuedFunction obj)
	{
		return tableValuedFunctionSerializationInfo;
	}

	public SerializationInfo Visit(IUserDefinedAggregate obj)
	{
		return userDefinedAggregateSerializationInfo;
	}

	public SerializationInfo Visit(IUserDefinedClrType obj)
	{
		return userDefinedClrTypeSerializationInfo;
	}

	public SerializationInfo Visit(IUserDefinedDataType obj)
	{
		return userDefinedDataTypeSerializationInfo;
	}

	public SerializationInfo Visit(IUserDefinedTableType obj)
	{
		return userDefinedTableTypeSerializationInfo;
	}

	public SerializationInfo Visit(IView obj)
	{
		return viewSerializationInfo;
	}
}
