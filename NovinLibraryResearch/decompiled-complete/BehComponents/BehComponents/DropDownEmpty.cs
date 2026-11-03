using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BehComponents.Properties;

namespace BehComponents;

[ToolboxItem(false)]
[Description("DropDown ComboBox ready for use. From CodeProject.com. Edited by Saeed Mohammadi")]
public class DropDownEmpty : UserControl
{
	public enum Alignments
	{
		Left,
		Right
	}

	public enum DropStates
	{
		Closed,
		Closing,
		Dropping,
		Dropped
	}

	public enum ImageSizeModes
	{
		Normal,
		Stretch,
		Zoom
	}

	private class ClearButton
	{
		private UserControl MainForm;

		public float X_coordinate;

		public float Y_coordinate;

		public float Width;

		public float Height;

		public Color BackColor = Color.White;

		public Color ForeColor = Color.Black;

		public Image ImageButton;

		public ContentAlignment ImageAlign = (ContentAlignment)32;

		public Size ImageFixedSize = new Size(0, 0);

		public ImageSizeModes ImageSizeMode = ImageSizeModes.Zoom;

		public string Text;

		public ContentAlignment TextAlign = (ContentAlignment)32;

		public Font FontText;

		public RightToLeft RightToLeft = (RightToLeft)1;

		public bool Enabled = true;

		public Color DisableColor = Color.FromArgb(150, 150, 150);

		public bool Visible = true;

		private bool _MouseOverOnButton;

		private bool _MouseDownOnButton;

		public ClearButton(UserControl form)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			MainForm = form;
		}

		public void Render()
		{
			Graphics val = ((Control)MainForm).CreateGraphics();
			RenderButton(val);
			val.Dispose();
		}

		public void Render(bool mouseOver, bool mouseDown)
		{
			if (_MouseDownOnButton != mouseDown || _MouseOverOnButton != mouseOver)
			{
				Graphics val = ((Control)MainForm).CreateGraphics();
				_MouseDownOnButton = mouseDown;
				_MouseOverOnButton = mouseOver;
				RenderButton(val);
				val.Dispose();
			}
		}

