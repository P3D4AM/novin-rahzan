using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using WinFormAnimation;

namespace CircularProgressBar;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(CircularProgressBar), "CircularProgressBar.bmp")]
[DefaultBindingProperty("Value")]
public class CircularProgressBar : ProgressBar
{
	private readonly Animator _animator;

	private int? _animatedStartAngle;

	private float? _animatedValue;

	private AnimationFunctions.Function _animationFunction;

	private Brush _backBrush;

	private KnownAnimationFunctions _knownAnimationFunction;

	private ProgressBarStyle? _lastStyle;

	private int _lastValue;

	[Category("Behavior")]
	public KnownAnimationFunctions AnimationFunction
	{
		get
		{
			return _knownAnimationFunction;
		}
		set
		{
			_animationFunction = AnimationFunctions.FromKnown(value);
			_knownAnimationFunction = value;
		}
	}

	[Category("Behavior")]
	public int AnimationSpeed { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public AnimationFunctions.Function CustomAnimationFunction
	{
		private get
		{
			return _animationFunction;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_knownAnimationFunction = KnownAnimationFunctions.None;
			_animationFunction = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	public override Font Font
	{
		get
		{
			return ((ProgressBar)this).Font;
		}
		set
		{
			((ProgressBar)this).Font = value;
		}
	}

	[Category("Appearance")]
	public Color InnerColor { get; set; }

	[Category("Layout")]
	public int InnerMargin { get; set; }

	[Category("Layout")]
	public int InnerWidth { get; set; }

	[Category("Appearance")]
	public Color OuterColor { get; set; }

	[Category("Layout")]
	public int OuterMargin { get; set; }

	[Category("Layout")]
	public int OuterWidth { get; set; }

	[Category("Appearance")]
	public Color ProgressColor { get; set; }

	[Category("Layout")]
	public int ProgressWidth { get; set; }

	[Category("Appearance")]
	public Font SecondaryFont { get; set; }

	[Category("Layout")]
	public int StartAngle { get; set; }

	[Category("Appearance")]
	public Color SubscriptColor { get; set; }

	[Category("Layout")]
	public Padding SubscriptMargin { get; set; }

	[Category("Appearance")]
	public string SubscriptText { get; set; }

	[Category("Appearance")]
	public Color SuperscriptColor { get; set; }

	[Category("Layout")]
	public Padding SuperscriptMargin { get; set; }

	[Category("Appearance")]
	public string SuperscriptText { get; set; }

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((ProgressBar)this).Text;
		}
		set
		{
			((ProgressBar)this).Text = value;
		}
	}

	[Category("Layout")]
	public Padding TextMargin { get; set; }

	public CircularProgressBar()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)141330, true);
		_animator = (((Component)this).DesignMode ? null : new Animator());
		AnimationFunction = KnownAnimationFunctions.Liner;
		AnimationSpeed = 500;
		((ProgressBar)this).MarqueeAnimationSpeed = 2000;
		StartAngle = 270;
		_lastValue = ((ProgressBar)this).Value;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font(((Control)this).Font.FontFamily, 72f, (FontStyle)1);
		SecondaryFont = new Font(((Control)this).Font.FontFamily, (float)((double)((Control)this).Font.Size * 0.5), (FontStyle)0);
		OuterMargin = -25;
		OuterWidth = 26;
		OuterColor = Color.Gray;
		ProgressWidth = 25;
		ProgressColor = Color.FromArgb(255, 128, 0);
		InnerMargin = 2;
		InnerWidth = -1;
		InnerColor = Color.FromArgb(224, 224, 224);
		TextMargin = new Padding(8, 8, 0, 0);
		((ProgressBar)this).Value = 68;
		SuperscriptMargin = new Padding(10, 35, 0, 0);
		SuperscriptColor = Color.FromArgb(166, 166, 166);
		SuperscriptText = "°C";
		SubscriptMargin = new Padding(10, -35, 0, 0);
		SubscriptColor = Color.FromArgb(166, 166, 166);
		SubscriptText = ".23";
		((Control)this).Size = new Size(320, 320);
	}

	private static PointF AddPoint(PointF p, int v)
	{
		p.X += v;
		p.Y += v;
		return p;
	}

	private static SizeF AddSize(SizeF s, int v)
	{
		s.Height += v;
		s.Width += v;
		return s;
	}

	private static Rectangle ToRectangle(RectangleF rect)
	{
		return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!((Component)this).DesignMode)
			{
				if ((int)((ProgressBar)this).Style == 2)
				{
					InitializeMarquee(_lastStyle != (ProgressBarStyle?)((ProgressBar)this).Style);
				}
				else
				{
					InitializeContinues(_lastStyle != (ProgressBarStyle?)((ProgressBar)this).Style);
				}
				_lastStyle = ((ProgressBar)this).Style;
			}
			if (_backBrush == null)
			{
				RecreateBackgroundBrush();
			}
			StartPaint(e.Graphics);
		}
		catch
		{
		}
	}

	protected override void OnParentBackColorChanged(EventArgs e)
	{
		RecreateBackgroundBrush();
	}

	protected override void OnParentBackgroundImageChanged(EventArgs e)
	{
		RecreateBackgroundBrush();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		if (((Control)this).Parent != null)
		{
			((Control)this).Parent.Invalidated -= new InvalidateEventHandler(ParentOnInvalidated);
			((Control)this).Parent.Resize -= ParentOnResize;
		}
		((Control)this).OnParentChanged(e);
		if (((Control)this).Parent != null)
		{
			((Control)this).Parent.Invalidated += new InvalidateEventHandler(ParentOnInvalidated);
			((Control)this).Parent.Resize += ParentOnResize;
		}
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		((Control)this).OnStyleChanged(e);
		((Control)this).Invalidate();
	}

	protected virtual void InitializeContinues(bool firstTime)
	{
		if (_lastValue == ((ProgressBar)this).Value && !firstTime)
		{
			return;
		}
		_lastValue = ((ProgressBar)this).Value;
		_animator.Stop();
		_animatedStartAngle = null;
		if (AnimationSpeed <= 0)
		{
			_animatedValue = ((ProgressBar)this).Value;
			((Control)this).Invalidate();
			return;
		}
		_animator.Paths = new Path(_animatedValue ?? ((float)((ProgressBar)this).Value), ((ProgressBar)this).Value, (ulong)AnimationSpeed, CustomAnimationFunction).ToArray();
		_animator.Repeat = false;
		_animator.Play(new SafeInvoker<float>(delegate(float v)
		{
			try
			{
				_animatedValue = v;
				((Control)this).Invalidate();
			}
			catch
			{
				_animator.Stop();
			}
		}, this));
	}

	protected virtual void InitializeMarquee(bool firstTime)
	{
		if (!firstTime && (_animator.ActivePath == null || (_animator.ActivePath.Duration == (ulong)((ProgressBar)this).MarqueeAnimationSpeed && _animator.ActivePath.Function == CustomAnimationFunction)))
		{
			return;
		}
		_animator.Stop();
		_animatedValue = null;
		if (AnimationSpeed <= 0)
		{
			_animatedStartAngle = 0;
			((Control)this).Invalidate();
			return;
		}
		_animator.Paths = new Path(0f, 359f, (ulong)((ProgressBar)this).MarqueeAnimationSpeed, CustomAnimationFunction).ToArray();
		_animator.Repeat = true;
		_animator.Play(new SafeInvoker<float>(delegate(float v)
		{
			try
			{
				_animatedStartAngle = (int)(v % 360f);
				((Control)this).Invalidate();
			}
			catch
			{
				_animator.Stop();
			}
		}, this));
	}

	protected virtual void ParentOnInvalidated(object sender, InvalidateEventArgs invalidateEventArgs)
	{
		RecreateBackgroundBrush();
	}

	protected virtual void ParentOnResize(object sender, EventArgs eventArgs)
	{
		RecreateBackgroundBrush();
	}

	protected virtual void RecreateBackgroundBrush()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		lock (this)
		{
			Brush backBrush = _backBrush;
			if (backBrush != null)
			{
				backBrush.Dispose();
			}
			_backBrush = (Brush)new SolidBrush(((Control)this).BackColor);
			if (((Control)this).BackColor.A == byte.MaxValue)
			{
				return;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent.Width > 0 && ((Control)this).Parent.Height > 0)
			{
				Bitmap val = new Bitmap(((Control)this).Parent.Width, ((Control)this).Parent.Height);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						PaintEventArgs val3 = new PaintEventArgs(val2, new Rectangle(new Point(0, 0), ((Image)val).Size));
						((Control)this).InvokePaintBackground(((Control)this).Parent, val3);
						((Control)this).InvokePaint(((Control)this).Parent, val3);
						if (((Control)this).BackColor.A > 0)
						{
							val2.FillRectangle(_backBrush, ((Control)this).Bounds);
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					_backBrush = (Brush)new TextureBrush((Image)(object)val);
					((TextureBrush)_backBrush).TranslateTransform((float)(-((Control)this).Bounds.X), (float)(-((Control)this).Bounds.Y));
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			_backBrush = (Brush)new SolidBrush(Color.FromArgb(255, ((Control)this).BackColor));
		}
	}

	protected virtual void StartPaint(Graphics g)
	{
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_01a0: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Invalid comparison between Unknown and I4
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		try
		{
			lock (this)
			{
				g.TextRenderingHint = (TextRenderingHint)4;
				g.SmoothingMode = (SmoothingMode)4;
				PointF pointF = AddPoint(Point.Empty, 2);
				SizeF sizeF = AddSize(((Control)this).Size, -4);
				if (OuterWidth + OuterMargin < 0)
				{
					int num = Math.Abs(OuterWidth + OuterMargin);
					pointF = AddPoint(Point.Empty, num);
					sizeF = AddSize(((Control)this).Size, -2 * num);
				}
				if (OuterColor != Color.Empty && OuterColor != Color.Transparent && OuterWidth != 0)
				{
					g.FillEllipse((Brush)new SolidBrush(OuterColor), new RectangleF(pointF, sizeF));
					if (OuterWidth >= 0)
					{
						pointF = AddPoint(pointF, OuterWidth);
						sizeF = AddSize(sizeF, -2 * OuterWidth);
						g.FillEllipse(_backBrush, new RectangleF(pointF, sizeF));
					}
				}
				pointF = AddPoint(pointF, OuterMargin);
				sizeF = AddSize(sizeF, -2 * OuterMargin);
				g.FillPie((Brush)new SolidBrush(ProgressColor), ToRectangle(new RectangleF(pointF, sizeF)), (float)(_animatedStartAngle ?? StartAngle), ((_animatedValue ?? ((float)((ProgressBar)this).Value)) - (float)((ProgressBar)this).Minimum) / (float)(((ProgressBar)this).Maximum - ((ProgressBar)this).Minimum) * 360f);
				if (ProgressWidth >= 0)
				{
					pointF = AddPoint(pointF, ProgressWidth);
					sizeF = AddSize(sizeF, -2 * ProgressWidth);
					g.FillEllipse(_backBrush, new RectangleF(pointF, sizeF));
				}
				pointF = AddPoint(pointF, InnerMargin);
				sizeF = AddSize(sizeF, -2 * InnerMargin);
				if (InnerColor != Color.Empty && InnerColor != Color.Transparent && InnerWidth != 0)
				{
					g.FillEllipse((Brush)new SolidBrush(InnerColor), new RectangleF(pointF, sizeF));
					if (InnerWidth >= 0)
					{
						pointF = AddPoint(pointF, InnerWidth);
						sizeF = AddSize(sizeF, -2 * InnerWidth);
						g.FillEllipse(_backBrush, new RectangleF(pointF, sizeF));
					}
				}
				if (((Control)this).Text == string.Empty)
				{
					return;
				}
				float x = pointF.X;
				Padding val = TextMargin;
				pointF.X = x + (float)((Padding)(ref val)).Left;
				float y = pointF.Y;
				val = TextMargin;
				pointF.Y = y + (float)((Padding)(ref val)).Top;
				float width = sizeF.Width;
				val = TextMargin;
				sizeF.Width = width - (float)((Padding)(ref val)).Right;
				float height = sizeF.Height;
				val = TextMargin;
				sizeF.Height = height - (float)((Padding)(ref val)).Bottom;
				StringFormat val2 = new StringFormat((StringFormatFlags)((int)((Control)this).RightToLeft == 1))
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)0
				};
				SizeF size = g.MeasureString(((Control)this).Text, ((Control)this).Font);
				PointF location = new PointF(pointF.X + (sizeF.Width - size.Width) / 2f, pointF.Y + (sizeF.Height - size.Height) / 2f);
				if (SubscriptText != string.Empty || SuperscriptText != string.Empty)
				{
					float num2 = 0f;
					SizeF size2 = SizeF.Empty;
					SizeF size3 = SizeF.Empty;
					if (SuperscriptText != string.Empty)
					{
						size2 = g.MeasureString(SuperscriptText, SecondaryFont);
						num2 = Math.Max(size2.Width, num2);
						float width2 = size2.Width;
						val = SuperscriptMargin;
						size2.Width = width2 - (float)((Padding)(ref val)).Right;
						float height2 = size2.Height;
						val = SuperscriptMargin;
						size2.Height = height2 - (float)((Padding)(ref val)).Bottom;
					}
					if (SubscriptText != string.Empty)
					{
						size3 = g.MeasureString(SubscriptText, SecondaryFont);
						num2 = Math.Max(size3.Width, num2);
						float width3 = size3.Width;
						val = SubscriptMargin;
						size3.Width = width3 - (float)((Padding)(ref val)).Right;
						float height3 = size3.Height;
						val = SubscriptMargin;
						size3.Height = height3 - (float)((Padding)(ref val)).Bottom;
					}
					location.X -= num2 / 4f;
					if (SuperscriptText != string.Empty)
					{
						PointF location2 = new PointF(location.X + size.Width - size2.Width / 2f, location.Y - size2.Height * 0.85f);
						float x2 = location2.X;
						val = SuperscriptMargin;
						location2.X = x2 + (float)((Padding)(ref val)).Left;
						float y2 = location2.Y;
						val = SuperscriptMargin;
						location2.Y = y2 + (float)((Padding)(ref val)).Top;
						g.DrawString(SuperscriptText, SecondaryFont, (Brush)new SolidBrush(SuperscriptColor), new RectangleF(location2, size2), val2);
					}
					if (SubscriptText != string.Empty)
					{
						PointF location3 = new PointF(location.X + size.Width - size3.Width / 2f, location.Y + size.Height * 0.85f);
						float x3 = location3.X;
						val = SubscriptMargin;
						location3.X = x3 + (float)((Padding)(ref val)).Left;
						float y3 = location3.Y;
						val = SubscriptMargin;
						location3.Y = y3 + (float)((Padding)(ref val)).Top;
						g.DrawString(SubscriptText, SecondaryFont, (Brush)new SolidBrush(SubscriptColor), new RectangleF(location3, size3), val2);
					}
				}
				g.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), new RectangleF(location, size), val2);
			}
		}
		catch
		{
		}
	}
}
