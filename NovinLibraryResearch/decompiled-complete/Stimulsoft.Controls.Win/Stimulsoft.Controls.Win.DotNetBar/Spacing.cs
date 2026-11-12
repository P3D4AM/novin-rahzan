using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class Spacing
{
	private int m_Left;

	private int m_Right;

	private int m_Top;

	private int m_Bottom;

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the left side.")]
	[DevCoSerialize]
	public int Left
	{
		get
		{
			return m_Left;
		}
		set
		{
			m_Left = value;
			OnSpacingChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the right side.")]
	[DevCoSerialize]
	public int Right
	{
		get
		{
			return m_Right;
		}
		set
		{
			m_Right = value;
			OnSpacingChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the top.")]
	[DevCoSerialize]
	public int Top
	{
		get
		{
			return m_Top;
		}
		set
		{
			m_Top = value;
			OnSpacingChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the bottom.")]
	[DevCoSerialize]
	public int Bottom
	{
		get
		{
			return m_Bottom;
		}
		set
		{
			m_Bottom = value;
			OnSpacingChanged();
		}
	}

	[Browsable(false)]
	public int Horizontal => Left + Right;

	[Browsable(false)]
	public int Vertical => Top + Bottom;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (m_Left == 0 && m_Right == 0 && m_Top == 0)
			{
				return m_Bottom == 0;
			}
			return false;
		}
	}

	public event EventHandler SpacingChanged;

	private void OnSpacingChanged()
	{
		if (this.SpacingChanged != null)
		{
			this.SpacingChanged(this, new EventArgs());
		}
	}
}
