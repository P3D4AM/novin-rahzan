using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlUdtMemberExpression : SqlScalarExpression
{
	private IScalar boundMember;

	private readonly SqlIdentifier memberName;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundMember;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Udt;

	internal override bool IsConstantExpression => false;

	public SqlIdentifier MemberName => memberName;

	internal void BindUdtDataMember(IBindingContext bindingContext, IClrDataType boundUdt, bool isStatic)
	{
		IUdtDataMember udtDataMember = boundUdt.DataMembers[memberName.Value];
		if (udtDataMember != null)
		{
			if (udtDataMember.IsStatic == isStatic)
			{
				boundMember = udtDataMember;
				base.BindingState |= BindingState.BindSucceeded;
				return;
			}
			base.BindingState |= BindingState.BindFailed;
			if (udtDataMember.IsStatic)
			{
				memberName.ReportBindError(SR.UdtPropertyIsStatic(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName), ErrorFlags.UnresolvedReference);
			}
			else
			{
				memberName.ReportBindError(SR.UdtPropertyIsNotStatic(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName), ErrorFlags.UnresolvedReference);
			}
		}
		else
		{
			if (boundUdt.IsSystem)
			{
				memberName.ReportBindError(SR.CouldNotFindPropertyOrField(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName));
			}
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal void BindUdtMethod(IBindingContext bindingContext, IClrDataType boundUdt, bool isStatic, SqlScalarExpressionCollection arguments)
	{
		IUdtMethod udtMethod = boundUdt.Methods[memberName.Value];
		if (udtMethod != null)
		{
			if (udtMethod.IsStatic == isStatic)
			{
				boundMember = udtMethod;
				base.BindingState |= BindingState.BindSucceeded;
			}
			else
			{
				base.BindingState |= BindingState.BindFailed;
				if (udtMethod.IsStatic)
				{
					memberName.ReportBindError(SR.UdtMemberIsStatic(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName), ErrorFlags.UnresolvedReference);
				}
				else
				{
					memberName.ReportBindError(SR.UdtMemberIsNotStatic(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName), ErrorFlags.UnresolvedReference);
				}
			}
		}
		else
		{
			if (boundUdt.IsSystem)
			{
				memberName.ReportBindError(SR.CouldNotFindMethod(memberName.Value, boundUdt.ClassName, boundUdt.AssemblyName), ErrorFlags.UnresolvedReference);
			}
			base.BindingState |= BindingState.BindFailed;
		}
		if (arguments == null)
		{
			return;
		}
		if (udtMethod != null)
		{
			base.BindingState |= BindUtils.Function.BindArguments(this, arguments, bindingContext, udtMethod);
			return;
		}
		foreach (SqlScalarExpression argument in arguments)
		{
			argument.Bind(bindingContext);
			base.BindingState |= argument.BindingState;
		}
	}

	internal void BindXmlMethod(IBindingContext bindingContext, IXmlDataType dataType, SqlScalarExpressionCollection arguments)
	{
		IScalarFunction scalarFunction = dataType.Methods[memberName.Value];
		if (scalarFunction != null)
		{
			boundMember = scalarFunction;
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			memberName.ReportBindError(SR.NotValidFunctionOrProperty(memberName.ToString()), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		if (arguments == null)
		{
			return;
		}
		if (scalarFunction != null)
		{
			base.BindingState |= BindUtils.Function.BindArguments(this, arguments, bindingContext, scalarFunction);
			return;
		}
		foreach (SqlScalarExpression argument in arguments)
		{
			argument.Bind(bindingContext);
			base.BindingState |= argument.BindingState;
		}
	}

	internal override void Unbind()
	{
		boundMember = null;
		base.Unbind();
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject == memberName)
		{
			result = ((boundMember != null) ? displayInfoProvider.GetDescription(boundMember) : null);
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider);
		}
		return result;
	}

	internal SqlUdtMemberExpression(SqlIdentifier memberName)
	{
		this.memberName = memberName;
	}
}
