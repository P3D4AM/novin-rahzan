using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.MicroCharts;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class HundredPctMicroChartStyle
{
	private List<Color> _ⴄ = new List<Color>();

	private EventHandler ވ;

	private int __2CFA = 2;

	private Color _ⴅ = ColorScheme.GetColor(14869735);

	[Category("Appearance")]
	[DefaultValue(2)]
	[Description("Indicates minimum single bar width.")]
	public int MinBarSize
	{
		get
		{
			return __2CFA;
		}
		set
		{
			__2CFA = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Color> BarColors => _ⴄ;

	[Description("Indicates color of slice outline.")]
	[Category("Appearance")]
	public Color BarOutlineColor
	{
		get
		{
			return _ⴅ;
		}
		set
		{
			_ⴅ = value;
			ߓ();
		}
	}

	public event EventHandler StyleChanged
	{
		add
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public HundredPctMicroChartStyle()
	{
		_ⴄ.AddRange(new Color[52]
		{
			ColorScheme.GetColor(12582912),
			ColorScheme.GetColor(16711680),
			ColorScheme.GetColor(16760832),
			ColorScheme.GetColor(16776960),
			ColorScheme.GetColor(9621584),
			ColorScheme.GetColor(45136),
			ColorScheme.GetColor(45296),
			ColorScheme.GetColor(28864),
			ColorScheme.GetColor(8288),
			ColorScheme.GetColor(7352480),
			ColorScheme.GetColor(15658209),
			ColorScheme.GetColor(2050429),
			ColorScheme.GetColor(5210557),
			ColorScheme.GetColor(12603469),
			ColorScheme.GetColor(10206041),
			ColorScheme.GetColor(8414370),
			ColorScheme.GetColor(4959430),
			ColorScheme.GetColor(16225862),
			ColorScheme.GetColor(15921906),
			ColorScheme.GetColor(14539203),
			ColorScheme.GetColor(13031920),
			ColorScheme.GetColor(15916251),
			ColorScheme.GetColor(15462877),
			ColorScheme.GetColor(15065324),
			ColorScheme.GetColor(14413555),
			ColorScheme.GetColor(16640730),
			ColorScheme.GetColor(14211288),
			ColorScheme.GetColor(12893591),
			ColorScheme.GetColor(9286626),
			ColorScheme.GetColor(15055287),
			ColorScheme.GetColor(14148540),
			ColorScheme.GetColor(13418969),
			ColorScheme.GetColor(12049896),
			ColorScheme.GetColor(16504245),
			ColorScheme.GetColor(12566463),
			ColorScheme.GetColor(9668947),
			ColorScheme.GetColor(5541332),
			ColorScheme.GetColor(9810903),
			ColorScheme.GetColor(14259860),
			ColorScheme.GetColor(12834459),
			ColorScheme.GetColor(11706823),
			ColorScheme.GetColor(9620956),
			ColorScheme.GetColor(16433295),
			ColorScheme.GetColor(10855845),
			ColorScheme.GetColor(4801577),
			ColorScheme.GetColor(1521245),
			ColorScheme.GetColor(3563666),
			ColorScheme.GetColor(9778996),
			ColorScheme.GetColor(7770684),
			ColorScheme.GetColor(6244730),
			ColorScheme.GetColor(3245467),
			ColorScheme.GetColor(14904329)
		});
	}

	protected virtual void OnStyleChanged(EventArgs e)
	{
		ވ?.Invoke(this, e);
	}

	private void ߓ()
	{
		OnStyleChanged(EventArgs.Empty);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarOutlineColor()
	{
		return _ⴅ != ColorScheme.GetColor(14869735);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBarOutlineColor()
	{
		BarOutlineColor = ColorScheme.GetColor(14869735);
	}
}
