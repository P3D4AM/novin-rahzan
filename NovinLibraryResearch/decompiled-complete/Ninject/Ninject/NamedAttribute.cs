using Ninject.Infrastructure;
using Ninject.Planning.Bindings;

namespace Ninject;

public class NamedAttribute : ConstraintAttribute
{
	public string Name { get; private set; }

	public NamedAttribute(string name)
	{
		Ensure.ArgumentNotNullOrEmpty(name, "name");
		Name = name;
	}

	public override bool Matches(IBindingMetadata metadata)
	{
		Ensure.ArgumentNotNull(metadata, "metadata");
		return metadata.Name == Name;
	}
}
