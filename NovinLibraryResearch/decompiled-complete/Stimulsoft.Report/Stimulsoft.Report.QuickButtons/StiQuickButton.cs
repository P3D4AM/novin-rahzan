using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.QuickButtons;

public abstract class StiQuickButton : IDisposable
{
	public abstract string Name { get; }

	public abstract Image Image { get; }

	public abstract Image SelectedImage { get; }

	public bool IsSelected { get; set; }

	public virtual StiQuickButtonAlignment Alignment => StiQuickButtonAlignment.Right;

	public void Dispose()
	{
		if (Image != null)
		{
			Image.Dispose();
		}
	}

	public abstract void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer);
}
