using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalSqlGenerationHelper : ISqlGenerationHelper
{
	public virtual string StatementTerminator => ";";

	public virtual string BatchTerminator => string.Empty;

	public virtual string SingleLineCommentToken => "--";

	public RelationalSqlGenerationHelper([JetBrains.Annotations.NotNull] RelationalSqlGenerationHelperDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual string GenerateParameterName(string name)
	{
		if (!name.StartsWith("@", StringComparison.Ordinal))
		{
			return "@" + name;
		}
		return name;
	}

	public virtual void GenerateParameterName(StringBuilder builder, string name)
	{
		builder.Append("@").Append(name);
	}

	public virtual string GenerateParameterNamePlaceholder(string name)
	{
		return GenerateParameterName(name);
	}

	public virtual void GenerateParameterNamePlaceholder(StringBuilder builder, string name)
	{
		GenerateParameterName(builder, name);
	}

	public virtual string EscapeIdentifier([JetBrains.Annotations.NotNull] string identifier)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier").Replace("\"", "\"\"");
	}

	public virtual void EscapeIdentifier([JetBrains.Annotations.NotNull] StringBuilder builder, [JetBrains.Annotations.NotNull] string identifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier");
		int length = builder.Length;
		builder.Append(identifier);
		builder.Replace("\"", "\"\"", length, identifier.Length);
	}

	public virtual string DelimitIdentifier(string identifier)
	{
		return "\"" + EscapeIdentifier(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier")) + "\"";
	}

	public virtual void DelimitIdentifier(StringBuilder builder, string identifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(identifier, "identifier");
		builder.Append('"');
		EscapeIdentifier(builder, identifier);
		builder.Append('"');
	}

	public virtual string DelimitIdentifier(string name, string schema)
	{
		return ((!string.IsNullOrEmpty(schema)) ? (DelimitIdentifier(schema) + ".") : string.Empty) + DelimitIdentifier(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"));
	}

	public virtual void DelimitIdentifier(StringBuilder builder, string name, string schema)
	{
		if (!string.IsNullOrEmpty(schema))
		{
			DelimitIdentifier(builder, schema);
			builder.Append(".");
		}
		DelimitIdentifier(builder, name);
	}

	public virtual string GenerateComment(string text)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(text, "text");
		StringBuilder stringBuilder = new StringBuilder();
		using (StringReader stringReader = new StringReader(text))
		{
			string value;
			while ((value = stringReader.ReadLine()) != null)
			{
				stringBuilder.Append(SingleLineCommentToken).Append(" ").AppendLine(value);
			}
		}
		return stringBuilder.ToString();
	}
}
