using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL.Utility;

internal class MyPopupForm : Form
{
	private bool mouseOnClose;

	private bool mouseOnLink;

	private bool mouseOnOptions;

	private int heightOfTitle;

	private bool gdiInitialized;

	private Rectangle rcBody;

	private Rectangle rcHeader;

	private Rectangle rcForm;

	private LinearGradientBrush brushBody;

	private LinearGradientBrush brushHeader;

	private Brush brushButtonHover;

	private Pen penButtonBorder;

	private Pen penContent;

	private Pen penBorder;

	private Brush brushForeColor;

	private Brush brushLinkHover;

	private Brush brushContent;

	private Brush brushTitle;

	public ClsNotif Parent { get; set; }

	private RectangleF RectContentText
	{
		get
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			Padding val;
			if (Parent.Image == null)
			{
				val = Parent.ContentPadding;
				float x = ((Padding)(ref val)).Left;
				int headerHeight = Parent.HeaderHeight;
				val = Parent.TitlePadding;
				int num = headerHeight + ((Padding)(ref val)).Top + heightOfTitle;
				val = Parent.TitlePadding;
				int num2 = num + ((Padding)(ref val)).Bottom;
				val = Parent.ContentPadding;
				float y = num2 + ((Padding)(ref val)).Top;
				int width = ((Control)this).Width;
				val = Parent.ContentPadding;
				int num3 = width - ((Padding)(ref val)).Left;
				val = Parent.ContentPadding;
				float width2 = num3 - ((Padding)(ref val)).Right - 16 - 5;
				int num4 = ((Control)this).Height - Parent.HeaderHeight;
				val = Parent.TitlePadding;
				int num5 = num4 - ((Padding)(ref val)).Top - heightOfTitle;
				val = Parent.TitlePadding;
				int num6 = num5 - ((Padding)(ref val)).Bottom;
				val = Parent.ContentPadding;
				int num7 = num6 - ((Padding)(ref val)).Top;
				val = Parent.ContentPadding;
				return new RectangleF(x, y, width2, num7 - ((Padding)(ref val)).Bottom - 1);
			}
			val = Parent.ImagePadding;
			int num8 = ((Padding)(ref val)).Left + Parent.ImageSize.Width;
			val = Parent.ImagePadding;
			int num9 = num8 + ((Padding)(ref val)).Right;
			val = Parent.ContentPadding;
			float x2 = num9 + ((Padding)(ref val)).Left;
			int headerHeight2 = Parent.HeaderHeight;
			val = Parent.TitlePadding;
			int num10 = headerHeight2 + ((Padding)(ref val)).Top + heightOfTitle;
			val = Parent.TitlePadding;
			int num11 = num10 + ((Padding)(ref val)).Bottom;
			val = Parent.ContentPadding;
			float y2 = num11 + ((Padding)(ref val)).Top;
			int width3 = ((Control)this).Width;
			val = Parent.ImagePadding;
			int num12 = width3 - ((Padding)(ref val)).Left - Parent.ImageSize.Width;
			val = Parent.ImagePadding;
			int num13 = num12 - ((Padding)(ref val)).Right;
			val = Parent.ContentPadding;
			int num14 = num13 - ((Padding)(ref val)).Left;
			val = Parent.ContentPadding;
			float width4 = num14 - ((Padding)(ref val)).Right - 16 - 5;
			int num15 = ((Control)this).Height - Parent.HeaderHeight;
			val = Parent.TitlePadding;
			int num16 = num15 - ((Padding)(ref val)).Top - heightOfTitle;
			val = Parent.TitlePadding;
			int num17 = num16 - ((Padding)(ref val)).Bottom;
			val = Parent.ContentPadding;
			int num18 = num17 - ((Padding)(ref val)).Top;
			val = Parent.ContentPadding;
			return new RectangleF(x2, y2, width4, num18 - ((Padding)(ref val)).Bottom - 1);
		}
	}

	private Rectangle RectClose => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3, 16, 16);

	private Rectangle RectOptions => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3 + 16 + 5, 16, 16);

	public event EventHandler LinkClick;

	public event EventHandler CloseClick;

	internal event EventHandler ContextMenuOpened;

	internal event EventHandler ContextMenuClosed;

	public MyPopupForm(ClsNotif parent)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		Parent = parent;
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Form)this).ShowInTaskbar = false;
		((Control)this).VisibleChanged += PopupNotifierForm_VisibleChanged;
		((Control)this).MouseMove += new MouseEventHandler(PopupNotifierForm_MouseMove);
		((Control)this).MouseUp += new MouseEventHandler(PopupNotifierForm_MouseUp);
		((Control)this).Paint += new PaintEventHandler(PopupNotifierForm_Paint);
	}

	private void PopupNotifierForm_VisibleChanged(object sender, EventArgs e)
	{
		if (((Control)this).Visible)
		{
			mouseOnClose = false;
			mouseOnLink = false;
			mouseOnOptions = false;
		}
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((Form)this).ClientSize = new Size(392, 66);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "MyPopupForm";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).ResumeLayout(false);
	}

	private int AddValueMax255(int input, int add)
	{
		if (input + add < 256)
		{
			return input + add;
		}
		return 255;
	}

	private int DedValueMin0(int input, int ded)
	{
		if (input - ded > 0)
		{
			return input - ded;
		}
		return 0;
	}

	private Color GetDarkerColor(Color color)
	{
		return Color.FromArgb(255, DedValueMin0(color.R, Parent.GradientPower), DedValueMin0(color.G, Parent.GradientPower), DedValueMin0(color.B, Parent.GradientPower));
	}

	private Color GetLighterColor(Color color)
	{
		return Color.FromArgb(255, AddValueMax255(color.R, Parent.GradientPower), AddValueMax255(color.G, Parent.GradientPower), AddValueMax255(color.B, Parent.GradientPower));
	}

	private void PopupNotifierForm_MouseMove(object sender, MouseEventArgs e)
	{
		if (Parent.ShowCloseButton)
		{
			mouseOnClose = RectClose.Contains(e.X, e.Y);
		}
		if (Parent.ShowOptionsButton)
		{
			mouseOnOptions = RectOptions.Contains(e.X, e.Y);
		}
		mouseOnLink = RectContentText.Contains(e.X, e.Y);
		((Control)this).Invalidate();
	}

	private void PopupNotifierForm_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		if ((int)e.Button != 1048576)
		{
			return;
		}
		if (RectClose.Contains(e.X, e.Y) && this.CloseClick != null)
		{
			this.CloseClick(this, EventArgs.Empty);
		}
		if (RectContentText.Contains(e.X, e.Y) && this.LinkClick != null)
		{
			this.LinkClick(this, EventArgs.Empty);
		}
		if (RectOptions.Contains(e.X, e.Y) && Parent.OptionsMenu != null)
		{
			if (this.ContextMenuOpened != null)
			{
				this.ContextMenuOpened(this, EventArgs.Empty);
			}
			((ToolStripDropDown)Parent.OptionsMenu).Show((Control)(object)this, new Point(RectOptions.Right - ((Control)Parent.OptionsMenu).Width, RectOptions.Bottom));
			((ToolStripDropDown)Parent.OptionsMenu).Closed += new ToolStripDropDownClosedEventHandler(OptionsMenu_Closed);
		}
	}

	private void OptionsMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		((ToolStripDropDown)Parent.OptionsMenu).Closed -= new ToolStripDropDownClosedEventHandler(OptionsMenu_Closed);
		if (this.ContextMenuClosed != null)
		{
			this.ContextMenuClosed(this, EventArgs.Empty);
		}
	}

	private void AllocateGDIObjects()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		rcBody = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		rcHeader = new Rectangle(0, 0, ((Control)this).Width, Parent.HeaderHeight);
		rcForm = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		brushBody = new LinearGradientBrush(rcBody, Parent.BodyColor, GetLighterColor(Parent.BodyColor), (LinearGradientMode)1);
		brushHeader = new LinearGradientBrush(rcHeader, Parent.HeaderColor, GetDarkerColor(Parent.HeaderColor), (LinearGradientMode)1);
		brushButtonHover = (Brush)new SolidBrush(Parent.ButtonHoverColor);
		penButtonBorder = new Pen(Parent.ButtonBorderColor);
		penContent = new Pen(Parent.ContentColor, 2f);
		penBorder = new Pen(Parent.BorderColor);
		brushForeColor = (Brush)new SolidBrush(((Control)this).ForeColor);
		brushLinkHover = (Brush)new SolidBrush(Parent.ContentHoverColor);
		brushContent = (Brush)new SolidBrush(Parent.ContentColor);
		brushTitle = (Brush)new SolidBrush(Parent.TitleColor);
		gdiInitialized = true;
	}

	private void DisposeGDIObjects()
	{
		if (gdiInitialized)
		{
			((Brush)brushBody).Dispose();
			((Brush)brushHeader).Dispose();
			brushButtonHover.Dispose();
			penButtonBorder.Dispose();
			penContent.Dispose();
			penBorder.Dispose();
			brushForeColor.Dispose();
			brushLinkHover.Dispose();
			brushContent.Dispose();
			brushTitle.Dispose();
		}
	}

	private void PopupNotifierForm_Paint(object sender, PaintEventArgs e)
	{
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		if (!gdiInitialized)
		{
			AllocateGDIObjects();
		}
		e.Graphics.FillRectangle((Brush)(object)brushBody, rcBody);
		e.Graphics.FillRectangle((Brush)(object)brushHeader, rcHeader);
		e.Graphics.DrawRectangle(penBorder, rcForm);
		if (Parent.ShowGrip)
		{
			e.Graphics.DrawImage((Image)(object)Resources.Grip, (((Control)this).Width - ((Image)Resources.Grip).Width) / 2, (Parent.HeaderHeight - 3) / 2);
		}
		if (Parent.ShowCloseButton)
		{
			if (mouseOnClose)
			{
				e.Graphics.FillRectangle(brushButtonHover, RectClose);
				e.Graphics.DrawRectangle(penButtonBorder, RectClose);
			}
			e.Graphics.DrawLine(penContent, RectClose.Left + 4, RectClose.Top + 4, RectClose.Right - 4, RectClose.Bottom - 4);
			e.Graphics.DrawLine(penContent, RectClose.Left + 4, RectClose.Bottom - 4, RectClose.Right - 4, RectClose.Top + 4);
		}
		if (Parent.ShowOptionsButton)
		{
			if (mouseOnOptions)
			{
				e.Graphics.FillRectangle(brushButtonHover, RectOptions);
				e.Graphics.DrawRectangle(penButtonBorder, RectOptions);
			}
			e.Graphics.FillPolygon(brushForeColor, new Point[3]
			{
				new Point(RectOptions.Left + 4, RectOptions.Top + 6),
				new Point(RectOptions.Left + 12, RectOptions.Top + 6),
				new Point(RectOptions.Left + 8, RectOptions.Top + 4 + 6)
			});
		}
		Padding val;
		if (Parent.Image != null)
		{
			Graphics graphics = e.Graphics;
			Image image = Parent.Image;
			val = Parent.ImagePadding;
			int left = ((Padding)(ref val)).Left;
			int headerHeight = Parent.HeaderHeight;
			val = Parent.ImagePadding;
			graphics.DrawImage(image, left, headerHeight + ((Padding)(ref val)).Top, Parent.ImageSize.Width, Parent.ImageSize.Height);
		}
		if (Parent.IsRightToLeft)
		{
			heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
			int num = ((Control)this).Width - 30;
			StringFormat val2 = new StringFormat((StringFormatFlags)1);
			Graphics graphics2 = e.Graphics;
			string titleText = Parent.TitleText;
			Font titleFont = Parent.TitleFont;
			Brush obj = brushTitle;
			float num2 = num;
			int headerHeight2 = Parent.HeaderHeight;
			val = Parent.TitlePadding;
			graphics2.DrawString(titleText, titleFont, obj, num2, (float)(headerHeight2 + ((Padding)(ref val)).Top), val2);
			((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
			Brush val3 = (mouseOnLink ? brushLinkHover : brushContent);
			StringFormat val4 = new StringFormat((StringFormatFlags)1);
			e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val3, RectContentText, val4);
			return;
		}
		heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
		val = Parent.TitlePadding;
		int num3 = ((Padding)(ref val)).Left;
		if (Parent.Image != null)
		{
			int num4 = num3;
			val = Parent.ImagePadding;
			int num5 = ((Padding)(ref val)).Left + Parent.ImageSize.Width;
			val = Parent.ImagePadding;
			num3 = num4 + (num5 + ((Padding)(ref val)).Right);
		}
		Graphics graphics3 = e.Graphics;
		string titleText2 = Parent.TitleText;
		Font titleFont2 = Parent.TitleFont;
		Brush obj2 = brushTitle;
		float num6 = num3;
		int headerHeight3 = Parent.HeaderHeight;
		val = Parent.TitlePadding;
		graphics3.DrawString(titleText2, titleFont2, obj2, num6, (float)(headerHeight3 + ((Padding)(ref val)).Top));
		((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
		Brush val5 = (mouseOnLink ? brushLinkHover : brushContent);
		e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val5, RectContentText);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			DisposeGDIObjects();
		}
		((Form)this).Dispose(disposing);
	}
}
