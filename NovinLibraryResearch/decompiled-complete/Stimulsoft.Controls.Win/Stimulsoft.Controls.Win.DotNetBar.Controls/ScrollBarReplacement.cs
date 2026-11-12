using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

internal class ScrollBarReplacement : IDisposable
{
	internal interface IScrollBarExtender
	{
		void SetValue(int newValue, ScrollEventType type);
	}

	private ScrollBarCore m_ScrollBarCore;

	private ScrollBar m_ParentScrollBar;

	private IScrollBarExtender m_ParentScrollBarWndProc;

	private bool m_IsVScrollBar;

	private ColorScheme m_ColorScheme;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool IsVScroll => m_IsVScrollBar;

	public eRenderMode RenderMode
	{
		get
		{
			return m_RenderMode;
		}
		set
		{
			if (m_RenderMode != value)
			{
				m_RenderMode = value;
				((Control)m_ParentScrollBar).Invalidate(true);
			}
		}
	}

	public BaseRenderer Renderer
	{
		get
		{
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
		}
	}

	internal ScrollBarCore ScrollBarCore => m_ScrollBarCore;

	public bool AppStyleScrollBar
	{
		get
		{
			return m_ScrollBarCore.IsAppScrollBarStyle;
		}
		set
		{
			m_ScrollBarCore.IsAppScrollBarStyle = value;
		}
	}

	public ScrollBarReplacement(ScrollBar sb)
	{
		m_ParentScrollBar = sb;
		m_ParentScrollBarWndProc = (IScrollBarExtender)m_ParentScrollBar;
		m_IsVScrollBar = m_ParentScrollBar is VScrollBar;
		m_ScrollBarCore = new ScrollBarCore((Control)(object)m_ParentScrollBar, isPassive: false);
		m_ScrollBarCore.ValueChanged += ScrollBarCore_ValueChanged;
		if (m_ParentScrollBar is HScrollBar)
		{
			m_ScrollBarCore.Orientation = eOrientation.Horizontal;
		}
		else
		{
			m_ScrollBarCore.Orientation = eOrientation.Vertical;
		}
		m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
		m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
		m_ScrollBarCore.Value = m_ParentScrollBar.Value;
		m_ScrollBarCore.Enabled = ((Control)m_ParentScrollBar).Enabled;
		((Control)m_ParentScrollBar).EnabledChanged += ParentScrollBar_EnabledChanged;
	}

	private void ParentScrollBar_EnabledChanged(object sender, EventArgs e)
	{
		m_ScrollBarCore.Enabled = ((Control)m_ParentScrollBar).Enabled;
	}

	internal void OnHandleCreated()
	{
		UpdateScrollValues();
	}

	internal void UpdateScrollValues()
	{
		Rectangle rectangle = new Rectangle(0, 0, ((Control)m_ParentScrollBar).Width, ((Control)m_ParentScrollBar).Height);
		if (m_ScrollBarCore.Minimum != m_ParentScrollBar.Minimum)
		{
			m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
		}
		if (m_ScrollBarCore.Maximum != m_ParentScrollBar.Maximum)
		{
			m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
		}
		if (m_ScrollBarCore.SmallChange != m_ParentScrollBar.SmallChange)
		{
			m_ScrollBarCore.SmallChange = m_ParentScrollBar.SmallChange;
		}
		if (m_ScrollBarCore.LargeChange != m_ParentScrollBar.LargeChange)
		{
			m_ScrollBarCore.LargeChange = m_ParentScrollBar.LargeChange;
		}
		if (m_ScrollBarCore.Value != m_ParentScrollBar.Value)
		{
			m_ScrollBarCore.Value = m_ParentScrollBar.Value;
		}
		if (rectangle != m_ScrollBarCore.DisplayRectangle)
		{
			m_ScrollBarCore.DisplayRectangle = rectangle;
		}
	}

	internal void OnMouseEnter(EventArgs e)
	{
		if (((Control)m_ParentScrollBar).Capture)
		{
			((Control)m_ParentScrollBar).Capture = false;
		}
	}

	internal void OnMouseLeave(EventArgs e)
	{
		m_ScrollBarCore.MouseLeave();
	}

	internal void OnMouseMove(MouseEventArgs e)
	{
		m_ScrollBarCore.MouseMove(e);
	}

	internal void NotifyInvalidate(Rectangle invalidatedArea)
	{
		m_ScrollBarCore.DisposeCashedView();
	}

	internal void OnMouseDown(MouseEventArgs e)
	{
		m_ScrollBarCore.MouseDown(e);
	}

	internal void OnMouseUp(MouseEventArgs e)
	{
		m_ScrollBarCore.MouseUp(e);
	}

	private void ScrollBarCore_ValueChanged(object sender, EventArgs e)
	{
		SetValue(m_ScrollBarCore.Value);
	}

	private void SetValue(int v)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (m_ParentScrollBar.Value != v || m_ParentScrollBar.Value == m_ScrollBarCore.GetMaximumValue())
		{
			ScrollEventType val = (ScrollEventType)1;
			if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.ThumbDecrease)
			{
				val = (ScrollEventType)0;
			}
			else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track)
			{
				val = (ScrollEventType)5;
			}
			else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Control)
			{
				val = ((v <= m_ParentScrollBar.Value) ? ((ScrollEventType)2) : ((ScrollEventType)3));
			}
			if ((int)val == 1 && m_ParentScrollBar.Value == v && m_ParentScrollBar.Value == m_ScrollBarCore.GetMaximumValue())
			{
				val = (ScrollEventType)7;
			}
			m_ParentScrollBarWndProc.SetValue(v, val);
		}
	}

	public void Dispose()
	{
	}

	internal void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		UpdateScrollValues();
		m_ScrollBarCore.Paint(GetItemPaintArgs(graphics));
	}

	private ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(m_ParentScrollBar as IOwner, (Control)(object)m_ParentScrollBar, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = false;
		return itemPaintArgs;
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (m_ColorScheme == null)
		{
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return m_ColorScheme;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (m_RenderMode == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
		{
			return m_Renderer;
		}
		if (m_DefaultRenderer == null)
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_DefaultRenderer;
	}
}
