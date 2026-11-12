using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlForXmlDirective : SqlCodeObject
{
	private SqlForXmlDirectivesType type;

	private string argument;

	internal override bool IsVisitable => true;

	public SqlForXmlDirectivesType Type => type;

	public virtual string Argument => argument;

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

	internal SqlForXmlDirective(SqlForXmlUnknownDirective directive, SqlForXmlDirectivesType type)
	{
		base.Position = directive.Position;
		switch (directive.Name.ToUpper())
		{
		case "BINARY BASE64":
			this.type = SqlForXmlDirectivesType.BinaryBase64 & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.Base64Error);
			}
			break;
		case "TYPE":
			this.type = SqlForXmlDirectivesType.Type & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.TypeError);
			}
			break;
		case "ROOT":
			this.type = SqlForXmlDirectivesType.Root & type;
			argument = directive.Argument;
			break;
		case "XMLDATA":
			this.type = SqlForXmlDirectivesType.XmlData & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.XmlSchemaError);
			}
			break;
		case "XMLSCHEMA":
			this.type = SqlForXmlDirectivesType.XmlSchema & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.XmlSchemaError);
			}
			else
			{
				argument = directive.Argument;
			}
			break;
		case "ELEMENTS XSINIL":
			this.type = SqlForXmlDirectivesType.ElementsXsiNil & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.ElementsError);
			}
			break;
		case "ELEMENTS ABSENT":
			this.type = SqlForXmlDirectivesType.ElementsAbsent & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.ElementsError);
			}
			break;
		case "ELEMENTS":
			this.type = SqlForXmlDirectivesType.Elements & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.ElementsError);
			}
			break;
		case "INCLUDE_NULL_VALUES":
			this.type = SqlForXmlDirectivesType.IncludeNullValues & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.IncludeNullValuesError);
			}
			break;
		case "WITHOUT_ARRAY_WRAPPER":
			this.type = SqlForXmlDirectivesType.WithoutArrayWrapper & type;
			if (this.type == SqlForXmlDirectivesType.None)
			{
				ReportParseError(SR.WithoutArrayWrapperError);
			}
			break;
		default:
			ReportParseError(SR.IncorrectSyntaxNear(directive.Name));
			break;
		}
	}

	internal SqlForXmlDirective(string argument)
	{
		this.argument = argument;
	}
}
