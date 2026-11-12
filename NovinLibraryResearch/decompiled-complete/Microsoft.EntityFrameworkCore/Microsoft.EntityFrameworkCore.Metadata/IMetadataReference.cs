using System;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMetadataReference<out T> : IDisposable
{
	T Object { get; }
}
