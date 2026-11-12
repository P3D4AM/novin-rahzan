using System;
using System.CodeDom;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class CodeConfiguration
{
	public static class Names
	{
		public static class Fields
		{
			public const string ByteArrayLookup = "byteArrayLookup";

			public const string CollationInfoLookup = "collationInfoLookup";

			public const string CollectionIdListLookup = "collectionIdListLookup";

			public const string DataTypeSpecLookup = "dataTypeSpecLookup";

			public const string ComputedColumnInfoLookup = "computedColumnInfoLookup";

			public const string IdentityColumnInfoLookup = "identityColumnInfoLookup";

			public const string IdToDataIndexMap = "idToDataIndexMap";

			public const string IdToTypeIdMap = "idToTypeIdMap";

			public const string IntLookup = "intLookup";

			public const string MetadataObjectData = "metadataObjectData";

			public const string PasswordLookup = "passwordLookup";

			public const string StringLookup = "stringLookup";
		}

		public static class Methods
		{
			public const string GetByteArrayById = "GetByteArrayById";

			public const string GetCollationInfoById = "GetCollationInfoById";

			public const string GetCollectionById = "GetCollectionById";

			public const string GetCollectionIdListById = "GetCollectionIdListById";

			public const string GetComputedColumnInfoById = "GetComputedColumnInfoById";

			public const string GetDataTypeSpecById = "GetDataTypeSpecById";

			public const string GetIdentityColumnInfoById = "GetIdentityColumnInfoById";

			public const string GetIntById = "GetIntById";

			public const string GetMetadataObjectById = "GetMetadataObjectById";

			public const string GetOrderedCollectionById = "GetOrderedCollectionById";

			public const string GetPasswordById = "GetPasswordById";

			public const string GetStringById = "GetStringById";

			public const string GetCollationInfo = "GetCollationInfo";

			public const string GetDataTypeSpec = "GetDataTypeSpec";

			public const string GetMetadataObjectData = "GetMetadataObjectData";

			public const string GetPackedValue = "GetPackedValue";

			public const string CreateMetadataObject = "CreateMetadataObject";

			public const string CreateMetadataModel = "CreateMetadataModel";
		}

		public static class Properties
		{
			public const string CollectionCount = "MetadataCollectionCount";

			public const string Instance = "Instance";

			public const string ObjectCount = "MetadataObjectCount";

			public const string RootObjectId = "RootObjectId";
		}

		public static class Types
		{
			public const string MetadataCollection = "IMetadataCollection";

			public const string MetadataOrderedCollection = "IMetadataOrderedCollection";

			public const string MetadataModel = "MetadataModel";

			public const string ObjectStore = "IMetadataObjectStore";
		}

		public static class Variables
		{
			public const string DataIndex = "dataIndex";

			public const string Index = "index";

			public const string Length = "length";

			public const string MetadataProvider = "metadataProvider";

			public const string Model = "model";

			public const string Name = "name";

			public const string ObjectId = "objectId";

			public const string Offset = "offset";

			public const string Server = "server";

			public const string TypeId = "typeId";
		}
	}

	public static class TypeReferences
	{
		public static CodeTypeReference Bool;

		public static CodeTypeReference Byte;

		public static CodeTypeReference SByte;

		public static CodeTypeReference Short;

		public static CodeTypeReference UShort;

		public static CodeTypeReference Int;

		public static CodeTypeReference UInt;

		public static CodeTypeReference Long;

		public static CodeTypeReference ULong;

		public static CodeTypeReference String;

		public static CodeTypeReference Object;

		public static CodeTypeReference ActivationOrder;

		public static CodeTypeReference DatabaseCompatibilityLevel;

		public static CodeTypeReference DatabasePermissionType;

		public static CodeTypeReference ExecutionContextType;

		public static CodeTypeReference ForeignKeyAction;

		public static CodeTypeReference GridDensity;

		public static CodeTypeReference LoginType;

		public static CodeTypeReference PermissionState;

		public static CodeTypeReference SortOrder;

		public static CodeTypeReference StatisticsType;

		public static CodeTypeReference SynonymBaseType;

		public static CodeTypeReference UserType;

		public static CodeTypeReference ByteArray;

		public static CodeTypeReference CollationInfo;

		public static CodeTypeReference ComputedColumnInfo;

		public static CodeTypeReference DataTypeSpec;

		public static CodeTypeReference IdentityColumnInfo;

		public static CodeTypeReference ObjectArray;

		public static CodeTypeReference Password;

		public static CodeTypeReference MetadataObject;

		public static CodeTypeReference MetadataObjectArray;

		public static CodeTypeReference MetadataProvider;

		public static CodeTypeReference MutableServer;

		public static CodeTypeReference MetadataModel;

		public static CodeTypeReference ObjectStore;

		private static Dictionary<Type, CodeTypeReference> typeRefLookup;

		static TypeReferences()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Expected O, but got Unknown
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Expected O, but got Unknown
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Expected O, but got Unknown
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Expected O, but got Unknown
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Expected O, but got Unknown
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Expected O, but got Unknown
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Expected O, but got Unknown
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Expected O, but got Unknown
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Expected O, but got Unknown
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Expected O, but got Unknown
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Expected O, but got Unknown
			Bool = new CodeTypeReference(typeof(bool).Name);
			Byte = new CodeTypeReference(typeof(byte).Name);
			SByte = new CodeTypeReference(typeof(sbyte).Name);
			Short = new CodeTypeReference(typeof(short).Name);
			UShort = new CodeTypeReference(typeof(ushort).Name);
			Int = new CodeTypeReference(typeof(int).Name);
			UInt = new CodeTypeReference(typeof(uint).Name);
			Long = new CodeTypeReference(typeof(long).Name);
			ULong = new CodeTypeReference(typeof(ulong).Name);
			String = new CodeTypeReference(typeof(string).Name);
			Object = new CodeTypeReference(typeof(object).Name);
			ActivationOrder = new CodeTypeReference(typeof(ActivationOrder).Name);
			DatabaseCompatibilityLevel = new CodeTypeReference(typeof(DatabaseCompatibilityLevel).Name);
			DatabasePermissionType = new CodeTypeReference(typeof(DatabasePermissionType).Name);
			ExecutionContextType = new CodeTypeReference(typeof(ExecutionContextType).Name);
			ForeignKeyAction = new CodeTypeReference(typeof(ForeignKeyAction).Name);
			GridDensity = new CodeTypeReference(typeof(GridDensity).Name);
			LoginType = new CodeTypeReference(typeof(LoginType).Name);
			PermissionState = new CodeTypeReference(typeof(PermissionState).Name);
			SortOrder = new CodeTypeReference(typeof(SortOrder).Name);
			StatisticsType = new CodeTypeReference(typeof(StatisticsType).Name);
			SynonymBaseType = new CodeTypeReference(typeof(SynonymBaseType).Name);
			UserType = new CodeTypeReference(typeof(UserType).Name);
			ByteArray = new CodeTypeReference(typeof(byte[]).Name);
			CollationInfo = new CodeTypeReference(typeof(CollationInfo).Name);
			ComputedColumnInfo = new CodeTypeReference(typeof(ComputedColumnInfo).Name);
			DataTypeSpec = new CodeTypeReference(typeof(DataTypeSpec).Name);
			IdentityColumnInfo = new CodeTypeReference(typeof(IdentityColumnInfo).Name);
			ObjectArray = new CodeTypeReference(typeof(object[]).Name);
			Password = new CodeTypeReference(typeof(IPassword).Name);
			MetadataObject = new CodeTypeReference(typeof(IMetadataObject).Name);
			MetadataObjectArray = new CodeTypeReference(typeof(IMetadataObject[]).Name);
			MetadataProvider = new CodeTypeReference(typeof(IMetadataProvider).Name);
			MutableServer = new CodeTypeReference(typeof(IMutableServer).Name);
			MetadataModel = new CodeTypeReference("MetadataModel");
			ObjectStore = new CodeTypeReference("IMetadataObjectStore");
			typeRefLookup = new Dictionary<Type, CodeTypeReference>(32);
			typeRefLookup.Add(typeof(bool), Bool);
			typeRefLookup.Add(typeof(byte), Byte);
			typeRefLookup.Add(typeof(sbyte), SByte);
			typeRefLookup.Add(typeof(short), Short);
			typeRefLookup.Add(typeof(ushort), UShort);
			typeRefLookup.Add(typeof(int), Int);
			typeRefLookup.Add(typeof(uint), UInt);
			typeRefLookup.Add(typeof(long), Long);
			typeRefLookup.Add(typeof(ulong), ULong);
			typeRefLookup.Add(typeof(string), String);
			typeRefLookup.Add(typeof(ActivationOrder), ActivationOrder);
			typeRefLookup.Add(typeof(DatabaseCompatibilityLevel), DatabaseCompatibilityLevel);
			typeRefLookup.Add(typeof(DatabasePermissionType), DatabasePermissionType);
			typeRefLookup.Add(typeof(ExecutionContextType), ExecutionContextType);
			typeRefLookup.Add(typeof(ForeignKeyAction), ForeignKeyAction);
			typeRefLookup.Add(typeof(GridDensity), GridDensity);
			typeRefLookup.Add(typeof(LoginType), LoginType);
			typeRefLookup.Add(typeof(PermissionState), PermissionState);
			typeRefLookup.Add(typeof(SortOrder), SortOrder);
			typeRefLookup.Add(typeof(StatisticsType), StatisticsType);
			typeRefLookup.Add(typeof(SynonymBaseType), SynonymBaseType);
			typeRefLookup.Add(typeof(UserType), UserType);
			typeRefLookup.Add(typeof(byte[]), ByteArray);
			typeRefLookup.Add(typeof(CollationInfo), CollationInfo);
			typeRefLookup.Add(typeof(ComputedColumnInfo), ComputedColumnInfo);
			typeRefLookup.Add(typeof(DataTypeSpec), DataTypeSpec);
			typeRefLookup.Add(typeof(IdentityColumnInfo), IdentityColumnInfo);
			typeRefLookup.Add(typeof(IPassword), Password);
		}

		public static CodeTypeReference GetTypeReference(Type type)
		{
			typeRefLookup.TryGetValue(type, out var value);
			return value;
		}
	}

	public static class MethodReferences
	{
		public static readonly CodeMethodReferenceExpression GetByteArrayById;

		public static readonly CodeMethodReferenceExpression GetCollationInfoById;

		public static readonly CodeMethodReferenceExpression GetComputedColumnInfoById;

		public static readonly CodeMethodReferenceExpression GetDataTypeSpecById;

		public static readonly CodeMethodReferenceExpression GetIdentityColumnInfoById;

		public static readonly CodeMethodReferenceExpression GetIntById;

		public static readonly CodeMethodReferenceExpression GetPasswordById;

		public static readonly CodeMethodReferenceExpression GetStringById;

		public static readonly CodeMethodReferenceExpression GetCollationInfo;

		public static readonly CodeMethodReferenceExpression GetDataTypeSpec;

		public static readonly CodeMethodReferenceExpression GetMetadataObjectData;

		public static readonly CodeMethodReferenceExpression GetPackedValue;

		public static readonly CodeMethodReferenceExpression DebugAssert;

		static MethodReferences()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_007f: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			CodeThisReferenceExpression val = new CodeThisReferenceExpression();
			GetByteArrayById = new CodeMethodReferenceExpression((CodeExpression)val, "GetByteArrayById");
			GetCollationInfoById = new CodeMethodReferenceExpression((CodeExpression)val, "GetCollationInfoById");
			GetDataTypeSpecById = new CodeMethodReferenceExpression((CodeExpression)val, "GetDataTypeSpecById");
			GetComputedColumnInfoById = new CodeMethodReferenceExpression((CodeExpression)val, "GetComputedColumnInfoById");
			GetIdentityColumnInfoById = new CodeMethodReferenceExpression((CodeExpression)val, "GetIdentityColumnInfoById");
			GetIntById = new CodeMethodReferenceExpression((CodeExpression)val, "GetIntById");
			GetPasswordById = new CodeMethodReferenceExpression((CodeExpression)val, "GetPasswordById");
			GetStringById = new CodeMethodReferenceExpression((CodeExpression)val, "GetStringById");
			GetCollationInfo = new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(TypeReferences.CollationInfo), "GetCollationInfo");
			GetDataTypeSpec = new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(TypeReferences.DataTypeSpec), "GetDataTypeSpec");
			GetMetadataObjectData = new CodeMethodReferenceExpression((CodeExpression)null, "GetMetadataObjectData");
			GetPackedValue = new CodeMethodReferenceExpression((CodeExpression)null, "GetPackedValue");
			DebugAssert = new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression("Debug"), "Assert");
		}
	}

	public static class EnumValues
	{
		public static class DatabaseCompatibilityLevel
		{
			public static CodeExpression Current;

			public static CodeExpression Version80;

			public static CodeExpression Version90;

			public static CodeExpression Version100;

			static DatabaseCompatibilityLevel()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_0044: Expected O, but got Unknown
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.DatabaseCompatibilityLevel);
				Current = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Current");
				Version80 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Version80");
				Version90 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Version90");
				Version100 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Version100");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Common.DatabaseCompatibilityLevel value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Common.DatabaseCompatibilityLevel.Current => Current, 
					Microsoft.SqlServer.Management.SqlParser.Common.DatabaseCompatibilityLevel.Version80 => Version80, 
					Microsoft.SqlServer.Management.SqlParser.Common.DatabaseCompatibilityLevel.Version90 => Version90, 
					Microsoft.SqlServer.Management.SqlParser.Common.DatabaseCompatibilityLevel.Version100 => Version100, 
					_ => null, 
				});
			}
		}

		public static class ActivationOrder
		{
			public static CodeExpression None;

			public static CodeExpression First;

			public static CodeExpression Last;

			static ActivationOrder()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_0034: Expected O, but got Unknown
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.ActivationOrder);
				None = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "None");
				First = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "First");
				Last = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Last");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.ActivationOrder value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.ActivationOrder.None => None, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ActivationOrder.First => First, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ActivationOrder.Last => Last, 
					_ => null, 
				});
			}
		}

		public static class GridDensity
		{
			public static CodeExpression Low;

			public static CodeExpression Medium;

			public static CodeExpression High;

			static GridDensity()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_0034: Expected O, but got Unknown
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.GridDensity);
				Low = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Low");
				Medium = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Medium");
				High = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "High");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.GridDensity value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.GridDensity.Low => Low, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.GridDensity.Medium => Medium, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.GridDensity.High => High, 
					_ => null, 
				});
			}
		}

		public static class StatisticsType
		{
			public static CodeExpression Explicit;

			public static CodeExpression ImplicitViaIndex;

			public static CodeExpression Auto;

			static StatisticsType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_0034: Expected O, but got Unknown
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.StatisticsType);
				Explicit = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Explicit");
				ImplicitViaIndex = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "ImplicitViaIndex");
				Auto = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Auto");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.StatisticsType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.StatisticsType.Auto => Auto, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.StatisticsType.Explicit => Explicit, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.StatisticsType.ImplicitViaIndex => ImplicitViaIndex, 
					_ => null, 
				});
			}
		}

		public static class SynonymBaseType
		{
			public static CodeExpression None;

			public static CodeExpression ClrAggregateFunction;

			public static CodeExpression ClrScalarFunction;

			public static CodeExpression ClrStoredProcedure;

			public static CodeExpression ClrTableValuedFunction;

			public static CodeExpression ExtendedStoredProcedure;

			public static CodeExpression ReplicationFilterProcedure;

			public static CodeExpression SqlInlineTableValuedFunction;

			public static CodeExpression SqlScalarFunction;

			public static CodeExpression SqlStoredProcedure;

			public static CodeExpression SqlTableValuedFunction;

			public static CodeExpression Table;

			public static CodeExpression View;

			static SynonymBaseType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Expected O, but got Unknown
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Expected O, but got Unknown
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Expected O, but got Unknown
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Expected O, but got Unknown
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Expected O, but got Unknown
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Expected O, but got Unknown
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Expected O, but got Unknown
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Expected O, but got Unknown
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Expected O, but got Unknown
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Expected O, but got Unknown
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Expected O, but got Unknown
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Expected O, but got Unknown
				new CodeTypeReferenceExpression(TypeReferences.SynonymBaseType);
				None = (CodeExpression)new CodeFieldReferenceExpression(None, "None");
				ClrAggregateFunction = (CodeExpression)new CodeFieldReferenceExpression(ClrAggregateFunction, "ClrAggregateFunction");
				ClrScalarFunction = (CodeExpression)new CodeFieldReferenceExpression(ClrScalarFunction, "ClrScalarFunction");
				ClrStoredProcedure = (CodeExpression)new CodeFieldReferenceExpression(ClrStoredProcedure, "ClrStoredProcedure");
				ClrTableValuedFunction = (CodeExpression)new CodeFieldReferenceExpression(ClrTableValuedFunction, "ClrTableValuedFunction");
				ExtendedStoredProcedure = (CodeExpression)new CodeFieldReferenceExpression(ExtendedStoredProcedure, "ExtendedStoredProcedure");
				ReplicationFilterProcedure = (CodeExpression)new CodeFieldReferenceExpression(ReplicationFilterProcedure, "ReplicationFilterProcedure");
				SqlInlineTableValuedFunction = (CodeExpression)new CodeFieldReferenceExpression(SqlInlineTableValuedFunction, "SqlInlineTableValuedFunction");
				SqlScalarFunction = (CodeExpression)new CodeFieldReferenceExpression(SqlScalarFunction, "SqlScalarFunction");
				SqlStoredProcedure = (CodeExpression)new CodeFieldReferenceExpression(SqlStoredProcedure, "SqlStoredProcedure");
				SqlTableValuedFunction = (CodeExpression)new CodeFieldReferenceExpression(SqlTableValuedFunction, "SqlTableValuedFunction");
				Table = (CodeExpression)new CodeFieldReferenceExpression(Table, "Table");
				View = (CodeExpression)new CodeFieldReferenceExpression(View, "View");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.None => None, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ClrAggregateFunction => ClrAggregateFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ClrScalarFunction => ClrScalarFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ClrStoredProcedure => ClrStoredProcedure, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ClrTableValuedFunction => ClrTableValuedFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ExtendedStoredProcedure => ExtendedStoredProcedure, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.ReplicationFilterProcedure => ReplicationFilterProcedure, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.SqlInlineTableValuedFunction => SqlInlineTableValuedFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.SqlScalarFunction => SqlScalarFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.SqlStoredProcedure => SqlStoredProcedure, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.SqlTableValuedFunction => SqlTableValuedFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.Table => Table, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SynonymBaseType.View => View, 
					_ => null, 
				});
			}
		}

		public static class DatabasePermissionType
		{
			public static CodeExpression Alter;

			public static CodeExpression AlterAnyApplicationRole;

			public static CodeExpression AlterAnyAssembly;

			public static CodeExpression AlterAnyAsymmetricKey;

			public static CodeExpression AlterAnyCertificate;

			public static CodeExpression AlterAnyContract;

			public static CodeExpression AlterAnyDatabaseAudit;

			public static CodeExpression AlterAnyDatabaseDdlTrigger;

			public static CodeExpression AlterAnyDatabaseEventNotification;

			public static CodeExpression AlterAnyDataspace;

			public static CodeExpression AlterAnyFulltextCatalog;

			public static CodeExpression AlterAnyMask;

			public static CodeExpression AlterAnyMessageType;

			public static CodeExpression AlterAnyRemoteServiceBinding;

			public static CodeExpression AlterAnyRole;

			public static CodeExpression AlterAnyRoute;

			public static CodeExpression AlterAnySchema;

			public static CodeExpression AlterAnySensitivityClassification;

			public static CodeExpression AlterAnyService;

			public static CodeExpression AlterAnySymmetricKey;

			public static CodeExpression AlterAnyUser;

			public static CodeExpression Authenticate;

			public static CodeExpression BackupDatabase;

			public static CodeExpression BackupLog;

			public static CodeExpression Checkpoint;

			public static CodeExpression Connect;

			public static CodeExpression ConnectReplication;

			public static CodeExpression Control;

			public static CodeExpression CreateAggregate;

			public static CodeExpression CreateAssembly;

			public static CodeExpression CreateAsymmetricKey;

			public static CodeExpression CreateCertificate;

			public static CodeExpression CreateContract;

			public static CodeExpression CreateDatabase;

			public static CodeExpression CreateDatabaseDdlEventNotification;

			public static CodeExpression CreateDefault;

			public static CodeExpression CreateFulltextCatalog;

			public static CodeExpression CreateFunction;

			public static CodeExpression CreateMessageType;

			public static CodeExpression CreateProcedure;

			public static CodeExpression CreateQueue;

			public static CodeExpression CreateRemoteServiceBinding;

			public static CodeExpression CreateRole;

			public static CodeExpression CreateRoute;

			public static CodeExpression CreateRule;

			public static CodeExpression CreateSchema;

			public static CodeExpression CreateService;

			public static CodeExpression CreateSymmetricKey;

			public static CodeExpression CreateSynonym;

			public static CodeExpression CreateTable;

			public static CodeExpression CreateType;

			public static CodeExpression CreateView;

			public static CodeExpression CreateXmlSchemaCollection;

			public static CodeExpression Delete;

			public static CodeExpression Execute;

			public static CodeExpression Impersonate;

			public static CodeExpression Insert;

			public static CodeExpression Receive;

			public static CodeExpression References;

			public static CodeExpression Select;

			public static CodeExpression Send;

			public static CodeExpression Showplan;

			public static CodeExpression SubscribeQueryNotifications;

			public static CodeExpression TakeOwnership;

			public static CodeExpression Unmask;

			public static CodeExpression Update;

			public static CodeExpression ViewAnyColumnEncryptionKeyDefinition;

			public static CodeExpression ViewAnyColumnMasterKeyDefinition;

			public static CodeExpression ViewAnySensitivityClassification;

			public static CodeExpression ViewChangeTracking;

			public static CodeExpression ViewDatabaseState;

			public static CodeExpression ViewDefinition;

			static DatabasePermissionType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Expected O, but got Unknown
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Expected O, but got Unknown
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Expected O, but got Unknown
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Expected O, but got Unknown
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Expected O, but got Unknown
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Expected O, but got Unknown
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Expected O, but got Unknown
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Expected O, but got Unknown
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Expected O, but got Unknown
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Expected O, but got Unknown
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Expected O, but got Unknown
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Expected O, but got Unknown
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Expected O, but got Unknown
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Expected O, but got Unknown
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_014b: Expected O, but got Unknown
				//IL_0155: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Expected O, but got Unknown
				//IL_0169: Unknown result type (might be due to invalid IL or missing references)
				//IL_0173: Expected O, but got Unknown
				//IL_017d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0187: Expected O, but got Unknown
				//IL_0191: Unknown result type (might be due to invalid IL or missing references)
				//IL_019b: Expected O, but got Unknown
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01af: Expected O, but got Unknown
				//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c3: Expected O, but got Unknown
				//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d7: Expected O, but got Unknown
				//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01eb: Expected O, but got Unknown
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ff: Expected O, but got Unknown
				//IL_0209: Unknown result type (might be due to invalid IL or missing references)
				//IL_0213: Expected O, but got Unknown
				//IL_021d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0227: Expected O, but got Unknown
				//IL_0231: Unknown result type (might be due to invalid IL or missing references)
				//IL_023b: Expected O, but got Unknown
				//IL_0245: Unknown result type (might be due to invalid IL or missing references)
				//IL_024f: Expected O, but got Unknown
				//IL_0259: Unknown result type (might be due to invalid IL or missing references)
				//IL_0263: Expected O, but got Unknown
				//IL_026d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0277: Expected O, but got Unknown
				//IL_0281: Unknown result type (might be due to invalid IL or missing references)
				//IL_028b: Expected O, but got Unknown
				//IL_0295: Unknown result type (might be due to invalid IL or missing references)
				//IL_029f: Expected O, but got Unknown
				//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b3: Expected O, but got Unknown
				//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c7: Expected O, but got Unknown
				//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02db: Expected O, but got Unknown
				//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ef: Expected O, but got Unknown
				//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0303: Expected O, but got Unknown
				//IL_030d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0317: Expected O, but got Unknown
				//IL_0321: Unknown result type (might be due to invalid IL or missing references)
				//IL_032b: Expected O, but got Unknown
				//IL_0335: Unknown result type (might be due to invalid IL or missing references)
				//IL_033f: Expected O, but got Unknown
				//IL_0349: Unknown result type (might be due to invalid IL or missing references)
				//IL_0353: Expected O, but got Unknown
				//IL_035d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0367: Expected O, but got Unknown
				//IL_0371: Unknown result type (might be due to invalid IL or missing references)
				//IL_037b: Expected O, but got Unknown
				//IL_0385: Unknown result type (might be due to invalid IL or missing references)
				//IL_038f: Expected O, but got Unknown
				//IL_0399: Unknown result type (might be due to invalid IL or missing references)
				//IL_03a3: Expected O, but got Unknown
				//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_03b7: Expected O, but got Unknown
				//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_03cb: Expected O, but got Unknown
				//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_03df: Expected O, but got Unknown
				//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_03f3: Expected O, but got Unknown
				//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0407: Expected O, but got Unknown
				//IL_0411: Unknown result type (might be due to invalid IL or missing references)
				//IL_041b: Expected O, but got Unknown
				//IL_0425: Unknown result type (might be due to invalid IL or missing references)
				//IL_042f: Expected O, but got Unknown
				//IL_0439: Unknown result type (might be due to invalid IL or missing references)
				//IL_0443: Expected O, but got Unknown
				//IL_044d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0457: Expected O, but got Unknown
				//IL_0461: Unknown result type (might be due to invalid IL or missing references)
				//IL_046b: Expected O, but got Unknown
				//IL_0475: Unknown result type (might be due to invalid IL or missing references)
				//IL_047f: Expected O, but got Unknown
				//IL_0489: Unknown result type (might be due to invalid IL or missing references)
				//IL_0493: Expected O, but got Unknown
				//IL_049d: Unknown result type (might be due to invalid IL or missing references)
				//IL_04a7: Expected O, but got Unknown
				//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_04bb: Expected O, but got Unknown
				//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_04cf: Expected O, but got Unknown
				//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_04e3: Expected O, but got Unknown
				//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f7: Expected O, but got Unknown
				//IL_0501: Unknown result type (might be due to invalid IL or missing references)
				//IL_050b: Expected O, but got Unknown
				//IL_0515: Unknown result type (might be due to invalid IL or missing references)
				//IL_051f: Expected O, but got Unknown
				//IL_0529: Unknown result type (might be due to invalid IL or missing references)
				//IL_0533: Expected O, but got Unknown
				//IL_053d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0547: Expected O, but got Unknown
				//IL_0551: Unknown result type (might be due to invalid IL or missing references)
				//IL_055b: Expected O, but got Unknown
				//IL_0565: Unknown result type (might be due to invalid IL or missing references)
				//IL_056f: Expected O, but got Unknown
				//IL_0579: Unknown result type (might be due to invalid IL or missing references)
				//IL_0583: Expected O, but got Unknown
				//IL_058d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0597: Expected O, but got Unknown
				//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_05ab: Expected O, but got Unknown
				new CodeTypeReferenceExpression(TypeReferences.DatabasePermissionType);
				Alter = (CodeExpression)new CodeFieldReferenceExpression(Alter, "Alter");
				AlterAnyApplicationRole = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyApplicationRole, "AlterAnyApplicationRole");
				AlterAnyAssembly = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyAssembly, "AlterAnyAssembly");
				AlterAnyAsymmetricKey = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyAsymmetricKey, "AlterAnyAsymmetricKey");
				AlterAnyCertificate = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyCertificate, "AlterAnyCertificate");
				AlterAnyContract = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyContract, "AlterAnyContract");
				AlterAnyDatabaseAudit = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyDatabaseAudit, "AlterAnyDatabaseAudit");
				AlterAnyDatabaseDdlTrigger = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyDatabaseDdlTrigger, "AlterAnyDatabaseDdlTrigger");
				AlterAnyDatabaseEventNotification = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyDatabaseEventNotification, "AlterAnyDatabaseEventNotification");
				AlterAnyDataspace = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyDataspace, "AlterAnyDataspace");
				AlterAnyFulltextCatalog = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyFulltextCatalog, "AlterAnyFulltextCatalog");
				AlterAnyMask = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyMask, "AlterAnyMask");
				AlterAnyMessageType = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyMessageType, "AlterAnyMessageType");
				AlterAnyRemoteServiceBinding = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyRemoteServiceBinding, "AlterAnyRemoteServiceBinding");
				AlterAnyRole = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyRole, "AlterAnyRole");
				AlterAnyRoute = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyRoute, "AlterAnyRoute");
				AlterAnySchema = (CodeExpression)new CodeFieldReferenceExpression(AlterAnySchema, "AlterAnySchema");
				AlterAnySensitivityClassification = (CodeExpression)new CodeFieldReferenceExpression(AlterAnySensitivityClassification, "AlterAnySensitivityClassification");
				AlterAnyService = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyService, "AlterAnyService");
				AlterAnySymmetricKey = (CodeExpression)new CodeFieldReferenceExpression(AlterAnySymmetricKey, "AlterAnySymmetricKey");
				AlterAnyUser = (CodeExpression)new CodeFieldReferenceExpression(AlterAnyUser, "AlterAnyUser");
				Authenticate = (CodeExpression)new CodeFieldReferenceExpression(Authenticate, "Authenticate");
				BackupDatabase = (CodeExpression)new CodeFieldReferenceExpression(BackupDatabase, "BackupDatabase");
				BackupLog = (CodeExpression)new CodeFieldReferenceExpression(BackupLog, "BackupLog");
				Checkpoint = (CodeExpression)new CodeFieldReferenceExpression(Checkpoint, "Checkpoint");
				Connect = (CodeExpression)new CodeFieldReferenceExpression(Connect, "Connect");
				ConnectReplication = (CodeExpression)new CodeFieldReferenceExpression(ConnectReplication, "ConnectReplication");
				Control = (CodeExpression)new CodeFieldReferenceExpression(Control, "Control");
				CreateAggregate = (CodeExpression)new CodeFieldReferenceExpression(CreateAggregate, "CreateAggregate");
				CreateAssembly = (CodeExpression)new CodeFieldReferenceExpression(CreateAssembly, "CreateAssembly");
				CreateAsymmetricKey = (CodeExpression)new CodeFieldReferenceExpression(CreateAsymmetricKey, "CreateAsymmetricKey");
				CreateCertificate = (CodeExpression)new CodeFieldReferenceExpression(CreateCertificate, "CreateCertificate");
				CreateContract = (CodeExpression)new CodeFieldReferenceExpression(CreateContract, "CreateContract");
				CreateDatabase = (CodeExpression)new CodeFieldReferenceExpression(CreateDatabase, "CreateDatabase");
				CreateDatabaseDdlEventNotification = (CodeExpression)new CodeFieldReferenceExpression(CreateDatabaseDdlEventNotification, "CreateDatabaseDdlEventNotification");
				CreateDefault = (CodeExpression)new CodeFieldReferenceExpression(CreateDefault, "CreateDefault");
				CreateFulltextCatalog = (CodeExpression)new CodeFieldReferenceExpression(CreateFulltextCatalog, "CreateFulltextCatalog");
				CreateFunction = (CodeExpression)new CodeFieldReferenceExpression(CreateFunction, "CreateFunction");
				CreateMessageType = (CodeExpression)new CodeFieldReferenceExpression(CreateMessageType, "CreateMessageType");
				CreateProcedure = (CodeExpression)new CodeFieldReferenceExpression(CreateProcedure, "CreateProcedure");
				CreateQueue = (CodeExpression)new CodeFieldReferenceExpression(CreateQueue, "CreateQueue");
				CreateRemoteServiceBinding = (CodeExpression)new CodeFieldReferenceExpression(CreateRemoteServiceBinding, "CreateRemoteServiceBinding");
				CreateRole = (CodeExpression)new CodeFieldReferenceExpression(CreateRole, "CreateRole");
				CreateRoute = (CodeExpression)new CodeFieldReferenceExpression(CreateRoute, "CreateRoute");
				CreateRule = (CodeExpression)new CodeFieldReferenceExpression(CreateRule, "CreateRule");
				CreateSchema = (CodeExpression)new CodeFieldReferenceExpression(CreateSchema, "CreateSchema");
				CreateService = (CodeExpression)new CodeFieldReferenceExpression(CreateService, "CreateService");
				CreateSymmetricKey = (CodeExpression)new CodeFieldReferenceExpression(CreateSymmetricKey, "CreateSymmetricKey");
				CreateSynonym = (CodeExpression)new CodeFieldReferenceExpression(CreateSynonym, "CreateSynonym");
				CreateTable = (CodeExpression)new CodeFieldReferenceExpression(CreateTable, "CreateTable");
				CreateType = (CodeExpression)new CodeFieldReferenceExpression(CreateType, "CreateType");
				CreateView = (CodeExpression)new CodeFieldReferenceExpression(CreateView, "CreateView");
				CreateXmlSchemaCollection = (CodeExpression)new CodeFieldReferenceExpression(CreateXmlSchemaCollection, "CreateXmlSchemaCollection");
				Delete = (CodeExpression)new CodeFieldReferenceExpression(Delete, "Delete");
				Execute = (CodeExpression)new CodeFieldReferenceExpression(Execute, "Execute");
				Impersonate = (CodeExpression)new CodeFieldReferenceExpression(Impersonate, "Impersonate");
				Insert = (CodeExpression)new CodeFieldReferenceExpression(Insert, "Insert");
				Receive = (CodeExpression)new CodeFieldReferenceExpression(Receive, "Receive");
				References = (CodeExpression)new CodeFieldReferenceExpression(References, "References");
				Select = (CodeExpression)new CodeFieldReferenceExpression(Select, "Select");
				Send = (CodeExpression)new CodeFieldReferenceExpression(Send, "Send");
				Showplan = (CodeExpression)new CodeFieldReferenceExpression(Showplan, "Showplan");
				SubscribeQueryNotifications = (CodeExpression)new CodeFieldReferenceExpression(SubscribeQueryNotifications, "SubscribeQueryNotifications");
				TakeOwnership = (CodeExpression)new CodeFieldReferenceExpression(TakeOwnership, "TakeOwnership");
				Unmask = (CodeExpression)new CodeFieldReferenceExpression(Unmask, "Unmask");
				Update = (CodeExpression)new CodeFieldReferenceExpression(Update, "Update");
				ViewAnyColumnEncryptionKeyDefinition = (CodeExpression)new CodeFieldReferenceExpression(ViewAnyColumnEncryptionKeyDefinition, "ViewAnyColumnEncryptionKeyDefinition");
				ViewAnyColumnMasterKeyDefinition = (CodeExpression)new CodeFieldReferenceExpression(ViewAnyColumnMasterKeyDefinition, "ViewAnyColumnMasterKeyDefinition");
				ViewChangeTracking = (CodeExpression)new CodeFieldReferenceExpression(ViewChangeTracking, "ViewChangeTracking");
				ViewAnySensitivityClassification = (CodeExpression)new CodeFieldReferenceExpression(ViewAnySensitivityClassification, "ViewAnySensitivityClassification");
				ViewDatabaseState = (CodeExpression)new CodeFieldReferenceExpression(ViewDatabaseState, "ViewDatabaseState");
				ViewDefinition = (CodeExpression)new CodeFieldReferenceExpression(ViewDefinition, "ViewDefinition");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Alter => Alter, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyApplicationRole => AlterAnyApplicationRole, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyAssembly => AlterAnyAssembly, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyAsymmetricKey => AlterAnyAsymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyCertificate => AlterAnyCertificate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyContract => AlterAnyContract, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyDatabaseAudit => AlterAnyDatabaseAudit, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyDatabaseDdlTrigger => AlterAnyDatabaseDdlTrigger, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyDatabaseEventNotification => AlterAnyDatabaseEventNotification, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyDataspace => AlterAnyDataspace, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyFulltextCatalog => AlterAnyFulltextCatalog, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyMask => AlterAnyMask, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyMessageType => AlterAnyMessageType, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyRemoteServiceBinding => AlterAnyRemoteServiceBinding, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyRole => AlterAnyRole, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyRoute => AlterAnyRoute, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnySchema => AlterAnySchema, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnySensitivityClassification => AlterAnySensitivityClassification, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyService => AlterAnyService, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnySymmetricKey => AlterAnySymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.AlterAnyUser => AlterAnyUser, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Authenticate => Authenticate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.BackupDatabase => BackupDatabase, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.BackupLog => BackupLog, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Checkpoint => Checkpoint, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Connect => Connect, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ConnectReplication => ConnectReplication, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Control => Control, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateAggregate => CreateAggregate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateAssembly => CreateAssembly, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateAsymmetricKey => CreateAsymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateCertificate => CreateCertificate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateContract => CreateContract, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateDatabase => CreateDatabase, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateDatabaseDdlEventNotification => CreateDatabaseDdlEventNotification, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateDefault => CreateDefault, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateFulltextCatalog => CreateFulltextCatalog, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateFunction => CreateFunction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateMessageType => CreateMessageType, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateProcedure => CreateProcedure, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateQueue => CreateQueue, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateRemoteServiceBinding => CreateRemoteServiceBinding, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateRole => CreateRole, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateRoute => CreateRoute, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateRule => CreateRule, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateSchema => CreateSchema, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateService => CreateService, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateSymmetricKey => CreateSymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateSynonym => CreateSynonym, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateTable => CreateTable, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateType => CreateType, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateView => CreateView, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.CreateXmlSchemaCollection => CreateXmlSchemaCollection, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Delete => Delete, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Execute => Execute, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Impersonate => Impersonate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Insert => Insert, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Receive => Receive, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.References => References, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Select => Select, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Send => Send, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Showplan => Showplan, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.SubscribeQueryNotifications => SubscribeQueryNotifications, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.TakeOwnership => TakeOwnership, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Unmask => Unmask, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.Update => Update, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewAnyColumnEncryptionKeyDefinition => ViewAnyColumnEncryptionKeyDefinition, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewAnyColumnMasterKeyDefinition => ViewAnyColumnMasterKeyDefinition, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewAnySensitivityClassification => ViewAnySensitivityClassification, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewChangeTracking => ViewChangeTracking, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewDatabaseState => ViewDatabaseState, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.DatabasePermissionType.ViewDefinition => ViewDefinition, 
					_ => null, 
				});
			}
		}

		public static class PermissionState
		{
			public static CodeExpression Deny;

			public static CodeExpression Grant;

			public static CodeExpression GrantWithGrant;

			public static CodeExpression Revoke;

			static PermissionState()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_0044: Expected O, but got Unknown
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.PermissionState);
				Deny = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Deny");
				Grant = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Grant");
				GrantWithGrant = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "GrantWithGrant");
				Revoke = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Revoke");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.PermissionState value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.PermissionState.Deny => Deny, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.PermissionState.Grant => Grant, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.PermissionState.GrantWithGrant => GrantWithGrant, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.PermissionState.Revoke => Revoke, 
					_ => null, 
				});
			}
		}

		public static class ExecutionContextType
		{
			public static CodeExpression Caller;

			public static CodeExpression ExecuteAsLogin;

			public static CodeExpression ExecuteAsUser;

			public static CodeExpression Owner;

			public static CodeExpression Self;

			static ExecutionContextType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Expected O, but got Unknown
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				//IL_0054: Expected O, but got Unknown
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.ExecutionContextType);
				Caller = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Caller");
				ExecuteAsLogin = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "ExecuteAsLogin");
				ExecuteAsUser = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "ExecuteAsUser");
				Owner = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Owner");
				Self = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Self");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType.Caller => Caller, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType.ExecuteAsLogin => ExecuteAsLogin, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType.ExecuteAsUser => ExecuteAsUser, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType.Owner => Owner, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ExecutionContextType.Self => Self, 
					_ => null, 
				});
			}
		}

		public static class ForeignKeyAction
		{
			public static CodeExpression Cascade;

			public static CodeExpression NoAction;

			public static CodeExpression SetDefault;

			public static CodeExpression SetNull;

			static ForeignKeyAction()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_0044: Expected O, but got Unknown
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.ForeignKeyAction);
				Cascade = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Cascade");
				NoAction = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "NoAction");
				SetDefault = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "SetDefault");
				SetNull = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "SetNull");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.ForeignKeyAction value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.ForeignKeyAction.Cascade => Cascade, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ForeignKeyAction.NoAction => NoAction, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ForeignKeyAction.SetDefault => SetDefault, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.ForeignKeyAction.SetNull => SetNull, 
					_ => null, 
				});
			}
		}

		public static class LoginType
		{
			public static CodeExpression AsymmetricKey;

			public static CodeExpression Certificate;

			public static CodeExpression Sql;

			public static CodeExpression Windows;

			static LoginType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_0044: Expected O, but got Unknown
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.LoginType);
				AsymmetricKey = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "AsymmetricKey");
				Certificate = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Certificate");
				Sql = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Sql");
				Windows = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Windows");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.LoginType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.LoginType.AsymmetricKey => AsymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.LoginType.Certificate => Certificate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.LoginType.Sql => Sql, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.LoginType.Windows => Windows, 
					_ => null, 
				});
			}
		}

		public static class SortOrder
		{
			public static CodeExpression Ascending;

			public static CodeExpression Descending;

			static SortOrder()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_0024: Expected O, but got Unknown
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.SortOrder);
				Ascending = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Ascending");
				Descending = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Descending");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.SortOrder value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.SortOrder.Ascending => Ascending, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.SortOrder.Descending => Descending, 
					_ => null, 
				});
			}
		}

		public static class UserType
		{
			public static CodeExpression AsymmetricKey;

			public static CodeExpression Certificate;

			public static CodeExpression NoLogin;

			public static CodeExpression SqlLogin;

			public static CodeExpression External;

			static UserType()
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Expected O, but got Unknown
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				//IL_0054: Expected O, but got Unknown
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Expected O, but got Unknown
				CodeTypeReferenceExpression val = new CodeTypeReferenceExpression(TypeReferences.UserType);
				AsymmetricKey = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "AsymmetricKey");
				Certificate = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "Certificate");
				NoLogin = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "NoLogin");
				SqlLogin = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "SqlLogin");
				External = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)val, "External");
			}

			public static CodeExpression GetValuefExpression(Microsoft.SqlServer.Management.SqlParser.Metadata.UserType value)
			{
				return (CodeExpression)(value switch
				{
					Microsoft.SqlServer.Management.SqlParser.Metadata.UserType.AsymmetricKey => AsymmetricKey, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.UserType.Certificate => Certificate, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.UserType.NoLogin => NoLogin, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.UserType.SqlLogin => SqlLogin, 
					Microsoft.SqlServer.Management.SqlParser.Metadata.UserType.External => External, 
					_ => null, 
				});
			}
		}
	}

	public static class Expressions
	{
		public static readonly CodeExpression False;

		public static readonly CodeExpression Null;

		public static readonly CodeExpression This;

		public static readonly CodeExpression True;

		public static readonly CodeExpression Zero;

		static Expressions()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			False = (CodeExpression)new CodePrimitiveExpression((object)false);
			Null = (CodeExpression)new CodePrimitiveExpression((object)null);
			This = (CodeExpression)new CodeThisReferenceExpression();
			True = (CodeExpression)new CodePrimitiveExpression((object)true);
			Zero = (CodeExpression)new CodePrimitiveExpression((object)0);
		}
	}

	public static class VariableReferences
	{
		public static readonly CodeVariableReferenceExpression DataIndex;

		public static readonly CodeVariableReferenceExpression Index;

		public static readonly CodeVariableReferenceExpression Length;

		public static readonly CodeVariableReferenceExpression MetadataProvider;

		public static readonly CodeVariableReferenceExpression Model;

		public static readonly CodeVariableReferenceExpression Name;

		public static readonly CodeVariableReferenceExpression ObjectId;

		public static readonly CodeVariableReferenceExpression Offset;

		public static readonly CodeVariableReferenceExpression TypeId;

		static VariableReferences()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			Length = new CodeVariableReferenceExpression("length");
			Index = new CodeVariableReferenceExpression("index");
			MetadataProvider = new CodeVariableReferenceExpression("metadataProvider");
			Model = new CodeVariableReferenceExpression("model");
			Name = new CodeVariableReferenceExpression("name");
			ObjectId = new CodeVariableReferenceExpression("objectId");
			Offset = new CodeVariableReferenceExpression("offset");
			TypeId = new CodeVariableReferenceExpression("typeId");
			DataIndex = new CodeVariableReferenceExpression("dataIndex");
		}
	}
}
