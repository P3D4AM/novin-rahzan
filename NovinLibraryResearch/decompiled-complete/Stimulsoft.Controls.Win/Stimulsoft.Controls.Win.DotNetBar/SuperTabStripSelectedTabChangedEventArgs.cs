namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripSelectedTabChangedEventArgs : ValueChangingSourceEventArgs<BaseItem, BaseItem, eEventSource>
{
	public SuperTabStripSelectedTabChangedEventArgs(BaseItem oldValue, BaseItem newValue, eEventSource source)
		: base(oldValue, newValue, source)
	{
	}
}
