using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Components;

public class StiFontEditorForm : Form
{
	private class StiFontStyleInfo
	{
		public string Name { get; }

		public FontStyle Style { get; }

		public override string ToString()
		{
			return Name;
		}

		public StiFontStyleInfo(string name, FontStyle style)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			Style = style;
			Name = name;
		}
	}

	private StiReport report;

	private bool isLocked;

	private List<string> recentFontNames = new List<string>();

	private List<string> resourceFontNames = new List<string>();

	private int maxRecentFontsCount = 3;

	private IContainer components;

	private Label labelFont;

	private Label labelFontStyle;

	private Label labelSize;

	private StiTextBox textBoxFont;

	private StiTextBox textBoxFontStyle;

	private StiTextBox textBoxSize;

	private ListBox listBoxFont;

	private ListBox listBoxFontStyle;

	private ListBox listBoxSize;

	private GroupBox groupBoxSample;

	private GroupBox groupBoxEffects;

	private StiGroupLine stiGroupLine1;

	private Button btCancel;

	private Button btOk;

	private Label labelSample;

	private CheckBox checkBoxUnderline;

	private CheckBox checkBoxStrikeout;

	public Font SelectedFont { get; set; }

	public StiFontEditorForm(Font font, StiReport report)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			this.report = report;
			SelectedFont = (Font)((font != null) ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new Font("Arial", 8f)));
			isLocked = true;
			Build();
			isLocked = false;
			((Control)labelSample).Font = new Font(SelectedFont, GetSelectedFontStyle());
			UpdateFont();
			((Control)textBoxFont).GotFocus += TextBoxFont_GotFocus;
			((Control)textBoxFont).LostFocus += TextBoxFont_LostFocus;
			((Control)textBoxFontStyle).GotFocus += TextBoxFontStyle_GotFocus;
			((Control)textBoxFontStyle).LostFocus += TextBoxFontStyle_LostFocus;
			listBoxFont.ItemHeight = StiScale.YYI(listBoxFont.ItemHeight);
			listBoxFontStyle.ItemHeight = StiScale.YYI(listBoxFontStyle.ItemHeight);
			listBoxSize.ItemHeight = StiScale.YYI(listBoxSize.ItemHeight);
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)labelFont).Text = StiLocalization.Get("PropertyMain", "Font") ?? "";
		((Control)labelFontStyle).Text = StiLocalization.Get("PropertyMain", "Style") ?? "";
		((Control)labelSize).Text = StiLocalization.Get("PropertyMain", "Size") ?? "";
		((Control)groupBoxEffects).Text = StiLocalization.Get("PropertyMain", "Effects");
		((Control)groupBoxSample).Text = StiLocalization.Get("FormFormatEditor", "Sample");
		((Control)checkBoxStrikeout).Text = StiLocalization.Get("PropertyMain", "Strikeout");
		((Control)checkBoxUnderline).Text = StiLocalization.Get("PropertyMain", "Underline");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
	}

	private void LoadResourceFonts()
	{
		resourceFontNames.Clear();
		if (report == null)
		{
			return;
		}
		List<StiResource> allFonts = StiFontResourceHelper.GetAllFonts(report);
		foreach (StiResource item in allFonts)
		{
			string resourceFontName = report.GetResourceFontName(item.Name);
			resourceFontNames.Add(resourceFontName);
		}
		FontFamily[] families = ((FontCollection)StiFontCollection.Instance).Families;
		foreach (FontFamily val in families)
		{
			resourceFontNames.Add(val.Name);
		}
	}

	private void LoadRecentFonts()
	{
		recentFontNames.Clear();
		StiSettings.Load();
		for (int i = 0; i < maxRecentFontsCount; i++)
		{
			string str = StiSettings.GetStr("StiDesigner", "RecentFont" + i, null);
			foreach (string resourceFontName in resourceFontNames)
			{
				if (resourceFontName.Equals(str))
				{
					recentFontNames.Add(str);
				}
			}
			FontFamily[] families = FontFamily.Families;
			foreach (FontFamily val in families)
			{
				if (val.Name.Equals(str))
				{
					recentFontNames.Add(val.Name);
				}
			}
		}
	}

	private void SaveRecentFonts()
	{
		recentFontNames.Insert(0, ((Control)textBoxFont).Text);
		List<string> list = new List<string>();
		foreach (string recentFontName in recentFontNames)
		{
			if (!list.Contains(recentFontName))
			{
				list.Add(recentFontName);
			}
		}
		StiSettings.Load();
		for (int i = 0; i < maxRecentFontsCount; i++)
		{
			if (i < list.Count)
			{
				StiSettings.Set("StiDesigner", "RecentFont" + i, list[i]);
			}
			else
			{
				StiSettings.Set("StiDesigner", "RecentFont" + i, string.Empty);
			}
		}
		StiSettings.Save();
	}

	private void Build()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		LoadResourceFonts();
		LoadRecentFonts();
		FillFonts();
		FillFontStyle();
		SetSelectedStyle(SelectedFont.Style);
		((Control)textBoxFont).Text = SelectedFont.FontFamily.Name;
		((Control)textBoxSize).Text = SelectedFont.Size.ToString(CultureInfo.CurrentCulture);
		checkBoxStrikeout.Checked = SelectedFont.Strikeout;
		checkBoxUnderline.Checked = SelectedFont.Underline;
	}

	private void FillFonts()
	{
		int num = 0;
		listBoxFont.Items.Clear();
		if (recentFontNames.Count > 0)
		{
			listBoxFont.Items.Add((object)"Recent");
			num++;
			foreach (string recentFontName in recentFontNames)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(recentFontName))
					{
						listBoxFont.Items.Add((object)recentFontName);
						if (SelectedFont.Name.Equals(recentFontName))
						{
							((ListControl)listBoxFont).SelectedIndex = num;
						}
						num++;
					}
				}
				catch
				{
				}
			}
		}
		if (resourceFontNames.Count > 0)
		{
			listBoxFont.Items.Add((object)"Custom");
			num++;
			foreach (string resourceFontName in resourceFontNames)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(resourceFontName))
					{
						listBoxFont.Items.Add((object)resourceFontName);
						if (SelectedFont.Name.Equals(resourceFontName))
						{
							((ListControl)listBoxFont).SelectedIndex = num;
						}
						num++;
					}
				}
				catch
				{
				}
			}
		}
		listBoxFont.Items.Add((object)"Fonts");
		FontFamily[] families = FontFamily.Families;
		foreach (FontFamily val in families)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace((val != null) ? val.Name : null))
				{
					num++;
					listBoxFont.Items.Add((object)val.Name);
					if (SelectedFont.Name.Equals(val.Name))
					{
						((ListControl)listBoxFont).SelectedIndex = num;
					}
				}
			}
			catch
			{
			}
		}
	}

	private void FillFontStyle()
	{
		listBoxFontStyle.Items.Clear();
		Font selectedFont = SelectedFont;
		if (selectedFont != null)
		{
			if (selectedFont.FontFamily.IsStyleAvailable((FontStyle)0))
			{
				listBoxFontStyle.Items.Add((object)new StiFontStyleInfo(StiLocalization.Get("PropertyMain", "Regular"), (FontStyle)0));
			}
			if (selectedFont.FontFamily.IsStyleAvailable((FontStyle)2))
			{
				listBoxFontStyle.Items.Add((object)new StiFontStyleInfo(StiLocalization.Get("PropertyMain", "Italic"), (FontStyle)2));
			}
			if (selectedFont.FontFamily.IsStyleAvailable((FontStyle)1))
			{
				listBoxFontStyle.Items.Add((object)new StiFontStyleInfo(StiLocalization.Get("PropertyMain", "Bold"), (FontStyle)1));
			}
			if (selectedFont.FontFamily.IsStyleAvailable((FontStyle)1) && selectedFont.FontFamily.IsStyleAvailable((FontStyle)2))
			{
				listBoxFontStyle.Items.Add((object)new StiFontStyleInfo(StiLocalization.Get("PropertyMain", "Regular") + " " + StiLocalization.Get("PropertyMain", "Italic"), (FontStyle)3));
			}
		}
	}

	private void SetSelectedStyle(FontStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if ((style & 8) != 0)
		{
			style = (FontStyle)(style ^ 8);
		}
		if ((style & 4) != 0)
		{
			style = (FontStyle)(style ^ 4);
		}
		int num = 0;
		foreach (object item in listBoxFontStyle.Items)
		{
			FontStyle style2 = ((StiFontStyleInfo)item).Style;
			if (style == style2)
			{
				((ListControl)listBoxFontStyle).SelectedIndex = num;
				break;
			}
			num++;
		}
	}

	private FontStyle GetSelectedStyle()
	{
		string text = listBoxFontStyle.SelectedItem.ToString();
		if (!text.Equals(StiLocalization.Get("PropertyMain", "Regular")))
		{
			if (!text.Equals(StiLocalization.Get("PropertyMain", "Italic")))
			{
				if (!text.Equals(StiLocalization.Get("PropertyMain", "Bold")))
				{
					if (!text.Equals(StiLocalization.Get("PropertyMain", "Bold") + " " + StiLocalization.Get("PropertyMain", "Italic")))
					{
						return (FontStyle)0;
					}
					return (FontStyle)3;
				}
				return (FontStyle)1;
			}
			return (FontStyle)2;
		}
		return (FontStyle)0;
	}

	private void UpdateFont()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (isLocked || listBoxFontStyle.SelectedItem == null)
		{
			return;
		}
		try
		{
			Font font = GetFont(((Control)textBoxFont).Text);
			float selectedFontSize = GetSelectedFontSize();
			FontStyle selectedFontStyle = GetSelectedFontStyle();
			SelectedFont.Dispose();
			SelectedFont = new Font(font.FontFamily, selectedFontSize, selectedFontStyle);
			((Control)labelSample).Invalidate();
		}
		catch
		{
		}
	}

	private FontStyle GetSelectedFontStyle()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (listBoxFontStyle.SelectedItem == null)
		{
			return (FontStyle)0;
		}
		FontStyle val = ((StiFontStyleInfo)listBoxFontStyle.SelectedItem).Style;
		if (checkBoxUnderline.Checked)
		{
			val = (FontStyle)(val | 4);
		}
		if (checkBoxStrikeout.Checked)
		{
			val = (FontStyle)(val | 8);
		}
		return val;
	}

	private float GetSelectedFontSize()
	{
		if (!float.TryParse(((Control)textBoxSize).Text, out var result) || !(result > 0f))
		{
			return 12f;
		}
		return result;
	}

	private Font GetFont(string fontName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		return new Font(StiFontCollection.GetFontFamily(fontName), GetSelectedFontSize(), GetSelectedFontStyle());
	}

	private void SetSelectedItemFont()
	{
		if (!listBoxFont.SelectedItem.ToString().Equals("Recent") && !listBoxFont.SelectedItem.ToString().Equals("Custom") && !listBoxFont.SelectedItem.ToString().Equals("Fonts") && listBoxFont.SelectedItem != null)
		{
			((Control)textBoxFont).Text = listBoxFont.SelectedItem.ToString();
			FillFontStyle();
			SetSelectedStyle((FontStyle)0);
			UpdateFont();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 70
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)textBoxFont).Focus();
			textBoxFont.Select(((Control)textBoxFont).Text.Length, 0);
			((Component)(object)timer).Dispose();
		};
		timer.Start();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		SaveRecentFonts();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void ListBoxFont_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Invalid comparison between Unknown and I4
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		object obj = listBoxFont.Items[e.Index];
		Brush black = Brushes.Black;
		if (obj.ToString().Equals("Recent"))
		{
			SolidBrush val = new SolidBrush(Color.FromArgb(238, 238, 238));
			try
			{
				graphics.FillRectangle((Brush)(object)val, e.Bounds);
				graphics.DrawString(Loc.Get("PropertyMain", "RecentFonts"), Control.DefaultFont, black, (float)(e.Bounds.X + StiScale.I3), (float)(e.Bounds.Y + StiScale.I1), StringFormat.GenericDefault);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (obj.ToString().Equals("Custom"))
		{
			SolidBrush val2 = new SolidBrush(Color.FromArgb(238, 238, 238));
			try
			{
				graphics.FillRectangle((Brush)(object)val2, e.Bounds);
				graphics.DrawString(Loc.Get("PropertyMain", "CustomFonts"), Control.DefaultFont, black, (float)(e.Bounds.X + StiScale.I3), (float)(e.Bounds.Y + StiScale.I1), StringFormat.GenericDefault);
				return;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (obj.ToString().Equals("Fonts"))
		{
			SolidBrush val3 = new SolidBrush(Color.FromArgb(238, 238, 238));
			try
			{
				graphics.FillRectangle((Brush)(object)val3, e.Bounds);
				graphics.DrawString(Loc.Get("PropertyMain", "SystemFonts"), Control.DefaultFont, black, (float)(e.Bounds.X + StiScale.I3), (float)(e.Bounds.Y + StiScale.I1), StringFormat.GenericDefault);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		black = (((e.State & 1) > 0) ? Brushes.White : Brushes.Black);
		e.DrawBackground();
		Font val4 = new Font("Microsoft Sans Serif", 9f);
		try
		{
			graphics.DrawString(obj.ToString(), val4, black, (RectangleF)e.Bounds, StringFormat.GenericDefault);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		e.DrawFocusRectangle();
	}

	private void ListBoxFont_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetSelectedItemFont();
	}

	private void ListBoxFont_MouseClick(object sender, MouseEventArgs e)
	{
		SetSelectedItemFont();
	}

	private void ListBoxFontStyle_MouseClick(object sender, MouseEventArgs e)
	{
		if (listBoxFontStyle.SelectedItem != null)
		{
			((Control)textBoxFontStyle).Text = ((StiFontStyleInfo)listBoxFontStyle.SelectedItem).Name;
			UpdateFont();
		}
	}

	private void ListBoxSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (listBoxSize.SelectedItem != null)
		{
			((Control)textBoxSize).Text = listBoxSize.SelectedItem.ToString();
		}
	}

	private void TextBoxSize_KeyPress(object sender, KeyPressEventArgs e)
	{
		char c = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != c)
		{
			e.Handled = true;
		}
		if (e.KeyChar == c && ((Control)(sender as StiTextBox)).Text.IndexOf(c) > -1)
		{
			e.Handled = true;
		}
	}

	private void TextBoxSize_TextChanged(object sender, EventArgs e)
	{
		if (listBoxSize.Items.Contains((object)((Control)textBoxSize).Text))
		{
			listBoxSize.SelectedItem = ((Control)textBoxSize).Text;
		}
		else
		{
			listBoxSize.ClearSelected();
		}
		UpdateFont();
	}

	private void ListBoxFontStyle_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxFontStyle).Text = ((StiFontStyleInfo)listBoxFontStyle.SelectedItem).Name;
		UpdateFont();
	}

	private void CheckBoxStrikeout_CheckedChanged(object sender, EventArgs e)
	{
		UpdateFont();
	}

	private void CheckBoxUnderline_CheckedChanged(object sender, EventArgs e)
	{
		UpdateFont();
	}

	private void TextBoxSize_Leave(object sender, EventArgs e)
	{
		if (!float.TryParse(((Control)textBoxSize).Text, out var result) || result == 0f)
		{
			((Control)textBoxSize).Text = "12";
		}
	}

	private void TextBoxFontStyle_Leave(object sender, EventArgs e)
	{
		if (listBoxFontStyle.SelectedItem is StiFontStyleInfo)
		{
			((Control)textBoxFontStyle).Text = ((StiFontStyleInfo)listBoxFontStyle.SelectedItem).Name;
		}
	}

	private void LabelSample_Paint(object sender, PaintEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		object obj = SelectedFont.Clone();
		Font val = (Font)((obj is Font) ? obj : null);
		try
		{
			StringFormat val2 = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			e.Graphics.DrawString("AaBbCc", val, Brushes.Black, (RectangleF)e.ClipRectangle, val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TextBoxFont_TextChanged(object sender, EventArgs e)
	{
		int num = -1;
		for (int i = 0; i < listBoxFont.Items.Count; i++)
		{
			string text = listBoxFont.Items[i].ToString();
			if (text.ToLower().Contains(((Control)textBoxFont).Text.ToLower()))
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			((ListControl)listBoxFont).SelectedIndex = num;
		}
	}

	private void textBoxFontStyle_TextChanged(object sender, EventArgs e)
	{
		int num = -1;
		for (int i = 0; i < listBoxFontStyle.Items.Count; i++)
		{
			string text = listBoxFontStyle.Items[i].ToString();
			if (text.ToLower().Contains(((Control)textBoxFontStyle).Text.ToLower()))
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			((ListControl)listBoxFontStyle).SelectedIndex = num;
		}
	}

	private void TextBoxFont_LostFocus(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		listBoxFont.SelectedIndexChanged += ListBoxFont_SelectedIndexChanged;
		listBoxFont.MouseClick -= new MouseEventHandler(ListBoxFont_MouseClick);
	}

	private void TextBoxFont_GotFocus(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		listBoxFont.SelectedIndexChanged -= ListBoxFont_SelectedIndexChanged;
		listBoxFont.MouseClick += new MouseEventHandler(ListBoxFont_MouseClick);
	}

	private void TextBoxFontStyle_LostFocus(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		listBoxFontStyle.SelectedIndexChanged += ListBoxFontStyle_SelectedIndexChanged;
		listBoxFontStyle.MouseClick -= new MouseEventHandler(ListBoxFontStyle_MouseClick);
	}

	private void TextBoxFontStyle_GotFocus(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		listBoxFontStyle.SelectedIndexChanged -= ListBoxFontStyle_SelectedIndexChanged;
		listBoxFontStyle.MouseClick += new MouseEventHandler(ListBoxFontStyle_MouseClick);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
		labelFont = new Label();
		labelFontStyle = new Label();
		labelSize = new Label();
		textBoxFont = new StiTextBox();
		textBoxFontStyle = new StiTextBox();
		textBoxSize = new StiTextBox();
		listBoxFont = new ListBox();
		listBoxFontStyle = new ListBox();
		listBoxSize = new ListBox();
		groupBoxSample = new GroupBox();
		labelSample = new Label();
		groupBoxEffects = new GroupBox();
		checkBoxUnderline = new CheckBox();
		checkBoxStrikeout = new CheckBox();
		btCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		btOk = new Button();
		((Control)groupBoxSample).SuspendLayout();
		((Control)groupBoxEffects).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelFont).AutoSize = true;
		((Control)labelFont).ForeColor = Color.DimGray;
		((Control)labelFont).Location = new Point(12, 11);
		((Control)labelFont).Name = "labelFont";
		((Control)labelFont).Size = new Size(28, 13);
		((Control)labelFont).TabIndex = 0;
		((Control)labelFont).Text = "Font";
		((Control)labelFontStyle).AutoSize = true;
		((Control)labelFontStyle).ForeColor = Color.DimGray;
		((Control)labelFontStyle).Location = new Point(282, 10);
		((Control)labelFontStyle).Name = "labelFontStyle";
		((Control)labelFontStyle).Size = new Size(51, 13);
		((Control)labelFontStyle).TabIndex = 1;
		((Control)labelFontStyle).Text = "FontStyle";
		((Control)labelSize).AutoSize = true;
		((Control)labelSize).ForeColor = Color.DimGray;
		((Control)labelSize).Location = new Point(415, 10);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(27, 13);
		((Control)labelSize).TabIndex = 2;
		((Control)labelSize).Text = "Size";
		((Control)textBoxFont).BackColor = Color.White;
		((Control)textBoxFont).Location = new Point(12, 30);
		((Control)textBoxFont).Name = "textBoxFont";
		((Control)textBoxFont).Size = new Size(258, 21);
		((Control)textBoxFont).TabIndex = 0;
		((Control)textBoxFont).TextChanged += TextBoxFont_TextChanged;
		((Control)textBoxFontStyle).BackColor = Color.White;
		((Control)textBoxFontStyle).Location = new Point(285, 30);
		((Control)textBoxFontStyle).Name = "textBoxFontStyle";
		((Control)textBoxFontStyle).Size = new Size(118, 21);
		((Control)textBoxFontStyle).TabIndex = 1;
		((Control)textBoxFontStyle).TextChanged += textBoxFontStyle_TextChanged;
		((Control)textBoxFontStyle).Leave += TextBoxFontStyle_Leave;
		((Control)textBoxSize).BackColor = Color.White;
		((Control)textBoxSize).Location = new Point(418, 30);
		((Control)textBoxSize).Name = "textBoxSize";
		((Control)textBoxSize).Size = new Size(93, 21);
		((Control)textBoxSize).TabIndex = 2;
		((Control)textBoxSize).TextChanged += TextBoxSize_TextChanged;
		((Control)textBoxSize).KeyPress += new KeyPressEventHandler(TextBoxSize_KeyPress);
		((Control)textBoxSize).Leave += TextBoxSize_Leave;
		((Control)listBoxFont).Anchor = (AnchorStyles)7;
		listBoxFont.DrawMode = (DrawMode)1;
		listBoxFont.IntegralHeight = false;
		listBoxFont.ItemHeight = 16;
		((Control)listBoxFont).Location = new Point(12, 62);
		((Control)listBoxFont).Name = "listBoxFont";
		((Control)listBoxFont).Size = new Size(260, 180);
		((Control)listBoxFont).TabIndex = 3;
		listBoxFont.MouseClick += new MouseEventHandler(ListBoxFont_MouseClick);
		listBoxFont.DrawItem += new DrawItemEventHandler(ListBoxFont_DrawItem);
		listBoxFont.SelectedIndexChanged += ListBoxFont_SelectedIndexChanged;
		((Control)listBoxFontStyle).Anchor = (AnchorStyles)7;
		((Control)listBoxFontStyle).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ListControl)listBoxFontStyle).FormattingEnabled = true;
		listBoxFontStyle.IntegralHeight = false;
		listBoxFontStyle.ItemHeight = 15;
		((Control)listBoxFontStyle).Location = new Point(285, 62);
		((Control)listBoxFontStyle).Name = "listBoxFontStyle";
		((Control)listBoxFontStyle).Size = new Size(120, 180);
		((Control)listBoxFontStyle).TabIndex = 4;
		listBoxFontStyle.MouseClick += new MouseEventHandler(ListBoxFontStyle_MouseClick);
		listBoxFontStyle.SelectedIndexChanged += ListBoxFontStyle_SelectedIndexChanged;
		((Control)listBoxSize).Anchor = (AnchorStyles)7;
		((Control)listBoxSize).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ListControl)listBoxSize).FormattingEnabled = true;
		listBoxSize.IntegralHeight = false;
		listBoxSize.ItemHeight = 15;
		listBoxSize.Items.AddRange(new object[16]
		{
			"8", "9", "10", "11", "12", "14", "16", "18", "20", "22",
			"24", "26", "28", "36", "48", "72"
		});
		((Control)listBoxSize).Location = new Point(418, 62);
		((Control)listBoxSize).Name = "listBoxSize";
		((Control)listBoxSize).Size = new Size(95, 180);
		((Control)listBoxSize).TabIndex = 5;
		listBoxSize.SelectedIndexChanged += ListBoxSize_SelectedIndexChanged;
		((Control)groupBoxSample).Anchor = (AnchorStyles)6;
		((Control)groupBoxSample).Controls.Add((Control)(object)labelSample);
		((Control)groupBoxSample).Location = new Point(285, 254);
		((Control)groupBoxSample).Name = "groupBoxSample";
		((Control)groupBoxSample).Size = new Size(228, 105);
		((Control)groupBoxSample).TabIndex = 7;
		groupBoxSample.TabStop = false;
		((Control)groupBoxSample).Text = "Sample";
		((Control)labelSample).Anchor = (AnchorStyles)15;
		((Control)labelSample).Location = new Point(6, 16);
		((Control)labelSample).MaximumSize = new Size(208, 77);
		((Control)labelSample).Name = "labelSample";
		((Control)labelSample).Size = new Size(208, 77);
		((Control)labelSample).TabIndex = 0;
		labelSample.TextAlign = (ContentAlignment)32;
		((Control)labelSample).Paint += new PaintEventHandler(LabelSample_Paint);
		((Control)groupBoxEffects).Anchor = (AnchorStyles)6;
		((Control)groupBoxEffects).Controls.Add((Control)(object)checkBoxUnderline);
		((Control)groupBoxEffects).Controls.Add((Control)(object)checkBoxStrikeout);
		((Control)groupBoxEffects).Location = new Point(13, 254);
		((Control)groupBoxEffects).Name = "groupBoxEffects";
		((Control)groupBoxEffects).Size = new Size(259, 105);
		((Control)groupBoxEffects).TabIndex = 6;
		groupBoxEffects.TabStop = false;
		((Control)groupBoxEffects).Text = "Effects";
		((Control)checkBoxUnderline).AutoSize = true;
		((Control)checkBoxUnderline).Location = new Point(12, 50);
		((Control)checkBoxUnderline).Name = "checkBoxUnderline";
		((Control)checkBoxUnderline).Size = new Size(71, 17);
		((Control)checkBoxUnderline).TabIndex = 1;
		((Control)checkBoxUnderline).Text = "Underline";
		((ButtonBase)checkBoxUnderline).UseVisualStyleBackColor = true;
		checkBoxUnderline.CheckedChanged += CheckBoxUnderline_CheckedChanged;
		((Control)checkBoxStrikeout).AutoSize = true;
		((Control)checkBoxStrikeout).Location = new Point(12, 24);
		((Control)checkBoxStrikeout).Name = "checkBoxStrikeout";
		((Control)checkBoxStrikeout).Size = new Size(68, 17);
		((Control)checkBoxStrikeout).TabIndex = 0;
		((Control)checkBoxStrikeout).Text = "Strikeout";
		((ButtonBase)checkBoxStrikeout).UseVisualStyleBackColor = true;
		checkBoxStrikeout.CheckedChanged += CheckBoxStrikeout_CheckedChanged;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(439, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).Anchor = (AnchorStyles)14;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 372);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(526, 46);
		((Control)stiGroupLine1).TabIndex = 8;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(358, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(526, 418);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)groupBoxEffects);
		((Control)this).Controls.Add((Control)(object)groupBoxSample);
		((Control)this).Controls.Add((Control)(object)listBoxSize);
		((Control)this).Controls.Add((Control)(object)listBoxFontStyle);
		((Control)this).Controls.Add((Control)(object)listBoxFont);
		((Control)this).Controls.Add((Control)(object)textBoxSize);
		((Control)this).Controls.Add((Control)(object)textBoxFontStyle);
		((Control)this).Controls.Add((Control)(object)textBoxFont);
		((Control)this).Controls.Add((Control)(object)labelSize);
		((Control)this).Controls.Add((Control)(object)labelFontStyle);
		((Control)this).Controls.Add((Control)(object)labelFont);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiFontEditorForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiFontEditorForm";
		((Form)this).Load += This_Load;
		((Control)groupBoxSample).ResumeLayout(false);
		((Control)groupBoxEffects).ResumeLayout(false);
		((Control)groupBoxEffects).PerformLayout();
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
