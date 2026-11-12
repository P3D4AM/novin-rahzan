using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationsAnnotationProvider : IMigrationsAnnotationProvider
{
	public MigrationsAnnotationProvider([JetBrains.Annotations.NotNull] MigrationsAnnotationProviderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual IEnumerable<IAnnotation> For(IModel model)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(IEntityType entityType)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(IForeignKey foreignKey)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(IIndex index)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(IKey key)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(IProperty property)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(ISequence sequence)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> For(ICheckConstraint checkConstraint)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IModel model)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IEntityType entityType)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IForeignKey foreignKey)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IIndex index)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IKey key)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(IProperty property)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(ISequence sequence)
	{
		return Enumerable.Empty<IAnnotation>();
	}

	public virtual IEnumerable<IAnnotation> ForRemove(ICheckConstraint checkConstraint)
	{
		return Enumerable.Empty<IAnnotation>();
	}
}
