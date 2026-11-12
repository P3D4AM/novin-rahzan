using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IDbFunction
{
	string Name { get; }

	string Schema { get; }

	IModel Model { get; }

	MethodInfo MethodInfo { get; }

	string StoreType { get; }

	RelationalTypeMapping TypeMapping { get; }

	IReadOnlyList<IDbFunctionParameter> Parameters { get; }

	Func<IReadOnlyCollection<SqlExpression>, SqlExpression> Translation { get; }
}
