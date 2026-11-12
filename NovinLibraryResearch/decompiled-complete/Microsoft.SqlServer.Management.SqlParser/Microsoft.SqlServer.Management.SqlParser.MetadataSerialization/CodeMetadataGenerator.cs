using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class CodeMetadataGenerator : MetadataGenerator
{
	private enum IdLookupGenOption
	{
		Switch,
		Table
	}

	private struct IdValuePair : IComparable<IdValuePair>
	{
		public readonly int Id;

		public readonly string Value;

		public IdValuePair(int id, string value)
		{
			Id = id;
			Value = value;
		}

		public int CompareTo(IdValuePair other)
		{
			return Id - other.Id;
		}
	}

	private struct ValuePair<T1, T2>
	{
		public readonly T1 Value1;

		public readonly T2 Value2;

		public ValuePair(T1 value1, T2 value2)
		{
			Value1 = value1;
			Value2 = value2;
		}
	}

	private delegate string GenItemExpression<T>(T obj);

	private readonly CodeGeneratorSettings settings;

	private readonly List<CodeTypeGenerator> typeGenerators;

	private CodeMetadataGenerator(ISerializationContext serializationContext, CodeGeneratorSettings settings)
		: base(serializationContext)
	{
		this.settings = settings;
		typeGenerators = new List<CodeTypeGenerator>();
	}

	public static void GenerateCode(ISerializationContext serializationContext, CodeGeneratorSettings settings)
	{
		new CodeMetadataGenerator(serializationContext, settings).GenerateCode();
	}

	private CodeTypeGenerator CreateAndAddTypeGenerator(ITypeInfo typeInfo)
	{
		CodeTypeGenerator codeTypeGenerator = new CodeTypeGenerator(typeInfo);
		typeGenerators.Add(codeTypeGenerator);
		return codeTypeGenerator;
	}

	private void GenerateCode()
	{
		SortedList<int, TypeBucket> sortedList = new SortedList<int, TypeBucket>();
		int[] array = new int[base.ObjectCount];
		int[] array2 = new int[base.ObjectCount];
		List<uint> list = new List<uint>(2 * base.ObjectCount);
		BucketIdGenerator bucketIdGenerator = new BucketIdGenerator();
		foreach (TypeDataTable dataTable in base.DataTables)
		{
			BitWidthTypeCompactor bitWidthTypeCompactor = new BitWidthTypeCompactor(dataTable, bucketIdGenerator);
			foreach (TypeDataTable.Row row in dataTable.Rows)
			{
				TypeBucket typeBucket = bitWidthTypeCompactor.RegisterDataRow(row.Id);
				if (!sortedList.ContainsKey(typeBucket.Id))
				{
					sortedList.Add(typeBucket.Id, typeBucket);
					TypeGenerator typeGenerator = CreateAndAddTypeGenerator(typeBucket);
					typeBucket.GenerateType(typeGenerator, row.Id);
				}
				array[row.Id] = typeBucket.Id;
				array2[row.Id] = list.Count;
				list.AddRange(typeBucket.GetPackedValues(row.Id));
			}
		}
		CodeDomProvider provider = CodeDomProvider.CreateProvider("cs");
		CodeNamespace root = GenerateMainNamespace();
		GenerateCode(provider, root, settings.GetMainFilePath());
		IntegerSequentialValueList metadataIdToTypeIdMap = new IntegerSequentialValueList(array, bucketIdGenerator.CurrentId);
		IntegerSequentialValueList metadataIdToDataIndexMap = new IntegerSequentialValueList(array2, list.Count);
		ISequentialValueList<uint> objectDataTable = new SequentialValueList<uint>(list);
		CodeNamespace root2 = GenerateTypeNamespace(metadataIdToTypeIdMap, metadataIdToDataIndexMap, objectDataTable);
		GenerateCode(provider, root2, settings.GetTypesFilePath());
		CodeNamespace root3 = GenerateObjectLookupNamespace();
		GenerateCode(provider, root3, settings.GetObjectsFilePath());
		CodeNamespace root4 = GenerateCollectionLookupNamespace(settings);
		GenerateCode(provider, root4, settings.GetCollectionsFilePath());
		CodeNamespace root5 = GenerateStringLookupNamespace();
		GenerateCode(provider, root5, settings.GetStringsFilePath());
	}

	private void GenerateCode(CodeDomProvider provider, CodeNamespace root, string path)
	{
		using StreamWriter streamWriter = new StreamWriter(path);
		GenerateCode(provider, root, streamWriter);
		streamWriter.Flush();
	}

	private void GenerateCode(CodeDomProvider provider, CodeNamespace root, StreamWriter writer)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		CodeCompileUnit val = new CodeCompileUnit();
		val.Namespaces.Add(root);
		provider.GenerateCodeFromCompileUnit(val, (TextWriter)writer, (CodeGeneratorOptions)null);
	}

	private CodeNamespace GenerateMainNamespace()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_01f9: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		CodeNamespace val = CreateNamespace();
		CodeTypeDeclaration val2 = CreatePartialTypeDeclaration();
		val2.BaseTypes.Add(CodeConfiguration.TypeReferences.ObjectStore);
		val2.IsPartial = true;
		((CodeTypeMember)val2).Attributes = (MemberAttributes)3;
		val.Types.Add(val2);
		CodeMemberField val3 = new CodeMemberField(new CodeTypeReference(settings.GetObjectStoreTypeName()), "instance");
		((CodeTypeMember)val3).Attributes = (MemberAttributes)20483;
		val2.Members.Add((CodeTypeMember)(object)val3);
		CodeMemberProperty val4 = new CodeMemberProperty();
		((CodeTypeMember)val4).Name = "Instance";
		val4.Type = val3.Type;
		val4.HasGet = true;
		val4.HasSet = false;
		val4.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)null, ((CodeTypeMember)val3).Name)));
		((CodeTypeMember)val4).Attributes = (MemberAttributes)24579;
		val2.Members.Add((CodeTypeMember)(object)val4);
		ValuePair<string, int>[] array = new ValuePair<string, int>[3]
		{
			new ValuePair<string, int>("MetadataObjectCount", base.ObjectCount),
			new ValuePair<string, int>("MetadataCollectionCount", base.CollectionCount),
			new ValuePair<string, int>("RootObjectId", base.RootObjectId)
		};
		for (int i = 0; i < array.Length; i++)
		{
			ValuePair<string, int> valuePair = array[i];
			CodeMemberProperty val5 = new CodeMemberProperty();
			((CodeTypeMember)val5).Name = valuePair.Value1;
			val5.Type = CodeConfiguration.TypeReferences.Int;
			val5.HasGet = true;
			val5.HasSet = false;
			val5.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodePrimitiveExpression((object)valuePair.Value2)));
			((CodeTypeMember)val5).Attributes = (MemberAttributes)24578;
			val2.Members.Add((CodeTypeMember)(object)val5);
		}
		CodeConstructor val6 = new CodeConstructor();
		((CodeTypeMember)val6).Attributes = (MemberAttributes)20480;
		val2.Members.Add((CodeTypeMember)(object)val6);
		CodeTypeConstructor val7 = new CodeTypeConstructor();
		((CodeMemberMethod)val7).Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)null, ((CodeTypeMember)val3).Name), (CodeExpression)new CodeObjectCreateExpression(((CodeTypeMember)val2).Name, Array.Empty<CodeExpression>())));
		val2.Members.Add((CodeTypeMember)(object)val7);
		return val;
	}

	private CodeNamespace GenerateTypeNamespace(IntegerSequentialValueList metadataIdToTypeIdMap, IntegerSequentialValueList metadataIdToDataIndexMap, ISequentialValueList<uint> objectDataTable)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		CodeNamespace val = CreateNamespace();
		CodeTypeDeclaration val2 = CreatePartialTypeDeclaration();
		val.Types.Add(val2);
		IdValuePair[] array = new IdValuePair[typeGenerators.Count];
		for (int i = 0; i < typeGenerators.Count; i++)
		{
			CodeTypeGenerator codeTypeGenerator = typeGenerators[i];
			string value = string.Format(CultureInfo.InvariantCulture, "new {0}({1}, {2})", codeTypeGenerator.TypeInfo.Name, "model", "dataIndex");
			array[i] = new IdValuePair(codeTypeGenerator.TypeInfo.Id, value);
			codeTypeGenerator.AddToType(val2);
		}
		Array.Sort(array);
		CodeMemberMethod val3 = new CodeMemberMethod();
		((CodeTypeMember)val3).Name = "CreateMetadataObject";
		val3.Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.MetadataModel, "model"));
		val3.Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.Int, "objectId"));
		val3.ReturnType = CodeConfiguration.TypeReferences.MetadataObject;
		((CodeTypeMember)val3).Attributes = (MemberAttributes)24578;
		val3.Statements.Add((CodeStatement)new CodeVariableDeclarationStatement(CodeConfiguration.TypeReferences.Int, "typeId", (CodeExpression)new CodeArrayIndexerExpression((CodeExpression)new CodeFieldReferenceExpression(CodeConfiguration.Expressions.This, "idToTypeIdMap"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)CodeConfiguration.VariableReferences.ObjectId })));
		val3.Statements.Add((CodeStatement)new CodeVariableDeclarationStatement(CodeConfiguration.TypeReferences.Int, "dataIndex", (CodeExpression)new CodeArrayIndexerExpression((CodeExpression)new CodeFieldReferenceExpression(CodeConfiguration.Expressions.This, "idToDataIndexMap"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)CodeConfiguration.VariableReferences.ObjectId })));
		val3.Statements.Add(CreateSwitchStatement("typeId", array, 3));
		val2.Members.Add((CodeTypeMember)(object)val3);
		ValuePair<IntegerSequentialValueList, string>[] array2 = new ValuePair<IntegerSequentialValueList, string>[2]
		{
			new ValuePair<IntegerSequentialValueList, string>(metadataIdToTypeIdMap, "idToTypeIdMap"),
			new ValuePair<IntegerSequentialValueList, string>(metadataIdToDataIndexMap, "idToDataIndexMap")
		};
		for (int j = 0; j < array2.Length; j++)
		{
			ValuePair<IntegerSequentialValueList, string> valuePair = array2[j];
			CodeTypeReference itemType;
			int perLineCount;
			if (valuePair.Value1.MaxValue <= 255)
			{
				itemType = CodeConfiguration.TypeReferences.Byte;
				perLineCount = 64;
			}
			else if (valuePair.Value1.MaxValue <= 65535)
			{
				itemType = CodeConfiguration.TypeReferences.UShort;
				perLineCount = 32;
			}
			else
			{
				itemType = CodeConfiguration.TypeReferences.Int;
				perLineCount = 16;
			}
			string itemFormat = "{0," + SerializationUtils.GetDigitCount(valuePair.Value1.MaxValue).ToString(CultureInfo.InvariantCulture) + "}";
			CodeMemberField val4 = CreateArrayMemberField(valuePair.Value2, itemType, valuePair.Value1, itemFormat, perLineCount);
			val2.Members.Add((CodeTypeMember)(object)val4);
		}
		CodeMemberField val5 = CreateArrayMemberField("metadataObjectData", CodeConfiguration.TypeReferences.UInt, objectDataTable, "0x{0:x8}", 16);
		val2.Members.Add((CodeTypeMember)(object)val5);
		CodeMemberMethod val6 = new CodeMemberMethod();
		((CodeTypeMember)val6).Name = "GetMetadataObjectData";
		val6.Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.Int, "index"));
		val6.ReturnType = CodeConfiguration.TypeReferences.UInt;
		((CodeTypeMember)val6).Attributes = (MemberAttributes)24578;
		val6.Statements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeArrayIndexerExpression((CodeExpression)new CodeFieldReferenceExpression(CodeConfiguration.Expressions.This, ((CodeTypeMember)val5).Name), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)CodeConfiguration.VariableReferences.Index })));
		val2.Members.Add((CodeTypeMember)(object)val6);
		return val;
	}

	private CodeNamespace GenerateObjectLookupNamespace()
	{
		CodeNamespace obj = CreateNamespace();
		CodeTypeDeclaration val = CreatePartialTypeDeclaration();
		obj.Types.Add(val);
		GenerateLookupTableForByteArray(val, IdLookupGenOption.Switch);
		GenerateLookupTableForCollationInfo(val, IdLookupGenOption.Switch);
		GenerateLookupTableForComputedColumnInfo(val, IdLookupGenOption.Switch);
		GenerateLookupTableForDataTypeSpec(val, IdLookupGenOption.Switch);
		GenerateLookupTableForIdentityColumnInfo(val, IdLookupGenOption.Switch);
		GenerateLookupTableForInt(val, IdLookupGenOption.Table);
		GenerateLookupTableForPassword(val, IdLookupGenOption.Switch);
		return obj;
	}

	private CodeNamespace GenerateCollectionLookupNamespace(CodeGeneratorSettings settings)
	{
		CodeNamespace obj = CreateNamespace();
		CodeTypeDeclaration val = CreatePartialTypeDeclaration();
		obj.Types.Add(val);
		GenerateLookupTableForCollectionIdList(val, settings);
		return obj;
	}

	private CodeNamespace GenerateStringLookupNamespace()
	{
		CodeNamespace obj = CreateNamespace();
		CodeTypeDeclaration val = CreatePartialTypeDeclaration();
		obj.Types.Add(val);
		GenerateLookupTableForString(val, IdLookupGenOption.Table);
		return obj;
	}

	private void GenerateLookupTableForByteArray(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<byte[]> createExprGenerator = (byte[] obj) => "null";
		AddLookupMethod(typeDeclaration, "GetByteArrayById", "byteArrayLookup", base.ByteArrayTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForCollationInfo(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<CollationInfo> createExprGenerator = (CollationInfo obj) => (obj == null) ? "null" : string.Format(CultureInfo.InvariantCulture, "CollationInfo.{0}(\"{1}\")", "GetCollationInfo", obj.Name);
		AddLookupMethod(typeDeclaration, "GetCollationInfoById", "collationInfoLookup", base.CollationInfoTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForCollectionIdList(CodeTypeDeclaration typeDeclaration, CodeGeneratorSettings settings)
	{
		AddLookupMethod(typeDeclaration, "GetCollectionIdListById", "collectionIdListLookup", base.CollectionIdListTable, null, IdLookupGenOption.Table);
		GenerateResource("collectionIdListLookup", base.CollectionIdListTable, settings);
	}

	private void GenerateLookupTableForComputedColumnInfo(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<ComputedColumnInfo> createExprGenerator = (ComputedColumnInfo obj) => (obj == null) ? "null" : string.Format(CultureInfo.InvariantCulture, "new ComputedColumnInfo(\"{0}\", {1})", obj.Text, obj.IsPersisted ? "true" : "false");
		AddLookupMethod(typeDeclaration, "GetComputedColumnInfoById", "computedColumnInfoLookup", base.ComputedColumnInfoTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForDataTypeSpec(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<DataTypeSpec> createExprGenerator = (DataTypeSpec obj) => (obj == null) ? "null" : string.Format(CultureInfo.InvariantCulture, "DataTypeSpec.{0}(\"{1}\")", "GetDataTypeSpec", obj.Name);
		AddLookupMethod(typeDeclaration, "GetDataTypeSpecById", "dataTypeSpecLookup", base.DataTypeSpecTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForIdentityColumnInfo(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<IdentityColumnInfo> createExprGenerator = (IdentityColumnInfo obj) => (obj == null) ? "null" : string.Format(CultureInfo.InvariantCulture, "new IdentityColumnInfo({0}, {1})", obj.Seed, obj.Increment);
		AddLookupMethod(typeDeclaration, "GetIdentityColumnInfoById", "identityColumnInfoLookup", base.IdentityColumnInfoTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForInt(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<int> createExprGenerator = (int obj) => obj.ToString(CultureInfo.InvariantCulture);
		AddLookupMethod(typeDeclaration, "GetIntById", "intLookup", base.IntTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForPassword(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<IPassword> createExprGenerator = (IPassword obj) => "null";
		AddLookupMethod(typeDeclaration, "GetPasswordById", "passwordLookup", base.PasswordTable, createExprGenerator, genOption);
	}

	private void GenerateLookupTableForString(CodeTypeDeclaration typeDeclaration, IdLookupGenOption genOption)
	{
		GenItemExpression<string> createExprGenerator = (string obj) => (obj == null) ? "null" : ("@\"" + obj.Replace("\"", "\"\"") + "\"");
		AddLookupMethod(typeDeclaration, "GetStringById", "stringLookup", base.StringTable, createExprGenerator, genOption);
	}

	private void AddLookupMethod<T>(CodeTypeDeclaration typeDeclaration, string methodName, string fieldName, ISequentialValueList<T> items, GenItemExpression<T> createExprGenerator, IdLookupGenOption genOption)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		string text = "id";
		CodeTypeReference val = new CodeTypeReference(typeof(T).Name);
		CodeMemberMethod val2 = new CodeMemberMethod();
		((CodeTypeMember)val2).Name = methodName;
		val2.Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.Int, text));
		val2.ReturnType = val;
		((CodeTypeMember)val2).Attributes = (MemberAttributes)24578;
		typeDeclaration.Members.Add((CodeTypeMember)(object)val2);
		if (items != null && items.Count > 0)
		{
			switch (genOption)
			{
			case IdLookupGenOption.Switch:
			{
				IdValuePair[] array2 = new IdValuePair[items.Count];
				int num2 = 0;
				foreach (T item in items)
				{
					array2[num2] = new IdValuePair(num2, createExprGenerator(item));
					num2++;
				}
				val2.Statements.Add(CreateSwitchStatement(text, array2, 3));
				break;
			}
			case IdLookupGenOption.Table:
			{
				CodeVariableReferenceExpression val3 = new CodeVariableReferenceExpression(text);
				CodeMemberField val4 = new CodeMemberField(new CodeTypeReference(val, 1), fieldName);
				if (createExprGenerator != null)
				{
					CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[items.Count];
					int num = 0;
					foreach (T item2 in items)
					{
						array[num++] = (CodeExpression)new CodeSnippetExpression(createExprGenerator(item2));
					}
					val4.InitExpression = (CodeExpression)new CodeArrayCreateExpression(new CodeTypeReference(val, 1), array);
				}
				else
				{
					val2.Statements.Add((CodeStatement)new CodeExpressionStatement((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Init" + fieldName), Array.Empty<CodeExpression>())));
				}
				typeDeclaration.Members.Add((CodeTypeMember)(object)val4);
				val2.Statements.Add((CodeStatement)new CodeExpressionStatement((CodeExpression)new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.DebugAssert, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)(object)val3, (CodeBinaryOperatorType)16, CodeConfiguration.Expressions.Zero) })));
				val2.Statements.Add((CodeStatement)new CodeExpressionStatement((CodeExpression)new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.DebugAssert, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)(object)val3, (CodeBinaryOperatorType)13, (CodeExpression)new CodePrimitiveExpression((object)items.Count)) })));
				val2.Statements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeArrayIndexerExpression((CodeExpression)new CodeFieldReferenceExpression(CodeConfiguration.Expressions.This, fieldName), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)val3 })));
				break;
			}
			}
		}
		else
		{
			val2.Statements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeDefaultValueExpression(val)));
		}
	}

	private void GenerateResource(string resourceName, ISequentialValueList<int[]> values, CodeGeneratorSettings settings)
	{
		using BinaryWriter binaryWriter = GetBinaryWriterForResource(settings.GetResourcePath(resourceName));
		binaryWriter.Write(values.Count);
		foreach (int[] value2 in values)
		{
			int value = ((value2 == null) ? (-1) : value2.Length);
			binaryWriter.Write(value);
			if (value2 != null)
			{
				for (int i = 0; i < value2.Length; i++)
				{
					binaryWriter.Write(value2[i]);
				}
			}
		}
	}

	private static BinaryWriter GetBinaryWriterForResource(string filePath)
	{
		return new BinaryWriter(File.Create(filePath));
	}

	private CodeTypeDeclaration CreatePartialTypeDeclaration()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		return new CodeTypeDeclaration(settings.GetObjectStoreTypeName())
		{
			Attributes = (MemberAttributes)16387,
			TypeAttributes = TypeAttributes.Sealed,
			IsPartial = true
		};
	}

	private CodeNamespace CreateNamespace()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Expected O, but got Unknown
		CodeNamespace val = new CodeNamespace(settings.GetCodeNamespace());
		val.Imports.Add(new CodeNamespaceImport("System"));
		val.Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
		val.Imports.Add(new CodeNamespaceImport("Microsoft.SqlServer.Management.SqlParser.Common"));
		val.Imports.Add(new CodeNamespaceImport("Microsoft.SqlServer.Management.SqlParser.Metadata"));
		val.Imports.Add(new CodeNamespaceImport("Microsoft.SqlServer.Management.SqlParser.MetadataProvider"));
		return val;
	}

	private static CodeStatement CreateSwitchStatement(string variableName, IdValuePair[] items, int indentLevel)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		CodeWriter codeWriter = new CodeWriter();
		codeWriter.IndentLevel += indentLevel;
		codeWriter.WriteLine("switch({0})", variableName);
		codeWriter.BeginCodeBlock();
		for (int i = 0; i < items.Length; i++)
		{
			codeWriter.WriteLine("case {0}:", items[i].Id);
			codeWriter.IndentLevel++;
			codeWriter.WriteLine("return {0};", items[i].Value);
			codeWriter.IndentLevel--;
		}
		codeWriter.WriteLine("default:");
		codeWriter.IndentLevel++;
		codeWriter.WriteLine("return null;");
		codeWriter.IndentLevel--;
		codeWriter.EndCodeBlock();
		codeWriter.IndentLevel -= indentLevel;
		return (CodeStatement)new CodeSnippetStatement(codeWriter.ToString());
	}

	private static CodeMemberField CreateArrayMemberField<T>(string fieldName, CodeTypeReference itemType, ISequentialValueList<T> items, string itemFormat, int perLineCount)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		GenItemExpression<T> createExprGenerator = (T obj) => string.Format(CultureInfo.InvariantCulture, itemFormat, obj);
		CodeArrayCreateExpression val = CreateArrayCreateExpression(itemType, items, createExprGenerator, perLineCount, 3);
		return new CodeMemberField(new CodeTypeReference(val.CreateType, 1), fieldName)
		{
			InitExpression = (CodeExpression)(object)val
		};
	}

	private static CodeArrayCreateExpression CreateArrayCreateExpression<T>(CodeTypeReference itemType, ISequentialValueList<T> items, GenItemExpression<T> createExprGenerator, int perLineCount, int indentLevel)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		CodeWriter codeWriter = new CodeWriter();
		codeWriter.IndentLevel += indentLevel;
		int num = 0;
		foreach (T item in items)
		{
			if (num++ % perLineCount == 0)
			{
				codeWriter.WriteLine();
			}
			codeWriter.Write(createExprGenerator(item) + ", ");
		}
		codeWriter.IndentLevel -= indentLevel;
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(codeWriter.ToString()) };
		return new CodeArrayCreateExpression(itemType, array);
	}
}
