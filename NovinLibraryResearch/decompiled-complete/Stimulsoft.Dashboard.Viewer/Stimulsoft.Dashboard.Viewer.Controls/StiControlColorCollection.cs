using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls;

public class StiControlColorCollection
{
	private Control control;

	public Color ForeColor { get; set; } = StiElementConsts.ForegroundColor;


	public Color BackColor { get; set; } = StiElementConsts.BackgroundColor;


	public Color GlyphColor { get; set; } = StiElementConsts.ForegroundColor;


	public Color SelectedForeColor { get; set; } = SystemColors.HighlightText;


	public Color SelectedBackColor { get; set; } = SystemColors.Highlight;


	public Color SelectedGlyphColor { get; set; } = Color.DimGray;


	public Color HotForeColor { get; set; } = SystemColors.ActiveCaptionText;


	public Color HotBackColor { get; set; } = SystemColors.ActiveCaption;


	public Color HotGlyphColor { get; set; } = Color.DimGray;


	public Color HotSelectedForeColor { get; set; } = SystemColors.MenuText;


	public Color HotSelectedBackColor { get; set; } = SystemColors.MenuHighlight;


	public Color HotSelectedGlyphColor { get; set; } = Color.DimGray;


	public Color SeparatorColor { get; set; } = Color.LightGray;


	public bool IsDarkMode { get; set; }

	public Control Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
			if (control != null)
			{
				control.BackColor = StiTransparentColor.Fix(BackColor);
				control.ForeColor = StiTransparentColor.Fix(ForeColor);
			}
		}
	}

	public static StiControlColorCollection GetElementColors(IStiElement element)
	{
		StiControlColorCollection stiControlColorCollection = new StiControlColorCollection();
		if (element is IStiControlElement element2)
		{
			StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element2);
			stiControlColorCollection.ForeColor = StiDashboardStyleHelper.GetForeColor(element, controlStyle.ForeColor);
			stiControlColorCollection.BackColor = StiTransparentColor.Fix(StiDashboardStyleHelper.GetBackColor(element, controlStyle.BackColor));
			stiControlColorCollection.GlyphColor = controlStyle.GlyphColor;
			stiControlColorCollection.SelectedForeColor = controlStyle.SelectedForeColor;
			stiControlColorCollection.SelectedBackColor = controlStyle.SelectedBackColor;
			stiControlColorCollection.SelectedGlyphColor = controlStyle.SelectedGlyphColor;
			stiControlColorCollection.HotForeColor = controlStyle.HotForeColor;
			stiControlColorCollection.HotBackColor = controlStyle.HotBackColor;
			stiControlColorCollection.HotGlyphColor = controlStyle.HotGlyphColor;
			stiControlColorCollection.HotSelectedForeColor = controlStyle.HotSelectedForeColor;
			stiControlColorCollection.HotSelectedBackColor = controlStyle.HotSelectedBackColor;
			stiControlColorCollection.HotSelectedGlyphColor = controlStyle.HotSelectedGlyphColor;
			stiControlColorCollection.SeparatorColor = controlStyle.SeparatorColor;
			stiControlColorCollection.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(element);
			return stiControlColorCollection;
		}
		stiControlColorCollection.ForeColor = StiDashboardStyleHelper.GetForeColor(element, stiControlColorCollection.ForeColor);
		stiControlColorCollection.BackColor = StiDashboardStyleHelper.GetBackColor(element, stiControlColorCollection.BackColor);
		stiControlColorCollection.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(element);
		return stiControlColorCollection;
	}

	public static StiControlColorCollection GetActionColors(IStiElement element = null)
	{
		StiControlColorCollection stiControlColorCollection = new StiControlColorCollection();
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		bool flag = StiDashboardStyleHelper.IsDarkStyle(element);
		stiControlColorCollection.ForeColor = StiDashboardStyleHelper.GetForeColor(element, controlStyle.ForeColor);
		if (element is IStiDashboard)
		{
			stiControlColorCollection.BackColor = StiTransparentColor.Fix(StiDashboardStyleHelper.GetDashboardBackColor(element as IStiDashboard, isViewer: true));
		}
		else
		{
			stiControlColorCollection.BackColor = StiTransparentColor.Fix(StiDashboardStyleHelper.GetBackColor(element, controlStyle.BackColor));
		}
		stiControlColorCollection.GlyphColor = controlStyle.GlyphColor;
		stiControlColorCollection.SelectedForeColor = controlStyle.SelectedForeColor;
		stiControlColorCollection.SelectedBackColor = controlStyle.SelectedBackColor;
		stiControlColorCollection.SelectedGlyphColor = controlStyle.SelectedGlyphColor;
		stiControlColorCollection.HotForeColor = controlStyle.HotForeColor;
		stiControlColorCollection.HotBackColor = controlStyle.HotBackColor;
		stiControlColorCollection.HotGlyphColor = controlStyle.HotGlyphColor;
		stiControlColorCollection.HotSelectedForeColor = controlStyle.HotSelectedForeColor;
		stiControlColorCollection.HotSelectedBackColor = controlStyle.HotSelectedBackColor;
		stiControlColorCollection.HotSelectedGlyphColor = controlStyle.HotSelectedGlyphColor;
		if (element is IStiDashboard && flag)
		{
			if (StiDashboardStyleHelper.GetStyle(element) == StiElementStyleIdent.DarkTurquoise)
			{
				stiControlColorCollection.SelectedBackColor = StiColor.Get("113344");
				stiControlColorCollection.HotBackColor = StiColor.Get("235e6d");
				stiControlColorCollection.HotSelectedBackColor = StiColor.Get("#f0621e");
			}
			else
			{
				stiControlColorCollection.SelectedBackColor = StiColor.Get("414141");
				stiControlColorCollection.HotBackColor = StiColor.Get("1f1f1f");
				stiControlColorCollection.HotSelectedBackColor = StiColor.Get("494949");
			}
		}
		stiControlColorCollection.SeparatorColor = controlStyle.SeparatorColor;
		stiControlColorCollection.IsDarkMode = flag;
		return stiControlColorCollection;
	}

	public Color GetForeColor(bool isSelected, bool isHot)
	{
		if (isSelected && isHot)
		{
			return HotSelectedForeColor;
		}
		if (isSelected)
		{
			return SelectedForeColor;
		}
		if (isHot)
		{
			return HotForeColor;
		}
		return ForeColor;
	}

	public Color GetBackColor(bool isSelected, bool isHot)
	{
		if (isSelected && isHot)
		{
			return HotSelectedBackColor;
		}
		if (isSelected)
		{
			return SelectedBackColor;
		}
		if (isHot)
		{
			return HotBackColor;
		}
		return BackColor;
	}

	public Color GetGlyphColor(bool isSelected, bool isHot)
	{
		if (isSelected && isHot)
		{
			return HotSelectedGlyphColor;
		}
		if (isSelected)
		{
			return SelectedGlyphColor;
		}
		if (isHot)
		{
			return HotGlyphColor;
		}
		return GlyphColor;
	}

	public StiControlColorCollection(Control control = null)
	{
		Control = control;
	}
}
