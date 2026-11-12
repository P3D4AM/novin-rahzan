using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2;

[DesignTimeVisible(false)]
public class StiButton : Button, IStiBlocksV2Control
{
	public StiButton()
	{
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			StiBlocksV2ThemeHelper.LoadControlTheme(this);
		}
	}

	public virtual StiBlocksV2ControlID GetV2ID()
	{
		return StiBlocksV2ControlID.StiButton;
	}
}
