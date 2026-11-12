using System;
using System.Collections.Generic;
using System.Globalization;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDataTypeSpecification : SqlCodeObject
{
	private IDataType boundType;

	private ErrorFlags errorFlags;

	private const int MaxLength = 8000;

	private const int MaxPrecision = 38;

	private const int MaxTimeScale = 7;

	private readonly bool isValid;

	private readonly bool isError;

	private readonly bool isCursor;

	private readonly SqlDataType dataType;

	private readonly int? arg1;

	private readonly int? arg2;

	private readonly bool isMaximum;

	private readonly SqlObjectIdentifier xmlSchemaCollection;

	private readonly SqlXmlDocumentConstraint xmlDocumentConstraint;

	internal override bool IsVisitable => true;

	public IDataType BoundType => boundType;

	public override IMetadataObject BoundObject => boundType;

	public SqlDataType DataType => dataType;

	public int? Argument1 => arg1;

	public int? Argument2 => arg2;

	public bool IsMaximum => isMaximum;

	public bool IsCursor => isCursor;

	public SqlXmlDocumentConstraint XmlDocumentConstraint => xmlDocumentConstraint;

	public SqlObjectIdentifier XmlSchemaCollection => xmlSchemaCollection;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (dataType != null)
			{
				yield return dataType;
			}
			if (xmlSchemaCollection != null)
			{
				yield return xmlSchemaCollection;
			}
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		boundType = null;
		if (isCursor)
		{
			boundType = bindingContext.MetadataFactory.DataType.Cursor;
		}
		else if (isError)
		{
			boundType = bindingContext.MetadataFactory.DataType.UnknownScalar;
		}
		else if (isValid)
		{
			SqlObjectIdentifier objectIdentifier = dataType.ObjectIdentifier;
			BatchBindingContext batch = bindingContext.Batch;
			DataTypeSpec typeSpec = dataType.TypeSpec;
			string value = objectIdentifier.ObjectName.Value;
			string value2 = objectIdentifier.SchemaName.Value;
			if (typeSpec != null)
			{
				if (!arg1.HasValue)
				{
					boundType = batch.FindSystemDataType(value2, typeSpec, IsMaximum);
				}
				else if (!arg2.HasValue)
				{
					boundType = batch.FindSystemDataType(value2, typeSpec, arg1.Value);
				}
				else
				{
					boundType = batch.FindSystemDataType(value2, typeSpec, arg1.Value, arg2.Value);
				}
			}
			if (boundType == null && !arg1.HasValue)
			{
				LexicalScope lexicalScope = GetLexicalScope();
				boundType = batch.FindUserDefinedType(value2, value, lexicalScope);
				if (boundType == null)
				{
					errorFlags = ErrorFlags.UnresolvedDatabaseObjectReference;
				}
			}
		}
		base.BindingState = ((boundType != null) ? BindingState.BindSucceeded : BindingState.BindFailed);
	}

	internal override void Unbind()
	{
		errorFlags = ErrorFlags.None;
		boundType = null;
		base.Unbind();
	}

	internal void ReportInvalidTypeBindError(string name, bool isColumn)
	{
		if (isColumn)
		{
			ReportBindError(SR.ColumnHasInvalidDataType(name), errorFlags);
		}
		else
		{
			ReportBindError(SR.ParamVarHasInvalidDataType(name), errorFlags);
		}
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		if (boundType != null && FindChildContainingLocation(ref cursorLocation) == dataType)
		{
			result = dataType.GetQuickInfoText(cursorLocation, displayInfoProvider, boundType);
		}
		return result;
	}

	internal override MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		MethodHelpText methodHelpText = null;
		if (IsCandidateForParamHelp())
		{
			string description = "";
			string type = "";
			bool shouldShowParentheses = true;
			bool isVarArg = false;
			methodHelpText = new MethodHelpText(BoundObject.Name, description, type, shouldShowParentheses, isVarArg);
			DataTypeSpec typeSpec = DataType.TypeSpec;
			if (typeSpec.ArgSpec1 != null)
			{
				methodHelpText.Parameters.Add(new Parameter(typeSpec.ArgSpec1.Name, "", typeSpec.ArgSpec1.ToString()));
				if (typeSpec.ArgSpec2 != null)
				{
					methodHelpText.Parameters.Add(new Parameter(typeSpec.ArgSpec2.Name, "", typeSpec.ArgSpec2.ToString()));
				}
			}
		}
		return methodHelpText;
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		MethodNameAndParamLocations result = null;
		if (IsCandidateForParamHelp())
		{
			TokenManager tokenManager = TokenManager;
			MethodNameAndParamLocations methodNameAndParamLocations = new MethodNameAndParamLocations(BoundObject.Name);
			methodNameAndParamLocations.NameStartLocation = base.StartLocation;
			methodNameAndParamLocations.NameEndLocation = base.EndLocation;
			int num = base.Position.startTokenNumber + 1;
			methodNameAndParamLocations.ParamSeperatorLocations = new List<Location>();
			BuildMethodNameAndParams(methodNameAndParamLocations, num, null);
			int num2 = tokenManager.FindTokenInRange(44, num, base.Position.endTokenNumber);
			if (num2 != -1)
			{
				methodNameAndParamLocations.ParamSeperatorLocations.Add(tokenManager.GetLocation(num2));
			}
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	private bool IsCandidateForParamHelp()
	{
		if (BoundObject != null && DataType != null && DataType.TypeSpec != null && DataType.TypeSpec.ArgSpec1 != null)
		{
			return arg1.HasValue;
		}
		return false;
	}

	internal static SqlDataTypeSpecification Cursor()
	{
		return new SqlDataTypeSpecification(isCursor: true);
	}

	internal static SqlDataTypeSpecification Error()
	{
		return new SqlDataTypeSpecification(isCursor: false);
	}

	internal SqlDataTypeSpecification(SqlDataType dataType)
	{
		this.dataType = dataType;
		isValid = CheckTypeName(dataType.ObjectIdentifier);
	}

	internal SqlDataTypeSpecification(SqlDataType dataType, SqlLiteralExpression arg1)
	{
		this.dataType = dataType;
		this.arg1 = arg1.AsInt32();
		isValid = CheckTypeName(dataType.ObjectIdentifier);
		if (!isValid)
		{
			return;
		}
		DataTypeSpec typeSpec = this.dataType.TypeSpec;
		bool flag = false;
		if (typeSpec != null)
		{
			_ = typeSpec.RequireLength;
			flag = typeSpec.ArgIsScale;
		}
		int num = 0;
		if (flag)
		{
			if (this.arg1.Value < 0 || this.arg1.Value > 7)
			{
				arg1.ReportParseError(SR.InvalidScale(this.arg1.Value));
				num++;
			}
		}
		else if (this.arg1.Value <= 0)
		{
			arg1.ReportParseError(SR.InvalidLengthOrPrecision(this.arg1.Value));
			num++;
		}
		else if (this.arg1.Value > 8000)
		{
			arg1.ReportParseError(SR.MaximumSizeErrorForAnyType(this.arg1.Value, this.dataType.ObjectIdentifier.ToString(), 8000));
			num++;
		}
		isValid = num == 0;
	}

	internal SqlDataTypeSpecification(SqlDataType dataType, SqlLiteralExpression arg1, SqlLiteralExpression arg2)
	{
		this.dataType = dataType;
		isValid = CheckTypeName(dataType.ObjectIdentifier);
		if (arg1 == null || arg2 == null)
		{
			isValid = false;
		}
		else
		{
			this.arg1 = arg1.AsInt32();
			this.arg2 = arg2.AsInt32();
		}
		if (isValid)
		{
			int num = 0;
			if (this.arg1.Value <= 0)
			{
				arg1.ReportParseError(SR.InvalidLengthOrPrecision(this.arg1.Value));
				num++;
			}
			if (this.arg2.Value < 0)
			{
				arg2.ReportParseError(SR.InvalidScale(this.arg2.Value));
				num++;
			}
			if (this.arg2.Value > this.arg1.Value)
			{
				ReportParseError(SR.ScalePrecisionMismatch);
				num++;
			}
			_ = this.arg1.Value;
			_ = 38;
			isValid = num == 0;
		}
	}

	internal SqlDataTypeSpecification(SqlDataType dataType, SqlObjectIdentifier objectIdentifier1, SqlObjectIdentifier objectIdentifier2)
	{
		this.dataType = dataType;
		isValid = CheckTypeName(dataType.ObjectIdentifier) && CheckTypeName(objectIdentifier1, isXmlSchemaCollection: true) && (objectIdentifier2 == null || CheckTypeName(objectIdentifier2, isXmlSchemaCollection: true));
		if (!isValid)
		{
			return;
		}
		if (this.dataType.IsXml)
		{
			if (objectIdentifier1.SchemaName.Value == null)
			{
				if (string.Compare(objectIdentifier1.ObjectName.Value, "CONTENT", StringComparison.OrdinalIgnoreCase) == 0)
				{
					xmlDocumentConstraint = SqlXmlDocumentConstraint.Content;
				}
				else if (string.Compare(objectIdentifier1.ObjectName.Value, "DOCUMENT", StringComparison.OrdinalIgnoreCase) == 0)
				{
					xmlDocumentConstraint = SqlXmlDocumentConstraint.Document;
				}
				else
				{
					xmlDocumentConstraint = SqlXmlDocumentConstraint.None;
				}
			}
			if (objectIdentifier2 != null)
			{
				if (xmlDocumentConstraint == SqlXmlDocumentConstraint.None)
				{
					objectIdentifier1.ReportSyntaxError();
					isValid = false;
				}
				else
				{
					xmlSchemaCollection = objectIdentifier2;
				}
			}
			else if (xmlDocumentConstraint != 0)
			{
				objectIdentifier1.ReportSyntaxError();
				isValid = false;
			}
			else
			{
				xmlSchemaCollection = objectIdentifier1;
			}
			return;
		}
		if (objectIdentifier1.SchemaName.Value != null || string.Compare(objectIdentifier1.ObjectName.Value, "max", StringComparison.OrdinalIgnoreCase) != 0)
		{
			objectIdentifier1.ReportSyntaxError();
			isValid = false;
		}
		if (objectIdentifier2 != null)
		{
			objectIdentifier2.ReportSyntaxError();
			isValid = false;
		}
		if (isValid)
		{
			if (this.dataType.IsBinary || this.dataType.IsChar || this.dataType.IsNChar)
			{
				string keyword = CultureInfo.CurrentCulture.TextInfo.ToLower(this.dataType.ObjectIdentifier.ObjectName.Value);
				this.dataType.ReportSyntaxErrorNearKeyword(keyword);
				isValid = false;
			}
			else
			{
				isMaximum = true;
			}
		}
	}

	private SqlDataTypeSpecification(bool isCursor)
	{
		this.isCursor = isCursor;
		isError = !isCursor;
		isValid = true;
	}

	internal override void RunConsistencyChecks(string path, int depth)
	{
		_ = isValid;
	}

	private static bool CheckTypeName(SqlObjectIdentifier objectIdentifier)
	{
		return CheckTypeName(objectIdentifier, isXmlSchemaCollection: false);
	}

	private static bool CheckTypeName(SqlObjectIdentifier objectIdentifier, bool isXmlSchemaCollection)
	{
		_ = objectIdentifier.SchemaName.Value;
		string value = objectIdentifier.SchemaName.Value;
		string value2 = objectIdentifier.ObjectName.Value;
		if (objectIdentifier.DatabaseName.Value != null)
		{
			if (isXmlSchemaCollection)
			{
				objectIdentifier.ReportParseError(SR.XmlSchemaCollectionMaxPrefixError(objectIdentifier.ToString()));
			}
			else
			{
				objectIdentifier.ReportParseError(SR.TypeNameMaxPrefixError(objectIdentifier.ToString(), 1));
			}
			return false;
		}
		return (value == null || value.Length > 0) && value2.Length > 0;
	}
}
