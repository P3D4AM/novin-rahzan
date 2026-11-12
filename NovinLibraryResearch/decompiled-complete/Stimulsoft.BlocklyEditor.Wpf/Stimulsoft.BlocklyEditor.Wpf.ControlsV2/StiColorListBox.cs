using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2;

public class StiColorListBox : ListBox, IStiBlocksV2Control
{
	public StiColorListBox()
	{
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			StiBlocksV2ThemeHelper.LoadControlTheme(this);
		}
	}

	public virtual StiBlocksV2ControlID GetV2ID()
	{
		return StiBlocksV2ControlID.StiColorListBox;
	}

	protected override DependencyObject GetContainerForItemOverride()
	{
		return (DependencyObject)(object)new StiColorListBoxItem();
	}
}
