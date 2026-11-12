using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public readonly struct MaterializationContext
{
	public static readonly MethodInfo GetValueBufferMethod = typeof(MaterializationContext).GetProperty("ValueBuffer").GetMethod;

	internal static readonly PropertyInfo ContextProperty = typeof(MaterializationContext).GetProperty("Context");

	public ValueBuffer ValueBuffer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
	}

	public DbContext Context
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
	}

	public MaterializationContext(in ValueBuffer valueBuffer, [JetBrains.Annotations.NotNull] DbContext context)
	{
		ValueBuffer = valueBuffer;
		Context = context;
	}
}
