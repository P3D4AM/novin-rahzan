namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IAnnotation
{
	string Name { get; }

	object Value { get; }
}
