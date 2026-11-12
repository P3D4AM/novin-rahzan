using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Reference<T> : IMetadataReference<T>, IDisposable
{
	private readonly IReferenceRoot<T> _root;

	private int _referenceCount = 1;

	public virtual T Object
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public Reference([JetBrains.Annotations.CanBeNull] T @object)
		: this(@object, (IReferenceRoot<T>)null)
	{
	}

	public Reference([JetBrains.Annotations.CanBeNull] T @object, [JetBrains.Annotations.CanBeNull] IReferenceRoot<T> root)
	{
		Object = @object;
		_root = root;
	}

	public virtual void Dispose()
	{
		if (_referenceCount-- == 1)
		{
			_root?.Release(this);
			Object = default(T);
		}
	}

	public virtual void IncreaseReferenceCount()
	{
		_referenceCount++;
	}
}
