using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2;

[DesignTimeVisible(false)]
public class StiChildFooter : ContentControl, IStiBlocksV2Control
{
	public StiChildFooter()
	{
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			StiBlocksV2ThemeHelper.LoadControlTheme(this);
		}
	}

	public StiBlocksV2ControlID GetV2ID()
	{
		return StiBlocksV2ControlID.StiChildFooter;
	}
}
