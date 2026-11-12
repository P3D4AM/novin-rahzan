using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class DebugView<TMetadata>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly TMetadata _metadata;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly Func<TMetadata, string> _toDebugString;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string _view;

	public virtual string View => _view ?? (_view = _toDebugString(_metadata));

	public DebugView([JetBrains.Annotations.NotNull] TMetadata metadata, [JetBrains.Annotations.NotNull] Func<TMetadata, string> toDebugString)
	{
		_metadata = metadata;
		_toDebugString = toDebugString;
	}
}
