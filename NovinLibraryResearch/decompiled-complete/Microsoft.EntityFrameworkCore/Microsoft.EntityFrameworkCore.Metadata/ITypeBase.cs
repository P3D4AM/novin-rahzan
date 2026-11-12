using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface ITypeBase : IAnnotatable
{
	IModel Model { get; }

	string Name { get; }

	Type ClrType { get; }
}
