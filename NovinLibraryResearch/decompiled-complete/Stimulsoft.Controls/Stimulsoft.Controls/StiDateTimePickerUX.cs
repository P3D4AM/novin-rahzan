using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiDateTimePickerUX : DateTimePicker
{
	private const int WM_ERASEBKGND = 20;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((DateTimePicker)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((DateTimePicker)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((DateTimePicker)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((DateTimePicker)this).BackColor = value;
			}
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (((Message)(ref m)).Msg == 20)
		{
			Graphics val = Graphics.FromHdc(((Message)(ref m)).WParam);
			val.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), ((Control)this).ClientRectangle);
			val.Dispose();
		}
		else
		{
			((DateTimePicker)this).WndProc(ref m);
		}
	}
}