		private void RenderButton(Graphics g)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Invalid comparison between Unknown and I4
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Expected O, but got Unknown
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Invalid comparison between Unknown and I4
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Invalid comparison between Unknown and I4
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected I4, but got Unknown
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Invalid comparison between Unknown and I4
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Invalid comparison between Unknown and I4
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Invalid comparison between Unknown and I4
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Invalid comparison between Unknown and I4
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Invalid comparison between Unknown and I4
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Expected I4, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Invalid comparison between Unknown and I4
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Invalid comparison between Unknown and I4
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Invalid comparison between Unknown and I4
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Invalid comparison between Unknown and I4
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Invalid comparison between Unknown and I4
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Invalid comparison between Unknown and I4
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Invalid comparison between Unknown and I4
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Expected O, but got Unknown
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Expected O, but got Unknown
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Invalid comparison between Unknown and I4
			if (!Visible || Width == 0f || Height == 0f)
			{
				return;
			}
			Color gold = Color.Gold;
			Color goldenrod = Color.Goldenrod;
			RectangleF rectangleF = new RectangleF(X_coordinate, Y_coordinate, Width, Height);
			Brush val = (Brush)(_MouseDownOnButton ? new SolidBrush(goldenrod) : ((!_MouseOverOnButton) ? new SolidBrush(BackColor) : new SolidBrush(gold)));
			g.FillRectangle(val, rectangleF);
			val.Dispose();
			if (ImageButton != null)
			{
				Size imageSize = GetImageSize();
				RectangleF rectangleF2 = default(RectangleF);
				rectangleF2.Size = imageSize;
				ContentAlignment imageAlign = ImageAlign;
				if ((int)imageAlign <= 32)
				{
					switch (imageAlign - 1)
					{
					default:
						if ((int)imageAlign != 16)
						{
							if ((int)imageAlign == 32)
							{
								float num = (Width - (float)imageSize.Width) / 2f;
								rectangleF2.X = X_coordinate + num;
								num = (Height - (float)imageSize.Height) / 2f;
								rectangleF2.Y = Y_coordinate + num;
							}
						}
						else
						{
							rectangleF2.X = X_coordinate;
							float num = (Height - (float)imageSize.Height) / 2f;
							rectangleF2.Y = Y_coordinate + num;
						}
						break;
					case 1:
					{
						float num = (Width - (float)imageSize.Width) / 2f;
						rectangleF2.X = X_coordinate + num;
						rectangleF2.Y = Y_coordinate;
						break;
					}
					case 0:
						rectangleF2.X = X_coordinate;
						rectangleF2.Y = Y_coordinate;
						break;
					case 3:
						rectangleF2.X = X_coordinate + Width - (float)imageSize.Width;
						rectangleF2.Y = Y_coordinate;
						break;
					case 2:
						break;
					}
				}
				else if ((int)imageAlign <= 256)
				{
					if ((int)imageAlign != 64)
					{
						if ((int)imageAlign == 256)
						{
							rectangleF2.X = X_coordinate;
							rectangleF2.Y = Y_coordinate + Height - (float)imageSize.Height;
						}
					}
					else
					{
						rectangleF2.X = X_coordinate + Width - (float)imageSize.Width;
						float num = (Height - (float)imageSize.Height) / 2f;
						rectangleF2.Y = Y_coordinate + num;
					}
				}
				else if ((int)imageAlign != 512)
				{
					if ((int)imageAlign == 1024)
					{
						rectangleF2.X = X_coordinate + Width - (float)imageSize.Width;
						rectangleF2.Y = Y_coordinate + Height - (float)imageSize.Height;
					}
				}
				else
				{
					float num = (Width - (float)imageSize.Width) / 2f;
					rectangleF2.X = X_coordinate + num;
					rectangleF2.Y = Y_coordinate + Height - (float)imageSize.Height;
				}
				if (ImageSizeMode == ImageSizeModes.Normal)
				{
					Rectangle rectangle = new Rectangle((int)rectangleF2.X, (int)rectangleF2.Y, (int)rectangleF2.Width, (int)rectangleF2.Height);
					g.DrawImageUnscaledAndClipped(ImageButton, rectangle);
				}
				else if (ImageSizeMode == ImageSizeModes.Stretch)
				{
					g.DrawImage(ImageButton, rectangleF2);
				}
				else if (ImageSizeMode == ImageSizeModes.Zoom)
				{
					g.DrawImage(ImageButton, rectangleF2);
				}
			}
			if (!(Text != string.Empty))
			{
				return;
			}
			StringFormat val2 = new StringFormat();
			if ((int)RightToLeft == 1)
			{
				val2.FormatFlags = (StringFormatFlags)1;
			}
			ContentAlignment textAlign = TextAlign;
			if ((int)textAlign <= 32)
			{
				switch (textAlign - 1)
				{
				default:
					if ((int)textAlign != 16)
					{
						if ((int)textAlign == 32)
						{
							val2.Alignment = (StringAlignment)1;
							val2.LineAlignment = (StringAlignment)1;
						}
					}
					else
					{
						val2.Alignment = (StringAlignment)0;
						val2.LineAlignment = (StringAlignment)1;
					}
					break;
				case 1:
					val2.Alignment = (StringAlignment)1;
					val2.LineAlignment = (StringAlignment)0;
					break;
				case 0:
					val2.Alignment = (StringAlignment)0;
					val2.LineAlignment = (StringAlignment)0;
					break;
				case 3:
					val2.Alignment = (StringAlignment)2;
					val2.LineAlignment = (StringAlignment)0;
					break;
				case 2:
					break;
				}
			}
			else if ((int)textAlign <= 256)
			{
				if ((int)textAlign != 64)
				{
					if ((int)textAlign == 256)
					{
						val2.Alignment = (StringAlignment)0;
						val2.LineAlignment = (StringAlignment)2;
					}
				}
				else
				{
					val2.Alignment = (StringAlignment)2;
					val2.LineAlignment = (StringAlignment)1;
				}
			}
			else if ((int)textAlign != 512)
			{
				if ((int)textAlign == 1024)
				{
					val2.Alignment = (StringAlignment)2;
					val2.LineAlignment = (StringAlignment)2;
				}
			}
			else
			{
				val2.Alignment = (StringAlignment)1;
				val2.LineAlignment = (StringAlignment)2;
			}
			Brush val3 = (Brush)((!Enabled) ? new SolidBrush(Color.FromArgb(150, 150, 150)) : new SolidBrush(ForeColor));
			g.DrawString(Text, FontText, val3, rectangleF, val2);
		}

		private Size GetImageSize()
		{
			Size result = ImageFixedSize;
			if (ImageButton != null)
			{
				int num = (int)Height;
				if (num < 0)
				{
					num = 0;
				}
				int num2 = (int)Width;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (result.Width == 0 && result.Height == 0)
				{
					result.Width = ImageButton.Width;
					result.Height = ImageButton.Height;
				}
				if (result.Height > num)
				{
					result.Height = num;
				}
				if (result.Width > num2)
				{
					result.Width = num2;
				}
				if (ImageSizeMode == ImageSizeModes.Zoom)
				{
					Size size = new Size(ImageButton.Width, ImageButton.Height);
					while (size.Width > result.Width || size.Height > result.Height)
					{
						size.Width = (int)((float)size.Width * 0.99f);
						size.Height = (int)((float)size.Height * 0.99f);
					}
					result = size;
				}
			}
			return result;
		}

		public bool IsHit(int x, int y)
		{
			if ((float)x >= X_coordinate && (float)x < X_coordinate + Width && (float)y >= Y_coordinate)
			{
				return (float)y < Y_coordinate + Height;
			}
			return false;
		}
	}

	internal sealed class DropDownContainer : Form, IMessageFilter
	{
		public delegate void DropWindowArgs(DropStates state);

		public bool Freeze;

		public event DropWindowArgs DropStateChange;

		public DropDownContainer(Control dropDownItem)
		{
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			dropDownItem.Location = new Point(1, 1);
			((Control)this).Controls.Add(dropDownItem);
			((Form)this).StartPosition = (FormStartPosition)0;
			((Form)this).ShowInTaskbar = false;
			Application.AddMessageFilter((IMessageFilter)(object)this);
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (!Freeze && ((Control)this).Visible && (Form.ActiveForm == null || !((object)Form.ActiveForm).Equals((object?)this)))
			{
				OnDropStateChange(DropStates.Closing);
				((Form)this).Close();
			}
			return false;
		}

		protected void OnDropStateChange(DropStates state)
		{
			if (this.DropStateChange != null)
			{
				this.DropStateChange(state);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			((Form)this).OnPaint(e);
			e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ((Form)this).ClientSize.Width - 1, ((Form)this).ClientSize.Height - 1));
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Application.RemoveMessageFilter((IMessageFilter)(object)this);
			((Control)this).Controls.RemoveAt(0);
			((Form)this).OnClosing(e);
		}
	}

	private IContainer components;

	private ToolTip toolTip1;

	private DropDownContainer dropContainer;

	private Control _dropDownItem;

	private bool closedWhileInControl;

	private Size storedSize;

	private int buttonWidth = 18;

	private bool _bMouseEntered;

	private bool _bGotFocused;

	private Color _GotFocusColor = Color.LightSkyBlue;

	private ClearButton _ClearButton;

	private int _iClearButtonWidth = 17;

	private bool _bClearButtonDowned;

	private string _Text;

	private ContentAlignment _TextAlign = (ContentAlignment)64;

	private bool _RtlLayout = true;

	private bool _ShowClearButton;

	private Alignments _ClearButtonAlignment;

	private string _ClearButtonToolTip = "";

	private bool _bClearButtonShownToolTip;

	private Color _ClearButtonBackColor = Color.White;

	private Size _AnchorSize = new Size(121, 21);

	private Alignments _DockSide;

	private bool _DesignView = true;

	private int _marginSide = 2;

	protected bool mousePressed;

	private DropStates _dropState;

	protected DropStates DropState => _dropState;

	public override string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			if (!(_Text == value))
			{
				_Text = value;
				((Control)this).Invalidate();
			}
		}
	}

	public ContentAlignment TextAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _TextAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (_TextAlign != value)
			{
				_TextAlign = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool RightToLeftLayout
	{
		get
		{
			return _RtlLayout;
		}
		set
		{
			if (_RtlLayout != value)
			{
				_RtlLayout = value;
				((Control)this).Invalidate();
			}
		}
	}

	public int ClearButtonWidth
	{
		get
		{
			return _iClearButtonWidth;
		}
		set
		{
			if (_iClearButtonWidth != value)
			{
				_iClearButtonWidth = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool ShowClearButton
	{
		get
		{
			return _ShowClearButton;
		}
		set
		{
			if (_ShowClearButton != value)
			{
				_ShowClearButton = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Alignments ClearButtonAlignment
	{
		get
		{
			return _ClearButtonAlignment;
		}
		set
		{
			if (_ClearButtonAlignment != value)
			{
				_ClearButtonAlignment = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Image ClearButtonImage
	{
		get
		{
			return _ClearButton.ImageButton;
		}
		set
		{
			if (_ClearButton.ImageButton != value)
			{
				_ClearButton.ImageButton = value;
				_ClearButton.Render();
			}
		}
	}

	public ContentAlignment ClearButtonImageAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _ClearButton.ImageAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (_ClearButton.ImageAlign != value)
			{
				_ClearButton.ImageAlign = value;
				_ClearButton.Render();
			}
		}
	}

	public Size ClearButtonImageFixedSize
	{
		get
		{
			return _ClearButton.ImageFixedSize;
		}
		set
		{
			if (!(_ClearButton.ImageFixedSize == value))
			{
				_ClearButton.ImageFixedSize = value;
				_ClearButton.Render();
			}
		}
	}

	public ImageSizeModes ClearButtonImageSizeMode
	{
		get
		{
			return _ClearButton.ImageSizeMode;
		}
		set
		{
			if (_ClearButton.ImageSizeMode != value)
			{
				_ClearButton.ImageSizeMode = value;
				_ClearButton.Render();
			}
		}
	}

	public Color ClearButtonForeColor
	{
		get
		{
			return _ClearButton.ForeColor;
		}
		set
		{
			_ClearButton.ForeColor = value;
			_ClearButton.Render();
		}
	}

	public Color ClearButtonBackColor
	{
		get
		{
			return _ClearButtonBackColor;
		}
		set
		{
			try
			{
				if (!(_ClearButton.BackColor == value))
				{
					_ClearButtonBackColor = value;
					((Control)this).Invalidate();
				}
			}
			catch
			{
			}
		}
	}

	public string ClearButtonText
	{
		get
		{
			return _ClearButton.Text;
		}
		set
		{
			if (!(_ClearButton.Text == value))
			{
				_ClearButton.Text = value;
				_ClearButton.Render();
			}
		}
	}

	public ContentAlignment ClearButtonTextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _ClearButton.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (_ClearButton.TextAlign != value)
			{
				_ClearButton.TextAlign = value;
				_ClearButton.Render();
			}
		}
	}

	public string ClearButtonToolTip
	{
		get
		{
			return _ClearButtonToolTip;
		}
		set
		{
			_ClearButtonToolTip = value;
		}
	}

	[Browsable(false)]
	public Size AnchorSize
	{
		get
		{
			return _AnchorSize;
		}
		set
		{
			if (!(_AnchorSize == value))
			{
				_AnchorSize = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Alignments DockSide
	{
		get
		{
			return _DockSide;
		}
		set
		{
			_DockSide = value;
		}
	}

	[DefaultValue(false)]
	protected bool DesignView
	{
		get
		{
			return _DesignView;
		}
		set
		{
			if (_DesignView != value)
			{
				_DesignView = value;
				if (_DesignView)
				{
					((Control)this).Size = storedSize;
					return;
				}
				storedSize = ((Control)this).Size;
				((Control)this).Size = _AnchorSize;
			}
		}
	}

	[Browsable(false)]
	public Rectangle AnchorClientBounds
	{
		get
		{
			Rectangle result;
			if (_RtlLayout)
			{
				int marginSide = _marginSide;
				int marginSide2 = _marginSide;
				int width = _AnchorSize.Width - marginSide - _marginSide;
				int height = _AnchorSize.Height - _marginSide - _marginSide;
				result = new Rectangle(marginSide, marginSide2, width, height);
			}
			else
			{
				int marginSide = _marginSide;
				int marginSide2 = _marginSide;
				int width = _AnchorSize.Width - _marginSide - _marginSide;
				int height = _AnchorSize.Height - _marginSide - _marginSide;
				result = new Rectangle(marginSide, marginSide2, width, height);
			}
			return result;
		}
	}

	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			try
			{
				if (_ShowClearButton && _ClearButton != null && _ClearButton.BackColor == ((Control)this).BackColor)
				{
					_ClearButtonBackColor = value;
				}
			}
			catch
			{
			}
			((Control)this).BackColor = value;
			((Control)this).Invalidate();
		}
	}

	protected virtual bool CanDrop
	{
		get
		{
			if (dropContainer != null)
			{
				return false;
			}
			if (dropContainer == null && closedWhileInControl)
			{
				closedWhileInControl = false;
				return false;
			}
			return !closedWhileInControl;
		}
	}

	public event EventHandler PropertyChanged;

	public event EventHandler ClearButtonClick;

	public event EventHandler OnOpenDropDown;

	public event EventHandler OnCloseDropDown;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		components = new Container();
		toolTip1 = new ToolTip(components);
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "DropDownEmpty";
		((Control)this).ResumeLayout(false);
	}

	protected void OnPropertyChanged()
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(null, null);
		}
	}

	public DropDownEmpty()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		((Control)this).BackColor = Color.White;
		_ClearButton = new ClearButton((UserControl)(object)this);
		_ClearButtonBackColor = ((Control)this).BackColor;
		_ClearButton.BackColor = ((Control)this).BackColor;
		_ClearButton.Enabled = ((Control)this).Enabled;
		_ClearButton.FontText = ((Control)this).Font;
		_ClearButton.ForeColor = ((Control)this).ForeColor;
		_ClearButton.RightToLeft = ((Control)this).RightToLeft;
		_ClearButton.Text = "";
		ClearButton_SetDefaultImage();
	}

	public void ClearButton_SetDefaultImage()
	{
		_ClearButton.ImageButton = (Image)(object)Resources.Clear;
		((Control)this).Invalidate();
	}

	public void InitializeDropDown(Control dropDownItem)
	{
		_DesignView = false;
		_dropState = DropStates.Closed;
		((Control)this).Size = _AnchorSize;
		if (((Control)this).Controls.Contains(dropDownItem))
		{
			((Control)this).Controls.Remove(dropDownItem);
		}
		_dropDownItem = dropDownItem;
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		if (_DesignView)
		{
			storedSize = ((Control)this).Size;
		}
		_AnchorSize.Width = ((Control)this).Width;
		if (!_DesignView)
		{
			_AnchorSize.Height = ((Control)this).Height;
		}
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		_bMouseEntered = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		_ClearButton.Render(mouseOver: false, mouseDown: false);
		toolTip1.Hide((IWin32Window)(object)this);
		_bMouseEntered = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseMove(e);
		if (!_ClearButton.Visible)
		{
			return;
		}
		if (_ClearButton.IsHit(e.X, e.Y))
		{
			if (_bClearButtonDowned && (int)e.Button != 0)
			{
				_ClearButton.Render(mouseOver: true, mouseDown: true);
			}
			else
			{
				_ClearButton.Render(mouseOver: true, mouseDown: false);
			}
			if (!_bClearButtonShownToolTip && _ClearButtonToolTip != string.Empty)
			{
				_bClearButtonShownToolTip = true;
				int num = (int)(_ClearButton.X_coordinate + _ClearButton.Width);
				int num2 = (int)(_ClearButton.Y_coordinate + _ClearButton.Height);
				toolTip1.Show(_ClearButtonToolTip, (IWin32Window)(object)this, num, num2, 5000);
			}
		}
		else
		{
			_ClearButton.Render(mouseOver: false, mouseDown: false);
			toolTip1.Hide((IWin32Window)(object)this);
			_bClearButtonShownToolTip = false;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((UserControl)this).OnMouseDown(e);
		if ((int)e.Button == 1048576)
		{
			if (_ClearButton.Visible && _ClearButton.IsHit(e.X, e.Y))
			{
				_bClearButtonDowned = true;
				_ClearButton.Render(mouseOver: true, mouseDown: true);
			}
			else
			{
				mousePressed = true;
				OpenDropDown();
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseUp(e);
		if ((int)e.Button == 1048576)
		{
			if (_ClearButton.Visible && _ClearButton.IsHit(e.X, e.Y))
			{
				_bClearButtonDowned = false;
				_ClearButton.Render(mouseOver: true, mouseDown: false);
				ClearButtonClicked(e);
			}
			else
			{
				mousePressed = false;
				((Control)this).Invalidate();
			}
		}
	}

	private void ClearButtonClicked(MouseEventArgs e)
	{
		toolTip1.Hide((IWin32Window)(object)this);
		if (this.ClearButtonClick != null)
		{
			this.ClearButtonClick(this, (EventArgs)(object)e);
		}
	}

	protected void OpenDropDown()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if (_dropDownItem == null)
		{
			throw new NotImplementedException("The drop down item has not been initialized!  Use the InitializeDropDown() method to do so.");
		}
		if (CanDrop)
		{
			dropContainer = new DropDownContainer(_dropDownItem);
			((Control)dropContainer).Bounds = GetDropDownBounds();
			dropContainer.DropStateChange += dropContainer_DropStateChange;
			((Form)dropContainer).FormClosed += new FormClosedEventHandler(dropContainer_Closed);
			((Control)((ContainerControl)this).ParentForm).Move += ParentForm_Move;
			_dropState = DropStates.Dropping;
			((Form)dropContainer).Show((IWin32Window)(object)this);
			_dropState = DropStates.Dropped;
			((Control)this).Invalidate();
			if (this.OnOpenDropDown != null)
			{
				this.OnOpenDropDown(null, null);
			}
		}
	}

	private void ParentForm_Move(object sender, EventArgs e)
	{
		((Control)dropContainer).Bounds = GetDropDownBounds();
	}

	public void CloseDropDown()
	{
		if (dropContainer != null)
		{
			_dropState = DropStates.Closing;
			dropContainer.Freeze = false;
			((Form)dropContainer).Close();
		}
	}

	private void dropContainer_DropStateChange(DropStates state)
	{
		_dropState = state;
	}

	private void dropContainer_Closed(object sender, FormClosedEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if (!((Control)dropContainer).IsDisposed)
		{
			dropContainer.DropStateChange -= dropContainer_DropStateChange;
			((Form)dropContainer).FormClosed -= new FormClosedEventHandler(dropContainer_Closed);
			((Control)((ContainerControl)this).ParentForm).Move -= ParentForm_Move;
			((Component)(object)dropContainer).Dispose();
		}
		dropContainer = null;
		closedWhileInControl = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle).Contains(Cursor.Position);
		_dropState = DropStates.Closed;
		((Control)this).Invalidate();
		if (this.OnCloseDropDown != null)
		{
			this.OnCloseDropDown(null, null);
		}
	}

	protected virtual Rectangle GetDropDownBounds()
	{
		Size size = new Size(_dropDownItem.Width + 2, _dropDownItem.Height + 2);
		Rectangle rectangle = ((_DockSide == Alignments.Left) ? new Rectangle(((Control)this).Parent.PointToScreen(new Point(((Control)this).Bounds.X, ((Control)this).Bounds.Bottom)), size) : new Rectangle(((Control)this).Parent.PointToScreen(new Point(((Control)this).Bounds.Right - _dropDownItem.Width, ((Control)this).Bounds.Bottom)), size));
		Rectangle workingArea = Screen.GetWorkingArea(rectangle);
		if (rectangle.X < workingArea.X)
		{
			rectangle.X = workingArea.X;
		}
		if (rectangle.Y < workingArea.Y)
		{
			rectangle.Y = workingArea.Y;
		}
		if (rectangle.Right > workingArea.Right && workingArea.Width > rectangle.Width)
		{
			rectangle.X = workingArea.Right - rectangle.Width;
		}
		if (rectangle.Bottom > workingArea.Bottom && workingArea.Height > rectangle.Height)
		{
			rectangle.Y = workingArea.Bottom - rectangle.Height;
		}
		return rectangle;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Invalid comparison between Unknown and I4
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Invalid comparison between Unknown and I4
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Invalid comparison between Unknown and I4
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected I4, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Invalid comparison between Unknown and I4
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Invalid comparison between Unknown and I4
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Invalid comparison between Unknown and I4
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Invalid comparison between Unknown and I4
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Invalid comparison between Unknown and I4
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Invalid comparison between Unknown and I4
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		Rectangle rectangle = AnchorClientBounds;
		if (ComboBoxRenderer.IsSupported)
		{
			ComboBoxRenderer.DrawTextBox(e.Graphics, new Rectangle(new Point(0, 0), _AnchorSize), getState());
			Color color = ((Control)this).BackColor;
			_ = ((Control)this).ForeColor;
			if (!((Control)this).Enabled)
			{
				color = SystemColors.Control;
				_ = Color.Gray;
			}
			else if (_bGotFocused)
			{
				color = _GotFocusColor;
				_ = Color.White;
			}
			Brush val = (Brush)new SolidBrush(color);
			try
			{
				e.Graphics.FillRectangle(val, AnchorClientBounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (_RtlLayout)
			{
				Rectangle rectangle2 = new Rectangle(_marginSide, _marginSide, buttonWidth, _AnchorSize.Height - _marginSide - _marginSide);
				ComboBoxRenderer.DrawDropDownButton(e.Graphics, rectangle2, getState());
			}
			else
			{
				Rectangle rectangle3 = new Rectangle(_AnchorSize.Width - _marginSide - buttonWidth, _marginSide, buttonWidth, _AnchorSize.Height - _marginSide - _marginSide);
				ComboBoxRenderer.DrawDropDownButton(e.Graphics, rectangle3, getState());
			}
		}
		else
		{
			Color color2 = ((Control)this).BackColor;
			_ = ((Control)this).ForeColor;
			if (!((Control)this).Enabled)
			{
				color2 = SystemColors.Control;
				_ = Color.Gray;
			}
			else if (_bGotFocused)
			{
				color2 = _GotFocusColor;
				_ = Color.White;
			}
			Brush val2 = (Brush)new SolidBrush(color2);
			try
			{
				e.Graphics.FillRectangle(val2, AnchorClientBounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (_RtlLayout)
			{
				ControlPaint.DrawComboButton(e.Graphics, new Rectangle(_marginSide, _marginSide, buttonWidth, _AnchorSize.Height - _marginSide - _marginSide), (ButtonState)((!((Control)this).Enabled) ? 256 : 0));
			}
			else
			{
				ControlPaint.DrawComboButton(e.Graphics, new Rectangle(_AnchorSize.Width - _marginSide - buttonWidth, _marginSide, buttonWidth, _AnchorSize.Height - _marginSide - _marginSide), (ButtonState)((!((Control)this).Enabled) ? 256 : 0));
			}
		}
		if (_ShowClearButton)
		{
			int num;
			int num2;
			int iClearButtonWidth;
			int num3;
			if (_ClearButtonAlignment == Alignments.Left)
			{
				if (_RtlLayout)
				{
					num = _marginSide + buttonWidth + 1;
					num2 = _marginSide + 1;
					iClearButtonWidth = _iClearButtonWidth;
					num3 = AnchorClientBounds.Height - _marginSide;
					rectangle = new Rectangle(num + _iClearButtonWidth + _marginSide, num2, _AnchorSize.Width - _marginSide - _marginSide - buttonWidth - 1 - _iClearButtonWidth - _marginSide, num3);
				}
				else
				{
					num = _marginSide;
					num2 = _marginSide + 1;
					iClearButtonWidth = _iClearButtonWidth;
					num3 = AnchorClientBounds.Height - _marginSide;
					rectangle = new Rectangle(num + _iClearButtonWidth + _marginSide, num2, _AnchorSize.Width - _marginSide - _marginSide - buttonWidth - 1 - _iClearButtonWidth - _marginSide, num3);
				}
			}
			else if (!_RtlLayout)
			{
				num = _AnchorSize.Width - _marginSide - buttonWidth - 1 - _iClearButtonWidth;
				num2 = _marginSide + 1;
				iClearButtonWidth = _iClearButtonWidth;
				num3 = AnchorClientBounds.Height - _marginSide;
				rectangle = new Rectangle(_marginSide, num2, _AnchorSize.Width - _marginSide - _marginSide - buttonWidth - 1 - _iClearButtonWidth - _marginSide, num3);
			}
			else
			{
				num = AnchorClientBounds.Width - _marginSide - _iClearButtonWidth;
				num2 = _marginSide + 1;
				iClearButtonWidth = _iClearButtonWidth;
				num3 = AnchorClientBounds.Height - _marginSide;
				rectangle = new Rectangle(_marginSide + buttonWidth + _marginSide, num2, _AnchorSize.Width - _marginSide - _marginSide - buttonWidth - 1 - _iClearButtonWidth - _marginSide, num3);
			}
			if (_bGotFocused)
			{
				_ClearButton.BackColor = _GotFocusColor;
			}
			else
			{
				_ClearButton.BackColor = _ClearButtonBackColor;
			}
			_ClearButton.FontText = ((Control)this).Font;
			_ClearButton.RightToLeft = ((Control)this).RightToLeft;
			_ClearButton.X_coordinate = num;
			_ClearButton.Y_coordinate = num2;
			_ClearButton.Width = iClearButtonWidth;
			_ClearButton.Height = num3;
			_ClearButton.Render();
		}
		TextFormatFlags val3 = (((int)((Control)this).RightToLeft != 1) ? ((TextFormatFlags)16) : ((TextFormatFlags)131088));
		ContentAlignment textAlign = _TextAlign;
		if ((int)textAlign <= 32)
		{
			switch (textAlign - 1)
			{
			default:
				if ((int)textAlign != 16)
				{
					if ((int)textAlign == 32)
					{
						val3 = (TextFormatFlags)(val3 | 4 | 1);
					}
				}
				else
				{
					val3 = (TextFormatFlags)(val3 | 4);
				}
				break;
			case 1:
				val3 = (TextFormatFlags)(val3 | 1);
				break;
			case 0:
				val3 = val3;
				break;
			case 3:
				val3 = (TextFormatFlags)(val3 | 2);
				break;
			case 2:
				break;
			}
		}
		else if ((int)textAlign <= 256)
		{
			if ((int)textAlign != 64)
			{
				if ((int)textAlign == 256)
				{
					val3 = (TextFormatFlags)(val3 | 8);
				}
			}
			else
			{
				val3 = (TextFormatFlags)(val3 | 4 | 2);
			}
		}
		else if ((int)textAlign != 512)
		{
			if ((int)textAlign == 1024)
			{
				val3 = (TextFormatFlags)(val3 | 8 | 2);
			}
		}
		else
		{
			val3 = (TextFormatFlags)(val3 | 8 | 1);
		}
		TextRenderer.DrawText((IDeviceContext)(object)e.Graphics, _Text, ((Control)this).Font, rectangle, ((Control)this).ForeColor, val3);
	}

	private ComboBoxState getState()
	{
		if (((Control)this).Enabled)
		{
			if (!_bMouseEntered)
			{
				if (!mousePressed && dropContainer == null)
				{
					return (ComboBoxState)1;
				}
				return (ComboBoxState)3;
			}
			return (ComboBoxState)2;
		}
		return (ComboBoxState)4;
	}

	public void FreezeDropDown(bool remainVisible)
	{
		if (dropContainer != null)
		{
			dropContainer.Freeze = true;
			if (!remainVisible)
			{
				((Control)dropContainer).Visible = false;
			}
		}
	}

	public void UnFreezeDropDown()
	{
		if (dropContainer != null)
		{
			dropContainer.Freeze = false;
			if (!((Control)dropContainer).Visible)
			{
				((Control)dropContainer).Visible = true;
			}
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		_bGotFocused = true;
		((Control)this).Invalidate();
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		_bGotFocused = false;
		((Control)this).Invalidate();
	}
}
