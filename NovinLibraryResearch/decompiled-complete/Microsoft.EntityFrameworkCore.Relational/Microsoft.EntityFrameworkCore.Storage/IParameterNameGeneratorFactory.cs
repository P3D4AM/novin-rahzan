namespace Microsoft.EntityFrameworkCore.Storage;

public interface IParameterNameGeneratorFactory
{
	ParameterNameGenerator Create();
}
