using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class frmCustomize : Form
{
	private TabControl tabCtrl;

	private Label label1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage1;

	private Button cmdNew;

	private Button cmdDelete;

	private Button cmdRename;

	private Button cmdReset;

	private CheckedListBox lstBars;

	private DotNetBarManager m_DotNetBar;

	private Button cmdKeyboard;

	private Button cmdClose;

	private Label label2;

	private Label label3;

	private Label label4;

	private ListBox lstCategories;

	private ItemsListBox lstCommands;

	private bool m_ItemDrag;

	private Cursor m_MoveCursor;

	private Cursor m_CopyCursor;

	private Cursor m_NACursor;

	private IDesignTimeProvider m_DesignTimeProvider;

	private int m_InsertPosition;

	private bool m_InsertBefore;

	private bool m_DragCopy;

	private Point m_MouseDownPt;

	public BaseItem DragItem;

	private BaseItem m_EditItem;

	private ButtonItem m_PopupMenu;

	private Label label5;

	private CheckBox chkShowFullMenus;

	private CheckBox chkFullAfterDelay;

	private Label label6;

	private Button button1;

	private CheckBox chkShowScreenTips;

	private CheckBox chkTipsShowShortcuts;

	private Label label7;

	private ComboItem comboItem1;

	private ComboItem comboItem2;

	private ComboItem comboItem3;

	private ComboItem comboItem4;

	private ComboItem comboItem5;

	private ComboBoxEx cboAnimations;

	private ComboItem comboItem6;

	private Timer m_Timer;

	private Container components;

	private bool m_BarsLoading;

	public bool DragInProgress => m_ItemDrag;

	public frmCustomize(DotNetBarManager ctrl)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		InitializeComponent();
		m_DotNetBar = ctrl;
		((Form)this).StartPosition = (FormStartPosition)1;
		m_ItemDrag = false;
		try
		{
			m_MoveCursor = new Cursor(typeof(DotNetBarManager), "DRAGMOVE.CUR");
			m_CopyCursor = new Cursor(typeof(DotNetBarManager), "DRAGCOPY.CUR");
			m_NACursor = new Cursor(typeof(DotNetBarManager), "DRAGNONE.CUR");
		}
		catch (Exception)
		{
			m_MoveCursor = null;
			m_CopyCursor = null;
			m_NACursor = null;
		}
		m_DesignTimeProvider = null;
		m_DragCopy = false;
		m_EditItem = null;
		using (LocalizationManager localizationManager = new LocalizationManager(m_DotNetBar))
		{
			((Control)cmdNew).Text = localizationManager.GetLocalizedString(((Control)cmdNew).Text);
			((Control)tabPage1).Text = localizationManager.GetLocalizedString(((Control)tabPage1).Text);
			((Control)cmdReset).Text = localizationManager.GetLocalizedString(((Control)cmdReset).Text);
			((Control)cmdRename).Text = localizationManager.GetLocalizedString(((Control)cmdRename).Text);
			((Control)cmdDelete).Text = localizationManager.GetLocalizedString(((Control)cmdDelete).Text);
			((Control)label1).Text = localizationManager.GetLocalizedString(((Control)label1).Text);
			((Control)tabPage2).Text = localizationManager.GetLocalizedString(((Control)tabPage2).Text);
			((Control)label4).Text = localizationManager.GetLocalizedString(((Control)label4).Text);
			((Control)label3).Text = localizationManager.GetLocalizedString(((Control)label3).Text);
			((Control)label2).Text = localizationManager.GetLocalizedString(((Control)label2).Text);
			((Control)tabPage3).Text = localizationManager.GetLocalizedString(((Control)tabPage3).Text);
			((Control)cmdKeyboard).Text = localizationManager.GetLocalizedString(((Control)cmdKeyboard).Text);
			((Control)cmdClose).Text = localizationManager.GetLocalizedString(((Control)cmdClose).Text);
			((Control)label5).Text = localizationManager.GetLocalizedString(((Control)label5).Text);
			((Control)chkShowFullMenus).Text = localizationManager.GetLocalizedString(((Control)chkShowFullMenus).Text);
			((Control)chkFullAfterDelay).Text = localizationManager.GetLocalizedString(((Control)chkFullAfterDelay).Text);
			((Control)label6).Text = localizationManager.GetLocalizedString(((Control)label6).Text);
			((Control)button1).Text = localizationManager.GetLocalizedString(((Control)button1).Text);
			((Control)chkShowScreenTips).Text = localizationManager.GetLocalizedString(((Control)chkShowScreenTips).Text);
			((Control)chkTipsShowShortcuts).Text = localizationManager.GetLocalizedString(((Control)chkTipsShowShortcuts).Text);
			((Control)label7).Text = localizationManager.GetLocalizedString(((Control)label7).Text);
			comboItem1.Text = localizationManager.GetLocalizedString(comboItem1.Text);
			comboItem2.Text = localizationManager.GetLocalizedString(comboItem2.Text);
			comboItem3.Text = localizationManager.GetLocalizedString(comboItem3.Text);
			comboItem4.Text = localizationManager.GetLocalizedString(comboItem4.Text);
			comboItem5.Text = localizationManager.GetLocalizedString(comboItem5.Text);
			comboItem6.Text = localizationManager.GetLocalizedString(comboItem6.Text);
			((Control)this).Text = localizationManager.GetLocalizedString(((Control)this).Text);
		}
		((Control)cmdReset).Visible = m_DotNetBar.ShowResetButton;
		((Control)cmdKeyboard).Visible = false;
		((Form)this).KeyPreview = true;
		if (!BarFunctions.SupportsAnimation)
		{
			((Control)cboAnimations).Visible = false;
			((Control)label7).Visible = false;
		}
		lstCommands.Style = m_DotNetBar.Style;
		tabCtrl.TabPages.Remove(tabPage2);
		((Control)cmdNew).Visible = false;
		((Control)cmdDelete).Visible = false;
		((Control)cmdRename).Visible = false;
		((Control)lstBars).Width = 300;
	}

	protected override void Dispose(bool disposing)
	{
		if (m_PopupMenu != null)
		{
			m_PopupMenu.Dispose();
			m_PopupMenu = null;
		}
		m_DotNetBar = null;
		if (m_MoveCursor != (Cursor)null)
		{
			m_MoveCursor.Dispose();
		}
		if (m_CopyCursor != (Cursor)null)
		{
			m_CopyCursor.Dispose();
		}
		if (m_NACursor != (Cursor)null)
		{
			m_NACursor.Dispose();
		}
		m_MoveCursor = null;
		m_CopyCursor = null;
		m_NACursor = null;
		if (components != null)
		{
			components.Dispose();
		}
		components = null;
		((Form)this).Dispose(disposing);
	}

	public void RefreshBars()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		m_BarsLoading = true;
		try
		{
			CheckState val = (CheckState)1;
			((ObjectCollection)lstBars.Items).Clear();
			foreach (Bar bar in m_DotNetBar.Bars)
			{
				if ((!bar.CanHide && !bar.CanCustomize && bar.LayoutType == eLayoutType.Toolbar) || bar.LayoutType != 0)
				{
					bar.SetDesignMode(b: true);
					bar.RecalcLayout();
					continue;
				}
				val = ((!bar.Visible) ? ((CheckState)0) : ((CheckState)1));
				lstBars.Items.Add((object)bar, val);
				bar.SetDesignMode(b: true);
				bar.RecalcLayout();
			}
		}
		finally
		{
			m_BarsLoading = false;
		}
	}

	public void RefreshCategories()
	{
		Hashtable hashtable = new Hashtable();
		lstCategories.Items.Clear();
		lstCategories.Sorted = true;
		for (int i = 0; i < m_DotNetBar.Items.Count; i++)
		{
			BaseItem baseItem = m_DotNetBar.Items[i];
			if (baseItem.Category != "" && !hashtable.ContainsKey(baseItem.Category))
			{
				hashtable.Add(baseItem.Category, baseItem.Category);
			}
		}
		foreach (string value in hashtable.Values)
		{
			lstCategories.Items.Add((object)value);
		}
	}

	protected void BarsCheck(object sender, ItemCheckEventArgs ec)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		if (m_BarsLoading || !(((ObjectCollection)lstBars.Items)[ec.Index] is Bar bar))
		{
			return;
		}
		if (!bar.CanHide && bar.Visible)
		{
			ec.NewValue = (CheckState)1;
			return;
		}
		if ((int)ec.NewValue == 1)
		{
			if (!bar.Visible)
			{
				bar.ShowBar();
			}
		}
		else if (bar.Visible)
		{
			bar.HideBar();
		}
		bar.InvokeUserVisibleChanged();
		((IOwner)m_DotNetBar).InvokeUserCustomize((object)bar, new EventArgs());
		((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarVisibilityChanged));
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		RefreshBars();
		RefreshCategories();
		chkShowFullMenus.Checked = m_DotNetBar.AlwaysShowFullMenus;
		chkFullAfterDelay.Checked = m_DotNetBar.ShowFullMenusOnHover;
		chkShowScreenTips.Checked = m_DotNetBar.ShowToolTips;
		chkTipsShowShortcuts.Checked = m_DotNetBar.ShowShortcutKeysInToolTips;
		switch (m_DotNetBar.PopupAnimation)
		{
		case ePopupAnimation.Fade:
			((ComboBox)cboAnimations).SelectedItem = comboItem5;
			break;
		case ePopupAnimation.None:
			((ComboBox)cboAnimations).SelectedItem = comboItem1;
			break;
		case ePopupAnimation.Random:
			((ComboBox)cboAnimations).SelectedItem = comboItem2;
			break;
		case ePopupAnimation.Slide:
			((ComboBox)cboAnimations).SelectedItem = comboItem4;
			break;
		case ePopupAnimation.SystemDefault:
			((ComboBox)cboAnimations).SelectedItem = comboItem6;
			break;
		case ePopupAnimation.Unfold:
			((ComboBox)cboAnimations).SelectedItem = comboItem3;
			break;
		case ePopupAnimation.ManagerControlled:
			break;
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		IOwner dotNetBar = m_DotNetBar;
		dotNetBar.SetFocusItem(null);
		foreach (Bar bar in m_DotNetBar.Bars)
		{
			bar.SetDesignMode(b: false);
			bar.RecalcLayout();
		}
		m_DotNetBar.AlwaysShowFullMenus = chkShowFullMenus.Checked;
		m_DotNetBar.ShowFullMenusOnHover = chkFullAfterDelay.Checked;
		m_DotNetBar.ShowToolTips = chkShowScreenTips.Checked;
		m_DotNetBar.ShowShortcutKeysInToolTips = chkTipsShowShortcuts.Checked;
		if (((ComboBox)cboAnimations).SelectedItem == comboItem5)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.Fade;
		}
		else if (((ComboBox)cboAnimations).SelectedItem == comboItem1)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.None;
		}
		else if (((ComboBox)cboAnimations).SelectedItem == comboItem2)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.Random;
		}
		else if (((ComboBox)cboAnimations).SelectedItem == comboItem4)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.Slide;
		}
		else if (((ComboBox)cboAnimations).SelectedItem == comboItem6)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.SystemDefault;
		}
		else if (((ComboBox)cboAnimations).SelectedItem == comboItem3)
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.Unfold;
		}
		else
		{
			m_DotNetBar.PopupAnimation = ePopupAnimation.SystemDefault;
		}
		m_DotNetBar.CustomizeClosing();
		((Form)this).OnClosing(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		if (m_DotNetBar != null && m_DotNetBar.ParentForm != null)
		{
			((Control)m_DotNetBar.ParentForm).BringToFront();
		}
	}

	protected void Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected void CatSelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)lstCategories).SelectedIndex < 0)
		{
			((ListBox)lstCommands).Items.Clear();
			return;
		}
		string text = lstCategories.Items[((ListControl)lstCategories).SelectedIndex] as string;
		ArrayList arrayList = new ArrayList();
		BaseItem baseItem = null;
		for (int i = 0; i < m_DotNetBar.Items.Count; i++)
		{
			BaseItem baseItem2 = m_DotNetBar.Items[i];
			if (!(baseItem2.Category != text) && !baseItem2.SystemItem)
			{
				baseItem = baseItem2.Copy();
				baseItem.SetDesignMode(b: true);
				baseItem.SetOwner(m_DotNetBar);
				arrayList.Add(baseItem);
			}
		}
		lstCommands.SetItems(arrayList);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	private void MouseMoveDrag(Point pScreen)
	{
		if (!m_ItemDrag)
		{
			return;
		}
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
		}
		foreach (Bar bar in m_DotNetBar.Bars)
		{
			if (!bar.Visible || !bar.AcceptDropItems)
			{
				continue;
			}
			InsertPosition insertPosition = ((IDesignTimeProvider)bar.ItemsContainer).GetInsertPosition(pScreen, DragItem);
			if (insertPosition != null)
			{
				if (insertPosition.TargetProvider == null)
				{
					if (m_NACursor != (Cursor)null)
					{
						Cursor.Current = m_NACursor;
					}
					else
					{
						Cursor.Current = Cursors.No;
					}
					break;
				}
				insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
				m_InsertPosition = insertPosition.Position;
				m_InsertBefore = insertPosition.Before;
				m_DesignTimeProvider = insertPosition.TargetProvider;
				if (m_DragCopy)
				{
					if (m_CopyCursor != (Cursor)null)
					{
						Cursor.Current = m_CopyCursor;
					}
					else
					{
						Cursor.Current = Cursors.Hand;
					}
				}
				else if (m_MoveCursor != (Cursor)null)
				{
					Cursor.Current = m_MoveCursor;
				}
				else
				{
					Cursor.Current = Cursors.Hand;
				}
				break;
			}
			if (m_NACursor != (Cursor)null)
			{
				Cursor.Current = m_NACursor;
			}
			else
			{
				Cursor.Current = Cursors.No;
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	private void MouseUpDrag(Point pScreen)
	{
		if (!m_ItemDrag)
		{
			return;
		}
		DestroyTimer();
		DragItem.InternalMouseLeave();
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			BaseItem parent = DragItem.Parent;
			if (parent != null)
			{
				if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0)
				{
					if (parent.SubItems.IndexOf(DragItem) < m_InsertPosition)
					{
						m_InsertPosition--;
					}
				}
				else
				{
					Bar bar = parent.ContainerControl as Bar;
					if (bar != null && DragItem.OriginalBarName == "")
					{
						DragItem.OriginalBarName = bar.Name;
					}
					else if (bar != null && bar.Name == DragItem.OriginalBarName)
					{
						DragItem.OriginalBarName = "";
					}
				}
				if (DragItem.OriginalPosition < 0)
				{
					if (parent == (BaseItem)m_DesignTimeProvider)
					{
						if (parent.SubItems.IndexOf(DragItem) != m_InsertPosition)
						{
							DragItem.OriginalPosition = parent.SubItems.IndexOf(DragItem);
						}
					}
					else
					{
						DragItem.OriginalPosition = parent.SubItems.IndexOf(DragItem);
					}
				}
				else if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition == DragItem.OriginalPosition)
				{
					DragItem.OriginalPosition = -1;
				}
				parent.SubItems.Remove(DragItem);
				object containerControl = parent.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is Bar)
				{
					((Bar)(object)val).RecalcLayout();
				}
				else if (val is MenuPanel)
				{
					((MenuPanel)(object)val).RecalcSize();
				}
			}
			else
			{
				DragItem.OriginalPosition = 0;
			}
			m_DesignTimeProvider.InsertItemAt(DragItem, m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
			((IOwner)m_DotNetBar).InvokeUserCustomize((object)DragItem, new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)DragItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemMoved));
		}
		else if (DragItem.Parent != null)
		{
			if (m_DotNetBar.ParentForm != null && !((Control)m_DotNetBar.ParentForm).Bounds.Contains(pScreen))
			{
				BaseItem parent2 = DragItem.Parent;
				DragItem.UserCustomized = true;
				DragItem.Visible = false;
				object containerControl2 = parent2.ContainerControl;
				Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
				if (val2 is Bar)
				{
					((Bar)(object)val2).RecalcLayout();
				}
				else if (val2 is MenuPanel)
				{
					((MenuPanel)(object)val2).RecalcSize();
				}
				((IOwner)m_DotNetBar).InvokeUserCustomize((object)DragItem, new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)DragItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted, val2));
			}
		}
		m_ItemDrag = false;
		Cursor.Current = Cursors.Default;
		((Control)this).Capture = false;
		IOwner dotNetBar = m_DotNetBar;
		dotNetBar.SetFocusItem(null);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27 && m_ItemDrag)
		{
			CancelDrag();
			return true;
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	internal void CancelDrag()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		if (m_ItemDrag)
		{
			DestroyTimer();
			if (m_DesignTimeProvider != null)
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			}
			m_ItemDrag = false;
			Cursor.Current = Cursors.Default;
			((Control)this).Capture = false;
			IOwner dotNetBar = m_DotNetBar;
			dotNetBar.SetFocusItem(null);
			if (DragItem != null)
			{
				DragItem.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 1, DragItem.LeftInternal + 1, DragItem.TopInternal + 1, 0));
				DragItem.Refresh();
			}
			DragItem = null;
		}
	}

	protected void Commands_OnMouseDown(object sender, MouseEventArgs e)
	{
		m_MouseDownPt = new Point(e.X, e.Y);
	}

	protected void Commands_OnMouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && (Math.Abs(m_MouseDownPt.X - e.X) > 2 || Math.Abs(m_MouseDownPt.Y - e.Y) > 2) && !m_ItemDrag && ((ListControl)lstCommands).SelectedIndex >= 0 && ((ListBox)lstCommands).Items[((ListControl)lstCommands).SelectedIndex] is BaseItem baseItem)
		{
			DragItem = baseItem.Copy();
			DragItem.SetDesignMode(b: true);
			m_ItemDrag = true;
			m_DragCopy = true;
			((Control)this).Capture = true;
			CreateTimer();
			if (m_NACursor != (Cursor)null)
			{
				Cursor.Current = m_NACursor;
			}
			else
			{
				Cursor.Current = Cursors.No;
			}
		}
	}

	public void DesignTimeContextMenu(BaseItem objItem)
	{
		using LocalizationManager localizationManager = new LocalizationManager(m_DotNetBar);
		m_EditItem = objItem;
		if (m_PopupMenu != null)
		{
			m_PopupMenu.Dispose();
		}
		m_PopupMenu = new ButtonItem("syscustomizepopupmenu");
		m_PopupMenu.Style = objItem.Style;
		ButtonItem buttonItem = new ButtonItem("reset");
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuReset);
		buttonItem.Click += ResetItemClick;
		m_PopupMenu.SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("delete");
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuDelete);
		buttonItem.Click += DeleteClick;
		m_PopupMenu.SubItems.Add(buttonItem);
		TextBoxItem textBoxItem = new TextBoxItem("name");
		textBoxItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuChangeName);
		textBoxItem.BeginGroup = true;
		textBoxItem.ControlText = objItem.Text;
		textBoxItem.LostFocus += ItemNameLostFocus;
		m_PopupMenu.SubItems.Add(textBoxItem);
		if (m_EditItem is ButtonItem)
		{
			ButtonItem buttonItem2 = m_EditItem as ButtonItem;
			buttonItem = new ButtonItem("defaultstyle");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuDefaultStyle);
			buttonItem.BeginGroup = true;
			buttonItem.Click += ButtonStyleClick;
			if (buttonItem2.ButtonStyle == eButtonStyle.Default)
			{
				buttonItem.Checked = true;
			}
			m_PopupMenu.SubItems.Add(buttonItem);
			buttonItem = new ButtonItem("textonly");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuTextOnly);
			buttonItem.Click += ButtonStyleClick;
			if (buttonItem2.ButtonStyle == eButtonStyle.TextOnlyAlways)
			{
				buttonItem.Checked = true;
			}
			m_PopupMenu.SubItems.Add(buttonItem);
			buttonItem = new ButtonItem("imageandtext");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuImageAndText);
			buttonItem.Click += ButtonStyleClick;
			if (buttonItem2.ButtonStyle == eButtonStyle.ImageAndText)
			{
				buttonItem.Checked = true;
			}
			m_PopupMenu.SubItems.Add(buttonItem);
		}
		buttonItem = new ButtonItem("begingroup");
		buttonItem.BeginGroup = true;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuBeginGroup);
		buttonItem.Checked = m_EditItem.BeginGroup;
		buttonItem.Click += BeginGroupClick;
		m_PopupMenu.SubItems.Add(buttonItem);
		m_DotNetBar.OnCustomizeContextMenu(this, m_PopupMenu);
		m_PopupMenu.SetOwner(m_DotNetBar);
		m_PopupMenu.PopupMenu(Control.MousePosition);
	}

	private void ResetItemClick(object sender, EventArgs e)
	{
		((IOwner)m_DotNetBar).InvokeResetDefinition(m_EditItem, e);
	}

	private void ItemNameLostFocus(object sender, EventArgs arg)
	{
		m_EditItem.Text = ((TextBoxItem)m_PopupMenu.SubItems["name"]).ControlText;
		m_EditItem.Refresh();
		((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
		((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemTextChanged));
	}

	private void ButtonStyleClick(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		ButtonItem buttonItem2 = m_EditItem as ButtonItem;
		if (buttonItem != null)
		{
			if (buttonItem.Name == "defaultstyle" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.Default;
				buttonItem2.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if (buttonItem.Name == "textonly" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.TextOnlyAlways;
				buttonItem2.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if (buttonItem.Name == "imageandtext" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.ImageAndText;
				buttonItem2.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
		}
	}

	private void BeginGroupClick(object sender, EventArgs e)
	{
		if (sender is ButtonItem buttonItem)
		{
			buttonItem.Checked = !buttonItem.Checked;
			m_EditItem.BeginGroup = buttonItem.Checked;
			buttonItem.Refresh();
			m_EditItem.Refresh();
			((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemBeginGroupChanged));
		}
	}

	private void DeleteClick(object sender, EventArgs e)
	{
		BaseItem parent = m_EditItem.Parent;
		m_EditItem.UserCustomized = true;
		m_EditItem.Visible = false;
		parent.Refresh();
		if (parent.ContainerControl is Bar bar)
		{
			bar.RecalcLayout();
		}
		((IOwner)m_DotNetBar).InvokeUserCustomize((object)m_EditItem, new EventArgs());
		((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)m_EditItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted));
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 1731)
		{
			((Control)this).Capture = true;
			if (m_MoveCursor != (Cursor)null)
			{
				Cursor.Current = m_MoveCursor;
			}
			else
			{
				Cursor.Current = Cursors.Hand;
			}
			m_ItemDrag = true;
			((Control)this).Focus();
			CreateTimer();
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		chkShowScreenTips = new CheckBox();
		tabCtrl = new TabControl();
		tabPage1 = new TabPage();
		cmdReset = new Button();
		cmdRename = new Button();
		cmdDelete = new Button();
		cmdNew = new Button();
		label1 = new Label();
		lstBars = new CheckedListBox();
		tabPage2 = new TabPage();
		lstCommands = new ItemsListBox();
		lstCategories = new ListBox();
		label4 = new Label();
		label3 = new Label();
		label2 = new Label();
		tabPage3 = new TabPage();
		cboAnimations = new ComboBoxEx();
		comboItem1 = new ComboItem();
		comboItem6 = new ComboItem();
		comboItem2 = new ComboItem();
		comboItem3 = new ComboItem();
		comboItem4 = new ComboItem();
		comboItem5 = new ComboItem();
		label7 = new Label();
		chkTipsShowShortcuts = new CheckBox();
		button1 = new Button();
		label6 = new Label();
		chkFullAfterDelay = new CheckBox();
		chkShowFullMenus = new CheckBox();
		label5 = new Label();
		cmdKeyboard = new Button();
		cmdClose = new Button();
		((Control)tabCtrl).SuspendLayout();
		((Control)tabPage1).SuspendLayout();
		((Control)tabPage2).SuspendLayout();
		((Control)tabPage3).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)chkShowScreenTips).Location = new Point(24, 152);
		((Control)chkShowScreenTips).Name = "chkShowScreenTips";
		((Control)chkShowScreenTips).Size = new Size(320, 16);
		((Control)chkShowScreenTips).TabIndex = 3;
		((Control)chkShowScreenTips).Text = "cust_chk_showst";
		((Control)tabCtrl).Controls.AddRange((Control[])(object)new Control[3]
		{
			(Control)tabPage1,
			(Control)tabPage2,
			(Control)tabPage3
		});
		((Control)tabCtrl).Location = new Point(6, 6);
		((Control)tabCtrl).Name = "tabCtrl";
		tabCtrl.SelectedIndex = 0;
		((Control)tabCtrl).Size = new Size(354, 303);
		((Control)tabCtrl).TabIndex = 0;
		((Control)tabPage1).Controls.AddRange((Control[])(object)new Control[6]
		{
			(Control)cmdReset,
			(Control)cmdRename,
			(Control)cmdDelete,
			(Control)cmdNew,
			(Control)label1,
			(Control)lstBars
		});
		tabPage1.Location = new Point(4, 22);
		((Control)tabPage1).Name = "tabPage1";
		((Control)tabPage1).Size = new Size(346, 277);
		tabPage1.TabIndex = 0;
		((Control)tabPage1).Text = "cust_tab_toolbars";
		tabPage1.Visible = false;
		((Control)cmdReset).Enabled = true;
		((ButtonBase)cmdReset).FlatStyle = (FlatStyle)3;
		((Control)cmdReset).Location = new Point(250, 118);
		((Control)cmdReset).Name = "cmdReset";
		((Control)cmdReset).Size = new Size(90, 24);
		((Control)cmdReset).TabIndex = 4;
		((Control)cmdReset).Text = "cust_btn_reset";
		((Control)cmdReset).Visible = false;
		((Control)cmdReset).Click += ResetBar;
		((Control)cmdRename).Enabled = false;
		((ButtonBase)cmdRename).FlatStyle = (FlatStyle)3;
		((Control)cmdRename).Location = new Point(250, 52);
		((Control)cmdRename).Name = "cmdRename";
		((Control)cmdRename).Size = new Size(90, 24);
		((Control)cmdRename).TabIndex = 2;
		((Control)cmdRename).Text = "cust_btn_rename";
		((Control)cmdRename).Click += RenameBar;
		((Control)cmdDelete).Enabled = false;
		((ButtonBase)cmdDelete).FlatStyle = (FlatStyle)3;
		((Control)cmdDelete).Location = new Point(250, 85);
		((Control)cmdDelete).Name = "cmdDelete";
		((Control)cmdDelete).Size = new Size(90, 24);
		((Control)cmdDelete).TabIndex = 3;
		((Control)cmdDelete).Text = "cust_btn_delete";
		((Control)cmdDelete).Click += DeleteBar;
		((ButtonBase)cmdNew).FlatStyle = (FlatStyle)3;
		((Control)cmdNew).Location = new Point(250, 19);
		((Control)cmdNew).Name = "cmdNew";
		((Control)cmdNew).Size = new Size(90, 24);
		((Control)cmdNew).TabIndex = 1;
		((Control)cmdNew).Text = "cust_btn_new";
		((Control)cmdNew).Click += NewBar;
		((Control)label1).Location = new Point(5, 5);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(259, 12);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "cust_lbl_tlbs";
		((Control)lstBars).Location = new Point(5, 19);
		((Control)lstBars).Name = "lstBars";
		((ListBox)lstBars).IntegralHeight = false;
		((Control)lstBars).Size = new Size(239, 249);
		((Control)lstBars).TabIndex = 0;
		((ListBox)lstBars).SelectedIndexChanged += BarSelectionChanged;
		lstBars.ItemCheck += new ItemCheckEventHandler(BarsCheck);
		((Control)tabPage2).Controls.AddRange((Control[])(object)new Control[5]
		{
			(Control)lstCategories,
			(Control)lstCommands,
			(Control)label4,
			(Control)label3,
			(Control)label2
		});
		tabPage2.Location = new Point(4, 22);
		((Control)tabPage2).Name = "tabPage2";
		((Control)tabPage2).Size = new Size(346, 277);
		tabPage2.TabIndex = 1;
		((Control)tabPage2).Text = "cust_tab_commands";
		tabPage2.Visible = false;
		((Control)lstCommands).BackColor = SystemColors.Control;
		((ListBox)lstCommands).DrawMode = (DrawMode)1;
		((ListBox)lstCommands).IntegralHeight = false;
		((Control)lstCommands).Location = new Point(136, 71);
		((Control)lstCommands).Name = "lstCommands";
		((Control)lstCommands).Size = new Size(200, 197);
		((Control)lstCommands).TabIndex = 2;
		((Control)lstCommands).MouseDown += new MouseEventHandler(Commands_OnMouseDown);
		((Control)lstCommands).MouseMove += new MouseEventHandler(Commands_OnMouseMove);
		((Control)lstCategories).Location = new Point(5, 71);
		((Control)lstCategories).Name = "lstCategories";
		lstCategories.IntegralHeight = false;
		((Control)lstCategories).Size = new Size(121, 197);
		((Control)lstCategories).TabIndex = 2;
		lstCategories.SelectedIndexChanged += CatSelectedIndexChanged;
		((Control)label4).Location = new Point(136, 56);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(197, 13);
		((Control)label4).TabIndex = 1;
		((Control)label4).Text = "cust_lbl_cmds";
		((Control)label3).Location = new Point(5, 56);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(123, 13);
		((Control)label3).TabIndex = 1;
		((Control)label3).Text = "cust_lbl_cats";
		((Control)label2).Location = new Point(4, 4);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(336, 46);
		((Control)label2).TabIndex = 0;
		((Control)label2).Text = "cust_lbl_cmdsins";
		((Control)tabPage3).Controls.AddRange((Control[])(object)new Control[9]
		{
			(Control)chkShowFullMenus,
			(Control)chkFullAfterDelay,
			(Control)button1,
			(Control)chkShowScreenTips,
			(Control)chkTipsShowShortcuts,
			(Control)cboAnimations,
			(Control)label7,
			(Control)label6,
			(Control)label5
		});
		tabPage3.Location = new Point(4, 22);
		((Control)tabPage3).Name = "tabPage3";
		((Control)tabPage3).Size = new Size(346, 277);
		tabPage3.TabIndex = 2;
		((Control)tabPage3).Text = "cust_tab_options";
		tabPage3.Visible = false;
		cboAnimations.DefaultStyle = false;
		cboAnimations.DisableInternalDrawing = false;
		((ComboBox)cboAnimations).DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cboAnimations).DropDownWidth = 120;
		cboAnimations.Images = null;
		cboAnimations.Items.AddRange(new object[6] { comboItem1, comboItem6, comboItem2, comboItem3, comboItem4, comboItem5 });
		((Control)cboAnimations).Location = new Point(24, 224);
		((Control)cboAnimations).Name = "cboAnimations";
		((Control)cboAnimations).Size = new Size(120, 21);
		cboAnimations.Style = eDotNetBarStyle.OfficeXP;
		((Control)cboAnimations).TabIndex = 5;
		comboItem1.BackColor = Color.Empty;
		comboItem1.FontName = "";
		comboItem1.FontSize = 8f;
		comboItem1.FontStyle = (FontStyle)0;
		comboItem1.ForeColor = Color.Empty;
		comboItem1.Image = null;
		comboItem1.ImageIndex = -1;
		comboItem1.ImagePosition = (HorizontalAlignment)0;
		comboItem1.Tag = null;
		comboItem1.Text = "cust_cbo_none";
		comboItem1.TextAlignment = (StringAlignment)0;
		comboItem1.TextLineAlignment = (StringAlignment)0;
		comboItem6.BackColor = Color.Empty;
		comboItem6.FontName = "";
		comboItem6.FontSize = 8f;
		comboItem6.FontStyle = (FontStyle)0;
		comboItem6.ForeColor = Color.Empty;
		comboItem6.Image = null;
		comboItem6.ImageIndex = -1;
		comboItem6.ImagePosition = (HorizontalAlignment)0;
		comboItem6.Tag = null;
		comboItem6.Text = "cust_cbo_system";
		comboItem6.TextAlignment = (StringAlignment)0;
		comboItem6.TextLineAlignment = (StringAlignment)0;
		comboItem2.BackColor = Color.Empty;
		comboItem2.FontName = "";
		comboItem2.FontSize = 8f;
		comboItem2.FontStyle = (FontStyle)0;
		comboItem2.ForeColor = Color.Empty;
		comboItem2.Image = null;
		comboItem2.ImageIndex = -1;
		comboItem2.ImagePosition = (HorizontalAlignment)0;
		comboItem2.Tag = null;
		comboItem2.Text = "cust_cbo_random";
		comboItem2.TextAlignment = (StringAlignment)0;
		comboItem2.TextLineAlignment = (StringAlignment)0;
		comboItem3.BackColor = Color.Empty;
		comboItem3.FontName = "";
		comboItem3.FontSize = 8f;
		comboItem3.FontStyle = (FontStyle)0;
		comboItem3.ForeColor = Color.Empty;
		comboItem3.Image = null;
		comboItem3.ImageIndex = -1;
		comboItem3.ImagePosition = (HorizontalAlignment)0;
		comboItem3.Tag = null;
		comboItem3.Text = "cust_cbo_unfold";
		comboItem3.TextAlignment = (StringAlignment)0;
		comboItem3.TextLineAlignment = (StringAlignment)0;
		comboItem4.BackColor = Color.Empty;
		comboItem4.FontName = "";
		comboItem4.FontSize = 8f;
		comboItem4.FontStyle = (FontStyle)0;
		comboItem4.ForeColor = Color.Empty;
		comboItem4.Image = null;
		comboItem4.ImageIndex = -1;
		comboItem4.ImagePosition = (HorizontalAlignment)0;
		comboItem4.Tag = null;
		comboItem4.Text = "cust_cbo_slide";
		comboItem4.TextAlignment = (StringAlignment)0;
		comboItem4.TextLineAlignment = (StringAlignment)0;
		comboItem5.BackColor = Color.Empty;
		comboItem5.FontName = "";
		comboItem5.FontSize = 8f;
		comboItem5.FontStyle = (FontStyle)0;
		comboItem5.ForeColor = Color.Empty;
		comboItem5.Image = null;
		comboItem5.ImageIndex = -1;
		comboItem5.ImagePosition = (HorizontalAlignment)0;
		comboItem5.Tag = null;
		comboItem5.Text = "cust_cbo_fade";
		comboItem5.TextAlignment = (StringAlignment)0;
		comboItem5.TextLineAlignment = (StringAlignment)0;
		((Control)label7).Location = new Point(24, 208);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(320, 16);
		((Control)label7).TabIndex = 6;
		((Control)label7).Text = "cust_lbl_menuan";
		((Control)chkTipsShowShortcuts).Location = new Point(40, 176);
		((Control)chkTipsShowShortcuts).Name = "chkTipsShowShortcuts";
		((Control)chkTipsShowShortcuts).Size = new Size(304, 16);
		((Control)chkTipsShowShortcuts).TabIndex = 4;
		((Control)chkTipsShowShortcuts).Text = "cust_chk_showsk";
		((ButtonBase)button1).FlatStyle = (FlatStyle)3;
		((Control)button1).Location = new Point(24, 80);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(180, 24);
		((Control)button1).TabIndex = 2;
		((Control)button1).Text = "cust_btn_resetusage";
		((Control)button1).Click += ResetUsageData;
		((Control)label6).Location = new Point(8, 128);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(336, 16);
		((Control)label6).TabIndex = 7;
		((Control)label6).Text = "cust_lbl_other";
		((Control)chkFullAfterDelay).Location = new Point(40, 56);
		((Control)chkFullAfterDelay).Name = "chkFullAfterDelay";
		((Control)chkFullAfterDelay).Size = new Size(304, 16);
		((Control)chkFullAfterDelay).TabIndex = 1;
		((Control)chkFullAfterDelay).Text = "cust_chk_delay";
		((Control)chkShowFullMenus).Location = new Point(24, 32);
		((Control)chkShowFullMenus).Name = "chkShowFullMenus";
		((Control)chkShowFullMenus).Size = new Size(320, 16);
		((Control)chkShowFullMenus).TabIndex = 0;
		((Control)chkShowFullMenus).Text = "cust_chk_fullmenus";
		((Control)label5).Location = new Point(8, 8);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(336, 16);
		((Control)label5).TabIndex = 8;
		((Control)label5).Text = "cust_lbl_pmt";
		((ButtonBase)cmdKeyboard).FlatStyle = (FlatStyle)3;
		((Control)cmdKeyboard).Location = new Point(160, 317);
		((Control)cmdKeyboard).Name = "cmdKeyboard";
		((Control)cmdKeyboard).Size = new Size(96, 24);
		((Control)cmdKeyboard).TabIndex = 1;
		((Control)cmdKeyboard).Text = "cust_btn_keyboard";
		cmdClose.DialogResult = (DialogResult)2;
		((ButtonBase)cmdClose).FlatStyle = (FlatStyle)3;
		((Control)cmdClose).Location = new Point(264, 317);
		((Control)cmdClose).Name = "cmdClose";
		((Control)cmdClose).Size = new Size(96, 24);
		((Control)cmdClose).TabIndex = 2;
		((Control)cmdClose).Text = "cust_btn_close";
		((Control)cmdClose).Click += Close_Click;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(368, 347);
		((Control)this).Controls.AddRange((Control[])(object)new Control[3]
		{
			(Control)cmdClose,
			(Control)cmdKeyboard,
			(Control)tabCtrl
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmCustomize";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "cust_caption";
		((Control)tabCtrl).ResumeLayout(false);
		((Control)tabPage1).ResumeLayout(false);
		((Control)tabPage2).ResumeLayout(false);
		((Control)tabPage3).ResumeLayout(false);
		((Form)this).AcceptButton = (IButtonControl)(object)cmdClose;
		((Form)this).CancelButton = (IButtonControl)(object)cmdClose;
		((Control)this).ResumeLayout(false);
	}

	private void NewBar(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		ToolbarName toolbarName = new ToolbarName();
		((Control)toolbarName.txtName).Text = "Custom Bar";
		using (LocalizationManager localizationManager = new LocalizationManager(m_DotNetBar))
		{
			((Control)toolbarName.txtName).Text = localizationManager.GetLocalizedString("sys_custombar");
		}
		((Form)toolbarName).StartPosition = (FormStartPosition)4;
		if ((int)((Form)toolbarName).ShowDialog((IWin32Window)(object)this) == 1)
		{
			Bar bar = new Bar(((Control)toolbarName.txtName).Text);
			bar.CustomBar = true;
			bar.CanHide = true;
			bar.SetDesignMode(b: true);
			bar.GrabHandleStyle = eGrabHandleStyle.StripeFlat;
			string text = "userBar";
			int num = 0;
			while (m_DotNetBar.Bars.Contains(text + num))
			{
				num++;
			}
			bar.Name = text + num;
			m_DotNetBar.Bars.Add(bar);
			bar.DockSide = eDockSide.None;
			lstBars.Items.Add((object)bar, (CheckState)1);
			if (m_DotNetBar.AllowUserBarCustomize)
			{
				bar.Items.Add(new CustomizeItem());
			}
			((IOwner)m_DotNetBar).InvokeUserCustomize((object)bar, new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.NewBarCreated));
		}
		((Form)toolbarName).Close();
		((Component)(object)toolbarName).Dispose();
	}

	private void BarSelectionChanged(object sender, EventArgs e)
	{
		if (((ListControl)lstBars).SelectedIndex >= 0)
		{
			Bar bar = ((ListBox)lstBars).SelectedItem as Bar;
			if (bar.CustomBar)
			{
				((Control)cmdRename).Enabled = true;
				((Control)cmdReset).Enabled = false;
				((Control)cmdDelete).Enabled = true;
			}
			else
			{
				((Control)cmdRename).Enabled = false;
				((Control)cmdReset).Enabled = m_DotNetBar.ShowResetButton;
				((Control)cmdDelete).Enabled = false;
			}
		}
		else
		{
			((Control)cmdRename).Enabled = false;
			((Control)cmdReset).Enabled = m_DotNetBar.ShowResetButton;
			((Control)cmdDelete).Enabled = false;
		}
	}

	private void RenameBar(object sender, EventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		if (((ListControl)lstBars).SelectedIndex >= 0 && ((ListBox)lstBars).SelectedItem is Bar bar)
		{
			ToolbarName toolbarName = new ToolbarName();
			toolbarName.RenameDialog = true;
			((Control)toolbarName.txtName).Text = ((Control)bar).Text;
			((Form)toolbarName).StartPosition = (FormStartPosition)4;
			if ((int)((Form)toolbarName).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((Control)bar).Text = ((Control)toolbarName.txtName).Text;
				((Control)lstBars).Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize((object)bar, new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarRenamed));
			}
			((Form)toolbarName).Close();
			((Component)(object)toolbarName).Dispose();
		}
	}

	private void DeleteBar(object sender, EventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		if (((ListControl)lstBars).SelectedIndex < 0 || !(((ListBox)lstBars).SelectedItem is Bar { CustomBar: not false } bar))
		{
			return;
		}
		using LocalizationManager localizationManager = new LocalizationManager(m_DotNetBar);
		if ((int)MessageBox.Show(localizationManager.GetLocalizedString(LocalizationKeys.CustomizeDialogOptionsConfirmDelete).Replace("<barname>", "'" + ((Control)bar).Text + "'"), ((Control)this).Text, (MessageBoxButtons)4) == 6)
		{
			((ObjectCollection)lstBars.Items).Remove((object)bar);
			m_DotNetBar.Bars.Remove(bar);
			((IOwner)m_DotNetBar).InvokeUserCustomize((object)bar, new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarDeleted));
			((Component)(object)bar).Dispose();
		}
	}

	private void ResetBar(object sender, EventArgs e)
	{
		IOwner dotNetBar = m_DotNetBar;
		if (((ListControl)lstBars).SelectedIndex >= 0)
		{
			if (((ListBox)lstBars).SelectedItem is Bar bar && (bar.CustomBar || m_DotNetBar.ShowResetButton))
			{
				dotNetBar?.InvokeResetDefinition(bar.ItemsContainer, new EventArgs());
			}
		}
		else
		{
			dotNetBar?.InvokeResetDefinition(null, new EventArgs());
		}
	}

	private void ResetUsageData(object sender, EventArgs e)
	{
		m_DotNetBar.ResetUsageData();
	}

	private void CreateTimer()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (m_Timer != null)
		{
			m_Timer.Start();
			return;
		}
		m_Timer = new Timer();
		m_Timer.Interval = 100;
		m_Timer.Tick += TimerTick;
		m_Timer.Start();
	}

	private void TimerTick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if ((int)Control.MouseButtons == 1048576)
		{
			MouseMoveDrag(Control.MousePosition);
		}
		else
		{
			MouseUpDrag(Control.MousePosition);
		}
	}

	private void DestroyTimer()
	{
		if (m_Timer != null)
		{
			m_Timer.Stop();
			((Component)(object)m_Timer).Dispose();
			m_Timer = null;
		}
	}

	internal DotNetBarManager GetDotNetBarManager()
	{
		return m_DotNetBar;
	}
}
