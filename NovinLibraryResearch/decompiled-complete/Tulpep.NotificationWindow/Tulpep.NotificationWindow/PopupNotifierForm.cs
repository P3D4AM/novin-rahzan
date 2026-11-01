using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tulpep.NotificationWindow.Properties;

namespace Tulpep.NotificationWindow;

internal class PopupNotifierForm : Form
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

	public PopupNotifier Parent { get; set; }

	private RectangleF RectContentText
	{
		get
		{
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			if (Parent.Image != null)
			{
				Padding imagePadding = Parent.ImagePadding;
				int num = ((Padding)(ref imagePadding)).Left + Parent.ImageSize.Width;
				Padding imagePadding2 = Parent.ImagePadding;
				int num2 = num + ((Padding)(ref imagePadding2)).Right;
				Padding contentPadding = Parent.ContentPadding;
				float x = num2 + ((Padding)(ref contentPadding)).Left;
				int headerHeight = Parent.HeaderHeight;
				Padding titlePadding = Parent.TitlePadding;
				int num3 = headerHeight + ((Padding)(ref titlePadding)).Top + heightOfTitle;
				Padding titlePadding2 = Parent.TitlePadding;
				int num4 = num3 + ((Padding)(ref titlePadding2)).Bottom;
				Padding contentPadding2 = Parent.ContentPadding;
				float y = num4 + ((Padding)(ref contentPadding2)).Top;
				int width = ((Control)this).Width;
				Padding imagePadding3 = Parent.ImagePadding;
				int num5 = width - ((Padding)(ref imagePadding3)).Left - Parent.ImageSize.Width;
				Padding imagePadding4 = Parent.ImagePadding;
				int num6 = num5 - ((Padding)(ref imagePadding4)).Right;
				Padding contentPadding3 = Parent.ContentPadding;
				int num7 = num6 - ((Padding)(ref contentPadding3)).Left;
				Padding contentPadding4 = Parent.ContentPadding;
				float width2 = num7 - ((Padding)(ref contentPadding4)).Right - 16 - 5;
				int num8 = ((Control)this).Height - Parent.HeaderHeight;
				Padding titlePadding3 = Parent.TitlePadding;
				int num9 = num8 - ((Padding)(ref titlePadding3)).Top - heightOfTitle;
				Padding titlePadding4 = Parent.TitlePadding;
				int num10 = num9 - ((Padding)(ref titlePadding4)).Bottom;
				Padding contentPadding5 = Parent.ContentPadding;
				int num11 = num10 - ((Padding)(ref contentPadding5)).Top;
				Padding contentPadding6 = Parent.ContentPadding;
				return new RectangleF(x, y, width2, num11 - ((Padding)(ref contentPadding6)).Bottom - 1);
			}
			Padding contentPadding7 = Parent.ContentPadding;
			float x2 = ((Padding)(ref contentPadding7)).Left;
			int headerHeight2 = Parent.HeaderHeight;
			Padding titlePadding5 = Parent.TitlePadding;
			int num12 = headerHeight2 + ((Padding)(ref titlePadding5)).Top + heightOfTitle;
			Padding titlePadding6 = Parent.TitlePadding;
			int num13 = num12 + ((Padding)(ref titlePadding6)).Bottom;
			Padding contentPadding8 = Parent.ContentPadding;
			float y2 = num13 + ((Padding)(ref contentPadding8)).Top;
			int width3 = ((Control)this).Width;
			Padding contentPadding9 = Parent.ContentPadding;
			int num14 = width3 - ((Padding)(ref contentPadding9)).Left;
			Padding contentPadding10 = Parent.ContentPadding;
			float width4 = num14 - ((Padding)(ref contentPadding10)).Right - 16 - 5;
			int num15 = ((Control)this).Height - Parent.HeaderHeight;
			Padding titlePadding7 = Parent.TitlePadding;
			int num16 = num15 - ((Padding)(ref titlePadding7)).Top - heightOfTitle;
			Padding titlePadding8 = Parent.TitlePadding;
			int num17 = num16 - ((Padding)(ref titlePadding8)).Bottom;
			Padding contentPadding11 = Parent.ContentPadding;
			int num18 = num17 - ((Padding)(ref contentPadding11)).Top;
			Padding contentPadding12 = Parent.ContentPadding;
			return new RectangleF(x2, y2, width4, num18 - ((Padding)(ref contentPadding12)).Bottom - 1);
		}
	}

	private Rectangle RectClose => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3, 16, 16);

	private Rectangle RectOptions => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3 + 16 + 5, 16, 16);

	public event EventHandler LinkClick;

	public event EventHandler CloseClick;

	internal event EventHandler ContextMenuOpened;

	internal event EventHandler ContextMenuClosed;

	public PopupNotifierForm(PopupNotifier parent)
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
		((Control)this).Name = "PopupNotifierForm";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).ResumeLayout(false);
	}

	private int AddValueMax255(int input, int add)
	{
		if (input + add >= 256)
		{
			return 255;
		}
		return input + add;
	}

	private int DedValueMin0(int input, int ded)
	{
		if (input - ded <= 0)
		{
			return 0;
		}
		return input - ded;
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
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
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
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
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
		if (Parent.Image != null)
		{
			Graphics graphics = e.Graphics;
			Image image = Parent.Image;
			Padding imagePadding = Parent.ImagePadding;
			int left = ((Padding)(ref imagePadding)).Left;
			int headerHeight = Parent.HeaderHeight;
			Padding imagePadding2 = Parent.ImagePadding;
			graphics.DrawImage(image, left, headerHeight + ((Padding)(ref imagePadding2)).Top, Parent.ImageSize.Width, Parent.ImageSize.Height);
		}
		if (Parent.IsRightToLeft)
		{
			heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
			int num = ((Control)this).Width - 30;
			StringFormat val = new StringFormat((StringFormatFlags)1);
			Graphics graphics2 = e.Graphics;
			string titleText = Parent.TitleText;
			Font titleFont = Parent.TitleFont;
			Brush obj = brushTitle;
			float num2 = num;
			int headerHeight2 = Parent.HeaderHeight;
			Padding titlePadding = Parent.TitlePadding;
			graphics2.DrawString(titleText, titleFont, obj, num2, (float)(headerHeight2 + ((Padding)(ref titlePadding)).Top), val);
			((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
			Brush val2 = (mouseOnLink ? brushLinkHover : brushContent);
			StringFormat val3 = new StringFormat((StringFormatFlags)1);
			e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val2, RectContentText, val3);
			return;
		}
		heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
		Padding titlePadding2 = Parent.TitlePadding;
		int num3 = ((Padding)(ref titlePadding2)).Left;
		if (Parent.Image != null)
		{
			int num4 = num3;
			Padding imagePadding3 = Parent.ImagePadding;
			int num5 = ((Padding)(ref imagePadding3)).Left + Parent.ImageSize.Width;
			Padding imagePadding4 = Parent.ImagePadding;
			num3 = num4 + (num5 + ((Padding)(ref imagePadding4)).Right);
		}
		Graphics graphics3 = e.Graphics;
		string titleText2 = Parent.TitleText;
		Font titleFont2 = Parent.TitleFont;
		Brush obj2 = brushTitle;
		float num6 = num3;
		int headerHeight3 = Parent.HeaderHeight;
		Padding titlePadding3 = Parent.TitlePadding;
		graphics3.DrawString(titleText2, titleFont2, obj2, num6, (float)(headerHeight3 + ((Padding)(ref titlePadding3)).Top));
		((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
		Brush val4 = (mouseOnLink ? brushLinkHover : brushContent);
		e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val4, RectContentText);
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
