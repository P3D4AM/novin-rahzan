using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

public class AnnotationCodeGenerator : IAnnotationCodeGenerator
{
	protected virtual AnnotationCodeGeneratorDependencies Dependencies { get; }

	public AnnotationCodeGenerator([JetBrains.Annotations.NotNull] AnnotationCodeGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual bool IsHandledByConvention(IModel model, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual bool IsHandledByConvention(IEntityType entityType, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual bool IsHandledByConvention(IKey key, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(key, "key");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual bool IsHandledByConvention(IProperty property, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual bool IsHandledByConvention(IForeignKey foreignKey, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual bool IsHandledByConvention(IIndex index, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(index, "index");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return false;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IModel model, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IEntityType entityType, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IKey key, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(key, "key");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IProperty property, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IForeignKey foreignKey, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}

	public virtual MethodCallCodeFragment GenerateFluentApi(IIndex index, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(index, "index");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		return null;
	}
}
