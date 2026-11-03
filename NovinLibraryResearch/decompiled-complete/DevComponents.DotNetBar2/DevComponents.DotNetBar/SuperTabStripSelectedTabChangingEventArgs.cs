namespace DevComponents.DotNetBar;

public class SuperTabStripSelectedTabChangingEventArgs : ValueChangingSourceEventArgs<BaseItem, BaseItem, eEventSource>
{
	public SuperTabStripSelectedTabChangingEventArgs(BaseItem oldValue, BaseItem newValue, eEventSource source)
		: base(oldValue, newValue, source)
	{
	}
}
