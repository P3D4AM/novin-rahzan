using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using Microsoft.Win32;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ToolTip : Control
{
	private const long च = 2147483648L;

	private const long छ = 67108864L;

	private const long ज = 33554432L;

	private const long ञ = 128L;

	private const long झ = 8L;

	private string _0CA9;

	private _0CB4 ݶ;

	private eDotNetBarStyle Ӷ = eDotNetBarStyle.Office2003;

	private ഓ _0B84;

	private bool ե;

	public Rectangle ReferenceRectangle = Rectangle.Empty;

	private static bool _ಪ = true;

	private int _ಫ = -1;

	private bool _ಬ;

	private Point _ಭ = Point.Empty;

	private bool _ಮ = true;

	public string Text
	{
		get
		{
			return _0CA9;
		}
		set
		{
			_0CA9 = value;
			if (MarkupEnabled && ሒ.መ(ref _0CA9))
			{
				ݶ = ሒ.ሕ(_0CA9);
			}
			else
			{
				ݶ = null;
			}
			if (ݶ != null)
			{
				AntiAlias = true;
			}
			if (_ಬ)
			{
				ಯ();
			}
		}
	}

	public static bool MarkupEnabled
	{
		get
		{
			return _ಪ;
		}
		set
		{
			_ಪ = value;
		}
	}

	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			ե = value;
		}
	}

	public Point FixedLocation
	{
		get
		{
			return _ಭ;
		}
		set
		{
			_ಭ = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 136;
			createParams.Caption = "";
			return createParams;
		}
	}

	public eDotNetBarStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
			if (BarFunctions.IsOffice2007Style(Ӷ))
			{
				AntiAlias = true;
			}
		}
	}

	public bool ShowDropShadow
	{
		get
		{
			return _ಮ;
		}
		set
		{
			_ಮ = value;
		}
	}

	public ToolTip()
	{
		_0CA9 = "";
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Font = SystemFonts.DefaultFont;
		((Control)this).BackColor = SystemColors.Control;
		((Control)this).ForeColor = SystemColors.ControlText;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Invalid comparison between Unknown and I4
		Color infoText = SystemColors.InfoText;
		Color color = SystemColors.Info;
		Color color2 = Color.Empty;
		Color color3 = ColorScheme.GetColor("767676");
		if (_ಫ == -1)
		{
			_ಫ = Screen.FromControl((Control)(object)this).BitsPerPixel;
		}
		if (_ಫ < 8)
		{
			color = Color.White;
			infoText = Color.Black;
		}
		else if (StyleManager.IsMetro(Ӷ))
		{
			MetroColorTable colorTable = MetroRender.GetColorTable();
			color = colorTable.CanvasColor;
			infoText = colorTable.ForeColor;
		}
		else if (BarFunctions.IsOffice2007Style(Ӷ) && ((Control)this).BackColor == SystemColors.Control)
		{
			color = ColorScheme.GetColor("FFFFFF");
			color2 = ColorScheme.GetColor("E4E4F0");
			infoText = ColorScheme.GetColor("4C4C4C");
		}
		else
		{
			infoText = ಳ();
			if (((Control)this).BackColor != SystemColors.Control)
			{
				color = ((Control)this).BackColor;
			}
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (ե)
		{
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		if (BarFunctions.IsOffice2007Style(Ӷ))
		{
			DisplayHelp.DrawRectangle(graphics, color3, clientRectangle);
		}
		else
		{
			ControlPaint.DrawBorder3D(graphics, clientRectangle, (Border3DStyle)5, (Border3DSide)2063);
		}
		clientRectangle.Inflate(-1, -1);
		if (color2.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, color);
		}
		else
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, color, color2);
		}
		clientRectangle.Offset(1, 0);
		graphics.SmoothingMode = smoothingMode;
		if (ݶ == null)
		{
			TextDrawing.DrawString(graphics, ಱ(), ((Control)this).Font, infoText, clientRectangle, ಲ());
			return;
		}
		MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, infoText, (int)((Control)this).RightToLeft == 1, e.ClipRectangle, hotKeyPrefixVisible: true);
		ݶ.Bounds = new Rectangle(clientRectangle.Location, ݶ.Bounds.Size);
		ݶ.Render(d);
	}

	private void ಯ()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			if (ե)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				val.SmoothingMode = (SmoothingMode)4;
			}
			Size size = ರ(val);
			((Control)this).Size = size;
			val.SmoothingMode = (SmoothingMode)0;
			val.TextRenderingHint = (TextRenderingHint)0;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!_ಭ.IsEmpty)
		{
			((Control)this).Location = _ಭ;
		}
		((Control)this).Invalidate();
		Rectangle bounds = ((Control)this).Bounds;
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, ((Control)this).Handle, bounds.Left + 5, bounds.Top + 5, bounds.Width - 2, bounds.Height - 2, 80);
			_0B84.ഖ();
		}
	}

	private Size ರ(Graphics _٥)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		Size empty = Size.Empty;
		if (ݶ == null)
		{
			empty = TextDrawing.MeasureString(_٥, ಱ(), ((Control)this).Font, Screen.PrimaryScreen.WorkingArea.Size, ಲ());
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			ݶ.Measure(Screen.PrimaryScreen.WorkingArea.Size, markupDrawContext);
			empty = ݶ.Bounds.Size;
			ݶ._0CC6(new Rectangle(Point.Empty, empty), markupDrawContext);
		}
		empty.Width += Dpi.Width4;
		empty.Height += Dpi.Height4;
		return empty;
	}

	public void ShowToolTip()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!((Control)this).IsHandleCreated)
		{
			((Control)this).CreateControl();
		}
		Graphics val = ((Control)this).CreateGraphics();
		if (ե)
		{
			val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			val.SmoothingMode = (SmoothingMode)4;
		}
		Size size = Size.Empty;
		try
		{
			val.PageUnit = (GraphicsUnit)2;
			size = ರ(val);
		}
		finally
		{
			val.SmoothingMode = (SmoothingMode)0;
			val.TextRenderingHint = (TextRenderingHint)0;
			val.Dispose();
		}
		val = null;
		Point mousePosition = Control.MousePosition;
		Rectangle rectangle = new Rectangle(Control.MousePosition.X, Control.MousePosition.Y, size.Width, size.Height);
		rectangle.Offset(12, 24);
		ঊ ঊ2 = BarFunctions.ॹ(mousePosition);
		if (ঊ2 != null)
		{
			Size size2 = ঊ2.ঌ.Size;
			size2.Width -= (int)((float)size2.Width * 0.2f);
			if (rectangle.Right > ঊ2.ঌ.Right)
			{
				rectangle.X = rectangle.Left - (rectangle.Right - ঊ2.ঌ.Right);
			}
			if (rectangle.Bottom > ঊ2.ঋ.Bottom)
			{
				if (ReferenceRectangle.IsEmpty)
				{
					rectangle.Y = mousePosition.Y - rectangle.Height;
				}
				else
				{
					rectangle.Y = ReferenceRectangle.Y - rectangle.Height - 1;
				}
			}
			if (rectangle.Contains(Control.MousePosition.X, Control.MousePosition.Y))
			{
				if (rectangle.Height + Control.MousePosition.Y + 1 <= ঊ2.ঌ.Height && (ReferenceRectangle.IsEmpty || !ReferenceRectangle.IntersectsWith(new Rectangle(rectangle.X, Control.MousePosition.Y + 1, rectangle.Width, rectangle.Height))))
				{
					rectangle.Y = Control.MousePosition.Y + 1;
				}
				else
				{
					rectangle.Y = Control.MousePosition.Y - rectangle.Height - 1;
				}
			}
		}
		if (!_ಭ.IsEmpty)
		{
			rectangle.Location = _ಭ;
		}
		((Control)this).Location = rectangle.Location;
		((Control)this).ClientSize = rectangle.Size;
		if (_089F.ࢣ && ShowDropShadow)
		{
			if (_0B84 == null)
			{
				_0B84 = new ഓ(_089F.ࢤ);
				((Control)_0B84).CreateControl();
			}
			((Control)_0B84).Hide();
		}
		_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, ((Control)this).Handle, rectangle.Left + 5, rectangle.Top + 5, rectangle.Width - 2, rectangle.Height - 2, 80);
			_0B84.ഖ();
		}
		_ಬ = true;
	}

	private string ಱ()
	{
		string text = _0CA9.Replace("\\\\n", "{spec_nl}");
		text = text.Replace("\\n", Environment.NewLine);
		return text.Replace("{spec_nl}", "\\n");
	}

	private eTextFormat ಲ()
	{
		return eTextFormat.VerticalCenter | eTextFormat.WordBreak;
	}

	private Color ಳ()
	{
		if (((Control)this).ForeColor != SystemColors.ControlText)
		{
			return ((Control)this).ForeColor;
		}
		Color result = SystemColors.WindowText;
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Colors\\", writable: false);
			string text = registryKey.GetValue("InfoText").ToString();
			registryKey.Close();
			registryKey = null;
			if (text == null || text == "")
			{
				return result;
			}
			char[] separator = new char[1] { ' ' };
			string[] array = text.Split(separator, 3);
			if (array.GetUpperBound(0) == 2)
			{
				result = Color.FromArgb(Convert.ToInt16(array[0]), Convert.ToInt16(array[1]), Convert.ToInt16(array[2]));
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(0), ((Control)this).Left + 5, ((Control)this).Top + 5, 0, 0, 81);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		((Control)this).OnVisibleChanged(e);
	}
}
