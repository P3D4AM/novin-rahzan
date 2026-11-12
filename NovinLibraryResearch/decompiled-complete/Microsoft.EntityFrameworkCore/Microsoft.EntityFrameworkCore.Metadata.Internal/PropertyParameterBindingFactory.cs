using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertyParameterBindingFactory : IPropertyParameterBindingFactory
{
	public virtual ParameterBinding FindParameter(IEntityType entityType, Type parameterType, string parameterName)
	{
		IList<string> candidateNames = GetCandidatePropertyNames(parameterName);
		return (from p in entityType.GetProperties()
			where p.ClrType == parameterType && candidateNames.Any((string c) => c.Equals(p.Name, StringComparison.Ordinal))
			select new PropertyParameterBinding(p)).FirstOrDefault();
	}

	private static IList<string> GetCandidatePropertyNames(string parameterName)
	{
		string text = char.ToUpperInvariant(parameterName[0]) + parameterName.Substring(1);
		return new List<string>
		{
			parameterName,
			text,
			"_" + parameterName,
			"_" + text,
			"m_" + parameterName,
			"m_" + text
		};
	}
}
