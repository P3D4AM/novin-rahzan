using System;
using System.CodeDom;
using System.Reflection;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class CodeTypeGenerator : TypeGenerator
{
	private readonly ITypeInfo typeInfo;

	private readonly CodeTypeDeclaration typeDeclaration;

	private readonly CodeConstructor typeCtor;

	public ITypeInfo TypeInfo => typeInfo;

	public CodeTypeGenerator(ITypeInfo typeInfo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		this.typeInfo = typeInfo;
		typeDeclaration = new CodeTypeDeclaration(typeInfo.Name);
		typeDeclaration.TypeAttributes = TypeAttributes.NestedPrivate | TypeAttributes.Sealed;
		typeDeclaration.BaseTypes.Add(typeInfo.BaseName);
		typeCtor = new CodeConstructor();
		((CodeMemberMethod)typeCtor).Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.MetadataModel, CodeConfiguration.VariableReferences.Model.VariableName));
		((CodeMemberMethod)typeCtor).Parameters.Add(new CodeParameterDeclarationExpression(CodeConfiguration.TypeReferences.Int, CodeConfiguration.VariableReferences.DataIndex.VariableName));
		typeCtor.BaseConstructorArgs.Add((CodeExpression)(object)CodeConfiguration.VariableReferences.Model);
		typeCtor.BaseConstructorArgs.Add((CodeExpression)(object)CodeConfiguration.VariableReferences.DataIndex);
		((CodeTypeMember)typeCtor).Attributes = (MemberAttributes)24576;
		typeDeclaration.Members.Add((CodeTypeMember)(object)typeCtor);
	}

	public void AddToNamespace(CodeNamespace codeNamespace)
	{
		codeNamespace.Types.Add(typeDeclaration);
	}

	public void AddToType(CodeTypeDeclaration typeDeclaration)
	{
		typeDeclaration.Members.Add((CodeTypeMember)(object)this.typeDeclaration);
	}

	public CodeObjectCreateExpression GenerateCreateExpression()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		return new CodeObjectCreateExpression(((CodeMemberMethod)typeCtor).ReturnType, (CodeExpression[])(object)new CodeExpression[2]
		{
			(CodeExpression)CodeConfiguration.VariableReferences.Model,
			(CodeExpression)CodeConfiguration.VariableReferences.DataIndex
		});
	}

	private void AddValueProperty(string propertyName, bool propertyValue)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression valueExpr = (CodeExpression)new CodePrimitiveExpression((object)propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Bool, valueExpr, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, byte propertyValue)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression valueExpr = (CodeExpression)new CodePrimitiveExpression((object)propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Byte, valueExpr, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, short propertyValue)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression valueExpr = (CodeExpression)new CodePrimitiveExpression((object)propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Short, valueExpr, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, int propertyValue)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression valueExpr = (CodeExpression)new CodePrimitiveExpression((object)propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Int, valueExpr, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, DatabaseCompatibilityLevel propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.DatabaseCompatibilityLevel.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.DatabaseCompatibilityLevel, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, ExecutionContextType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.ExecutionContextType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.ExecutionContextType, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, ForeignKeyAction propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.ForeignKeyAction.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.ForeignKeyAction, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, LoginType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.LoginType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.LoginType, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, SortOrder propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.SortOrder.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.SortOrder, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, UserType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.UserType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.UserType, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, DatabasePermissionType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.DatabasePermissionType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.DatabasePermissionType, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, PermissionState propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.PermissionState.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.PermissionState, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, ActivationOrder propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.ActivationOrder.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.ActivationOrder, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, GridDensity propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.GridDensity.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.GridDensity, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, StatisticsType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.StatisticsType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.StatisticsType, valuefExpression, (MemberAttributes)4);
	}

	private void AddValueProperty(string propertyName, SynonymBaseType propertyValue)
	{
		CodeExpression valuefExpression = CodeConfiguration.EnumValues.SynonymBaseType.GetValuefExpression(propertyValue);
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.SynonymBaseType, valuefExpression, (MemberAttributes)4);
	}

	private void AddRefPropertyAsByteArray(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsByteArray(propertyName, idExpr);
	}

	private void AddRefPropertyAsByteArray(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetByteArrayById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.ByteArray, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsCollationInfo(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsCollationInfo(propertyName, idExpr);
	}

	private void AddRefPropertyAsCollationInfo(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetCollationInfoById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.CollationInfo, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsComputedColumnInfo(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsComputedColumnInfo(propertyName, idExpr);
	}

	private void AddRefPropertyAsComputedColumnInfo(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetComputedColumnInfoById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.ComputedColumnInfo, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsDataTypeSpec(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsDataTypeSpec(propertyName, idExpr);
	}

	private void AddRefPropertyAsDataTypeSpec(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetDataTypeSpecById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.DataTypeSpec, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsIdentityColumnInfo(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsIdentityColumnInfo(propertyName, idExpr);
	}

	private void AddRefPropertyAsIdentityColumnInfo(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetIdentityColumnInfoById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.IdentityColumnInfo, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsInt(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsInt(propertyName, idExpr);
	}

	private void AddRefPropertyAsInt(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetIntById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Int, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsPassword(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsPassword(propertyName, idExpr);
	}

	private void AddRefPropertyAsPassword(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetPasswordById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.Password, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddRefPropertyAsString(string propertyName, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddRefPropertyAsString(propertyName, idExpr);
	}

	private void AddRefPropertyAsString(string propertyName, CodeExpression idExpr)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetStringById, (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, CodeConfiguration.TypeReferences.String, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddMetadataRefProperty(string propertyName, Type type, int id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddMetadataRefProperty(propertyName, type, idExpr);
	}

	private void AddMetadataRefProperty(string propertyName, Type type, CodeExpression idExpr)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeTypeReference propertyType = new CodeTypeReference(type.Name);
		AddMetadataRefProperty(propertyName, propertyType, idExpr);
	}

	private void AddMetadataRefProperty(string propertyName, CodeTypeReference propertyType, CodeExpression idExpr)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(CodeConfiguration.Expressions.This, "GetMetadataObjectById", (CodeTypeReference[])(object)new CodeTypeReference[1] { propertyType }), (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, propertyType, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	private void AddMetadataCollection(string propertyName, Type type, int id, bool isOrdered)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeExpression idExpr = (CodeExpression)new CodePrimitiveExpression((object)id);
		AddMetadataCollection(propertyName, type, idExpr, isOrdered);
	}

	private void AddMetadataCollection(string propertyName, Type type, CodeExpression idExpr, bool isOrdered)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		CodeTypeReference itemType = new CodeTypeReference(type.Name);
		AddMetadataCollection(propertyName, itemType, idExpr, isOrdered);
	}

	private void AddMetadataCollection(string propertyName, CodeTypeReference itemType, CodeExpression idExpr, bool isOrdered)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		CodeTypeReference propertyType = new CodeTypeReference(isOrdered ? "IMetadataOrderedCollection" : "IMetadataCollection", (CodeTypeReference[])(object)new CodeTypeReference[1] { itemType });
		string text = (isOrdered ? "GetOrderedCollectionById" : "GetCollectionById");
		CodeMethodInvokeExpression valueExpr = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(CodeConfiguration.Expressions.This, text, (CodeTypeReference[])(object)new CodeTypeReference[1] { itemType }), (CodeExpression[])(object)new CodeExpression[1] { idExpr });
		AddCodeMemberProperty(propertyName, propertyType, (CodeExpression)(object)valueExpr, (MemberAttributes)4);
	}

	public override void AddFinalProperty(MetadataPropertyInfo propertyInfo, int value)
	{
		Microsoft.SqlServer.Management.SqlParser.Metadata.TypeInfo returnType = propertyInfo.ReturnType;
		if (returnType.IsMetadata)
		{
			AddMetadataRefProperty(propertyInfo.Name, returnType.Type, value);
			return;
		}
		if (returnType.IsCollection)
		{
			CollectionTypeInfo obj = (CollectionTypeInfo)returnType;
			bool isOrdered = obj.IsOrdered;
			Microsoft.SqlServer.Management.SqlParser.Metadata.TypeInfo elementType = obj.ElementType;
			AddMetadataCollection(propertyInfo.Name, elementType.Type, value, isOrdered);
			return;
		}
		switch (returnType.Id)
		{
		case 1:
			AddValueProperty(propertyInfo.Name, value == 1);
			break;
		case 9:
			AddValueProperty(propertyInfo.Name, (DatabaseCompatibilityLevel)value);
			break;
		case 12:
			AddValueProperty(propertyInfo.Name, (ExecutionContextType)value);
			break;
		case 13:
			AddValueProperty(propertyInfo.Name, (ForeignKeyAction)value);
			break;
		case 141:
			AddValueProperty(propertyInfo.Name, (LoginType)value);
			break;
		case 144:
			AddValueProperty(propertyInfo.Name, (SortOrder)value);
			break;
		case 149:
			AddValueProperty(propertyInfo.Name, (UserType)value);
			break;
		case 10:
			AddValueProperty(propertyInfo.Name, (DatabasePermissionType)value);
			break;
		case 142:
			AddValueProperty(propertyInfo.Name, (PermissionState)value);
			break;
		case 0:
			AddValueProperty(propertyInfo.Name, (ActivationOrder)value);
			break;
		case 14:
			AddValueProperty(propertyInfo.Name, (GridDensity)value);
			break;
		case 145:
			AddValueProperty(propertyInfo.Name, (StatisticsType)value);
			break;
		case 147:
			AddValueProperty(propertyInfo.Name, (SynonymBaseType)value);
			break;
		case 3:
			AddRefPropertyAsByteArray(propertyInfo.Name, value);
			break;
		case 5:
			AddRefPropertyAsCollationInfo(propertyInfo.Name, value);
			break;
		case 6:
			AddRefPropertyAsComputedColumnInfo(propertyInfo.Name, value);
			break;
		case 8:
			AddRefPropertyAsDataTypeSpec(propertyInfo.Name, value);
			break;
		case 137:
			AddRefPropertyAsIdentityColumnInfo(propertyInfo.Name, value);
			break;
		case 139:
			AddRefPropertyAsInt(propertyInfo.Name, value);
			break;
		case 89:
			AddRefPropertyAsPassword(propertyInfo.Name, value);
			break;
		case 146:
			AddRefPropertyAsString(propertyInfo.Name, value);
			break;
		}
	}

	public override void AddIndirectProperty(MetadataPropertyInfo propertyInfo, int fieldNumber, int startOffset, int length)
	{
		CodeExpression idExpr = (CodeExpression)(object)CreateGetPackedValueExpr(fieldNumber, startOffset, length);
		Microsoft.SqlServer.Management.SqlParser.Metadata.TypeInfo returnType = propertyInfo.ReturnType;
		if (returnType.IsMetadata)
		{
			AddMetadataRefProperty(propertyInfo.Name, returnType.Type, idExpr);
			return;
		}
		if (returnType.IsCollection)
		{
			CollectionTypeInfo obj = (CollectionTypeInfo)returnType;
			bool isOrdered = obj.IsOrdered;
			Microsoft.SqlServer.Management.SqlParser.Metadata.TypeInfo elementType = obj.ElementType;
			AddMetadataCollection(propertyInfo.Name, elementType.Type, idExpr, isOrdered);
			return;
		}
		switch (propertyInfo.ReturnType.Id)
		{
		case 1:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.Bool, idExpr);
			break;
		case 9:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.DatabaseCompatibilityLevel, idExpr);
			break;
		case 12:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.ExecutionContextType, idExpr);
			break;
		case 13:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.ForeignKeyAction, idExpr);
			break;
		case 141:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.LoginType, idExpr);
			break;
		case 144:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.SortOrder, idExpr);
			break;
		case 149:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.UserType, idExpr);
			break;
		case 10:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.DatabasePermissionType, idExpr);
			break;
		case 142:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.PermissionState, idExpr);
			break;
		case 0:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.ActivationOrder, idExpr);
			break;
		case 14:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.GridDensity, idExpr);
			break;
		case 145:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.StatisticsType, idExpr);
			break;
		case 147:
			AddIndirectValueProperty(propertyInfo.Name, CodeConfiguration.TypeReferences.SynonymBaseType, idExpr);
			break;
		case 3:
			AddRefPropertyAsByteArray(propertyInfo.Name, idExpr);
			break;
		case 5:
			AddRefPropertyAsCollationInfo(propertyInfo.Name, idExpr);
			break;
		case 6:
			AddRefPropertyAsComputedColumnInfo(propertyInfo.Name, idExpr);
			break;
		case 8:
			AddRefPropertyAsDataTypeSpec(propertyInfo.Name, idExpr);
			break;
		case 137:
			AddRefPropertyAsIdentityColumnInfo(propertyInfo.Name, idExpr);
			break;
		case 139:
			AddRefPropertyAsInt(propertyInfo.Name, idExpr);
			break;
		case 89:
			AddRefPropertyAsPassword(propertyInfo.Name, idExpr);
			break;
		case 146:
			AddRefPropertyAsString(propertyInfo.Name, idExpr);
			break;
		}
	}

	private void AddIndirectValueProperty(string propertyName, CodeTypeReference propertyType, CodeExpression idExpr)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		CodeExpression valueExpr = (CodeExpression)new CodeCastExpression(propertyType, idExpr);
		AddCodeMemberProperty(propertyName, propertyType, valueExpr, (MemberAttributes)4);
	}

	private void AddIndirectMetadataReferenceProperty<T>(string propertyName, int fieldNumber, int startOffset, int length)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		CodeTypeReference propertyType = new CodeTypeReference(typeof(T).Name);
		CodeExpression idExpr = (CodeExpression)(object)CreateGetPackedValueExpr(fieldNumber, startOffset, length);
		AddMetadataRefProperty(propertyName, propertyType, idExpr);
	}

	private void AddIndirectCollectionProperty(string propertyName, Type type, CodeExpression idExpr, bool isOrdered)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		CodeTypeReference itemType = new CodeTypeReference(type);
		AddMetadataCollection(propertyName, itemType, idExpr, isOrdered);
	}

	private void AddCodeMemberProperty(string propertyName, CodeTypeReference propertyType, CodeExpression valueExpr, MemberAttributes propertyAttribs)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if (valueExpr == null)
		{
			valueExpr = CodeConfiguration.Expressions.Null;
		}
		CodeMemberProperty val = CreateMemberProperty(propertyName, propertyType, propertyAttribs);
		val.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement(valueExpr));
		typeDeclaration.Members.Add((CodeTypeMember)(object)val);
	}

	private static CodeMemberProperty CreateMemberProperty(string propertyName, CodeTypeReference propertyType, MemberAttributes propertyAttribs)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		return new CodeMemberProperty
		{
			Name = propertyName,
			Type = propertyType,
			HasGet = true,
			HasSet = false,
			Attributes = (MemberAttributes)(0x6000 | propertyAttribs)
		};
	}

	private void AddMemberField(string fieldName, CodeTypeReference fieldType)
	{
		CodeMemberField val = CreateMemberField(fieldName, fieldType);
		typeDeclaration.Members.Add((CodeTypeMember)(object)val);
	}

	private static CodeMemberField CreateMemberField(string fieldName, CodeTypeReference fieldType)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		return new CodeMemberField(fieldType, fieldName)
		{
			Attributes = (MemberAttributes)20480
		};
	}

	private static CodeMethodInvokeExpression CreateGetPackedValueExpr(int fieldNumber, int startOffset, int length)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		return new CodeMethodInvokeExpression(CodeConfiguration.MethodReferences.GetPackedValue, (CodeExpression[])(object)new CodeExpression[3]
		{
			(CodeExpression)new CodePrimitiveExpression((object)fieldNumber),
			(CodeExpression)new CodePrimitiveExpression((object)startOffset),
			(CodeExpression)new CodePrimitiveExpression((object)length)
		});
	}
}
