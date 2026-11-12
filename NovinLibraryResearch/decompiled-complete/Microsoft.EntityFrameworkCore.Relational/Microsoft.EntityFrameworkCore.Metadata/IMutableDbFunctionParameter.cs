using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableDbFunctionParameter : IDbFunctionParameter
{
	new IMutableDbFunction Function { get; }

	new string StoreType
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	new RelationalTypeMapping TypeMapping
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
