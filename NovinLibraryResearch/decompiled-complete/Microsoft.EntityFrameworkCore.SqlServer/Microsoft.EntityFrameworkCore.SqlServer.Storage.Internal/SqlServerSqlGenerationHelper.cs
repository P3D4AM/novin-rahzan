using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerSqlGenerationHelper : RelationalSqlGenerationHelper
{
	public override string BatchTerminator => "GO" + Environment.NewLine + Environment.NewLine;

	public SqlServerSqlGenerationHelper([JetBrains.Annotations.NotNull] RelationalSqlGenerationHelperDependencies dependencies)
		: base(dependencies)
	{
	}

	public override string EscapeIdentifier(string identifier)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier").Replace("]", "]]");
	}

	public override void EscapeIdentifier(StringBuilder builder, string identifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier");
		int length = builder.Length;
		builder.Append(identifier);
		builder.Replace("]", "]]", length, identifier.Length);
	}

	public override string DelimitIdentifier(string identifier)
	{
		return "[" + EscapeIdentifier(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier")) + "]";
	}

	public override void DelimitIdentifier(StringBuilder builder, string identifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier");
		builder.Append('[');
		EscapeIdentifier(builder, identifier);
		builder.Append(']');
	}
}
