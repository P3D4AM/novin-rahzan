namespace Ninject.Activation.Strategies;

public class InitializableStrategy : ActivationStrategy
{
	public override void Activate(IContext context, InstanceReference reference)
	{
		reference.IfInstanceIs(delegate(IInitializable x)
		{
			x.Initialize();
		});
	}
}
