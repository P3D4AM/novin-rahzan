using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.MicroCharts;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Click")]
[Designer("DevComponents.DotNetBar.Design.MicroChartItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class MicroChartItem : PopupItem, IPersonalizedMenuItem
{
	internal static readonly int ⴘ = 3;

	private int __238D;

	private Size _ᢧ = Size.Empty;

	private string _ⴙ = "";

	private List<double> _ⴚ = new List<double>();

	private List<string> _ⴛ = new List<string>();

	private Image _ⴜ;

	private float _ⴝ;

	private BackgroundWorker _ⴞ;

	private Semaphore _ⴟ = new Semaphore(1, 1);

	private bool _ᢻ = true;

	private bool _ⴠ = true;

	private bool _ⴡ;

	private bool _ⴢ = true;

	private EventHandler ⴔ;

	private _2CE7 _ⴣ = _2CE7._2CEA;

	private bool _ᆿ;

	private bool _ⴤ;

	private Bitmap _ⴥ;

	private Point __2D26 = Point.Empty;

	private static RoundRectangleShapeDescriptor _ᵧ = new RoundRectangleShapeDescriptor(3);

	private _2CE7[] _ⴧ = new _2CE7[0];

	private int __2D28 = -1;

	private LineMicroChartStyle __2D29;

	private PlotMicroChartStyle __2D2A;

	private BarMicroChartStyle __2D2B;

	private BarMicroChartStyle __2D2C;

	private BarMicroChartStyle _ⴭ;

	private PieMicroChartStyle __2D2E;

	private AreaMicroChartStyle __2D2F;

	private HundredPctMicroChartStyle _ⴰ;

	private int _ⴱ = 54;

	private int _ⴲ = 15;

	private eMicroChartType _ⴳ = eMicroChartType.Line;

	private bool __0EB9 = true;

	private eMicroChartTextPosition __238C;

	private bool _ᣅ = true;

	private Padding _ᢾ = new Padding(0, 0, 0, 0);

	private Padding __0AE5 = new Padding(0, 0, 0, 0);

	private Color __0CD1 = Color.Empty;

	private eBackgroundImagePosition __1C3D = eBackgroundImagePosition.Tile;

	private Image _ⴴ;

	private double _ⴵ = double.NaN;

	private double _ⴶ = double.NaN;

	private eMenuVisibility _ⴷ;

	private bool _ⴸ;

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
	public int TextWidth
	{
		get
		{
			return __238D;
		}
		set
		{
			__238D = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates format string for the value when it is displayed as tool-tip for data point.")]
	public string TooltipValueFormatString
	{
		get
		{
			return _ⴙ;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (value != _ⴙ)
			{
				string ⴙ = _ⴙ;
				_ⴙ = value;
				ⴹ(ⴙ, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<double> DataPoints
	{
		get
		{
			return _ⴚ;
		}
		set
		{
			if (value != _ⴚ)
			{
				List<double> ⴚ = _ⴚ;
				_ⴚ = value;
				ⴖ(ⴚ, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<string> DataPointTooltips
	{
		get
		{
			return _ⴛ;
		}
		set
		{
			if (value != _ⴛ)
			{
				List<string> ⴛ = _ⴛ;
				_ⴛ = value;
				ⴺ(ⴛ, value);
			}
		}
	}

	[Description("Indicates  whether transition animation between same chart with different data-points is enabled.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool AnimationEnabled
	{
		get
		{
			return _ᢻ;
		}
		set
		{
			_ᢻ = value;
		}
	}

	private bool ᥧ => _ᢻ;

	[Category("Behavior")]
	[Description("Indicates whether chart is tracking mouse movement to show data-point and its value on tooltip.")]
	[DefaultValue(true)]
	public bool TrackChartPoints
	{
		get
		{
			return _ⴠ;
		}
		set
		{
			_ⴠ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it.")]
	public bool MouseOverEnabled
	{
		get
		{
			return _ⴡ;
		}
		set
		{
			_ⴡ = value;
		}
	}

	[Browsable(false)]
	public int MouseOverDataPointIndex
	{
		get
		{
			if (_ⴣ.ᮄ)
			{
				return -1;
			}
			return _ⴣ.Ⳬ;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse over tooltip for data point is enabled")]
	public bool MouseOverDataPointTooltipEnabled
	{
		get
		{
			return _ⴢ;
		}
		set
		{
			_ⴢ = value;
		}
	}

	private _2CE7 ⴗ
	{
		get
		{
			return _ⴣ;
		}
		set
		{
			if (!(_ⴣ._2CE8 != value._2CE8))
			{
				return;
			}
			_ⴣ = value;
			if (_ⴣ.ᮄ && base.ToolTipVisible)
			{
				HideToolTip();
				Tooltip = "";
			}
			else if (_ⴠ && _ⴢ)
			{
				if (_ⴛ.Count > 0 && _ⴛ.Count > value.Ⳬ)
				{
					string text = _ⴛ[value.Ⳬ];
					if (text.IndexOf("{0}") >= 0)
					{
						text = string.Format(text, value._༡);
					}
					Tooltip = text;
				}
				else if (string.IsNullOrEmpty(_ⴙ))
				{
					Tooltip = value._༡.ToString();
				}
				else
				{
					Tooltip = value._༡.ToString(_ⴙ);
				}
				if (!base.ToolTipVisible)
				{
					ShowToolTip();
				}
			}
			OnMouseOverDataPointChanged(EventArgs.Empty);
			Refresh();
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Line micro-chart.")]
	public LineMicroChartStyle LineChartStyle => __2D29;

	[Category("Style")]
	[Description("Identifies style used to customize appearance of Plot micro-chart.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public PlotMicroChartStyle PlotChartStyle => __2D2A;

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Column micro-chart.")]
	public BarMicroChartStyle ColumnChartStyle => __2D2B;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Identifies style used to customize appearance of Bar micro-chart.")]
	public BarMicroChartStyle BarChartStyle => __2D2C;

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Win-Lose micro-chart.")]
	public BarMicroChartStyle WinLoseChartStyle => _ⴭ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Pie micro-chart.")]
	[Category("Style")]
	public PieMicroChartStyle PieChartStyle => __2D2E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Identifies style used to customize appearance of Area micro-chart.")]
	public AreaMicroChartStyle AreaChartStyle => __2D2F;

	[Description("Identifies style used to customize appearance of 100% micro-chart.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HundredPctMicroChartStyle HundredPctChartStyle => _ⴰ;

	[DefaultValue(54)]
	[Category("Appearance")]
	[Description("Indicates width of the chart part of the control.")]
	public int ChartWidth
	{
		get
		{
			return _ⴱ;
		}
		set
		{
			if (value != _ⴱ)
			{
				if (_ⴱ <= 0)
				{
					throw new ArgumentException("CharWidth must be greater than 0");
				}
				int ⴱ = _ⴱ;
				_ⴱ = value;
				ⵉ(ⴱ, value);
			}
		}
	}

	[Description("Indicates height of the chart part of the control.")]
	[DefaultValue(15)]
	[Category("Appearance")]
	public int ChartHeight
	{
		get
		{
			return _ⴲ;
		}
		set
		{
			if (value != _ⴲ)
			{
				if (_ⴱ <= 0)
				{
					throw new ArgumentException("CharHeight must be greater than 0");
				}
				int ⴲ = _ⴲ;
				_ⴲ = value;
				ⵊ(ⴲ, value);
			}
		}
	}

	[Description("Indicates type of the chart rendered.")]
	[Category("Appearance")]
	[DefaultValue(eMicroChartType.Line)]
	public eMicroChartType ChartType
	{
		get
		{
			return _ⴳ;
		}
		set
		{
			if (value != _ⴳ)
			{
				eMicroChartType ⴳ = _ⴳ;
				_ⴳ = value;
				ⵋ(ⴳ, value);
			}
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[DefaultValue(true)]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[DefaultValue(eMicroChartTextPosition.Left)]
	[Description("Indicates text-position in relation to the chart.")]
	[Category("Appearance")]
	public eMicroChartTextPosition TextPosition
	{
		get
		{
			return __238C;
		}
		set
		{
			if (value != __238C)
			{
				eMicroChartTextPosition ك = __238C;
				__238C = value;
				_23B7(ك, value);
			}
		}
	}

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Description("The text contained in the item.")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether caption/label set using Text property is visible.")]
	[Category("Appearance")]
	public bool TextVisible
	{
		get
		{
			return _ᣅ;
		}
		set
		{
			_ᣅ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Gets or sets text padding.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding TextPadding => _ᢾ;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Description("Gets or sets switch margin.")]
	public new Padding Margin => __0AE5;

	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
		}
	}

	[DefaultValue(eBackgroundImagePosition.Tile)]
	[Category("Appearance")]
	[Description("Specifies background image position")]
	[Browsable(true)]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return __1C3D;
		}
		set
		{
			if (__1C3D != value)
			{
				__1C3D = value;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates background image used by the control.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image BackgroundImage
	{
		get
		{
			return _ⴴ;
		}
		set
		{
			_ⴴ = value;
			Refresh();
		}
	}

	[DefaultValue(double.NaN)]
	[Description("Indicates maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
	[Category("Behavior")]
	public double DataMaxValue
	{
		get
		{
			return _ⴵ;
		}
		set
		{
			if (value != _ⴵ)
			{
				double ⴵ = _ⴵ;
				_ⴵ = value;
				InvalidateChart(animateTransition: false);
				Refresh();
				OnDataMaxValueChanged(ⴵ, value);
			}
		}
	}

	[DefaultValue(double.NaN)]
	[Category("Behavior")]
	[Description("Indicates minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
	public double DataMinValue
	{
		get
		{
			return _ⴶ;
		}
		set
		{
			if (value != _ⴶ)
			{
				double ⴶ = _ⴶ;
				_ⴶ = value;
				InvalidateChart(animateTransition: false);
				Refresh();
				OnDataMinValueChanged(ⴶ, value);
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates item's visibility when on pop-up menu.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[Category("Appearance")]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return _ⴷ;
		}
		set
		{
			if (_ⴷ != value)
			{
				_ⴷ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool RecentlyUsed
	{
		get
		{
			return _ⴸ;
		}
		set
		{
			if (_ⴸ != value)
			{
				_ⴸ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "RecentlyUsed");
				}
			}
		}
	}

	public event EventHandler MouseOverDataPointChanged
	{
		add
		{
			EventHandler eventHandler = ⴔ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⴔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⴔ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⴔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public MicroChartItem()
		: this("", "")
	{
	}

	public MicroChartItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public MicroChartItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		__0AE5.PropertyChanged += ᣏ;
		__2D29 = new LineMicroChartStyle();
		__2D29.StyleChanged += ⵇ;
		__2D2A = new PlotMicroChartStyle();
		__2D2A.StyleChanged += ⵇ;
		__2D2B = new BarMicroChartStyle();
		__2D2B.StyleChanged += ⵇ;
		__2D2C = new BarMicroChartStyle();
		__2D2C.StyleChanged += ⵇ;
		_ⴭ = new BarMicroChartStyle();
		_ⴭ.StyleChanged += ⵇ;
		__2D2E = new PieMicroChartStyle();
		__2D2E.StyleChanged += ⵇ;
		__2D2F = new AreaMicroChartStyle();
		__2D2F.StyleChanged += ⵇ;
		_ⴰ = new HundredPctMicroChartStyle();
		_ⴰ.StyleChanged += ⵇ;
	}

	public override BaseItem Copy()
	{
		MicroChartItem microChartItem = new MicroChartItem(m_Name);
		CopyToItem(microChartItem);
		return microChartItem;
	}

	internal void _0EBA(MicroChartItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is MicroChartItem microChartItem)
		{
			microChartItem.ChartHeight = ChartHeight;
			microChartItem.ChartType = ChartType;
			microChartItem.ChartWidth = ChartWidth;
			microChartItem.DataPoints = DataPoints;
			microChartItem.MouseOverEnabled = MouseOverEnabled;
			microChartItem.TextVisible = TextVisible;
			microChartItem.TrackChartPoints = TrackChartPoints;
			base.CopyToItem((BaseItem)microChartItem);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_ⴞ != null)
		{
			_ⴞ.Dispose();
			_ⴞ = null;
		}
		Image ⴥ = (Image)(object)_ⴥ;
		_ⴥ = null;
		if (ⴥ != null)
		{
			ⴥ.Dispose();
		}
		base.Dispose(disposing);
	}

	public override void RecalcSize()
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		int num = _ⴱ;
		int num2 = _ⴲ;
		if (!(ContainerControl is MicroChart))
		{
			num = Dpi.Width(_ⴱ);
			num2 = Dpi.Height(_ⴲ);
		}
		Padding padding = Dpi.Size(__0AE5);
		m_Rect.Width = num + padding.Horizontal + Dpi.Width8;
		m_Rect.Height = num2 + padding.Vertical + Dpi.Height8;
		if (base.IsOnMenu && !(Parent is ItemContainer))
		{
			Size size = ო();
			m_Rect.Width += size.Width + 7;
			if (base.IsOnCustomizeMenu)
			{
				m_Rect.Width += size.Height + 2;
			}
		}
		if (_ᣅ && !string.IsNullOrEmpty(Text))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					int num3 = Dpi.Width(__238D);
					Size size2 = (_ᢧ = _20B1._20B2(this, val2, num3, font, (num3 > 0) ? eTextFormat.WordBreak : eTextFormat.SingleLine, (int)val.RightToLeft == 1));
					Padding padding2 = Dpi.Size(_ᢾ);
					size2.Width += padding2.Horizontal;
					size2.Height += padding2.Vertical;
					int num4 = Dpi.Width(ⴘ);
					if (__238C == eMicroChartTextPosition.Left || __238C == eMicroChartTextPosition.Right)
					{
						size2.Width += num4;
						m_Rect.Width += size2.Width;
						m_Rect.Height = Math.Max(m_Rect.Height, size2.Height);
					}
					else
					{
						size2.Height += num4;
						m_Rect.Height += size2.Height;
						m_Rect.Width = Math.Max(m_Rect.Width, size2.Width);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		base.RecalcSize();
	}

	private Size ო()
	{
		if (m_Parent != null)
		{
			if (m_Parent is ImageItem imageItem)
			{
				return imageItem.SubItemsImageSize;
			}
			return ImageSize;
		}
		return ImageSize;
	}

	private void ⴹ(string ك, string ل)
	{
	}

	private void ⴖ(List<double> ك, List<double> ل)
	{
		ⴻ();
	}

	private void ⴺ(List<string> ك, List<string> ل)
	{
	}

	private void ⴻ()
	{
		if (_ⴥ != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && val.IsHandleCreated && ᥧ)
			{
				Image ⴜ = _ⴜ;
				_ⴜ = (Image)(object)_ⴥ;
				if (ⴜ != null)
				{
					ⴜ.Dispose();
				}
				_ⴥ = null;
				ⵀ(ⵁ: false);
				if (_ⴞ == null)
				{
					_ⴞ = new BackgroundWorker();
					_ⴞ.WorkerSupportsCancellation = true;
					_ⴞ.DoWork += ⴽ;
					_ⴞ.RunWorkerCompleted += ⴼ;
					_ⴞ.RunWorkerAsync();
				}
				return;
			}
		}
		ⵀ(ⵁ: false);
	}

	private void ⴼ(object _٠, RunWorkerCompletedEventArgs ؿ)
	{
		_ⴟ.WaitOne();
		BackgroundWorker ⴞ = _ⴞ;
		Image ⴜ = _ⴜ;
		_ⴜ = null;
		if (ⴜ != null)
		{
			ⴜ.Dispose();
		}
		_ⴞ = null;
		ⴞ?.Dispose();
		Refresh();
		_ⴟ.Release();
	}

	private void ⴽ(object _٠, DoWorkEventArgs ؿ)
	{
		while (_ⴝ < 1f)
		{
			_ⴝ += 0.1f;
			if (ؿ.Cancel)
			{
				break;
			}
			Refresh();
			try
			{
				using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				manualResetEvent.WaitOne(60);
			}
			catch
			{
			}
		}
		_ⴝ = 0f;
	}

	protected virtual void OnMouseOverDataPointChanged(EventArgs e)
	{
		ⴔ?.Invoke(this, e);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!_ᆿ)
		{
			_ᆿ = true;
			Refresh();
		}
	}

	public override void InternalMouseLeave()
	{
		ⴗ = _2CE7._2CEA;
		if (_ᆿ)
		{
			_ᆿ = false;
			Refresh();
		}
		base.InternalMouseLeave();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (!_ⴤ && (int)objArg.Button == 1048576)
		{
			_ⴤ = true;
			Refresh();
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (_ⴤ && (int)objArg.Button == 1048576)
		{
			_ⴤ = false;
			Refresh();
		}
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (m_Rect.Contains(objArg.X, objArg.Y) && _ⴧ != null)
		{
			_2CE7 _2CE72 = _2CE7._2CEA;
			int width = _ⴱ;
			int height = _ⴲ;
			if (!(ContainerControl is MicroChart))
			{
				width = Dpi.Width(_ⴱ);
				height = Dpi.Height(_ⴲ);
			}
			if (new Rectangle(__2D26.X, __2D26.Y, width, height).Contains(objArg.X, objArg.Y))
			{
				_2CE7[] ⴧ = _ⴧ;
				for (int i = 0; i < ⴧ.Length; i++)
				{
					_2CE7 _2CE73 = ⴧ[i];
					Point ⴿ = new Point(objArg.X - __2D26.X, objArg.Y - __2D26.Y);
					Rectangle _2CE8 = _2CE73._2CE8;
					Rectangle _2CE9 = _2CE73._2CE9;
					_2CE8.Offset(__2D26);
					if (!_2CE9.IsEmpty)
					{
						_2CE9.Offset(__2D26);
					}
					if ((_ⴳ == eMicroChartType.Pie && ⴾ(ⴿ, _2CE73)) || (_ⴳ != eMicroChartType.Pie && (_2CE8.Contains(objArg.X, objArg.Y) || _2CE9.Contains(objArg.X, objArg.Y))))
					{
						_2CE72 = _2CE73;
						break;
					}
				}
			}
			ⴗ = _2CE72;
		}
		base.InternalMouseMove(objArg);
	}

	private bool ⴾ(Point ⴿ, _2CE7 Ⳳ)
	{
		int num = _ⴱ;
		int num2 = _ⴲ;
		if (!(ContainerControl is MicroChart))
		{
			num = Dpi.Width(_ⴱ);
			num2 = Dpi.Height(_ⴲ);
		}
		int num3 = Math.Min(num2, num);
		double num4 = num3 / 2 + (num - num3) / 2;
		double num5 = num3 / 2 + (num2 - num3) / 2;
		double x = (double)ⴿ.X - num4;
		double y = (double)ⴿ.Y - num5;
		double num6 = Math.Atan2(y, x);
		if (num6 < 0.0)
		{
			num6 += Math.PI * 2.0;
		}
		double num7 = num6 * 180.0 / Math.PI;
		if (num7 >= Ⳳ.ᒊ && num7 <= Ⳳ.ᒊ + Ⳳ.ᒋ)
		{
			return true;
		}
		return false;
	}

	public void InvalidateChart(bool animateTransition)
	{
		if (animateTransition)
		{
			ⴻ();
		}
		else
		{
			ⵀ(ⵁ: false);
		}
	}

	private void ⵀ(bool ⵁ)
	{
		Image ⴥ = (Image)(object)_ⴥ;
		_ⴥ = null;
		if (ⴥ != null)
		{
			ⴥ.Dispose();
		}
		if (!ⵁ)
		{
			Refresh();
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Expected O, but got Unknown
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		if (_ⴚ != null && __2D28 != _ⴚ.Count)
		{
			ⵀ(ⵁ: true);
		}
		if (_ⴥ == null)
		{
			_ⴥ = ⵆ();
		}
		Graphics graphics = p.Graphics;
		Rectangle rect = m_Rect;
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		bool rightToLeft = p.RightToLeft;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		bool enabled = GetEnabled(p.ContainerControl);
		if (flag)
		{
			Size size = ო();
			if (!rightToLeft)
			{
				rect.X += size.Width + 7;
			}
			rect.Width -= size.Width + 7;
			ⵃ(p, effectiveStyle);
		}
		if (_ⴡ && enabled && (_ᆿ || Expanded))
		{
			ⵂ(p, effectiveStyle);
		}
		if (_ⴴ != null)
		{
			BarFunctions._0982(graphics, rect, _ⴴ, __1C3D, 255);
		}
		Padding padding = Dpi.Size(__0AE5);
		Point point = new Point(rect.X + Dpi.Width4 + padding.Left, rect.Y + Dpi.Height4 + padding.Top);
		int num = _ⴲ;
		int num2 = _ⴱ;
		if (!(ContainerControl is MicroChart))
		{
			num2 = Dpi.Width(_ⴱ);
			num = Dpi.Height(_ⴲ);
		}
		if (_ᣅ && !string.IsNullOrEmpty(Text))
		{
			Rectangle rectangle = rect;
			int num3 = Dpi.Width(ⴘ);
			Padding padding2 = Dpi.Size(_ᢾ);
			if ((__238C == eMicroChartTextPosition.Left && !rightToLeft) || (__238C == eMicroChartTextPosition.Right && rightToLeft))
			{
				rectangle.Width = _ᢧ.Width;
				rectangle.Y += padding2.Top + padding.Top + Dpi.Height1;
				rectangle.X += padding.Left + padding2.Left + Dpi.Width1;
				rectangle.Height = Math.Max(_ᢧ.Height + Dpi.Height2, num + Dpi.Height6);
				point.X = rectangle.Right + padding2.Right + num3;
			}
			else if ((__238C == eMicroChartTextPosition.Right && !rightToLeft) || (__238C == eMicroChartTextPosition.Left && rightToLeft))
			{
				rectangle.Width = _ᢧ.Width;
				rectangle.Y += padding2.Top + padding.Top + Dpi.Height1;
				rectangle.X += padding.Left + padding2.Left + num2 + num3 + Dpi.Width1;
				rectangle.Height = Math.Max(_ᢧ.Height + Dpi.Height2, num + Dpi.Height6);
			}
			else if (__238C == eMicroChartTextPosition.Top)
			{
				rectangle.Width = _ᢧ.Width;
				rectangle.Y += padding2.Top + padding.Top + Dpi.Height1;
				rectangle.X += padding.Left;
				rectangle.Height = _ᢧ.Height;
				point.Y = rectangle.Bottom + padding2.Bottom + num3;
				if (_ᢧ.Width > num2)
				{
					point.X += (_ᢧ.Width - num2) / 2;
				}
				else
				{
					rectangle.X = point.X + (num2 - _ᢧ.Width) / 2;
				}
			}
			else if (__238C == eMicroChartTextPosition.Bottom)
			{
				rectangle.Width = _ᢧ.Width;
				rectangle.Y += padding2.Top + padding.Top + num + num3 + Dpi.Height1;
				rectangle.X += padding.Left;
				rectangle.Height = _ᢧ.Height;
				if (_ᢧ.Width > num2)
				{
					point.X += (_ᢧ.Width - num2) / 2;
				}
				else
				{
					rectangle.X = point.X + (num2 - _ᢧ.Width) / 2;
				}
			}
			Color color = TextColor;
			if (color.IsEmpty)
			{
				color = LabelItem._1933(p, EffectiveStyle, GetEnabled(), color);
			}
			Font font = p.Font;
			eTextFormat eTextFormat2 = eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			if (base.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat2 & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				if ((eTextFormat2 & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
				{
					rectangle.Y = TopInternal + (rect.Height - base.TextMarkupBody.Bounds.Height) / 2;
				}
				else if ((eTextFormat2 & eTextFormat.Bottom) == eTextFormat.Bottom)
				{
					rectangle.Y += base.TextMarkupBody.Bounds.Height - rectangle.Height + Dpi.Height1;
				}
				rectangle.Height = base.TextMarkupBody.Bounds.Height;
				base.TextMarkupBody.Bounds = rectangle;
				base.TextMarkupBody.Render(markupDrawContext);
			}
			else if (p != null && p.GlassEnabled && Parent is ᱭ && !(p.ContainerControl is Ꮮ))
			{
				if (!p.ໟ)
				{
					_21F8._21FC(graphics, Text, font, rectangle, TextDrawing.GetTextFormat(eTextFormat2));
				}
			}
			else
			{
				TextDrawing.DrawString(graphics, Text, font, color, rectangle, eTextFormat2);
			}
		}
		if (_ⴜ != null)
		{
			Rectangle rectangle2 = new Rectangle(point.X, point.Y, num2, num);
			ColorMatrix val = new ColorMatrix();
			val[3, 3] = 1f - _ⴝ;
			ImageAttributes val2 = new ImageAttributes();
			try
			{
				val2.SetColorMatrix(val);
				graphics.DrawImage(_ⴜ, rectangle2, 0, 0, rectangle2.Width, rectangle2.Height, (GraphicsUnit)2, val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			val[3, 3] = _ⴝ;
			ImageAttributes val3 = new ImageAttributes();
			try
			{
				val3.SetColorMatrix(val);
				graphics.DrawImage((Image)(object)_ⴥ, rectangle2, 0, 0, rectangle2.Width, rectangle2.Height, (GraphicsUnit)2, val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		else
		{
			graphics.DrawImage((Image)(object)_ⴥ, point);
		}
		__2D26 = point;
		if (_ⴠ && !_ⴣ.ᮄ)
		{
			Rectangle _2CE8 = _ⴣ._2CE8;
			_2CE8.Offset(point);
			Pen val4 = new Pen(Brushes.DarkGray, (float)Dpi.Width1);
			try
			{
				graphics.FillEllipse(Brushes.White, _2CE8);
				graphics.DrawEllipse(val4, _2CE8);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			_2CE8.Inflate(-Dpi.Width2, -Dpi.Height2);
			SolidBrush val5 = new SolidBrush(_ⴣ._22AC);
			try
			{
				graphics.FillEllipse((Brush)(object)val5, _2CE8);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle rect2 = m_Rect;
			rect2.Inflate(-1, -1);
			_1A6B._1A6C(graphics, rect2, p.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(p.Graphics);
	}

	private void ⵂ(ItemPaintArgs _065C, eDotNetBarStyle _19CF)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		Rectangle displayRectangle = DisplayRectangle;
		displayRectangle.Inflate(-1, 0);
		Graphics graphics = _065C.Graphics;
		if (BarFunctions.IsOffice2007Style(_19CF) && !(_065C.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ButtonItemStateColorTable ᯔ = (Expanded ? ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].Expanded : ((!_ⴤ) ? ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver : ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].Pressed));
			ᯊ.ᯓ(graphics, ᯔ, displayRectangle, _ᵧ);
			return;
		}
		if (!_065C.Colors.ItemHotBackground2.IsEmpty)
		{
			LinearGradientBrush val = BarFunctions.ॴ(displayRectangle, _065C.Colors.ItemHotBackground, _065C.Colors.ItemHotBackground2, _065C.Colors.ItemHotBackgroundGradientAngle);
			graphics.FillRectangle((Brush)(object)val, displayRectangle);
			((Brush)val).Dispose();
		}
		else
		{
			graphics.FillRectangle((Brush)new SolidBrush(_065C.Colors.ItemHotBackground), displayRectangle);
		}
		_089F.र(graphics, new Pen(_065C.Colors.ItemHotBorder), displayRectangle);
	}

	private void ⵃ(ItemPaintArgs _0BD2, eDotNetBarStyle _19CF)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		Rectangle rectangle = ⵄ(_0BD2.RightToLeft);
		Graphics graphics = _0BD2.Graphics;
		if (!BarFunctions.IsOffice2007Style(_19CF))
		{
			if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
			{
				if (!_0BD2.Colors.MenuUnusedSide2.IsEmpty)
				{
					LinearGradientBrush val = BarFunctions.ॴ(new Rectangle(m_Rect.Left, m_Rect.Top, rectangle.Width, m_Rect.Height), _0BD2.Colors.MenuUnusedSide, _0BD2.Colors.MenuUnusedSide2, _0BD2.Colors.MenuUnusedSideGradientAngle);
					graphics.FillRectangle((Brush)(object)val, rectangle);
					((Brush)val).Dispose();
				}
				else
				{
					graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.MenuUnusedSide), rectangle);
				}
			}
			else if (!_0BD2.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(rectangle, _0BD2.Colors.MenuSide, _0BD2.Colors.MenuSide2, _0BD2.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				((Brush)val2).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.MenuSide), rectangle);
			}
		}
		if (!BarFunctions.IsOffice2007Style(_19CF) || !(GlobalManager.Renderer is Office2007Renderer))
		{
			return;
		}
		Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
		if (menu != null)
		{
			if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
			{
				DisplayHelp.FillRectangle(graphics, rectangle, menu.SideUnused);
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, rectangle, menu.Side);
			}
		}
		if (menu != null && !menu.SideBorder.IsEmpty)
		{
			if (_0BD2.RightToLeft)
			{
				DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
			}
			else
			{
				DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 2, rectangle.Y, rectangle.Right - 2, rectangle.Bottom - 1, menu.SideBorder, 1);
			}
		}
		if (menu != null && !menu.SideBorderLight.IsEmpty)
		{
			if (_0BD2.RightToLeft)
			{
				DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
			}
			else
			{
				DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
			}
		}
	}

	private Rectangle ⵄ(bool ⵅ)
	{
		Size size = ო();
		size.Width += 7;
		if (base.IsOnCustomizeMenu)
		{
			size.Width += size.Height + 8;
		}
		Rectangle rect = m_Rect;
		Rectangle result = new Rectangle(rect.Left, rect.Top, size.Width, rect.Height);
		if (ⵅ)
		{
			result.X = rect.Right - result.Width;
		}
		return result;
	}

	private Bitmap ⵆ()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		int num = _ⴱ;
		int num2 = _ⴲ;
		if (!(ContainerControl is MicroChart))
		{
			num = Dpi.Width(_ⴱ);
			num2 = Dpi.Height(_ⴲ);
		}
		Bitmap val = new Bitmap(num, num2, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = (SmoothingMode)2;
			ⳛ ⳛ = ⵈ();
			ⳝ ⳝ = new ⳝ(_ⴚ, val2, num, num2, _ⴵ, _ⴶ);
			ⳛ.CreateChart(ⳝ);
			_ⴧ = ⳝ.ⳟ;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (_ⴚ != null)
		{
			__2D28 = _ⴚ.Count;
		}
		else
		{
			__2D28 = -1;
		}
		return val;
	}

	private void ⵇ(object _٠, EventArgs ؿ)
	{
		ⵀ(ⵁ: false);
	}

	private ⳛ ⵈ()
	{
		ⳛ result = null;
		if (_ⴳ == eMicroChartType.Line)
		{
			ⴆ ⴆ = new ⴆ();
			ⴆ.ߕ = __2D29;
			result = ⴆ;
		}
		else if (_ⴳ == eMicroChartType.Plot)
		{
			ⵏ ⵏ = new ⵏ();
			ⵏ.ߕ = __2D2A;
			result = ⵏ;
		}
		else if (_ⴳ == eMicroChartType.Column)
		{
			ⴁ ⴁ = new ⴁ();
			ⴁ.Style = __2D2B;
			result = ⴁ;
		}
		else if (_ⴳ == eMicroChartType.Bar)
		{
			ⴀ ⴀ = new ⴀ();
			ⴀ.Style = __2D2C;
			result = ⴀ;
		}
		else if (_ⴳ == eMicroChartType.WinLose)
		{
			ⵐ ⵐ = new ⵐ();
			ⵐ.Style = _ⴭ;
			result = ⵐ;
		}
		else if (_ⴳ == eMicroChartType.Pie)
		{
			ⵑ ⵑ = new ⵑ();
			ⵑ.Style = __2D2E;
			result = ⵑ;
		}
		else if (_ⴳ == eMicroChartType.Area)
		{
			ⳮ ⳮ = new ⳮ();
			ⳮ.ߕ = __2D2F;
			result = ⳮ;
		}
		else if (_ⴳ == eMicroChartType.HundredPercentBar)
		{
			ⴂ ⴂ = new ⴂ();
			ⴂ.Style = _ⴰ;
			result = ⴂ;
		}
		return result;
	}

	private void ⵉ(int ك, int ل)
	{
		ⵀ(ⵁ: false);
	}

	private void ⵊ(int ك, int ل)
	{
		ⵀ(ⵁ: false);
	}

	private void ⵋ(eMicroChartType ك, eMicroChartType ل)
	{
		ⵀ(ⵁ: false);
	}

	private void _23B7(eMicroChartTextPosition ك, eMicroChartTextPosition ل)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextPadding()
	{
		if (_ᢾ.Bottom == 0 && _ᢾ.Top == 0 && _ᢾ.Left == 0)
		{
			return _ᢾ.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextPadding()
	{
		_ᢾ = new Padding(0, 0, 0, 0);
	}

	private void _23BA(object _٠, PropertyChangedEventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMargin()
	{
		if (__0AE5.Bottom == 0 && __0AE5.Top == 0 && __0AE5.Left == 0)
		{
			return __0AE5.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetMargin()
	{
		__0AE5 = new Padding(0, 0, 0, 0);
	}

	private void ᣏ(object _٠, PropertyChangedEventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected virtual void OnDataMaxValueChanged(double oldValue, double newValue)
	{
	}

	protected virtual void OnDataMinValueChanged(double oldValue, double newValue)
	{
	}
}
