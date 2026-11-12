using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Design.Components;

public class StiColorButtonXF : StiToolButton, IStiPopupParentControl
{
	private Color selectedColor = Color.Transparent;

	private Panel rootPanel;

	private StiPopup colorPopup;

	private StiColorSelectionControl colorSelectionControl;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((ButtonBase)this).Text;
		}
		set
		{
			((ButtonBase)this).Text = value;
		}
	}

	internal StiColorBoxPopupForm ColorBoxPopupForm { get; set; }

	[Category("Behavior")]
	public Color SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			selectedColor = value;
			InvokeSelectedColorChanged();
			((Control)this).Invalidate();
		}
	}

	public event EventHandler SelectedColorChanged;

	protected void InvokeSelectedColorChanged()
	{
		this.SelectedColorChanged?.Invoke(this, EventArgs.Empty);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		base.OnPaint(p);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(clientRectangle.X + StiScaleUI.I2, clientRectangle.Bottom - StiScaleUI.I5, clientRectangle.Width - StiScaleUI.I14, StiScaleUI.I4);
		if (!((Control)this).Enabled)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(SelectedColor);
		try
		{
			graphics.FillRectangle(val, rectangle);
			if (SelectedColor == Color.Transparent)
			{
				graphics.DrawRectangle(SystemPens.ControlDark, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ColorPopup_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		base.Pushed = false;
		((Control)this).Invalidate();
	}

	private void ColorSelectionControl_ColorChanged(object sender, EventArgs e)
	{
		selectedColor = colorSelectionControl.SelectedColor;
		InvokeSelectedColorChanged();
		((ToolStripDropDown)colorPopup).Close();
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		base.OnMouseDown(e);
		colorPopup = new StiPopup((Control)(object)GetPanel());
		((ToolStripDropDown)colorPopup).Closed += new ToolStripDropDownClosedEventHandler(ColorPopup_Closed);
		((ToolStripDropDown)colorPopup).Show(new Point(((Control)this).RectangleToScreen(((Control)this).ClientRectangle).Left, ((Control)this).RectangleToScreen(((Control)this).ClientRectangle).Bottom));
	}

	protected override void DrawImage(Graphics g, Rectangle imageRect)
	{
		imageRect.Y -= StiScale.I2;
		base.DrawImage(g, imageRect);
	}

	private StiColorSelectionControl GetColorControl()
	{
		StiColorSelectionControl stiColorSelectionControl = new StiColorSelectionControl(SelectedColor, isExpression: false, null);
		((Control)stiColorSelectionControl).Location = new Point(StiScale.I1, StiScale.I1);
		((Control)stiColorSelectionControl).Width = StiScale.YYI(163.0);
		((Control)stiColorSelectionControl).Anchor = (AnchorStyles)13;
		stiColorSelectionControl.SelectedColor = SelectedColor;
		colorSelectionControl = stiColorSelectionControl;
		colorSelectionControl.ColorChanged += ColorSelectionControl_ColorChanged;
		return colorSelectionControl;
	}

	private Panel GetPanel()
	{
		rootPanel = GetRootPanel();
		((Control)rootPanel).Height = GetColorControl().GetHeight() + StiScale.I2;
		((Control)rootPanel).Controls.Add((Control)(object)GetColorControl());
		return rootPanel;
	}

	private Panel GetRootPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(165.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		return val;
	}

	public StiColorButtonXF()
	{
		base.IsDrawDropDownArrow = true;
		base.Style = StiToolBarButtonStyle.PushButton;
	}
}
