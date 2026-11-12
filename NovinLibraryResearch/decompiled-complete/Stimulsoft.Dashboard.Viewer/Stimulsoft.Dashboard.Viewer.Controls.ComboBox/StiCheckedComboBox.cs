using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Dashboard.Viewer.Elements;

namespace Stimulsoft.Dashboard.Viewer.Controls.ComboBox;

[ToolboxItem(false)]
public class StiCheckedComboBox : Panel
{
	private StiComboBoxItem allItem = new StiComboBoxItem(StiLocHelper.LocAll);

	private DateTime dropDownClosedTime;

	private bool lockSearch;

	private int selectedIndexBeforeDropDown;

	private bool isHot;

	private StiControlColorCollection colors;

	private StiCheckedListBox ListBox { get; } = new StiCheckedListBox();


	private StiHintTextBox SearchBox { get; set; }

	private Panel ContainerPanel { get; } = new Panel();


	private StiAllCheckBox AllCheckBox { get; } = new StiAllCheckBox();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool BlockSelectedIndexChanged { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public CheckState AllCheckBoxState
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((CheckBox)AllCheckBox).CheckState;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((CheckBox)AllCheckBox).CheckState = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowCheckBoxes
	{
		get
		{
			return ListBox.ShowCheckBoxes;
		}
		set
		{
			ListBox.ShowCheckBoxes = value;
		}
	}

	private StiDropDown DropDownPopup { get; } = new StiDropDown();


	private bool IsHot
	{
		get
		{
			return isHot;
		}
		set
		{
			if (isHot != value)
			{
				isHot = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectedIndex
	{
		get
		{
			return ((ListControl)ListBox).SelectedIndex;
		}
		set
		{
			((ListControl)ListBox).SelectedIndex = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public object SelectedItem
	{
		get
		{
			return ((ListBox)ListBox).SelectedItem;
		}
		set
		{
			((ListBox)ListBox).SelectedItem = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected internal ObjectCollection Items => ((CheckedListBox)ListBox).Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected internal List<StiComboBoxItem> OriginItems { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowSearchBox { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowAllValue { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DroppedDown
	{
		get
		{
			if (DropDownPopup != null)
			{
				return ((ToolStripDropDown)DropDownPopup).Visible;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsTransparent { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			if (!IsTransparent)
			{
				return ((Control)this).BackColor;
			}
			return Color.Transparent;
		}
		set
		{
			((Control)this).BackColor = (IsTransparent ? Color.Transparent : value);
		}
	}

	public event ItemCheckEventHandler ItemCheck
	{
		add
		{
			((CheckedListBox)ListBox).ItemCheck += value;
		}
		remove
		{
			((CheckedListBox)ListBox).ItemCheck -= value;
		}
	}

	public event EventHandler AllItemCheck
	{
		add
		{
			((CheckBox)AllCheckBox).CheckStateChanged += value;
		}
		remove
		{
			((CheckBox)AllCheckBox).CheckStateChanged -= value;
		}
	}

	public event EventHandler SelectedIndexChanged
	{
		add
		{
			((ListBox)ListBox).SelectedIndexChanged += value;
		}
		remove
		{
			((ListBox)ListBox).SelectedIndexChanged -= value;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		DrawItem(graphics);
		DrawItemZones(graphics);
		DrawButton(graphics);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		IsHot = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		IsHot = false;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		IsHot = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (ShowCheckBoxes)
		{
			StiComboBoxItemZone mouseOverZone = GetMouseOverZone();
			if (mouseOverZone != null)
			{
				if (mouseOverZone.IsAll)
				{
					((CheckBox)AllCheckBox).Checked = false;
					InvokeSelectedIndexChanged();
				}
				else if (mouseOverZone.IsStacked)
				{
					while (((CheckedListBox)ListBox).CheckedIndices.Count > 0)
					{
						((CheckedListBox)ListBox).SetItemChecked(((CheckedListBox)ListBox).CheckedIndices[0], false);
					}
					InvokeSelectedIndexChanged();
				}
				else
				{
					int num = ((ObjectCollection)((CheckedListBox)ListBox).Items).IndexOf(mouseOverZone.Item);
					((CheckedListBox)ListBox).SetItemCheckState(num, (CheckState)0);
					InvokeSelectedIndexChanged();
				}
				return;
			}
		}
		if (!DroppedDown && (DateTime.Now - dropDownClosedTime).TotalSeconds > 0.5)
		{
			ShowDropDown();
		}
	}

	private void SearchBox_TextChanged(object sender, EventArgs e)
	{
		if (!lockSearch)
		{
			DoSearch();
		}
	}

	private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!ShowCheckBoxes && !lockSearch)
		{
			lockSearch = true;
			object selectedItem = SelectedItem;
			CloseDropDown();
			CloseSearch();
			SelectedItem = selectedItem;
			lockSearch = false;
		}
	}

	private void ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void DropDownPopupClosed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		dropDownClosedTime = DateTime.Now;
		if (SelectedIndex == -1 && selectedIndexBeforeDropDown != -1)
		{
			CloseSearch();
			SelectedIndex = selectedIndexBeforeDropDown;
		}
	}

	private void CloseSearch()
	{
		if (SearchBox != null)
		{
			((Control)SearchBox).Text = "";
		}
		DoSearch();
	}

	private void DoSearch()
	{
		if (SearchBox == null)
		{
			return;
		}
		string value = ((Control)SearchBox).Text?.ToLowerInvariant();
		IEnumerable<StiComboBoxItem> enumerable;
		if (!string.IsNullOrEmpty(value))
		{
			enumerable = OriginItems.Where((StiComboBoxItem i) => i.Label.ToLowerInvariant().Contains(value));
		}
		else
		{
			IEnumerable<StiComboBoxItem> originItems = OriginItems;
			enumerable = originItems;
		}
		IEnumerable<StiComboBoxItem> source = enumerable;
		((ListBox)ListBox).BeginUpdate();
		((ObjectCollection)((CheckedListBox)ListBox).Items).Clear();
		ObjectCollection items = ((CheckedListBox)ListBox).Items;
		object[] array = source.ToArray();
		((ObjectCollection)items).AddRange(array);
		for (int j = 0; j < ((ObjectCollection)((CheckedListBox)ListBox).Items).Count; j++)
		{
			if (((ObjectCollection)((CheckedListBox)ListBox).Items)[j] is StiComboBoxItem { Checked: not false })
			{
				((CheckedListBox)ListBox).SetItemChecked(j, true);
			}
		}
		((ListBox)ListBox).EndUpdate();
	}

	private void InvokeSelectedIndexChanged()
	{
		if (!BlockSelectedIndexChanged)
		{
			if (SelectedIndex != -1)
			{
				SelectedIndex = -1;
			}
			else if (((ObjectCollection)Items).Count > 0)
			{
				SelectedIndex = 0;
			}
		}
	}

	private void DrawItem(Graphics g)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		if (ShowCheckBoxes)
		{
			if (IsHot)
			{
				Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
				List<StiComboBoxItemZone> itemZones = GetItemZones();
				if (itemZones.Any())
				{
					int num = itemZones.Last().Rect.Right + StiScale.I4;
					rect.X += num;
					rect.Width -= num;
				}
				if (rect.Contains(((Control)this).PointToClient(Cursor.Position)))
				{
					StiDrawing.FillRectangle(g, Colors.HotBackColor, rect);
				}
			}
			return;
		}
		Rectangle rect2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		bool flag = false;
		bool flag2 = rect2.Contains(((Control)this).PointToClient(Cursor.Position));
		Color foreColor = Colors.GetForeColor(flag, flag2);
		Color backColor = Colors.GetBackColor(flag, flag2);
		if (flag || flag2)
		{
			StiDrawing.FillRectangle(g, backColor, rect2);
		}
		string text = SelectedItem?.ToString();
		StringFormat val = new StringFormat();
		try
		{
			val.Alignment = (StringAlignment)0;
			val.FormatFlags = (StringFormatFlags)4096;
			val.LineAlignment = (StringAlignment)1;
			val.Trimming = (StringTrimming)3;
			StiDrawing.DrawString(g, text, ((Control)this).Font, foreColor, rect2, val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawItemZones(Graphics g)
	{
		if (!ShowCheckBoxes)
		{
			return;
		}
		List<StiComboBoxItemZone> itemZones = GetItemZones();
		if (!itemZones.Any())
		{
			return;
		}
		foreach (StiComboBoxItemZone item in itemZones)
		{
			bool flag = item.Rect.Contains(((Control)this).PointToClient(Cursor.Position));
			bool isHotClose = StiZonePainter.GetCloseRect(item.Rect).Contains(((Control)this).PointToClient(Cursor.Position));
			StiZonePainter.DrawItemZone(g, item.Text, ((Control)this).Font, flag, isHotClose, Colors.SelectedForeColor, Colors.SelectedBackColor, item.Rect);
		}
	}

	protected virtual void DrawButton(Graphics g)
	{
		Rectangle rect = new Rectangle(((Control)this).Width - GetButtonWidth(), 0, GetButtonWidth(), ((Control)this).Height);
		StiButtonPainter.DrawButton(g, rect, IsHot, Colors.GlyphColor, Colors.HotGlyphColor);
	}

	private StiComboBoxItemZone GetMouseOverZone()
	{
		List<StiComboBoxItemZone> itemZones = GetItemZones();
		foreach (StiComboBoxItemZone item in itemZones)
		{
			if (StiZonePainter.GetCloseRect(item.Rect).Contains(((Control)this).PointToClient(Cursor.Position)))
			{
				return item;
			}
		}
		return null;
	}

	public List<StiComboBoxItem> GetCheckedItems()
	{
		if (OriginItems == null)
		{
			return new List<StiComboBoxItem>();
		}
		return OriginItems.Where((StiComboBoxItem c) => c.Checked)?.ToList();
	}

	public void SetItems(List<StiComboBoxItem> items)
	{
		OriginItems = items;
		((ListBox)ListBox).BeginUpdate();
		((ObjectCollection)((CheckedListBox)ListBox).Items).Clear();
		ObjectCollection items2 = ((CheckedListBox)ListBox).Items;
		object[] array = items.ToArray();
		((ObjectCollection)items2).AddRange(array);
		((ListBox)ListBox).EndUpdate();
	}

	public void SetAllItemsCheckState(CheckState state)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		for (int j = 0; j < ((ObjectCollection)Items).Count; j++)
		{
			((CheckedListBox)ListBox).SetItemCheckState(j, state);
		}
		OriginItems.ForEach(delegate(StiComboBoxItem i)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			i.Checked = (int)state == 1;
		});
	}

	private List<StiComboBoxItemZone> GetItemZones()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		int x = 0;
		int index = 0;
		List<StiComboBoxItem> items = ((ShowAllValue && (int)((CheckBox)AllCheckBox).CheckState == 1) ? new List<StiComboBoxItem> { allItem } : GetCheckedItems());
		List<StiComboBoxItemZone> list = items.Select(GetItemZoneSize).Select(delegate(Size i)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Invalid comparison between Unknown and I4
			Rectangle rect = new Rectangle(x, 0, i.Width + (int)(StiScale.Factor * 14.0), ((Control)this).Height);
			x += rect.Width + StiScale.I4;
			if (ShowAllValue && (int)((CheckBox)AllCheckBox).CheckState == 1)
			{
				return StiComboBoxItemZone.MakeAll(rect);
			}
			StiComboBoxItemZone result = StiComboBoxItemZone.Make(items[index].ToString(), rect, items[index]);
			index++;
			return result;
		}).ToList();
		if (!list.Any())
		{
			return list;
		}
		if (list.Last().Rect.Right <= ((Control)this).Width - GetButtonWidth())
		{
			return list;
		}
		string text = string.Format(Loc.Get("Dashboard", "NSelected"), list.Count);
		int width = TextRenderer.MeasureText(text, ((Control)this).Font).Width;
		return new List<StiComboBoxItemZone> { StiComboBoxItemZone.MakeStacked(text, new Rectangle(0, 0, width + (int)(14.0 * StiScale.Factor), ((Control)this).Height)) };
	}

	private Size GetItemZoneSize(object item)
	{
		string text = item.ToString();
		if (string.IsNullOrEmpty(text))
		{
			text = new string(' ', 5);
		}
		return TextRenderer.MeasureText(text, ((Control)this).Font);
	}

	protected virtual int GetButtonWidth()
	{
		return SystemInformation.HorizontalScrollBarArrowWidth;
	}

	protected virtual void CloseDropDown()
	{
		StiDropDown dropDownPopup = DropDownPopup;
		if (dropDownPopup != null)
		{
			((ToolStripDropDown)dropDownPopup).Close();
		}
	}

	protected virtual void ShowDropDown()
	{
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		BlockSelectedIndexChanged = true;
		selectedIndexBeforeDropDown = SelectedIndex;
		int val = ((OriginItems == null) ? 10 : OriginItems.Count);
		int height = StiScale.YYI(StiElementConsts.ComboBox.ItemHeight) * Math.Min(val, 10) + StiScale.I6;
		int num = StiScale.I2;
		((Control)DropDownPopup).AutoSize = false;
		((Control)DropDownPopup).Width = ((Control)this).Width;
		((Control)DropDownPopup).Height = height;
		((ToolStrip)DropDownPopup).BackColor = Colors.BackColor;
		if (ShowSearchBox)
		{
			if (SearchBox == null)
			{
				CreateSearchBox();
			}
			((Control)SearchBox).BackColor = Colors.BackColor;
			((Control)SearchBox).ForeColor = Colors.ForeColor;
			SearchBox.BorderColor = Colors.SeparatorColor;
			((Control)SearchBox).Location = new Point(StiScale.I4, num);
			((Control)SearchBox).Width = ((Control)this).Width - StiScale.I10;
			((Control)SearchBox).Height = StiScale.I(24);
			((Control)SearchBox).Font = ((Control)this).Font;
			num += ((Control)SearchBox).Height + StiScale.I4;
		}
		if (ShowAllValue)
		{
			((Control)AllCheckBox).Location = new Point(0, num);
			((Control)AllCheckBox).Width = ((Control)this).Width;
			((Control)AllCheckBox).Height = ((ListBox)ListBox).ItemHeight;
			AllCheckBox.Colors = Colors;
			((Control)AllCheckBox).Font = ((Control)this).Font;
			num += ((Control)AllCheckBox).Height + StiScale.I4;
		}
		ListBox.BlockSelectedIndexChanged = true;
		((Control)ListBox).Location = new Point(0, num);
		((Control)ListBox).Size = new Size(((Control)this).Width - StiScale.I2, height);
		ListBox.Colors = Colors;
		((Control)ListBox).Font = ((Control)this).Font;
		if (ShowSearchBox)
		{
			((Control)ContainerPanel).Controls.Add((Control)(object)SearchBox);
		}
		if (ShowAllValue)
		{
			((Control)ContainerPanel).Controls.Add((Control)(object)AllCheckBox);
		}
		((Control)ContainerPanel).Controls.Add((Control)(object)ListBox);
		ToolStripControlHost val2 = new ToolStripControlHost((Control)(object)ContainerPanel);
		((ToolStripItem)val2).AutoSize = false;
		((Control)DropDownPopup).Width = ((Control)this).Width;
		((Control)DropDownPopup).Height = ((Control)ListBox).Bottom + StiScale.I4;
		((ToolStrip)DropDownPopup).Items.Clear();
		((ToolStrip)DropDownPopup).Items.Add((ToolStripItem)(object)val2);
		((Control)ContainerPanel).Width = ((Control)DropDownPopup).Width;
		((Control)ContainerPanel).Height = ((Control)DropDownPopup).Height;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Point point = ((Control)this).PointToScreen(new Point(clientRectangle.Left, clientRectangle.Top + clientRectangle.Height));
		Rectangle workingArea = Screen.FromControl((Control)(object)this).WorkingArea;
		if (point.X + ((Control)this).Size.Width > workingArea.Left + workingArea.Width)
		{
			point.X = workingArea.Left + workingArea.Width - ((Control)this).Size.Width;
		}
		if (point.Y + ((Control)this).Size.Height > workingArea.Top + workingArea.Height)
		{
			point.Y -= ((Control)this).Size.Height + clientRectangle.Height;
		}
		point = ((Control)this).PointToClient(point);
		((ToolStripDropDown)DropDownPopup).Show((Control)(object)this, point);
		StiHintTextBox searchBox = SearchBox;
		if (searchBox != null)
		{
			((Control)searchBox).Focus();
		}
		ListBox.BlockSelectedIndexChanged = false;
		BlockSelectedIndexChanged = false;
	}

	private void CreateSearchBox()
	{
		SearchBox = new StiHintTextBox();
		SearchBox.FixedHeight = false;
		SearchBox.Hint = Loc.GetCleaned("Editor", "TypeToSearch");
		((Control)SearchBox).TextChanged += SearchBox_TextChanged;
	}

	public void SetItemChecked(int index, bool value)
	{
		((CheckedListBox)ListBox).SetItemChecked(index, value);
	}

	public void SetAllItemsState(CheckState state)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((CheckBox)AllCheckBox).CheckState = state;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		((Control)this).Dispose(disposing);
		if (ListBox != null && !((Control)ListBox).IsDisposed)
		{
			((CheckedListBox)ListBox).ItemCheck -= new ItemCheckEventHandler(ListBox_ItemCheck);
			((ListBox)ListBox).SelectedIndexChanged -= ListBox_SelectedIndexChanged;
			((Component)(object)ListBox).Dispose();
		}
		if (SearchBox != null && !((Control)SearchBox).IsDisposed)
		{
			((Control)SearchBox).TextChanged -= SearchBox_TextChanged;
			((Component)(object)SearchBox).Dispose();
		}
		if (ContainerPanel != null && !((Control)ContainerPanel).IsDisposed)
		{
			((Component)(object)ContainerPanel).Dispose();
		}
		if (DropDownPopup != null && !((Control)DropDownPopup).IsDisposed)
		{
			((ToolStripDropDown)DropDownPopup).Closed -= new ToolStripDropDownClosedEventHandler(DropDownPopupClosed);
			((Component)(object)DropDownPopup).Dispose();
		}
		if (AllCheckBox != null && !((Control)AllCheckBox).IsDisposed)
		{
			((Component)(object)AllCheckBox).Dispose();
		}
		if (DropDownPopup != null && !((Control)DropDownPopup).IsDisposed)
		{
			((Component)(object)DropDownPopup).Dispose();
		}
	}

	public StiCheckedComboBox()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		Colors = new StiControlColorCollection((Control)(object)this);
		((Control)this).Height = (int)(24.0 * StiScale.Factor);
		dropDownClosedTime = DateTime.Now;
		((ToolStripDropDown)DropDownPopup).Closed += new ToolStripDropDownClosedEventHandler(DropDownPopupClosed);
		ListBox.IntegralHeight = false;
		((CheckedListBox)ListBox).ItemCheck += new ItemCheckEventHandler(ListBox_ItemCheck);
		((ListBox)ListBox).SelectedIndexChanged += ListBox_SelectedIndexChanged;
	}
}
