using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class Annotatable : IMutableAnnotatable, IAnnotatable
{
	private SortedDictionary<string, Annotation> _annotations;

	public virtual object this[string name]
	{
		get
		{
			return FindAnnotation(name)?.Value;
		}
		set
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
			if (value == null)
			{
				RemoveAnnotation(name);
			}
			else
			{
				SetAnnotation(name, value);
			}
		}
	}

	public virtual IEnumerable<Annotation> GetAnnotations()
	{
		IEnumerable<Annotation> enumerable = _annotations?.Values;
		return enumerable ?? Enumerable.Empty<Annotation>();
	}

	public virtual Annotation AddAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Annotation annotation = CreateAnnotation(name, value);
		return AddAnnotation(name, annotation);
	}

	protected virtual Annotation AddAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Annotation annotation)
	{
		if (FindAnnotation(name) != null)
		{
			throw new InvalidOperationException(CoreStrings.DuplicateAnnotation(name));
		}
		SetAnnotation(name, annotation, null);
		return annotation;
	}

	public virtual void SetAnnotation(string name, object value)
	{
		Annotation annotation = FindAnnotation(name);
		if (annotation == null || !object.Equals(annotation.Value, value))
		{
			SetAnnotation(name, CreateAnnotation(name, value), annotation);
		}
	}

	protected virtual Annotation SetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Annotation annotation, [JetBrains.Annotations.CanBeNull] Annotation oldAnnotation)
	{
		if (_annotations == null)
		{
			_annotations = new SortedDictionary<string, Annotation>();
		}
		_annotations[name] = annotation;
		return OnAnnotationSet(name, annotation, oldAnnotation);
	}

	protected virtual Annotation OnAnnotationSet([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] Annotation annotation, [JetBrains.Annotations.CanBeNull] Annotation oldAnnotation)
	{
		return annotation;
	}

	public virtual Annotation FindAnnotation([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		if (_annotations != null)
		{
			if (!_annotations.TryGetValue(name, out var value))
			{
				return null;
			}
			return value;
		}
		return null;
	}

	public virtual Annotation RemoveAnnotation([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		Annotation annotation = FindAnnotation(name);
		if (annotation == null)
		{
			return null;
		}
		_annotations.Remove(name);
		if (_annotations.Count == 0)
		{
			_annotations = null;
		}
		OnAnnotationSet(name, null, annotation);
		return annotation;
	}

	protected virtual Annotation CreateAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value)
	{
		return new Annotation(name, value);
	}

	IEnumerable<IAnnotation> IAnnotatable.GetAnnotations()
	{
		return GetAnnotations();
	}

	IAnnotation IAnnotatable.FindAnnotation(string name)
	{
		return FindAnnotation(name);
	}

	IAnnotation IMutableAnnotatable.AddAnnotation(string name, object value)
	{
		return AddAnnotation(name, value);
	}

	IAnnotation IMutableAnnotatable.RemoveAnnotation(string name)
	{
		return RemoveAnnotation(name);
	}
}
