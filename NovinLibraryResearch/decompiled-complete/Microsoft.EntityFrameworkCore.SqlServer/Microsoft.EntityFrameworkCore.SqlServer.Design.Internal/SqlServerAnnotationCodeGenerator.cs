using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;

public class SqlServerAnnotationCodeGenerator : AnnotationCodeGenerator
{
	public SqlServerAnnotationCodeGenerator([JetBrains.Annotations.NotNull] AnnotationCodeGeneratorDependencies dependencies)
		: base(dependencies)
	{
	}

	public override bool IsHandledByConvention(IModel model, IAnnotation annotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
		if (annotation.Name == "Relational:DefaultSchema")
		{
			return string.Equals("dbo", (string)annotation.Value);
		}
		if (annotation.Name == "SqlServer:ValueGenerationStrategy")
		{
			return (SqlServerValueGenerationStrategy)annotation.Value == SqlServerValueGenerationStrategy.IdentityColumn;
		}
		return false;
	}

	public override MethodCallCodeFragment GenerateFluentApi(IKey key, IAnnotation annotation)
	{
		if (!(annotation.Name == "SqlServer:Clustered"))
		{
			return null;
		}
		if ((bool)annotation.Value)
		{
			return new MethodCallCodeFragment("IsClustered");
		}
		return new MethodCallCodeFragment("IsClustered", false);
	}

	public override MethodCallCodeFragment GenerateFluentApi(IIndex index, IAnnotation annotation)
	{
		if (annotation.Name == "SqlServer:Clustered")
		{
			if ((bool)annotation.Value)
			{
				return new MethodCallCodeFragment("IsClustered");
			}
			return new MethodCallCodeFragment("IsClustered", false);
		}
		if (annotation.Name == "SqlServer:Include")
		{
			return new MethodCallCodeFragment("IncludeProperties", annotation.Value);
		}
		return null;
	}
}
