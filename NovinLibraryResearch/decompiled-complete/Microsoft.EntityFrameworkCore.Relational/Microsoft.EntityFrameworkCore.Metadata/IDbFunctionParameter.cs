using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IDbFunctionParameter
{
	IDbFunction Function { get; }

	string Name { get; }

	Type ClrType { get; }

	string StoreType { get; }

	RelationalTypeMapping TypeMapping { get; }
}
