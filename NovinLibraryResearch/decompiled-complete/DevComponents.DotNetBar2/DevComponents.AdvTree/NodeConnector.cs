using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace DevComponents.AdvTree;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class NodeConnector : Component
{
	private int _17D9 = 1;

	private Color _17DA = SystemColors.Highlight;

	private eNodeConnectorType _17DB;

	private EventHandler ៜ;

	private DashStyle __17DD = (DashStyle)2;

	[Browsable(true)]
	[DefaultValue(1)]
	[Description("Indicates connector line width.")]
	[Category("Appearance")]
	public int LineWidth
	{
		get
		{
			return _17D9;
		}
		set
		{
			_17D9 = value;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates color of the connector line.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color LineColor
	{
		get
		{
			return _17DA;
		}
		set
		{
			_17DA = value;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates visual type of the connector.")]
	[DefaultValue(eNodeConnectorType.Line)]
	[Browsable(false)]
	[Category("Appearance")]
	public eNodeConnectorType ConnectorType
	{
		get
		{
			return _17DB;
		}
		set
		{
			_17DB = value;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates DashStyle for the connector line")]
	[Category("Appearance")]
	public DashStyle DashStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __17DD;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			__17DD = value;
		}
	}

	public event EventHandler AppearanceChanged
	{
		add
		{
			EventHandler eventHandler = ៜ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ៜ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ៜ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ៜ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public NodeConnector()
	{
	}//IL_0014: Unknown result type (might be due to invalid IL or missing references)


	public NodeConnector(int lineWidth, eNodeConnectorType type)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		LineWidth = lineWidth;
		ConnectorType = type;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeLineColor()
	{
		return _17DA != SystemColors.Highlight;
	}

	private void OnAppearanceChanged()
	{
		if (ៜ != null)
		{
			ៜ(this, new EventArgs());
		}
	}
}
