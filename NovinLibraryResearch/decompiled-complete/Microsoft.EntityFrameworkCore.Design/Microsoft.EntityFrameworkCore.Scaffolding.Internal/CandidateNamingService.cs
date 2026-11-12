using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CandidateNamingService : ICandidateNamingService
{
	public virtual string GenerateCandidateIdentifier(DatabaseTable originalTable)
	{
		return GenerateCandidateIdentifier(originalTable.Name);
	}

	public virtual string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
	{
		return GenerateCandidateIdentifier(originalColumn.Name);
	}

	public virtual string GetDependentEndCandidateNavigationPropertyName(IForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		string text = FindCandidateNavigationName(foreignKey.Properties);
		if (string.IsNullOrEmpty(text))
		{
			return foreignKey.PrincipalEntityType.ShortName();
		}
		return text;
	}

	public virtual string GetPrincipalEndCandidateNavigationPropertyName(IForeignKey foreignKey, string dependentEndNavigationPropertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(dependentEndNavigationPropertyName, "dependentEndNavigationPropertyName");
		IEnumerable<IForeignKey> obj = (from fk in foreignKey.PrincipalEntityType?.GetReferencingForeignKeys()
			where foreignKey.DeclaringEntityType == fk.DeclaringEntityType
			select fk);
		if (obj == null || obj.Count() <= 1)
		{
			return foreignKey.DeclaringEntityType.ShortName();
		}
		return foreignKey.DeclaringEntityType.ShortName() + dependentEndNavigationPropertyName;
	}

	private static string GenerateCandidateIdentifier(string originalIdentifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(originalIdentifier, "originalIdentifier");
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		bool flag2 = true;
		foreach (char c in originalIdentifier)
		{
			bool flag3 = !char.IsLetterOrDigit(c);
			if (flag3 || (flag && char.IsUpper(c)))
			{
				flag2 = true;
				flag = false;
				if (flag3)
				{
					continue;
				}
			}
			stringBuilder.Append(flag2 ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
			flag2 = false;
			if (char.IsLower(c))
			{
				flag = true;
			}
		}
		return stringBuilder.ToString();
	}

	private static string FindCandidateNavigationName(IEnumerable<IProperty> properties)
	{
		if (!properties.Any())
		{
			return string.Empty;
		}
		string empty = string.Empty;
		IProperty property = properties.First();
		empty = ((properties.Count() != 1) ? FindCommonPrefix(property.Name, properties.Select((IProperty p) => p.Name)) : property.Name);
		return StripId(empty);
	}

	private static string FindCommonPrefix(string firstName, IEnumerable<string> propertyNames)
	{
		int num = 0;
		foreach (char c in firstName)
		{
			foreach (string propertyName in propertyNames)
			{
				if (propertyName.Length <= num || propertyName[num] != c)
				{
					return firstName.Substring(0, num);
				}
			}
			num++;
		}
		return firstName.Substring(0, num);
	}

	private static string StripId(string commonPrefix)
	{
		if (commonPrefix.Length <= 2 || !commonPrefix.EndsWith("id", StringComparison.OrdinalIgnoreCase))
		{
			return commonPrefix;
		}
		return commonPrefix.Substring(0, commonPrefix.Length - 2);
	}
}
