using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiMapCategoriesForm : Form
{
	[ToolboxItem(false)]
	private class StiMapToggleButton : Control
	{
		private bool isError;

		private bool isMouseOver;

		private bool isPressed;

		private Image icon;

		private Image errorIcon;

		public string MapID { get; }

		public bool IsChecked { get; set; }

		public event EventHandler Checked;

		public StiMapToggleButton(StiMapToolboxInfo info, bool isError = false)
		{
			MapID = info.MapID;
			((Control)this).Width = (int)Math.Ceiling(96.0 * StiScale.X);
			((Control)this).Height = (int)Math.Ceiling(96.0 * StiScale.Y);
			((Control)this).Text = (StiLocalization.IsRussian ? info.LocRuName : info.LocEnName);
			icon = (Image)((info.Icon != null) ? ((object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Map." + info.Icon + "_x2.png", makeTransparent: false)) : ((object)GetCustomMapIcon(info)));
			if (isError)
			{
				this.isError = true;
				((Control)this).Enabled = false;
				string text = ((StiScale.Id == SystemScaleID.x2) ? "_x2" : string.Empty);
				errorIcon = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Error_16x16" + text + ".png", makeTransparent: false);
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Expected O, but got Unknown
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Expected O, but got Unknown
			((Control)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			if (isError)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(255, 240, 240, 240));
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(Color.FromArgb(255, 181, 193, 206));
				try
				{
					graphics.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (isPressed || IsChecked)
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(255, 163, 189, 227));
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(Color.FromArgb(255, 181, 193, 206));
				try
				{
					graphics.DrawRectangle(val4, rectangle);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (isMouseOver)
			{
				SolidBrush val5 = new SolidBrush(Color.FromArgb(255, 213, 225, 242));
				try
				{
					graphics.FillRectangle((Brush)(object)val5, rectangle);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val6 = new Pen(Color.FromArgb(255, 181, 193, 206));
				try
				{
					graphics.DrawRectangle(val6, rectangle);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			else
			{
				graphics.FillRectangle(Brushes.White, rectangle);
				Pen val7 = new Pen(Color.FromArgb(255, 181, 193, 206));
				try
				{
					graphics.DrawRectangle(val7, rectangle);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			if (icon != null)
			{
				Rectangle rectangle2 = new Rectangle((((Control)this).Width - 64) / 2, (((Control)this).Height - 64) / 2, 64, 64);
				graphics.DrawImage(icon, rectangle2);
			}
			if (((Control)this).Text != null)
			{
				rectangle.Height -= 2;
				SolidBrush val8 = new SolidBrush(Color.FromArgb(255, 68, 68, 68));
				try
				{
					StringFormat val9 = new StringFormat();
					try
					{
						val9.Alignment = (StringAlignment)1;
						val9.LineAlignment = (StringAlignment)2;
						graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val8, (RectangleF)rectangle, val9);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			if (errorIcon != null)
			{
				Rectangle rectangle3 = new Rectangle(((Control)this).Width - errorIcon.Width - 5, 5, errorIcon.Width, errorIcon.Height);
				graphics.DrawImage(errorIcon, rectangle3);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			isMouseOver = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			isMouseOver = false;
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			((Control)this).OnMouseDown(e);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			isPressed = false;
			if (!IsChecked)
			{
				IsChecked = true;
				this.Checked?.Invoke(this, EventArgs.Empty);
			}
			((Control)this).Invalidate();
		}

		private Image GetCustomMapIcon(StiMapToolboxInfo info)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if (info.CustomIcon != null)
			{
				return (Image)new Bitmap((Stream)new MemoryStream(info.CustomIcon));
			}
			return null;
		}
	}

	private class StiCategoryItem
	{
		public StiMapCategory Category { get; }

		public string Text { get; }

		public override string ToString()
		{
			return Text;
		}

		public StiCategoryItem(StiMapCategoriesForm form, StiMapCategory category)
		{
			Category = category;
			Dictionary<StiMapCategory, AllItemsCache> cacheCategories = form.cacheCategories;
			switch (category)
			{
			case StiMapCategory.All:
			{
				int num = cacheCategories.Sum((KeyValuePair<StiMapCategory, AllItemsCache> x) => x.Value.Count);
				Text = string.Format("{0} ({1})", StiLocalization.Get("PropertyEnum", "StiRestrictionsAll"), num);
				break;
			}
			case StiMapCategory.NorthAmerica:
				Text = (StiLocalization.IsRussian ? $"Северная Америка ({cacheCategories[category].Count})" : $"North America ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.SouthAmerica:
				Text = (StiLocalization.IsRussian ? $"Южная Америка ({cacheCategories[category].Count})" : $"South America ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.Africa:
				Text = (StiLocalization.IsRussian ? $"Африка ({cacheCategories[category].Count})" : $"Africa ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.Asia:
				Text = (StiLocalization.IsRussian ? $"Азия ({cacheCategories[category].Count})" : $"Asia ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.Europe:
				Text = (StiLocalization.IsRussian ? $"Европа ({cacheCategories[category].Count})" : $"Europe ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.Oceania:
				Text = (StiLocalization.IsRussian ? $"Океания ({cacheCategories[category].Count})" : $"Oceania ({cacheCategories[category].Count})");
				break;
			case StiMapCategory.Custom:
				Text = (StiLocalization.IsRussian ? $"Пользовательские ({cacheCategories[category].Count})" : $"Custom ({cacheCategories[category].Count})");
				break;
			default:
				throw new NotSupportedException();
			}
		}
	}

	private class AllItemsCache : List<StiMapToolboxInfo>
	{
		public bool ContainsKey(string key)
		{
			return this.FirstOrDefault((StiMapToolboxInfo x) => x.MapID.ToLowerInvariant() == key) != null;
		}

		public List<StiMapToolboxInfo> GetFilteredItems(string filter)
		{
			return this.Where((StiMapToolboxInfo x) => (!string.IsNullOrEmpty(x.LocEnName) && x.LocEnName.ToLowerInvariant().Contains(filter)) || (!string.IsNullOrEmpty(x.LocRuName) && x.LocRuName.ToLowerInvariant().Contains(filter)) || x.MapID.ToLowerInvariant().Contains(filter)).ToList();
		}

		public AllItemsCache()
		{
		}

		public AllItemsCache(List<StiMapToolboxInfo> list)
			: base((IEnumerable<StiMapToolboxInfo>)list)
		{
		}
	}

	private Dictionary<StiMapCategory, AllItemsCache> cacheCategories = new Dictionary<StiMapCategory, AllItemsCache>();

	private Dictionary<string, StiMapToggleButton> cacheAllButtons = new Dictionary<string, StiMapToggleButton>();

	private List<StiMapToggleButton> popularMapsButtons = new List<StiMapToggleButton>();

	private List<StiMapToggleButton> customMapsButtons = new List<StiMapToggleButton>();

	private AllItemsCache allItems = new AllItemsCache();

	private string lastFilter = string.Empty;

	private StiMapToggleButton checkedButton;

	private bool lockSearch;

	private IContainer components;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine stiGroupLine1;

	private Panel panelCategory;

	private Panel panelSearch;

	private Panel panelItems;

	private Panel panelItemsRoot;

	private StiComboBox comboBoxCategories;

	private StiHintTextBox textBoxSearch;

	private Label labelCategory;

	private Label labelName;

	public string MapID => checkedButton?.MapID;

	public StiMapCategoriesForm(StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Control)btOk).Enabled = false;
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this, skipLocation: false, skipSize: true);
			Localize();
			Init(report);
			DoScaling();
			((Control)textBoxSearch).Focus();
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		textBoxSearch.Hint = StiLocalization.Get("Editor", "TypeToSearch");
		((Control)labelCategory).Text = StiLocalization.Get("PropertyMain", "Category");
		((Control)labelName).Text = StiLocalization.Get("PropertyMain", "Name");
		((Control)this).Text = "Add Map";
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if ((keyData & 0xFFFF) == 27 && ((Control)textBoxSearch).CanFocus && ((Control)textBoxSearch).Text.Length > 0)
		{
			((Control)textBoxSearch).Text = string.Empty;
			return true;
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	private void MapButton_Checked(object sender, EventArgs e)
	{
		if (checkedButton != null)
		{
			checkedButton.IsChecked = false;
			((Control)checkedButton).Invalidate();
		}
		checkedButton = (StiMapToggleButton)sender;
		((Control)btOk).Enabled = true;
	}

	private void MapButton_DoubleClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void ComboBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
	{
		lockSearch = true;
		((Control)textBoxSearch).Text = string.Empty;
		lastFilter = string.Empty;
		lockSearch = false;
		ShowCurrentCategory();
	}

	private void TextBoxSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
		if ((int)keyCode == 27)
		{
			((Control)textBoxSearch).Text = string.Empty;
		}
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		if (!lockSearch)
		{
			ProcessSearch(((Control)textBoxSearch).Text);
		}
	}

	private void DoScaling()
	{
		((Control)textBoxSearch).Width = ((Control)comboBoxCategories).Width;
		((Control)panelItems).Width = ((Control)comboBoxCategories).Width + (int)(112.0 * StiScale.X);
	}

	private void Init(StiReport report)
	{
		List<StiMapToolboxInfo> list = null;
		List<StiMapToolboxInfo> mapToolboxItems = StiToolboxHelper.GetMapToolboxItems();
		foreach (StiMapToolboxInfo item in mapToolboxItems)
		{
			StiMapCategory valueOrDefault = item.Category.GetValueOrDefault();
			if (valueOrDefault == StiMapCategory.PopularMaps)
			{
				list = item.Infos;
				continue;
			}
			AllItemsCache allItemsCache = new AllItemsCache();
			foreach (StiMapToolboxInfo info in item.Infos)
			{
				string key = info.MapID.ToLowerInvariant();
				allItemsCache.Add(info);
				if (!allItems.ContainsKey(key))
				{
					allItems.Add(info);
					StiMapToggleButton stiMapToggleButton = new StiMapToggleButton(info);
					stiMapToggleButton.Checked += MapButton_Checked;
					((Control)stiMapToggleButton).DoubleClick += MapButton_DoubleClick;
					cacheAllButtons.Add(key, stiMapToggleButton);
				}
			}
			allItemsCache = new AllItemsCache(allItemsCache.OrderBy((StiMapToolboxInfo x) => x.MapID).ToList());
			cacheCategories.Add(valueOrDefault, allItemsCache);
		}
		foreach (StiMapToolboxInfo item2 in list)
		{
			StiMapToggleButton stiMapToggleButton2 = new StiMapToggleButton(item2);
			stiMapToggleButton2.Checked += MapButton_Checked;
			((Control)stiMapToggleButton2).DoubleClick += MapButton_DoubleClick;
			popularMapsButtons.Add(stiMapToggleButton2);
		}
		if (report != null)
		{
			StiCustomMapFinder.Init(report);
			Dictionary<string, StiMapSvgContainer> maps = StiCustomMapFinder.GetMaps();
			if (maps.Count > 0)
			{
				AllItemsCache allItemsCache2 = new AllItemsCache();
				foreach (string item3 in maps.Keys.OrderBy((string x) => x))
				{
					string text = item3.ToLowerInvariant();
					if (!allItems.ContainsKey(text))
					{
						string icon = StiCustomMapFinder.GetIcon(report, text);
						byte[] icon2 = ((icon != null) ? Convert.FromBase64String(icon) : StiCustomMapFinder.GetDefaultIcon());
						StiMapSvgContainer stiMapSvgContainer = maps[item3];
						StiMapToolboxInfo stiMapToolboxInfo = new StiMapToolboxInfo(text, icon2);
						StiMapToggleButton stiMapToggleButton3 = new StiMapToggleButton(stiMapToolboxInfo, stiMapSvgContainer.IsNotCorrect);
						stiMapToggleButton3.Checked += MapButton_Checked;
						((Control)stiMapToggleButton3).DoubleClick += MapButton_DoubleClick;
						customMapsButtons.Add(stiMapToggleButton3);
						allItemsCache2.Add(stiMapToolboxInfo);
						allItems.Add(stiMapToolboxInfo);
						cacheAllButtons.Add(text, stiMapToggleButton3);
					}
				}
				cacheCategories.Add(StiMapCategory.Custom, allItemsCache2);
			}
		}
		Array values = Enum.GetValues(typeof(StiMapCategory));
		foreach (StiMapCategory item4 in values)
		{
			if (item4 != StiMapCategory.PopularMaps && (item4 != StiMapCategory.Custom || customMapsButtons.Count != 0))
			{
				((ComboBox)comboBoxCategories).Items.Add((object)new StiCategoryItem(this, item4));
			}
		}
		int num = StiSettings.GetInt("MapCategories", "Category", 0);
		switch (num)
		{
		case 7:
			if (customMapsButtons.Count == 0)
			{
				num = 0;
			}
			break;
		default:
			num = 0;
			break;
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
			break;
		}
		((ListControl)comboBoxCategories).SelectedIndex = num;
		allItems = new AllItemsCache(allItems.OrderBy((StiMapToolboxInfo x) => x.MapID).ToList());
		ShowCurrentCategory();
	}

	private void ShowMaps(List<StiMapToolboxInfo> tags, bool isAll)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		if (checkedButton != null)
		{
			((Control)btOk).Enabled = false;
			checkedButton.IsChecked = false;
			((Control)checkedButton).Invalidate();
			checkedButton = null;
		}
		((Control)panelItems).Controls.Clear();
		double num = 12.0 * StiScale.X;
		double num2 = 12.0 * StiScale.Y;
		if (isAll)
		{
			Label val = new Label
			{
				ForeColor = Color.DimGray,
				Location = new Point((int)(12.0 * StiScale.X), (int)(12.0 * StiScale.Y)),
				Size = new Size((int)(168.0 * StiScale.X), (int)(18.0 * StiScale.Y)),
				Text = (StiLocalization.IsRussian ? "Популярные карты" : "Popular maps")
			};
			((Control)panelItems).Controls.Add((Control)(object)val);
			num2 += (double)((Control)val).Height + 2.0 * StiScale.Y;
			int num3 = 0;
			foreach (StiMapToggleButton popularMapsButton in popularMapsButtons)
			{
				((Control)popularMapsButton).Left = (int)num;
				((Control)popularMapsButton).Top = (int)num2;
				((Control)panelItems).Controls.Add((Control)(object)popularMapsButton);
				num3++;
				if (num3 == 4)
				{
					num = 12.0 * StiScale.X;
					num2 += (double)((Control)popularMapsButton).Height + 12.0 * StiScale.Y;
					num3 = 0;
				}
				else
				{
					num += (double)((Control)popularMapsButton).Width + 12.0 * StiScale.X;
				}
			}
			num2 += (double)((Control)popularMapsButtons[0]).Height + 12.0 * StiScale.Y;
			Label val2 = new Label
			{
				ForeColor = Color.DimGray,
				Location = new Point((int)(12.0 * StiScale.X), (int)num2),
				Size = new Size((int)(168.0 * StiScale.X), (int)(18.0 * StiScale.Y)),
				Text = (StiLocalization.IsRussian ? "А-Я" : "A-Z")
			};
			((Control)panelItems).Controls.Add((Control)(object)val2);
			num2 += (double)((Control)val2).Height + 2.0 * StiScale.Y;
		}
		int num4 = 0;
		num = 12.0 * StiScale.X;
		foreach (StiMapToolboxInfo tag in tags)
		{
			StiMapToggleButton stiMapToggleButton = cacheAllButtons[tag.MapID.ToLowerInvariant()];
			((Control)stiMapToggleButton).Left = (int)num;
			((Control)stiMapToggleButton).Top = (int)num2;
			((Control)panelItems).Controls.Add((Control)(object)stiMapToggleButton);
			num4++;
			if (num4 == 4)
			{
				num = 12.0 * StiScale.X;
				num2 += (double)((Control)stiMapToggleButton).Height + 12.0 * StiScale.Y;
				num4 = 0;
			}
			else
			{
				num += (double)((Control)stiMapToggleButton).Width + 12.0 * StiScale.X;
			}
		}
	}

	private void ProcessSearch(string filter)
	{
		if (!(lastFilter == filter))
		{
			lastFilter = filter;
			if (string.IsNullOrEmpty(filter))
			{
				ShowCurrentCategory();
				return;
			}
			filter = filter.ToLowerInvariant();
			List<StiMapToolboxInfo> filteredItems = allItems.GetFilteredItems(filter);
			ShowMaps(filteredItems, isAll: false);
		}
	}

	private void ShowCurrentCategory()
	{
		if (((ComboBox)comboBoxCategories).SelectedItem is StiCategoryItem stiCategoryItem)
		{
			if (stiCategoryItem.Category == StiMapCategory.All)
			{
				ShowMaps(allItems, isAll: true);
			}
			else
			{
				ShowMaps(cacheCategories[stiCategoryItem.Category], isAll: false);
			}
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (checkedButton != null)
		{
			StiSettings.Load();
			StiSettings.Set("DbsMapEditor", "LastMapID", checkedButton.MapID);
			StiSettings.Save();
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("MapCategories", "Category", ((ListControl)comboBoxCategories).SelectedIndex);
		StiSettings.Save();
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		Timer timer = new Timer();
		timer.Interval = 300;
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)textBoxSearch).Focus();
			((Component)(object)timer).Dispose();
		};
		timer.Start();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		panelCategory = new Panel();
		labelCategory = new Label();
		comboBoxCategories = new StiComboBox();
		panelSearch = new Panel();
		labelName = new Label();
		textBoxSearch = new StiHintTextBox();
		panelItems = new Panel();
		panelItemsRoot = new Panel();
		btOk = new Button();
		btCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		((Control)panelCategory).SuspendLayout();
		((Control)panelSearch).SuspendLayout();
		((Control)panelItemsRoot).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelCategory).BackColor = Color.White;
		((Control)panelCategory).Controls.Add((Control)(object)labelCategory);
		((Control)panelCategory).Controls.Add((Control)(object)comboBoxCategories);
		((Control)panelCategory).Dock = (DockStyle)1;
		((Control)panelCategory).Location = new Point(0, 0);
		((Control)panelCategory).Margin = new Padding(6);
		((Control)panelCategory).Name = "panelCategory";
		((Control)panelCategory).Size = new Size(950, 83);
		((Control)panelCategory).TabIndex = 1;
		((Control)labelCategory).ForeColor = Color.DimGray;
		((Control)labelCategory).Location = new Point(24, 19);
		((Control)labelCategory).Margin = new Padding(0);
		((Control)labelCategory).Name = "labelCategory";
		((Control)labelCategory).Size = new Size(200, 46);
		((Control)labelCategory).TabIndex = 2;
		((Control)labelCategory).Text = "Category";
		labelCategory.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCategories).Anchor = (AnchorStyles)12;
		comboBoxCategories.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxCategories).Location = new Point(224, 25);
		((Control)comboBoxCategories).Margin = new Padding(6);
		((Control)comboBoxCategories).Name = "comboBoxCategories";
		((Control)comboBoxCategories).Size = new Size(698, 42);
		((Control)comboBoxCategories).TabIndex = 3;
		((ComboBox)comboBoxCategories).SelectedIndexChanged += ComboBoxCategories_SelectedIndexChanged;
		((Control)panelSearch).BackColor = Color.White;
		((Control)panelSearch).Controls.Add((Control)(object)labelName);
		((Control)panelSearch).Controls.Add((Control)(object)textBoxSearch);
		((Control)panelSearch).Dock = (DockStyle)1;
		((Control)panelSearch).Location = new Point(0, 83);
		((Control)panelSearch).Margin = new Padding(6);
		((Control)panelSearch).Name = "panelSearch";
		((Control)panelSearch).Size = new Size(950, 83);
		((Control)panelSearch).TabIndex = 4;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(24, 15);
		((Control)labelName).Margin = new Padding(0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(200, 46);
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		labelName.TextAlign = (ContentAlignment)16;
		((Control)textBoxSearch).Anchor = (AnchorStyles)12;
		((Control)textBoxSearch).BackColor = SystemColors.Window;
		((Control)textBoxSearch).ForeColor = SystemColors.ControlText;
		((Control)textBoxSearch).Location = new Point(224, 15);
		((Control)textBoxSearch).Margin = new Padding(6);
		((Control)textBoxSearch).Name = "textBoxSearch";
		((Control)textBoxSearch).Size = new Size(698, 42);
		((Control)textBoxSearch).TabIndex = 6;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)textBoxSearch).PreviewKeyDown += new PreviewKeyDownEventHandler(TextBoxSearch_PreviewKeyDown);
		((Control)panelItems).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelItems).AutoScroll = true;
		((Control)panelItems).BackColor = Color.White;
		((Control)panelItems).Location = new Point(0, 0);
		((Control)panelItems).Margin = new Padding(6);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(926, 961);
		((Control)panelItems).TabIndex = 8;
		((Control)panelItemsRoot).BackColor = Color.White;
		((Control)panelItemsRoot).Controls.Add((Control)(object)panelItems);
		((Control)panelItemsRoot).Dock = (DockStyle)5;
		((Control)panelItemsRoot).Location = new Point(0, 166);
		((Control)panelItemsRoot).Margin = new Padding(6);
		((Control)panelItemsRoot).Name = "panelItemsRoot";
		((Control)panelItemsRoot).Size = new Size(950, 984);
		((Control)panelItemsRoot).TabIndex = 7;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(614, 23);
		((Control)btOk).Margin = new Padding(6);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 48);
		((Control)btOk).TabIndex = 11;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(776, 23);
		((Control)btCancel).Margin = new Padding(6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 48);
		((Control)btCancel).TabIndex = 12;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 1150);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(950, 92);
		((Control)stiGroupLine1).TabIndex = 10;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(950, 1242);
		((Control)this).Controls.Add((Control)(object)panelItemsRoot);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)panelSearch);
		((Control)this).Controls.Add((Control)(object)panelCategory);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiMapCategoriesForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiMapCategoriesForm";
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)panelCategory).ResumeLayout(false);
		((Control)panelSearch).ResumeLayout(false);
		((Control)panelItemsRoot).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
