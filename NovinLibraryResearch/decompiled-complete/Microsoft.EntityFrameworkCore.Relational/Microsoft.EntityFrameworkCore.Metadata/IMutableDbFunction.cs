using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableDbFunction : IDbFunction
{
	new string Name
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	new string Schema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

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

	new IMutableModel Model { get; }

	new Func<IReadOnlyCollection<SqlExpression>, SqlExpression> Translation
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
