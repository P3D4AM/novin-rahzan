using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.ColorPickerItem;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiColorSelectionControl : Control
{
	private const int MaxRecentColorsCount = 10;

	private string propName;

	private object instance;

	private Color foregroundTitle = (StiUX.IsDark ? StiUX.Foreground : Color.Gray);

	private bool isExpression;

	private string colorFocus;

	private Hashtable colorRectanglesHash = new Hashtable();

	private Rectangle transparentRectangle;

	private bool transparentFocus;

	private Rectangle moreColorsRectangle;

	private bool moreColorsFocus;

	private Rectangle editInsertExpressionRectangle;

	private bool editInsertExpressionFocus;

	private List<string> recentColors = new List<string>();

	private int sizeItemColor = StiScale.I12;

	private int marginStep = StiScale.I4;

	private ToolTip tooltTip;

	private bool isSelected;

	private bool isFocused;

	private Color selectedColor;

	private string expression;

	private string LocThemeColors => Loc.Get("Wizards", "Themes");

	private string LocStandardColors => Loc.Get("Gui", "colorpickerdialog_tabstandard");

	private string LocNoFill => StiLocalization.Get("PropertyColor", "Transparent");

	private string LocFromStyle => StiLocalization.Get("FormStyleDesigner", "FromStyle");

	private string LocMoreColors => Loc.Get("Gui", "colorpicker_morecolors").Replace("&", "").Replace(".", "");

	private string LocRecent => Loc.Get("FormDatabaseEdit", "RecentConnections");

	private string LocExpression => Loc.GetMain("Expression");

	private string LocEditExpression => Loc.Get("Dashboard", "EditExpression");

	private string LocNewExpression => Loc.Get("FormRichTextEditor", "Insert");

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return Color.White;
			}
			return StiUX.ButtonBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	public Color SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			selectedColor = value;
			colorFocus = ColorTranslator.ToHtml(value);
			StiAppExpressionHelper.RemoveExpression(instance, propName);
			expression = null;
			InvokeColorChanged();
		}
	}

	public string SelectedColotText => ColorTranslator.ToHtml(SelectedColor);

	public string Expression
	{
		get
		{
			return expression;
		}
		set
		{
			StiAppExpressionHelper.SetExpression(instance, propName, value);
			expression = value;
			InvokeExpressionChanged();
		}
	}

	public bool IsFromStyle { get; set; }

	public event EventHandler ColorChanged;

	public event EventHandler ExpressionChanged;

	public StiColorSelectionControl(Color color, bool isExpression, ITypeDescriptorContext context)
	{
		((Control)this).ResizeRedraw = true;
		((Control)this).DoubleBuffered = true;
		selectedColor = color;
		this.isExpression = isExpression;
		if (isExpression)
		{
			propName = context.PropertyDescriptor.Name;
			instance = context.Instance;
			expression = StiAppExpressionHelper.GetExpressionValue(instance, propName);
		}
		InitializeRecentColors();
	}

	protected void InvokeColorChanged()
	{
		this.ColorChanged?.Invoke(this, EventArgs.Empty);
	}

	protected void InvokeExpressionChanged()
	{
		this.ExpressionChanged?.Invoke(this, EventArgs.Empty);
	}

	private void InitializeRecentColors()
	{
		StiSettings.Load();
		for (int i = 0; i < 10; i++)
		{
			string str = StiSettings.GetStr("StiDesigner", "RecentColor" + i, null);
			if (!string.IsNullOrEmpty(str))
			{
				recentColors.Add(str);
			}
		}
	}

	private void SaveRecentColor(Color color)
	{
		string colorText = ColorTranslator.ToHtml(color);
		recentColors = recentColors.Where((string c) => c != colorText).ToList();
		recentColors.Insert(0, colorText);
		StiSettings.Load();
		for (int i = 0; i < 10; i++)
		{
			if (i < recentColors.Count)
			{
				StiSettings.Set("StiDesigner", "RecentColor" + i, recentColors[i]);
			}
			else
			{
				StiSettings.Set("StiDesigner", "RecentColor" + i, string.Empty);
			}
		}
		StiSettings.Save();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		SetFocusColor(e.Location.X, e.Location.Y);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		colorFocus = null;
		moreColorsFocus = false;
		transparentFocus = false;
		editInsertExpressionFocus = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		SetSelectedColor(e.Location.X, e.Location.Y);
		CheckMoreColorsClick(e.Location.X, e.Location.Y);
		CheckEditInsertExpressionClick(e.Location.X, e.Location.Y);
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetBoundsCore(x, y, width, GetHeight(), specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		Build(e.Graphics);
	}

	private void Build(Graphics g)
	{
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		int num = 0;
		int num2 = 0;
		isSelected = false;
		isFocused = false;
		int x = num;
		int num3 = num2;
		Rectangle rectTitle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
		DrawItemTitle(g, rectTitle, LocThemeColors);
		num3 += rectTitle.Height + marginStep;
		ArrayList themeColors = GetThemeColors();
		for (int i = 0; i < themeColors.Count; i++)
		{
			object obj = themeColors[i];
			x = num + marginStep;
			if (i == 1)
			{
				num3 += marginStep;
			}
			string[] array = (string[])obj;
			foreach (string colorText in array)
			{
				DrawColorRect(g, new Rectangle(x, num3, sizeItemColor, sizeItemColor), colorText);
				x += sizeItemColor + marginStep;
			}
			num3 += sizeItemColor;
		}
		x = num;
		num3 += marginStep;
		rectTitle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
		DrawItemTitle(g, rectTitle, LocStandardColors);
		num3 += rectTitle.Height;
		string[] standardColors = GetStandardColors();
		x = num + marginStep;
		num3 += marginStep;
		string[] array2 = standardColors;
		foreach (string colorText2 in array2)
		{
			DrawColorRect(g, new Rectangle(x, num3, sizeItemColor, sizeItemColor), colorText2);
			x += sizeItemColor + marginStep;
		}
		num3 += sizeItemColor + marginStep;
		List<string> list = GetRecentColors();
		if (list.Count > 0)
		{
			x = num;
			rectTitle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
			DrawItemTitle(g, rectTitle, LocRecent);
			num3 += rectTitle.Height;
			x = num + marginStep;
			num3 += marginStep;
			foreach (string item in list)
			{
				DrawColorRect(g, new Rectangle(x, num3, sizeItemColor, sizeItemColor), item);
				x += sizeItemColor + marginStep;
			}
			num3 += sizeItemColor + marginStep;
		}
		x = num;
		DrawSeparator(g, x, num3);
		num3++;
		transparentRectangle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
		string text = ColorTranslator.ToHtml(Color.Transparent);
		SetColotToHash(transparentRectangle, text);
		DrawFocusItem(g, transparentRectangle, transparentFocus);
		Rectangle rectangle = new Rectangle(x + marginStep, num3 + marginStep, sizeItemColor, sizeItemColor);
		if (SelectedColotText.Equals(text))
		{
			DrawSelectedRect(g, rectangle);
		}
		else
		{
			Pen val = new Pen(StiUX.IsDark ? StiUX.ButtonBorder : Color.LightGray);
			try
			{
				g.DrawRectangle(val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Rectangle rectItem = new Rectangle(rectangle.Right + marginStep, transparentRectangle.Y, transparentRectangle.Right - rectangle.Right + marginStep, transparentRectangle.Height);
		string textItem = (IsFromStyle ? LocFromStyle : LocNoFill);
		DrawItemText(g, rectItem, textItem);
		num3 += transparentRectangle.Height;
		x = num;
		DrawSeparator(g, x, num3);
		num3++;
		x = num;
		moreColorsRectangle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
		DrawFocusItem(g, moreColorsRectangle, moreColorsFocus);
		Rectangle rectangle2 = new Rectangle(x + marginStep, num3 + marginStep, sizeItemColor, sizeItemColor);
		g.DrawImage(GetImageMoreColor(), rectangle2);
		rectItem = new Rectangle(rectangle2.Right + marginStep, rectangle2.Y, moreColorsRectangle.Right - rectangle2.Right + marginStep, rectangle2.Height);
		DrawItemText(g, rectItem, LocMoreColors);
		num3 += moreColorsRectangle.Height;
		if (isExpression)
		{
			x = num;
			if (!string.IsNullOrEmpty(expression))
			{
				rectTitle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
				DrawItemTitle(g, rectTitle, LocExpression);
				num3 += rectTitle.Height;
				Rectangle rect = new Rectangle(x + marginStep, num3 + marginStep, sizeItemColor, sizeItemColor);
				DrawSelectedRect(g, rect);
				rectItem = new Rectangle(rect.Right + marginStep, num3, GetWidth() - rect.Right + marginStep, StiScale.XXI(20.0));
				DrawItemText(g, rectItem, expression);
				num3 += rectItem.Height;
				num3++;
			}
			x = num;
			editInsertExpressionRectangle = new Rectangle(x, num3, GetWidth(), StiScale.XXI(20.0));
			DrawFocusItem(g, editInsertExpressionRectangle, editInsertExpressionFocus);
			string textItem2 = (string.IsNullOrEmpty(expression) ? LocNewExpression : LocEditExpression);
			DrawItemText(g, new Rectangle(GetItemTextX(), num3, GetWidth() - GetItemTextX(), StiScale.XXI(20.0)), textItem2);
			x = num;
			DrawSeparator(g, x, num3);
		}
	}

	private List<string> GetRecentColors()
	{
		List<string> list = new List<string>();
		if (!IsShowSelectedColor())
		{
			list.Add(SelectedColotText);
			list.AddRange(recentColors);
			if (list.Count > 10)
			{
				list.RemoveAt(list.Count - 1);
			}
		}
		else
		{
			list = recentColors;
		}
		return list;
	}

	private bool IsShowSelectedColor()
	{
		if (string.IsNullOrEmpty(SelectedColotText))
		{
			return true;
		}
		string[] standardColors = GetStandardColors();
		if (SelectedColotText == ColorTranslator.ToHtml(Color.Transparent))
		{
			return true;
		}
		string[] array = standardColors;
		foreach (string text in array)
		{
			if (text == SelectedColotText)
			{
				return true;
			}
		}
		ArrayList themeColors = GetThemeColors();
		foreach (string[] item in themeColors)
		{
			string[] array3 = item;
			foreach (string text2 in array3)
			{
				if (text2 == SelectedColotText)
				{
					return true;
				}
			}
		}
		foreach (string recentColor in recentColors)
		{
			if (recentColor == SelectedColotText)
			{
				return true;
			}
		}
		return false;
	}

	private void DrawSeparator(Graphics g, int x, int y)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		Pen val = new Pen(StiUX.IsDark ? StiUX.Separator : Color.LightGray, 1f);
		try
		{
			g.DrawLine(val, new Point(x, y), new Point(x + GetWidth(), y));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawFocusItem(Graphics g, Rectangle rect, bool isFocus)
	{
		if (isFocus)
		{
			StiDrawing.FillRectangle(g, StiUX.IsDark ? StiUX.ItemBackgroundHover : Color.FromArgb(224, 236, 255), rect.X, rect.Y, rect.Width + 1, rect.Height);
		}
	}

	private void DrawItemTitle(Graphics g, Rectangle rectTitle, string textTitle)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Color color = (StiUX.IsDark ? StiUX.BackgroundLight : StiColor.Get("f2f2f2"));
		StiDrawing.FillRectangle(g, color, rectTitle.X, rectTitle.Y, rectTitle.Width + 1, rectTitle.Height);
		Font val = new Font("Arial", 8f);
		try
		{
			TextFormatFlags val2 = (TextFormatFlags)4;
			TextRenderer.DrawText((IDeviceContext)(object)g, textTitle, val, rectTitle, foregroundTitle, val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawItemText(Graphics g, Rectangle rectItem, string textItem)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Font val = new Font("Arial", 8f);
		try
		{
			TextFormatFlags val2 = (TextFormatFlags)4;
			TextRenderer.DrawText((IDeviceContext)(object)g, textItem, val, rectItem, foregroundTitle, val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Image GetImageMoreColor()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		string text = "";
		if (StiScale2.Factor >= 4.0)
		{
			text = "_x4";
		}
		else if (StiScale2.Factor >= 3.0)
		{
			text = "_x3";
		}
		else if (StiScale2.Factor >= 2.0)
		{
			text = "_x2";
		}
		else if (StiScale2.Factor >= 1.5)
		{
			text = "_x1_5";
		}
		string text2 = "SystemImages.ColorPickerCustomColors" + text + ".png";
		return (Image)new Bitmap(typeof(DotNetBarManager).Assembly.GetManifestResourceStream("Stimulsoft.Controls.Win." + text2));
	}

	private void DrawColorRect(Graphics g, Rectangle rect, string colorText)
	{
		SetColotToHash(rect, colorText);
		Color color = ColorTranslator.FromHtml(colorText);
		StiDrawing.FillRectangle(g, color, rect);
		if (!isSelected && colorText.Equals(SelectedColotText) && !isExpression)
		{
			DrawSelectedRect(g, rect);
			isSelected = true;
		}
		else if (!isFocused && colorText.Equals(colorFocus) && rect.Contains(((Control)this).PointToClient(Cursor.Position)))
		{
			DrawSelectedRect(g, rect);
			isFocused = true;
		}
	}

	private void DrawSelectedRect(Graphics g, Rectangle rect)
	{
		StiDrawing.DrawRectangle(g, Color.Yellow, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3));
		StiDrawing.DrawRectangle(g, Color.Orange, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1));
	}

	private void SetColotToHash(Rectangle rect, string colorText)
	{
		if (!colorRectanglesHash.Contains(colorText))
		{
			List<Rectangle> value = new List<Rectangle> { rect };
			colorRectanglesHash[colorText] = value;
		}
		else
		{
			List<Rectangle> list = colorRectanglesHash[colorText] as List<Rectangle>;
			list.Add(rect);
		}
	}

	private ArrayList GetThemeColors()
	{
		ArrayList arrayList = new ArrayList();
		arrayList.Add(new string[10] { "#FFFFFF", "#000000", "#EEECE1", "#1F497D", "#4F81BD", "#C0504D", "#9BBB59", "#8064A2", "#4BACC6", "#F79646" });
		arrayList.Add(new string[10] { "#F2F2F2", "#7F7F7F", "#DDD9C3", "#C6D9F0", "#DBE5F1", "#F2DCDB", "#EBF1DD", "#E5E0EC", "#DBEEF3", "#FDEADA" });
		arrayList.Add(new string[10] { "#D8D8D8", "#595959", "#C4BD97", "#8DB3E2", "#B8CCE4", "#E5B9B7", "#D7E3BC", "#CCC1D9", "#B7DDE8", "#FBD5B5" });
		arrayList.Add(new string[10] { "#BFBFBF", "#3F3F3F", "#938953", "#548DD4", "#95B3D7", "#D99694", "#C3D69B", "#B2A1C7", "#92CDDC", "#FAC08F" });
		arrayList.Add(new string[10] { "#A5A5A5", "#262626", "#494429", "#17365D", "#366092", "#953734", "#76923C", "#5F497A", "#31859B", "#E36C09" });
		arrayList.Add(new string[10] { "#7F7F7F", "#0C0C0C", "#1D1B10", "#0F243E", "#244061", "#632423", "#4F6128", "#3F3151", "#205867", "#974806" });
		return arrayList;
	}

	private string[] GetStandardColors()
	{
		return new string[10] { "#C00000", "#FF0000", "#FFC000", "#FFFF00", "#92D050", "#00B050", "#00B0F0", "#0070C0", "#002060", "#7030A0" };
	}

	private int GetItemTextX()
	{
		return 2 * marginStep + sizeItemColor;
	}

	private int GetWidth()
	{
		return ((Control)this).Width;
	}

	public int GetHeight()
	{
		int num = StiScale.YYI(144.0);
		if (GetRecentColors().Count > 0)
		{
			num += StiScale.YYI(40.0);
		}
		num += StiScale.YYI(41.0);
		if (isExpression)
		{
			num += StiScale.YYI(20.0);
			if (!string.IsNullOrEmpty(expression))
			{
				num += StiScale.YYI(40.0);
			}
		}
		return num;
	}

	private void SetFocusColor(int xPoin, int yPoint)
	{
		Point pt = new Point(xPoin, yPoint);
		bool flag = false;
		moreColorsFocus = false;
		transparentFocus = false;
		editInsertExpressionFocus = false;
		string text = colorFocus;
		bool flag2 = false;
		foreach (string key in colorRectanglesHash.Keys)
		{
			List<Rectangle> list = colorRectanglesHash[key] as List<Rectangle>;
			foreach (Rectangle item in list)
			{
				if (item.Contains(xPoin, yPoint))
				{
					flag2 = true;
					flag = true;
					text = key;
					if (!transparentRectangle.Contains(pt))
					{
						ShowTooltip(key);
					}
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (text != colorFocus)
		{
			colorFocus = text;
			if (!transparentRectangle.Contains(pt))
			{
				ShowTooltip(text);
			}
			((Control)this).Invalidate();
			return;
		}
		if (!flag)
		{
			colorFocus = null;
			HideTooltip();
		}
		transparentFocus = transparentRectangle.Contains(pt);
		moreColorsFocus = moreColorsRectangle.Contains(pt);
		editInsertExpressionFocus = editInsertExpressionRectangle.Contains(pt);
		((Control)this).Invalidate();
	}

	private void SetSelectedColor(int xPoin, int yPoint)
	{
		foreach (string key in colorRectanglesHash.Keys)
		{
			List<Rectangle> list = colorRectanglesHash[key] as List<Rectangle>;
			foreach (Rectangle item in list)
			{
				if (item.Contains(xPoin, yPoint))
				{
					SelectedColor = ColorTranslator.FromHtml(key);
					return;
				}
			}
		}
	}

	private void CheckMoreColorsClick(int x, int y)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		if (moreColorsRectangle.Contains(new Point(x, y)))
		{
			CustomColorDialogMetro customColorDialogMetro = new CustomColorDialogMetro();
			((Form)customColorDialogMetro).StartPosition = (FormStartPosition)1;
			if (!SelectedColor.IsEmpty)
			{
				customColorDialogMetro.SetNewColor(SelectedColor);
				customColorDialogMetro.CurrentColor = SelectedColor;
			}
			((Form)customColorDialogMetro).ShowDialog((IWin32Window)(object)((IEnumerable)Application.OpenForms).Cast<Form>().LastOrDefault());
			if ((int)((Form)customColorDialogMetro).DialogResult == 1 && !customColorDialogMetro.NewColor.IsEmpty)
			{
				SaveRecentColor(customColorDialogMetro.NewColor);
				SelectedColor = customColorDialogMetro.NewColor;
			}
			((Component)(object)customColorDialogMetro).Dispose();
		}
	}

	private void CheckEditInsertExpressionClick(int x, int y)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		if (editInsertExpressionRectangle.Contains(new Point(x, y)))
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				expression = "\"" + SelectedColor.ToHex() + "\"";
			}
			StiReport report = (instance as StiComponent)?.Report;
			string text = expression;
			DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorAppExpression, null, allowPackExpression: true, new string[5] { "\"Red\"", "RGB(255, 0, 0)", "\"33ff95\"", "MyStringVariable", "MyData.MyColumn > 0 ? \"Red\" : \"Green\"" });
			if ((int)val == 1)
			{
				Expression = text;
			}
		}
	}

	private void ShowTooltip(string text)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (tooltTip == null)
		{
			tooltTip = new ToolTip
			{
				InitialDelay = 1000,
				ReshowDelay = 1000
			};
		}
		if (!tooltTip.Active || !(tooltTip.GetToolTip((Control)(object)this) == text))
		{
			ToolTip obj = tooltTip;
			if (obj != null)
			{
				obj.Hide((IWin32Window)(object)this);
			}
			ToolTip obj2 = tooltTip;
			if (obj2 != null)
			{
				obj2.SetToolTip((Control)(object)this, text);
			}
		}
	}

	private void HideTooltip()
	{
		ToolTip obj = tooltTip;
		if (obj != null)
		{
			obj.Hide((IWin32Window)(object)this);
		}
	}
}
