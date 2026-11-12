using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MSFlexGridLib;
using stdole;

namespace AxMSFlexGridLib;

[DefaultProperty("Text")]
[Clsid("{6262d3a0-531b-11cf-91f6-c2863c385e30}")]
[DesignTimeVisible(true)]
public class AxMSFlexGrid : AxHost
{
	private IMSFlexGrid ocx;

	private AxMSFlexGridEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(4)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int Rows
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Rows", (ActiveXInvokeKind)1);
			}
			return ocx.Rows;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Rows", (ActiveXInvokeKind)2);
			}
			ocx.Rows = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(5)]
	public virtual int Cols
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Cols", (ActiveXInvokeKind)1);
			}
			return ocx.Cols;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Cols", (ActiveXInvokeKind)2);
			}
			ocx.Cols = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(6)]
	public virtual int FixedRows
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FixedRows", (ActiveXInvokeKind)1);
			}
			return ocx.FixedRows;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FixedRows", (ActiveXInvokeKind)2);
			}
			ocx.FixedRows = value;
		}
	}

	[DispId(7)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int FixedCols
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FixedCols", (ActiveXInvokeKind)1);
			}
			return ocx.FixedCols;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FixedCols", (ActiveXInvokeKind)2);
			}
			ocx.FixedCols = value;
		}
	}

	[DispId(1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual short Version
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Version", (ActiveXInvokeKind)1);
			}
			return ocx.Version;
		}
	}

	[DispId(2)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string FormatString
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FormatString", (ActiveXInvokeKind)1);
			}
			return ocx.FormatString;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FormatString", (ActiveXInvokeKind)2);
			}
			ocx.FormatString = value;
		}
	}

	[Browsable(false)]
	[DispId(8)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int TopRow
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TopRow", (ActiveXInvokeKind)1);
			}
			return ocx.TopRow;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TopRow", (ActiveXInvokeKind)2);
			}
			ocx.TopRow = value;
		}
	}

	[DispId(9)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int LeftCol
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("LeftCol", (ActiveXInvokeKind)1);
			}
			return ocx.LeftCol;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("LeftCol", (ActiveXInvokeKind)2);
			}
			ocx.LeftCol = value;
		}
	}

	[DispId(10)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int Row
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Row", (ActiveXInvokeKind)1);
			}
			return ocx.Row;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Row", (ActiveXInvokeKind)2);
			}
			ocx.Row = value;
		}
	}

	[DispId(11)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int Col
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Col", (ActiveXInvokeKind)1);
			}
			return ocx.Col;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Col", (ActiveXInvokeKind)2);
			}
			ocx.Col = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(12)]
	public virtual int RowSel
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("RowSel", (ActiveXInvokeKind)1);
			}
			return ocx.RowSel;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("RowSel", (ActiveXInvokeKind)2);
			}
			ocx.RowSel = value;
		}
	}

	[DispId(13)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int ColSel
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ColSel", (ActiveXInvokeKind)1);
			}
			return ocx.ColSel;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ColSel", (ActiveXInvokeKind)2);
			}
			ocx.ColSel = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(0)]
	[Browsable(false)]
	public override string Text
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return ocx.Text;
			}
			return ((AxHost)this).Text;
		}
		set
		{
			((AxHost)this).Text = value;
			if (ocx != null)
			{
				ocx.Text = value;
			}
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(-501)]
	[ComAliasName("System.UInt32")]
	public override Color BackColor
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return AxHost.GetColorFromOleColor(ocx.BackColor);
			}
			return ((AxHost)this).BackColor;
		}
		set
		{
			((AxHost)this).BackColor = value;
			if (ocx != null)
			{
				ocx.BackColor = AxHost.GetOleColorFromColor(value);
			}
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(-513)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return AxHost.GetColorFromOleColor(ocx.ForeColor);
			}
			return ((AxHost)this).ForeColor;
		}
		set
		{
			((AxHost)this).ForeColor = value;
			if (ocx != null)
			{
				ocx.ForeColor = AxHost.GetOleColorFromColor(value);
			}
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(14)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color BackColorFixed
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorFixed", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.BackColorFixed);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorFixed", (ActiveXInvokeKind)2);
			}
			ocx.BackColorFixed = AxHost.GetOleColorFromColor(value);
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(15)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color ForeColorFixed
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ForeColorFixed", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.ForeColorFixed);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ForeColorFixed", (ActiveXInvokeKind)2);
			}
			ocx.ForeColorFixed = AxHost.GetOleColorFromColor(value);
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(16)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color BackColorSel
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorSel", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.BackColorSel);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorSel", (ActiveXInvokeKind)2);
			}
			ocx.BackColorSel = AxHost.GetOleColorFromColor(value);
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(17)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color ForeColorSel
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ForeColorSel", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.ForeColorSel);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ForeColorSel", (ActiveXInvokeKind)2);
			}
			ocx.ForeColorSel = AxHost.GetOleColorFromColor(value);
		}
	}

	[ComAliasName("System.UInt32")]
	[DispId(18)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color BackColorBkg
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorBkg", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.BackColorBkg);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BackColorBkg", (ActiveXInvokeKind)2);
			}
			ocx.BackColorBkg = AxHost.GetOleColorFromColor(value);
		}
	}

	[DispId(19)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool WordWrap
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("WordWrap", (ActiveXInvokeKind)1);
			}
			return ocx.WordWrap;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("WordWrap", (ActiveXInvokeKind)2);
			}
			ocx.WordWrap = value;
		}
	}

	[Bindable(BindableSupport.Yes)]
	[DispId(-512)]
	[ComAliasName("stdole.IFontDisp")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Font Font
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return AxHost.GetFontFromIFont((object)ocx.Font);
			}
			return ((AxHost)this).Font;
		}
		set
		{
			((AxHost)this).Font = value;
			if (ocx != null)
			{
				ocx.Font = (IFontDisp)AxHost.GetIFontFromFont(value);
			}
		}
	}

	[DispId(84)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual float FontWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontWidth", (ActiveXInvokeKind)1);
			}
			return ocx.FontWidth;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontWidth", (ActiveXInvokeKind)2);
			}
			ocx.FontWidth = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(77)]
	[Browsable(false)]
	public virtual string CellFontName
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontName", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontName;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontName", (ActiveXInvokeKind)2);
			}
			ocx.CellFontName = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(78)]
	public virtual float CellFontSize
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontSize", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontSize;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontSize", (ActiveXInvokeKind)2);
			}
			ocx.CellFontSize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(79)]
	public virtual bool CellFontBold
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontBold", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontBold;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontBold", (ActiveXInvokeKind)2);
			}
			ocx.CellFontBold = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(80)]
	[Browsable(false)]
	public virtual bool CellFontItalic
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontItalic", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontItalic;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontItalic", (ActiveXInvokeKind)2);
			}
			ocx.CellFontItalic = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(81)]
	public virtual bool CellFontUnderline
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontUnderline", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontUnderline;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontUnderline", (ActiveXInvokeKind)2);
			}
			ocx.CellFontUnderline = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(82)]
	public virtual bool CellFontStrikeThrough
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontStrikeThrough", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontStrikeThrough;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontStrikeThrough", (ActiveXInvokeKind)2);
			}
			ocx.CellFontStrikeThrough = value;
		}
	}

	[Browsable(false)]
	[DispId(83)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual float CellFontWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontWidth", (ActiveXInvokeKind)1);
			}
			return ocx.CellFontWidth;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellFontWidth", (ActiveXInvokeKind)2);
			}
			ocx.CellFontWidth = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(20)]
	public virtual TextStyleSettings TextStyle
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TextStyle", (ActiveXInvokeKind)1);
			}
			return ocx.TextStyle;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TextStyle", (ActiveXInvokeKind)2);
			}
			ocx.TextStyle = value;
		}
	}

	[DispId(21)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual TextStyleSettings TextStyleFixed
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TextStyleFixed", (ActiveXInvokeKind)1);
			}
			return ocx.TextStyleFixed;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("TextStyleFixed", (ActiveXInvokeKind)2);
			}
			ocx.TextStyleFixed = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(22)]
	public virtual bool ScrollTrack
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScrollTrack", (ActiveXInvokeKind)1);
			}
			return ocx.ScrollTrack;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScrollTrack", (ActiveXInvokeKind)2);
			}
			ocx.ScrollTrack = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(23)]
	public virtual FocusRectSettings FocusRect
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FocusRect", (ActiveXInvokeKind)1);
			}
			return ocx.FocusRect;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FocusRect", (ActiveXInvokeKind)2);
			}
			ocx.FocusRect = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(24)]
	public virtual HighLightSettings HighLight
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("HighLight", (ActiveXInvokeKind)1);
			}
			return ocx.HighLight;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("HighLight", (ActiveXInvokeKind)2);
			}
			ocx.HighLight = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(25)]
	public virtual bool Redraw
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Redraw", (ActiveXInvokeKind)1);
			}
			return ocx.Redraw;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Redraw", (ActiveXInvokeKind)2);
			}
			ocx.Redraw = value;
		}
	}

	[DispId(26)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual ScrollBarsSettings ScrollBars
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScrollBars", (ActiveXInvokeKind)1);
			}
			return ocx.ScrollBars;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScrollBars", (ActiveXInvokeKind)2);
			}
			ocx.ScrollBars = value;
		}
	}

	[DispId(27)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int MouseRow
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MouseRow", (ActiveXInvokeKind)1);
			}
			return ocx.MouseRow;
		}
	}

	[DispId(28)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int MouseCol
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MouseCol", (ActiveXInvokeKind)1);
			}
			return ocx.MouseCol;
		}
	}

	[DispId(29)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int CellLeft
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellLeft", (ActiveXInvokeKind)1);
			}
			return ocx.CellLeft;
		}
	}

	[DispId(30)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int CellTop
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellTop", (ActiveXInvokeKind)1);
			}
			return ocx.CellTop;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(31)]
	public virtual int CellWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellWidth", (ActiveXInvokeKind)1);
			}
			return ocx.CellWidth;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(32)]
	public virtual int CellHeight
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellHeight", (ActiveXInvokeKind)1);
			}
			return ocx.CellHeight;
		}
	}

	[DispId(33)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int RowHeightMin
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("RowHeightMin", (ActiveXInvokeKind)1);
			}
			return ocx.RowHeightMin;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("RowHeightMin", (ActiveXInvokeKind)2);
			}
			ocx.RowHeightMin = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(-511)]
	public virtual FillStyleSettings FillStyle
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FillStyle", (ActiveXInvokeKind)1);
			}
			return ocx.FillStyle;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FillStyle", (ActiveXInvokeKind)2);
			}
			ocx.FillStyle = value;
		}
	}

	[DispId(34)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual GridLineSettings GridLines
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLines", (ActiveXInvokeKind)1);
			}
			return ocx.GridLines;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLines", (ActiveXInvokeKind)2);
			}
			ocx.GridLines = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(35)]
	public virtual GridLineSettings GridLinesFixed
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLinesFixed", (ActiveXInvokeKind)1);
			}
			return ocx.GridLinesFixed;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLinesFixed", (ActiveXInvokeKind)2);
			}
			ocx.GridLinesFixed = value;
		}
	}

	[DispId(36)]
	[ComAliasName("System.UInt32")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color GridColor
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridColor", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.GridColor);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridColor", (ActiveXInvokeKind)2);
			}
			ocx.GridColor = AxHost.GetOleColorFromColor(value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ComAliasName("System.UInt32")]
	[DispId(37)]
	public virtual Color GridColorFixed
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridColorFixed", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.GridColorFixed);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridColorFixed", (ActiveXInvokeKind)2);
			}
			ocx.GridColorFixed = AxHost.GetOleColorFromColor(value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ComAliasName("System.UInt32")]
	[DispId(38)]
	[Browsable(false)]
	public virtual Color CellBackColor
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellBackColor", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.CellBackColor);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellBackColor", (ActiveXInvokeKind)2);
			}
			ocx.CellBackColor = AxHost.GetOleColorFromColor(value);
		}
	}

	[DispId(39)]
	[Browsable(false)]
	[ComAliasName("System.UInt32")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Color CellForeColor
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellForeColor", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.CellForeColor);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellForeColor", (ActiveXInvokeKind)2);
			}
			ocx.CellForeColor = AxHost.GetOleColorFromColor(value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(40)]
	public virtual short CellAlignment
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellAlignment", (ActiveXInvokeKind)1);
			}
			return ocx.CellAlignment;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellAlignment", (ActiveXInvokeKind)2);
			}
			ocx.CellAlignment = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(41)]
	[Browsable(false)]
	public virtual TextStyleSettings CellTextStyle
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellTextStyle", (ActiveXInvokeKind)1);
			}
			return ocx.CellTextStyle;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellTextStyle", (ActiveXInvokeKind)2);
			}
			ocx.CellTextStyle = value;
		}
	}

	[DispId(43)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual short CellPictureAlignment
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellPictureAlignment", (ActiveXInvokeKind)1);
			}
			return ocx.CellPictureAlignment;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellPictureAlignment", (ActiveXInvokeKind)2);
			}
			ocx.CellPictureAlignment = value;
		}
	}

	[DispId(45)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string Clip
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Clip", (ActiveXInvokeKind)1);
			}
			return ocx.Clip;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Clip", (ActiveXInvokeKind)2);
			}
			ocx.Clip = value;
		}
	}

	[DispId(46)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual short Sort
	{
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Sort", (ActiveXInvokeKind)2);
			}
			ocx.Sort = value;
		}
	}

	[DispId(47)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual SelectionModeSettings SelectionMode
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SelectionMode", (ActiveXInvokeKind)1);
			}
			return ocx.SelectionMode;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SelectionMode", (ActiveXInvokeKind)2);
			}
			ocx.SelectionMode = value;
		}
	}

	[DispId(48)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual MergeCellsSettings MergeCells
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MergeCells", (ActiveXInvokeKind)1);
			}
			return ocx.MergeCells;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MergeCells", (ActiveXInvokeKind)2);
			}
			ocx.MergeCells = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(51)]
	public virtual bool AllowBigSelection
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AllowBigSelection", (ActiveXInvokeKind)1);
			}
			return ocx.AllowBigSelection;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AllowBigSelection", (ActiveXInvokeKind)2);
			}
			ocx.AllowBigSelection = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(52)]
	public virtual AllowUserResizeSettings AllowUserResizing
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AllowUserResizing", (ActiveXInvokeKind)1);
			}
			return ocx.AllowUserResizing;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AllowUserResizing", (ActiveXInvokeKind)2);
			}
			ocx.AllowUserResizing = value;
		}
	}

	[DispId(-504)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual BorderStyleSettings BorderStyle
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BorderStyle", (ActiveXInvokeKind)1);
			}
			return ocx.BorderStyle;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BorderStyle", (ActiveXInvokeKind)2);
			}
			ocx.BorderStyle = value;
		}
	}

	[DispId(-515)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ComAliasName("System.Int32")]
	[Browsable(false)]
	public virtual int hWnd
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("hWnd", (ActiveXInvokeKind)1);
			}
			return ocx.hWnd;
		}
	}

	[DispId(-514)]
	[Bindable(BindableSupport.Yes)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(true)]
	public override bool Enabled
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return ocx.Enabled;
			}
			return ((AxHost)this).Enabled;
		}
		set
		{
			((AxHost)this).Enabled = value;
			if (ocx != null)
			{
				ocx.Enabled = value;
			}
		}
	}

	[DispId(-520)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual AppearanceSettings Appearance
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Appearance", (ActiveXInvokeKind)1);
			}
			return ocx.Appearance;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Appearance", (ActiveXInvokeKind)2);
			}
			ocx.Appearance = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(53)]
	public virtual MousePointerSettings MousePointer
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MousePointer", (ActiveXInvokeKind)1);
			}
			return ocx.MousePointer;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MousePointer", (ActiveXInvokeKind)2);
			}
			ocx.MousePointer = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(54)]
	[ComAliasName("stdole.IPictureDisp")]
	public virtual Image MouseIcon
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MouseIcon", (ActiveXInvokeKind)1);
			}
			return AxHost.GetPictureFromIPicture((object)ocx.MouseIcon);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MouseIcon", (ActiveXInvokeKind)2);
			}
			ocx.MouseIcon = (IPictureDisp)AxHost.GetIPictureFromPicture(value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(50)]
	public virtual PictureTypeSettings PictureType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PictureType", (ActiveXInvokeKind)1);
			}
			return ocx.PictureType;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PictureType", (ActiveXInvokeKind)2);
			}
			ocx.PictureType = value;
		}
	}

	[DispId(49)]
	[ComAliasName("stdole.IPictureDisp")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Image Picture
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Picture", (ActiveXInvokeKind)1);
			}
			return AxHost.GetPictureFromIPicture((object)ocx.Picture);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(42)]
	[ComAliasName("stdole.IPictureDisp")]
	public virtual Image CellPicture
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellPicture", (ActiveXInvokeKind)1);
			}
			return AxHost.GetPictureFromIPicture((object)ocx.CellPicture);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CellPicture", (ActiveXInvokeKind)2);
			}
			ocx.CellPicture = (IPictureDisp)AxHost.GetIPictureFromPicture(value);
		}
	}

	[Bindable(BindableSupport.Yes)]
	[DispId(76)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual IRowCursor DataSource
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DataSource", (ActiveXInvokeKind)1);
			}
			return ocx.DataSource;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DataSource", (ActiveXInvokeKind)2);
			}
			ocx.DataSource = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(89)]
	public virtual short GridLineWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLineWidth", (ActiveXInvokeKind)1);
			}
			return ocx.GridLineWidth;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("GridLineWidth", (ActiveXInvokeKind)2);
			}
			ocx.GridLineWidth = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(91)]
	public virtual string FontName
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontName", (ActiveXInvokeKind)1);
			}
			return ocx.FontName;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontName", (ActiveXInvokeKind)2);
			}
			ocx.FontName = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(92)]
	[Browsable(false)]
	public virtual float FontSize
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontSize", (ActiveXInvokeKind)1);
			}
			return ocx.FontSize;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontSize", (ActiveXInvokeKind)2);
			}
			ocx.FontSize = value;
		}
	}

	[DispId(93)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool FontBold
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontBold", (ActiveXInvokeKind)1);
			}
			return ocx.FontBold;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontBold", (ActiveXInvokeKind)2);
			}
			ocx.FontBold = value;
		}
	}

	[DispId(94)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool FontItalic
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontItalic", (ActiveXInvokeKind)1);
			}
			return ocx.FontItalic;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontItalic", (ActiveXInvokeKind)2);
			}
			ocx.FontItalic = value;
		}
	}

	[DispId(95)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool FontStrikethru
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontStrikethru", (ActiveXInvokeKind)1);
			}
			return ocx.FontStrikethru;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontStrikethru", (ActiveXInvokeKind)2);
			}
			ocx.FontStrikethru = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(96)]
	public virtual bool FontUnderline
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontUnderline", (ActiveXInvokeKind)1);
			}
			return ocx.FontUnderline;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("FontUnderline", (ActiveXInvokeKind)2);
			}
			ocx.FontUnderline = value;
		}
	}

	[DispId(-611)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(true)]
	public override bool RightToLeft
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return ocx.RightToLeft;
			}
			return ((AxHost)this).RightToLeft;
		}
		set
		{
			((AxHost)this).RightToLeft = value;
			if (ocx != null)
			{
				ocx.RightToLeft = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1551)]
	public virtual OLEDropConstants OLEDropMode
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("OLEDropMode", (ActiveXInvokeKind)1);
			}
			return ocx.OLEDropMode;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("OLEDropMode", (ActiveXInvokeKind)2);
			}
			ocx.OLEDropMode = value;
		}
	}

	public event EventHandler ClickEvent;

	public event DMSFlexGridEvents_KeyDownEventHandler KeyDownEvent;

	public event EventHandler DblClick;

	public event DMSFlexGridEvents_KeyPressEventHandler KeyPressEvent;

	public event DMSFlexGridEvents_KeyUpEventHandler KeyUpEvent;

	public event DMSFlexGridEvents_MouseDownEventHandler MouseDownEvent;

	public event DMSFlexGridEvents_MouseMoveEventHandler MouseMoveEvent;

	public event DMSFlexGridEvents_MouseUpEventHandler MouseUpEvent;

	public event EventHandler SelChange;

	public event EventHandler RowColChange;

	public event EventHandler EnterCell;

	public event EventHandler LeaveCell;

	public event EventHandler Scroll;

	public event DMSFlexGridEvents_CompareEventHandler Compare;

	public event DMSFlexGridEvents_OLEStartDragEventHandler OLEStartDrag;

	public event DMSFlexGridEvents_OLEGiveFeedbackEventHandler OLEGiveFeedback;

	public event DMSFlexGridEvents_OLESetDataEventHandler OLESetData;

	public event DMSFlexGridEvents_OLECompleteDragEventHandler OLECompleteDrag;

	public event DMSFlexGridEvents_OLEDragOverEventHandler OLEDragOver;

	public event DMSFlexGridEvents_OLEDragDropEventHandler OLEDragDrop;

	public AxMSFlexGrid()
		: base("6262d3a0-531b-11cf-91f6-c2863c385e30")
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		((AxHost)this).SetAboutBoxDelegate(new AboutBoxDelegate(AboutBox));
	}

	public virtual short get_FixedAlignment(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_FixedAlignment", (ActiveXInvokeKind)0);
		}
		return ocx.get_FixedAlignment(index);
	}

	public virtual void set_FixedAlignment(int index, short fixedAlignment)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_FixedAlignment", (ActiveXInvokeKind)0);
		}
		ocx.set_FixedAlignment(index, fixedAlignment);
	}

	public virtual void OLEDrag()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("OLEDrag", (ActiveXInvokeKind)0);
		}
		ocx.OLEDrag();
	}

	public virtual void AboutBox()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("AboutBox", (ActiveXInvokeKind)0);
		}
		ocx.AboutBox();
	}

	public virtual string get_TextArray(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_TextArray", (ActiveXInvokeKind)0);
		}
		return ocx.get_TextArray(index);
	}

	public virtual void set_TextArray(int index, string textArray)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_TextArray", (ActiveXInvokeKind)0);
		}
		ocx.set_TextArray(index, textArray);
	}

	public virtual short get_ColAlignment(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_ColAlignment", (ActiveXInvokeKind)0);
		}
		return ocx.get_ColAlignment(index);
	}

	public virtual void set_ColAlignment(int index, short colAlignment)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_ColAlignment", (ActiveXInvokeKind)0);
		}
		ocx.set_ColAlignment(index, colAlignment);
	}

	public virtual int get_ColWidth(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_ColWidth", (ActiveXInvokeKind)0);
		}
		return ocx.get_ColWidth(index);
	}

	public virtual void set_ColWidth(int index, int colWidth)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_ColWidth", (ActiveXInvokeKind)0);
		}
		ocx.set_ColWidth(index, colWidth);
	}

	public virtual int get_RowHeight(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_RowHeight", (ActiveXInvokeKind)0);
		}
		return ocx.get_RowHeight(index);
	}

	public virtual void set_RowHeight(int index, int rowHeight)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_RowHeight", (ActiveXInvokeKind)0);
		}
		ocx.set_RowHeight(index, rowHeight);
	}

	public virtual bool get_MergeRow(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_MergeRow", (ActiveXInvokeKind)0);
		}
		return ocx.get_MergeRow(index);
	}

	public virtual void set_MergeRow(int index, bool mergeRow)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_MergeRow", (ActiveXInvokeKind)0);
		}
		ocx.set_MergeRow(index, mergeRow);
	}

	public virtual bool get_MergeCol(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_MergeCol", (ActiveXInvokeKind)0);
		}
		return ocx.get_MergeCol(index);
	}

	public virtual void set_MergeCol(int index, bool mergeCol)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_MergeCol", (ActiveXInvokeKind)0);
		}
		ocx.set_MergeCol(index, mergeCol);
	}

	public virtual void set_RowPosition(int index, int param0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_RowPosition", (ActiveXInvokeKind)0);
		}
		ocx.set_RowPosition(index, param0);
	}

	public virtual void set_ColPosition(int index, int param0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_ColPosition", (ActiveXInvokeKind)0);
		}
		ocx.set_ColPosition(index, param0);
	}

	public virtual int get_RowData(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_RowData", (ActiveXInvokeKind)0);
		}
		return ocx.get_RowData(index);
	}

	public virtual void set_RowData(int index, int rowData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_RowData", (ActiveXInvokeKind)0);
		}
		ocx.set_RowData(index, rowData);
	}

	public virtual int get_ColData(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_ColData", (ActiveXInvokeKind)0);
		}
		return ocx.get_ColData(index);
	}

	public virtual void set_ColData(int index, int colData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_ColData", (ActiveXInvokeKind)0);
		}
		ocx.set_ColData(index, colData);
	}

	public virtual string get_TextMatrix(int row, int col)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_TextMatrix", (ActiveXInvokeKind)0);
		}
		return ocx.get_TextMatrix(row, col);
	}

	public virtual void set_TextMatrix(int row, int col, string textMatrix)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_TextMatrix", (ActiveXInvokeKind)0);
		}
		ocx.set_TextMatrix(row, col, textMatrix);
	}

	public virtual void AddItem(string item)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("AddItem", (ActiveXInvokeKind)0);
		}
		object[] parameters = new object[2]
		{
			item,
			Missing.Value
		};
		Type typeFromHandle = typeof(IMSFlexGrid);
		MethodInfo method = typeFromHandle.GetMethod("AddItem");
		method.Invoke(ocx, parameters);
	}

	public virtual void AddItem(string item, object index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("AddItem", (ActiveXInvokeKind)0);
		}
		ocx.AddItem(item, index);
	}

	public virtual void RemoveItem(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RemoveItem", (ActiveXInvokeKind)0);
		}
		ocx.RemoveItem(index);
	}

	public virtual void Clear()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Clear", (ActiveXInvokeKind)0);
		}
		ocx.Clear();
	}

	public virtual void Refresh()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Refresh", (ActiveXInvokeKind)0);
		}
		ocx.Refresh();
	}

	public virtual bool get_RowIsVisible(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_RowIsVisible", (ActiveXInvokeKind)0);
		}
		return ocx.get_RowIsVisible(index);
	}

	public virtual bool get_ColIsVisible(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_ColIsVisible", (ActiveXInvokeKind)0);
		}
		return ocx.get_ColIsVisible(index);
	}

	public virtual int get_RowPos(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_RowPos", (ActiveXInvokeKind)0);
		}
		return ocx.get_RowPos(index);
	}

	public virtual int get_ColPos(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_ColPos", (ActiveXInvokeKind)0);
		}
		return ocx.get_ColPos(index);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxMSFlexGridEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(DMSFlexGridEvents));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = (IMSFlexGrid)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnClickEvent(object sender, EventArgs e)
	{
		if (this.ClickEvent != null)
		{
			this.ClickEvent(sender, e);
		}
	}

	internal void RaiseOnKeyDownEvent(object sender, DMSFlexGridEvents_KeyDownEvent e)
	{
		if (this.KeyDownEvent != null)
		{
			this.KeyDownEvent(sender, e);
		}
	}

	internal void RaiseOnDblClick(object sender, EventArgs e)
	{
		if (this.DblClick != null)
		{
			this.DblClick(sender, e);
		}
	}

	internal void RaiseOnKeyPressEvent(object sender, DMSFlexGridEvents_KeyPressEvent e)
	{
		if (this.KeyPressEvent != null)
		{
			this.KeyPressEvent(sender, e);
		}
	}

	internal void RaiseOnKeyUpEvent(object sender, DMSFlexGridEvents_KeyUpEvent e)
	{
		if (this.KeyUpEvent != null)
		{
			this.KeyUpEvent(sender, e);
		}
	}

	internal void RaiseOnMouseDownEvent(object sender, DMSFlexGridEvents_MouseDownEvent e)
	{
		if (this.MouseDownEvent != null)
		{
			this.MouseDownEvent(sender, e);
		}
	}

	internal void RaiseOnMouseMoveEvent(object sender, DMSFlexGridEvents_MouseMoveEvent e)
	{
		if (this.MouseMoveEvent != null)
		{
			this.MouseMoveEvent(sender, e);
		}
	}

	internal void RaiseOnMouseUpEvent(object sender, DMSFlexGridEvents_MouseUpEvent e)
	{
		if (this.MouseUpEvent != null)
		{
			this.MouseUpEvent(sender, e);
		}
	}

	internal void RaiseOnSelChange(object sender, EventArgs e)
	{
		if (this.SelChange != null)
		{
			this.SelChange(sender, e);
		}
	}

	internal void RaiseOnRowColChange(object sender, EventArgs e)
	{
		if (this.RowColChange != null)
		{
			this.RowColChange(sender, e);
		}
	}

	internal void RaiseOnEnterCell(object sender, EventArgs e)
	{
		if (this.EnterCell != null)
		{
			this.EnterCell(sender, e);
		}
	}

	internal void RaiseOnLeaveCell(object sender, EventArgs e)
	{
		if (this.LeaveCell != null)
		{
			this.LeaveCell(sender, e);
		}
	}

	internal void RaiseOnScroll(object sender, EventArgs e)
	{
		if (this.Scroll != null)
		{
			this.Scroll(sender, e);
		}
	}

	internal void RaiseOnCompare(object sender, DMSFlexGridEvents_CompareEvent e)
	{
		if (this.Compare != null)
		{
			this.Compare(sender, e);
		}
	}

	internal void RaiseOnOLEStartDrag(object sender, DMSFlexGridEvents_OLEStartDragEvent e)
	{
		if (this.OLEStartDrag != null)
		{
			this.OLEStartDrag(sender, e);
		}
	}

	internal void RaiseOnOLEGiveFeedback(object sender, DMSFlexGridEvents_OLEGiveFeedbackEvent e)
	{
		if (this.OLEGiveFeedback != null)
		{
			this.OLEGiveFeedback(sender, e);
		}
	}

	internal void RaiseOnOLESetData(object sender, DMSFlexGridEvents_OLESetDataEvent e)
	{
		if (this.OLESetData != null)
		{
			this.OLESetData(sender, e);
		}
	}

	internal void RaiseOnOLECompleteDrag(object sender, DMSFlexGridEvents_OLECompleteDragEvent e)
	{
		if (this.OLECompleteDrag != null)
		{
			this.OLECompleteDrag(sender, e);
		}
	}

	internal void RaiseOnOLEDragOver(object sender, DMSFlexGridEvents_OLEDragOverEvent e)
	{
		if (this.OLEDragOver != null)
		{
			this.OLEDragOver(sender, e);
		}
	}

	internal void RaiseOnOLEDragDrop(object sender, DMSFlexGridEvents_OLEDragDropEvent e)
	{
		if (this.OLEDragDrop != null)
		{
			this.OLEDragDrop(sender, e);
		}
	}
}
