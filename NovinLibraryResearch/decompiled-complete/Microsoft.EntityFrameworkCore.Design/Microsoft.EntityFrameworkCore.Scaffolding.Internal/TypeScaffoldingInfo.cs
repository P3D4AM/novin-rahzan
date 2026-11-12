using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class TypeScaffoldingInfo
{
	public virtual Type ClrType { get; }

	public virtual bool IsInferred { get; }

	public virtual bool? ScaffoldUnicode { get; }

	public virtual bool? ScaffoldFixedLength { get; }

	public virtual int? ScaffoldMaxLength { get; }

	public TypeScaffoldingInfo([JetBrains.Annotations.NotNull] Type clrType, bool inferred, bool? scaffoldUnicode, int? scaffoldMaxLength, bool? scaffoldFixedLength)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		IsInferred = inferred;
		ScaffoldUnicode = scaffoldUnicode;
		ScaffoldMaxLength = scaffoldMaxLength;
		ScaffoldFixedLength = scaffoldFixedLength;
		ClrType = clrType;
	}
}
