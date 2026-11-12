using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiStyleBox : StiToolButton, IStiPopupParentControl
{
	private IStiDesignerBase designer;

	private bool isCrossTabStyle;

	private int selectedIndex;

	private StiBaseStyle selectedStyle;

	private bool showStyle = true;

	private bool showEditStyles;

	private bool showReportStyles = true;

	private bool showCrossTabStyles = true;

	private bool showChartStyles = true;

	private bool showPredefinedStyles;

	private bool showCustomStyles;

	private List<Color> customColors;

	private bool showNone = true;

	internal static int OneStyleHeight => StiScale.YYI(25.0);

	internal StiStyleBoxForm StyleBoxForm { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	protected override bool AllowDrawText => false;

	public IStiDesignerBase Designer
	{
		get
		{
			return designer;
		}
		set
		{
			designer = value;
		}
	}

	[Browsable(false)]
	public bool IsCrossTabStyle
	{
		get
		{
			return isCrossTabStyle;
		}
		set
		{
			isCrossTabStyle = value;
		}
	}

	[Browsable(false)]
	public int SelectedIndex
	{
		get
		{
			return selectedIndex;
		}
		set
		{
			selectedIndex = value;
		}
	}

	[Browsable(false)]
	public StiBaseStyle SelectedStyle
	{
		get
		{
			return selectedStyle;
		}
		set
		{
			selectedStyle = value;
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, EventArgs.Empty);
			}
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public bool ShowStyle
	{
		get
		{
			return showStyle;
		}
		set
		{
			if (showStyle != value)
			{
				showStyle = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool ShowEditStyles
	{
		get
		{
			return showEditStyles;
		}
		set
		{
			if (showEditStyles != value)
			{
				showEditStyles = value;
			}
		}
	}

	[Browsable(false)]
	public bool ShowReportStyles
	{
		get
		{
			return showReportStyles;
		}
		set
		{
			if (showReportStyles != value)
			{
				showReportStyles = value;
			}
		}
	}

	[Browsable(false)]
	public bool ShowCrossTabStyles
	{
		get
		{
			return showCrossTabStyles;
		}
		set
		{
			if (showCrossTabStyles != value)
			{
				showCrossTabStyles = value;
			}
		}
	}

	[Browsable(false)]
	public bool ShowChartStyles
	{
		get
		{
			return showChartStyles;
		}
		set
		{
			if (showChartStyles != value)
			{
				showChartStyles = value;
			}
		}
	}

	[Browsable(false)]
	public bool ShowPredefinedStyles
	{
		get
		{
			return showPredefinedStyles;
		}
		set
		{
			if (showPredefinedStyles != value)
			{
				showPredefinedStyles = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool ShowCustomStyles
	{
		get
		{
			return showCustomStyles;
		}
		set
		{
			if (showCustomStyles != value)
			{
				showCustomStyles = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public List<Color> CustomColors
	{
		get
		{
			return customColors;
		}
		set
		{
			if (customColors != value)
			{
				customColors = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool ShowNone
	{
		get
		{
			return showNone;
		}
		set
		{
			if (showNone != value)
			{
				showNone = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public new StiToolBarButtonStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Browsable(false)]
	public new bool Pushed
	{
		get
		{
			return base.Pushed;
		}
		set
		{
			base.Pushed = value;
		}
	}

	public event EventHandler DropDown;

	public event EventHandler DropUp;

	public event EventHandler SelectedIndexChanged;

	public event EventHandler EditStylesInvoked;

	protected override void OnPaint(PaintEventArgs p)
	{
		base.OnPaint(p);
		if (!ShowStyle)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		clientRectangle.Inflate(-1, -1);
		clientRectangle.Width--;
		clientRectangle.Height--;
		clientRectangle.Width -= StiScale.I10;
		if (((Control)this).Enabled)
		{
			if (!IsMouseOver && StyleBoxForm == null)
			{
				graphics.FillRectangle(SystemBrushes.Window, clientRectangle);
				graphics.DrawRectangle(Pens.Gray, clientRectangle);
			}
		}
		else
		{
			graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
			graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
		}
		if (SelectedStyle != null)
		{
			clientRectangle.Height++;
			StiBaseStyle.DrawStyle(graphics, clientRectangle, SelectedStyle, (DrawItemState)0, fillBackground: false, paintImage: true);
		}
		else
		{
			DrawText(graphics);
		}
	}

	private void StyleBoxForm_ClosePopup(object sender, EventArgs e)
	{
		if (StyleBoxForm != null)
		{
			StyleBoxForm.designer = null;
			((Component)(object)StyleBoxForm).Dispose();
			StyleBoxForm = null;
			Pushed = false;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		if (!LockPopupInvoke && StyleBoxForm == null)
		{
			InvokeDropDown(this, EventArgs.Empty);
			StyleBoxForm = new StiStyleBoxForm((Control)(object)this, designer, ShowEditStyles, ShowReportStyles, ShowCrossTabStyles, ShowChartStyles);
			Rectangle rect = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			int num = 0;
			if (designer != null)
			{
				foreach (StiBaseStyle style in designer.Report.Styles)
				{
					if (style is StiStyle && ShowReportStyles)
					{
						num++;
					}
					else if (style is StiChartStyle && ShowChartStyles)
					{
						num++;
					}
					else if (style is StiCrossTabStyle && ShowCrossTabStyles)
					{
						num++;
					}
				}
			}
			if (ShowPredefinedStyles)
			{
				num += StiOptions.Designer.Styles.Count;
			}
			if (ShowCustomStyles)
			{
				num += CustomColors.Count;
			}
			if (IsCrossTabStyle)
			{
				num = StiOptions.Designer.CrossTab.StyleColors.Length;
			}
			int width = Math.Max(StiScale.XXI(140.0), ((Control)this).Width);
			int num2 = num * OneStyleHeight + StiScale.I4 + (ShowNone ? OneStyleHeight : 0) + (ShowEditStyles ? OneStyleHeight : 0);
			int currentHeightScreen = GetCurrentHeightScreen(rect);
			bool flag = false;
			int num3 = currentHeightScreen - rect.Top - ((Control)this).Height - 2;
			int y = rect.Top + ((Control)this).Height + 1;
			if (num2 < num3)
			{
				flag = true;
			}
			else if (num2 < rect.Top - 1)
			{
				y = rect.Top - 1 - num2;
				flag = true;
			}
			if (!flag)
			{
				if (num3 < 100)
				{
					num2 = rect.Top - 1;
					y = rect.Top - 1 - num2;
				}
				else
				{
					num2 = num3;
				}
			}
			StyleBoxForm.SetLocation(rect.Left, y, width, num2);
			StyleBoxForm.ClosePopup += StyleBoxForm_ClosePopup;
			StyleBoxForm.ShowPopupForm();
		}
		else
		{
			LockPopupInvoke = false;
			Pushed = true;
		}
	}

	private int GetCurrentHeightScreen(Rectangle rect)
	{
		for (int i = 0; i < Screen.AllScreens.Length; i++)
		{
			Screen val = Screen.AllScreens[i];
			if (val.WorkingArea.IntersectsWith(rect))
			{
				return val.WorkingArea.Height - StiScale.I2;
			}
		}
		return Screen.FromControl((Control)(object)this).WorkingArea.Height - StiScale.I2;
	}

	public virtual void InvokeDropDown(object sender, EventArgs e)
	{
		this.DropDown?.Invoke(sender, e);
	}

	public virtual void InvokeDropUp(object sender, EventArgs e)
	{
		this.DropUp?.Invoke(sender, e);
	}

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(sender, e);
	}

	public virtual void InvokeEditStylesInvoked(object sender, EventArgs e)
	{
		this.EditStylesInvoked?.Invoke(sender, e);
	}

	public StiStyleBox()
	{
		base.IsDrawDropDownArrow = true;
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		base.Style = StiToolBarButtonStyle.PushButton;
	}
}
