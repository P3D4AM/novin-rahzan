using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(false)]
public class frmCustomize : Form
{
	private TabControl _2960;

	private Label ḅ;

	private TabPage _2961;

	private TabPage _2962;

	private TabPage _2963;

	private Button _2964;

	private Button _2965;

	private Button _2966;

	private Button _2180;

	private CheckedListBox _2967;

	private DotNetBarManager _2968;

	private Button _2969;

	private Button _296A;

	private Label Ḇ;

	private Label Ḋ;

	private Label ḋ;

	private ListBox _296B;

	private ᆓ _296C;

	private bool _296D;

	private Cursor _09E4;

	private Cursor ᇩ;

	private Cursor _09E5;

	private IDesignTimeProvider _৫;

	private int _৬;

	private bool _৭;

	private bool _296E;

	private Point _0AC2;

	public BaseItem DragItem;

	private BaseItem _296F;

	private ButtonItem ಔ;

	private Label ḍ;

	private CheckBox _2970;

	private CheckBox _2971;

	private Label Ḏ;

	private Button ḇ;

	private CheckBox _2972;

	private CheckBox _2973;

	private Label Ḑ;

	private ComboItem _2974;

	private ComboItem _2975;

	private ComboItem _2976;

	private ComboItem _2977;

	private ComboItem _2978;

	private ComboBoxEx _2979;

	private ComboItem _297A;

	private Timer ᆪ;

	private Container Պ;

	private bool _297B;

	public bool DragInProgress => _296D;

	public frmCustomize(DotNetBarManager ctrl)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		ـ();
		_2968 = ctrl;
		((Form)this).StartPosition = (FormStartPosition)1;
		_296D = false;
		try
		{
			_09E4 = new Cursor(typeof(DotNetBarManager), "DRAGMOVE.CUR");
			ᇩ = new Cursor(typeof(DotNetBarManager), "DRAGCOPY.CUR");
			_09E5 = new Cursor(typeof(DotNetBarManager), "DRAGNONE.CUR");
		}
		catch (Exception)
		{
			_09E4 = null;
			ᇩ = null;
			_09E5 = null;
		}
		_৫ = null;
		_296E = false;
		_296F = null;
		using (LocalizationManager localizationManager = new LocalizationManager(_2968))
		{
			((Control)_2964).Text = localizationManager.GetLocalizedString(((Control)_2964).Text);
			((Control)_2963).Text = localizationManager.GetLocalizedString(((Control)_2963).Text);
			((Control)_2180).Text = localizationManager.GetLocalizedString(((Control)_2180).Text);
			((Control)_2966).Text = localizationManager.GetLocalizedString(((Control)_2966).Text);
			((Control)_2965).Text = localizationManager.GetLocalizedString(((Control)_2965).Text);
			((Control)ḅ).Text = localizationManager.GetLocalizedString(((Control)ḅ).Text);
			((Control)_2961).Text = localizationManager.GetLocalizedString(((Control)_2961).Text);
			((Control)ḋ).Text = localizationManager.GetLocalizedString(((Control)ḋ).Text);
			((Control)Ḋ).Text = localizationManager.GetLocalizedString(((Control)Ḋ).Text);
			((Control)Ḇ).Text = localizationManager.GetLocalizedString(((Control)Ḇ).Text);
			((Control)_2962).Text = localizationManager.GetLocalizedString(((Control)_2962).Text);
			((Control)_2969).Text = localizationManager.GetLocalizedString(((Control)_2969).Text);
			((Control)_296A).Text = localizationManager.GetLocalizedString(((Control)_296A).Text);
			((Control)ḍ).Text = localizationManager.GetLocalizedString(((Control)ḍ).Text);
			((Control)_2970).Text = localizationManager.GetLocalizedString(((Control)_2970).Text);
			((Control)_2971).Text = localizationManager.GetLocalizedString(((Control)_2971).Text);
			((Control)Ḏ).Text = localizationManager.GetLocalizedString(((Control)Ḏ).Text);
			((Control)ḇ).Text = localizationManager.GetLocalizedString(((Control)ḇ).Text);
			((Control)_2972).Text = localizationManager.GetLocalizedString(((Control)_2972).Text);
			((Control)_2973).Text = localizationManager.GetLocalizedString(((Control)_2973).Text);
			((Control)Ḑ).Text = localizationManager.GetLocalizedString(((Control)Ḑ).Text);
			_2974.Text = localizationManager.GetLocalizedString(_2974.Text);
			_2975.Text = localizationManager.GetLocalizedString(_2975.Text);
			_2976.Text = localizationManager.GetLocalizedString(_2976.Text);
			_2977.Text = localizationManager.GetLocalizedString(_2977.Text);
			_2978.Text = localizationManager.GetLocalizedString(_2978.Text);
			_297A.Text = localizationManager.GetLocalizedString(_297A.Text);
			((Control)this).Text = localizationManager.GetLocalizedString(((Control)this).Text);
		}
		((Control)_2180).Visible = _2968.ShowResetButton;
		((Control)_2969).Visible = false;
		((Form)this).KeyPreview = true;
		if (!BarFunctions.SupportsAnimation)
		{
			((Control)_2979).Visible = false;
			((Control)Ḑ).Visible = false;
		}
		_296C.ߕ = _2968.Style;
	}

	protected override void Dispose(bool disposing)
	{
		if (ಔ != null)
		{
			ಔ.Dispose();
			ಔ = null;
		}
		_2968 = null;
		if (_09E4 != (Cursor)null)
		{
			_09E4.Dispose();
		}
		if (ᇩ != (Cursor)null)
		{
			ᇩ.Dispose();
		}
		if (_09E5 != (Cursor)null)
		{
			_09E5.Dispose();
		}
		_09E4 = null;
		ᇩ = null;
		_09E5 = null;
		if (Պ != null)
		{
			Պ.Dispose();
		}
		Պ = null;
		((Form)this).Dispose(disposing);
	}

	public void RefreshBars()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		_297B = true;
		try
		{
			CheckState val = (CheckState)1;
			((ObjectCollection)_2967.Items).Clear();
			foreach (Bar bar in _2968.Bars)
			{
				if ((!bar.CanHide && !bar.CanCustomize && bar.LayoutType == eLayoutType.Toolbar) || bar.LayoutType != 0)
				{
					bar.SetDesignMode(b: true);
					bar.RecalcLayout();
					continue;
				}
				val = ((!bar.Visible) ? ((CheckState)0) : ((CheckState)1));
				_2967.Items.Add((object)bar, val);
				bar.SetDesignMode(b: true);
				bar.RecalcLayout();
			}
		}
		finally
		{
			_297B = false;
		}
	}

	public void RefreshCategories()
	{
		Hashtable hashtable = new Hashtable();
		_296B.Items.Clear();
		_296B.Sorted = true;
		for (int i = 0; i < _2968.Items.Count; i++)
		{
			BaseItem baseItem = _2968.Items[i];
			if (baseItem.Category != "" && !hashtable.ContainsKey(baseItem.Category))
			{
				hashtable.Add(baseItem.Category, baseItem.Category);
			}
		}
		foreach (string value in hashtable.Values)
		{
			_296B.Items.Add((object)value);
		}
	}

	protected void BarsCheck(object sender, ItemCheckEventArgs ec)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		if (_297B || !(((ObjectCollection)_2967.Items)[ec.Index] is Bar bar))
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
				bar._0C3A();
			}
		}
		else if (bar.Visible)
		{
			bar.స();
		}
		bar.ఒ();
		((IOwner)_2968).InvokeUserCustomize((object)bar, new EventArgs());
		((IOwner)_2968).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarVisibilityChanged));
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		RefreshBars();
		RefreshCategories();
		_2970.Checked = _2968.AlwaysShowFullMenus;
		_2971.Checked = _2968.ShowFullMenusOnHover;
		_2972.Checked = _2968.ShowToolTips;
		_2973.Checked = _2968.ShowShortcutKeysInToolTips;
		switch (_2968.PopupAnimation)
		{
		case ePopupAnimation.Fade:
			((ComboBox)_2979).SelectedItem = _2978;
			break;
		case ePopupAnimation.None:
			((ComboBox)_2979).SelectedItem = _2974;
			break;
		case ePopupAnimation.Random:
			((ComboBox)_2979).SelectedItem = _2975;
			break;
		case ePopupAnimation.Slide:
			((ComboBox)_2979).SelectedItem = _2977;
			break;
		case ePopupAnimation.SystemDefault:
			((ComboBox)_2979).SelectedItem = _297A;
			break;
		case ePopupAnimation.Unfold:
			((ComboBox)_2979).SelectedItem = _2976;
			break;
		case ePopupAnimation.ManagerControlled:
			break;
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		IOwner owner = _2968;
		owner.SetFocusItem(null);
		foreach (Bar bar in _2968.Bars)
		{
			bar.SetDesignMode(b: false);
			bar.RecalcLayout();
		}
		_2968.AlwaysShowFullMenus = _2970.Checked;
		_2968.ShowFullMenusOnHover = _2971.Checked;
		_2968.ShowToolTips = _2972.Checked;
		_2968.ShowShortcutKeysInToolTips = _2973.Checked;
		if (((ComboBox)_2979).SelectedItem == _2978)
		{
			_2968.PopupAnimation = ePopupAnimation.Fade;
		}
		else if (((ComboBox)_2979).SelectedItem == _2974)
		{
			_2968.PopupAnimation = ePopupAnimation.None;
		}
		else if (((ComboBox)_2979).SelectedItem == _2975)
		{
			_2968.PopupAnimation = ePopupAnimation.Random;
		}
		else if (((ComboBox)_2979).SelectedItem == _2977)
		{
			_2968.PopupAnimation = ePopupAnimation.Slide;
		}
		else if (((ComboBox)_2979).SelectedItem == _297A)
		{
			_2968.PopupAnimation = ePopupAnimation.SystemDefault;
		}
		else if (((ComboBox)_2979).SelectedItem == _2976)
		{
			_2968.PopupAnimation = ePopupAnimation.Unfold;
		}
		else
		{
			_2968.PopupAnimation = ePopupAnimation.SystemDefault;
		}
		_2968.CustomizeClosing();
		((Form)this).OnClosing(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		if (_2968 != null && _2968.ParentForm != null)
		{
			((Control)_2968.ParentForm).BringToFront();
		}
	}

	protected void Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected void CatSelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)_296B).SelectedIndex < 0)
		{
			((ListBox)_296C).Items.Clear();
			return;
		}
		string text = _296B.Items[((ListControl)_296B).SelectedIndex] as string;
		ArrayList arrayList = new ArrayList();
		BaseItem baseItem = null;
		for (int i = 0; i < _2968.Items.Count; i++)
		{
			BaseItem baseItem2 = _2968.Items[i];
			if (!(baseItem2.Category != text) && !baseItem2.SystemItem)
			{
				baseItem = baseItem2.Copy();
				if (baseItem is ButtonItem)
				{
					((ButtonItem)baseItem).FixedSize = Size.Empty;
				}
				baseItem.SetDesignMode(b: true);
				baseItem.SetOwner(_2968);
				arrayList.Add(baseItem);
			}
		}
		_296C.ᆔ(arrayList);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	private void _297C(Point ॺ)
	{
		if (!_296D)
		{
			return;
		}
		if (_৫ != null)
		{
			_৫.DrawReversibleMarker(_৬, _৭);
			_৫ = null;
		}
		foreach (Bar bar in _2968.Bars)
		{
			if (!bar.Visible || !bar.AcceptDropItems)
			{
				continue;
			}
			InsertPosition insertPosition = ((IDesignTimeProvider)bar.ItemsContainer).GetInsertPosition(ॺ, DragItem);
			if (insertPosition != null)
			{
				if (insertPosition.TargetProvider == null)
				{
					if (_09E5 != (Cursor)null)
					{
						Cursor.Current = _09E5;
					}
					else
					{
						Cursor.Current = Cursors.No;
					}
					break;
				}
				insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
				_৬ = insertPosition.Position;
				_৭ = insertPosition.Before;
				_৫ = insertPosition.TargetProvider;
				if (_296E)
				{
					if (ᇩ != (Cursor)null)
					{
						Cursor.Current = ᇩ;
					}
					else
					{
						Cursor.Current = Cursors.Hand;
					}
				}
				else if (_09E4 != (Cursor)null)
				{
					Cursor.Current = _09E4;
				}
				else
				{
					Cursor.Current = Cursors.Hand;
				}
				break;
			}
			if (_09E5 != (Cursor)null)
			{
				Cursor.Current = _09E5;
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

	private void _297D(Point ॺ)
	{
		if (!_296D)
		{
			return;
		}
		ᆵ();
		DragItem.InternalMouseLeave();
		if (_৫ != null)
		{
			_৫.DrawReversibleMarker(_৬, _৭);
			BaseItem parent = DragItem.Parent;
			if (parent != null)
			{
				if (parent == (BaseItem)_৫ && _৬ > 0)
				{
					if (parent.SubItems.IndexOf(DragItem) < _৬)
					{
						_৬--;
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
					if (parent == (BaseItem)_৫)
					{
						if (parent.SubItems.IndexOf(DragItem) != _৬)
						{
							DragItem.OriginalPosition = parent.SubItems.IndexOf(DragItem);
						}
					}
					else
					{
						DragItem.OriginalPosition = parent.SubItems.IndexOf(DragItem);
					}
				}
				else if (parent == (BaseItem)_৫ && _৬ == DragItem.OriginalPosition)
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
			_৫.InsertItemAt(DragItem, _৬, _৭);
			_৫ = null;
			((IOwner)_2968).InvokeUserCustomize((object)DragItem, new EventArgs());
			((IOwner)_2968).InvokeEndUserCustomize((object)DragItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemMoved));
		}
		else if (DragItem.Parent != null)
		{
			if (_2968.ParentForm != null && !((Control)_2968.ParentForm).Bounds.Contains(ॺ))
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
				((IOwner)_2968).InvokeUserCustomize((object)DragItem, new EventArgs());
				((IOwner)_2968).InvokeEndUserCustomize((object)DragItem, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted, val2));
			}
		}
		_296D = false;
		Cursor.Current = Cursors.Default;
		((Control)this).Capture = false;
		IOwner owner = _2968;
		owner.SetFocusItem(null);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27 && _296D)
		{
			_297E();
			return true;
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	internal void _297E()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		if (_296D)
		{
			ᆵ();
			if (_৫ != null)
			{
				_৫.DrawReversibleMarker(_৬, _৭);
			}
			_296D = false;
			Cursor.Current = Cursors.Default;
			((Control)this).Capture = false;
			IOwner owner = _2968;
			owner.SetFocusItem(null);
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
		_0AC2 = new Point(e.X, e.Y);
	}

	protected void Commands_OnMouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && (Math.Abs(_0AC2.X - e.X) > 2 || Math.Abs(_0AC2.Y - e.Y) > 2) && !_296D && ((ListControl)_296C).SelectedIndex >= 0 && ((ListBox)_296C).Items[((ListControl)_296C).SelectedIndex] is BaseItem baseItem)
		{
			DragItem = baseItem.Copy();
			DragItem.SetDesignMode(b: true);
			_296D = true;
			_296E = true;
			((Control)this).Capture = true;
			ᆱ();
			if (_09E5 != (Cursor)null)
			{
				Cursor.Current = _09E5;
			}
			else
			{
				Cursor.Current = Cursors.No;
			}
		}
	}

	public void DesignTimeContextMenu(BaseItem objItem)
	{
		using LocalizationManager localizationManager = new LocalizationManager(_2968);
		_296F = objItem;
		if (ಔ != null)
		{
			ಔ.Dispose();
		}
		ಔ = new ButtonItem("syscustomizepopupmenu");
		ಔ.Style = objItem.Style;
		ButtonItem buttonItem = new ButtonItem("reset");
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuReset);
		buttonItem.Click += _297F;
		ಔ.SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("delete");
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuDelete);
		buttonItem.Click += _2983;
		ಔ.SubItems.Add(buttonItem);
		TextBoxItem textBoxItem = new TextBoxItem("name");
		textBoxItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuChangeName);
		textBoxItem.BeginGroup = true;
		textBoxItem.ControlText = objItem.Text;
		textBoxItem.LostFocus += _2980;
		ಔ.SubItems.Add(textBoxItem);
		if (_296F is ButtonItem)
		{
			ButtonItem buttonItem2 = _296F as ButtonItem;
			buttonItem = new ButtonItem("defaultstyle");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuDefaultStyle);
			buttonItem.BeginGroup = true;
			buttonItem.Click += _2981;
			if (buttonItem2.ButtonStyle == eButtonStyle.Default)
			{
				buttonItem.Checked = true;
			}
			ಔ.SubItems.Add(buttonItem);
			buttonItem = new ButtonItem("textonly");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuTextOnly);
			buttonItem.Click += _2981;
			if (buttonItem2.ButtonStyle == eButtonStyle.TextOnlyAlways)
			{
				buttonItem.Checked = true;
			}
			ಔ.SubItems.Add(buttonItem);
			buttonItem = new ButtonItem("imageandtext");
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuImageAndText);
			buttonItem.Click += _2981;
			if (buttonItem2.ButtonStyle == eButtonStyle.ImageAndText)
			{
				buttonItem.Checked = true;
			}
			ಔ.SubItems.Add(buttonItem);
		}
		buttonItem = new ButtonItem("begingroup");
		buttonItem.BeginGroup = true;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeMenuBeginGroup);
		buttonItem.Checked = _296F.BeginGroup;
		buttonItem.Click += _2982;
		ಔ.SubItems.Add(buttonItem);
		_2968.OnCustomizeContextMenu(this, ಔ);
		ಔ.SetOwner(_2968);
		ಔ.PopupMenu(Control.MousePosition);
	}

	private void _297F(object _٠, EventArgs ؿ)
	{
		((IOwner)_2968).InvokeResetDefinition(_296F, ؿ);
	}

	private void _2980(object _٠, EventArgs _1AE9)
	{
		_296F.Text = ((TextBoxItem)ಔ.SubItems["name"]).ControlText;
		_296F.Refresh();
		((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
		((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemTextChanged));
	}

	private void _2981(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = _٠ as ButtonItem;
		ButtonItem buttonItem2 = _296F as ButtonItem;
		if (buttonItem != null)
		{
			if (buttonItem.Name == "defaultstyle" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.Default;
				buttonItem2.Refresh();
				((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
				((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if (buttonItem.Name == "textonly" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.TextOnlyAlways;
				buttonItem2.Refresh();
				((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
				((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if (buttonItem.Name == "imageandtext" && !buttonItem.Checked)
			{
				buttonItem2.ButtonStyle = eButtonStyle.ImageAndText;
				buttonItem2.Refresh();
				((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
				((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
		}
	}

	private void _2982(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem buttonItem)
		{
			buttonItem.Checked = !buttonItem.Checked;
			_296F.BeginGroup = buttonItem.Checked;
			buttonItem.Refresh();
			_296F.Refresh();
			((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
			((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemBeginGroupChanged));
		}
	}

	private void _2983(object _٠, EventArgs ؿ)
	{
		BaseItem parent = _296F.Parent;
		_296F.UserCustomized = true;
		_296F.Visible = false;
		parent.Refresh();
		if (parent.ContainerControl is Bar bar)
		{
			bar.RecalcLayout();
		}
		((IOwner)_2968).InvokeUserCustomize((object)_296F, new EventArgs());
		((IOwner)_2968).InvokeEndUserCustomize((object)_296F, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted));
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 1731)
		{
			((Control)this).Capture = true;
			if (_09E4 != (Cursor)null)
			{
				Cursor.Current = _09E4;
			}
			else
			{
				Cursor.Current = Cursors.Hand;
			}
			_296D = true;
			((Control)this).Focus();
			ᆱ();
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	private void ـ()
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
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		_2972 = new CheckBox();
		_2960 = new TabControl();
		_2963 = new TabPage();
		_2180 = new Button();
		_2966 = new Button();
		_2965 = new Button();
		_2964 = new Button();
		ḅ = new Label();
		_2967 = new CheckedListBox();
		_2961 = new TabPage();
		_296C = new ᆓ();
		_296B = new ListBox();
		ḋ = new Label();
		Ḋ = new Label();
		Ḇ = new Label();
		_2962 = new TabPage();
		_2979 = new ComboBoxEx();
		_2974 = new ComboItem();
		_297A = new ComboItem();
		_2975 = new ComboItem();
		_2976 = new ComboItem();
		_2977 = new ComboItem();
		_2978 = new ComboItem();
		Ḑ = new Label();
		_2973 = new CheckBox();
		ḇ = new Button();
		Ḏ = new Label();
		_2971 = new CheckBox();
		_2970 = new CheckBox();
		ḍ = new Label();
		_2969 = new Button();
		_296A = new Button();
		((Control)_2960).SuspendLayout();
		((Control)_2963).SuspendLayout();
		((Control)_2961).SuspendLayout();
		((Control)_2962).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)_2972).Location = new Point(24, 152);
		((Control)_2972).Name = "chkShowScreenTips";
		((Control)_2972).Size = new Size(320, 16);
		((Control)_2972).TabIndex = 3;
		((Control)_2972).Text = "cust_chk_showst";
		((Control)_2960).Controls.AddRange((Control[])(object)new Control[3]
		{
			(Control)_2963,
			(Control)_2961,
			(Control)_2962
		});
		((Control)_2960).Location = new Point(6, 6);
		((Control)_2960).Name = "tabCtrl";
		_2960.SelectedIndex = 0;
		((Control)_2960).Size = new Size(354, 303);
		((Control)_2960).TabIndex = 0;
		((Control)_2963).Controls.AddRange((Control[])(object)new Control[6]
		{
			(Control)_2180,
			(Control)_2966,
			(Control)_2965,
			(Control)_2964,
			(Control)ḅ,
			(Control)_2967
		});
		_2963.Location = new Point(4, 22);
		((Control)_2963).Name = "tabPage1";
		((Control)_2963).Size = new Size(346, 277);
		_2963.TabIndex = 0;
		((Control)_2963).Text = "cust_tab_toolbars";
		_2963.Visible = false;
		((Control)_2180).Enabled = true;
		((ButtonBase)_2180).FlatStyle = (FlatStyle)3;
		((Control)_2180).Location = new Point(250, 118);
		((Control)_2180).Name = "cmdReset";
		((Control)_2180).Size = new Size(90, 24);
		((Control)_2180).TabIndex = 4;
		((Control)_2180).Text = "cust_btn_reset";
		((Control)_2180).Visible = false;
		((Control)_2180).Click += _2988;
		((Control)_2966).Enabled = false;
		((ButtonBase)_2966).FlatStyle = (FlatStyle)3;
		((Control)_2966).Location = new Point(250, 52);
		((Control)_2966).Name = "cmdRename";
		((Control)_2966).Size = new Size(90, 24);
		((Control)_2966).TabIndex = 2;
		((Control)_2966).Text = "cust_btn_rename";
		((Control)_2966).Click += _2986;
		((Control)_2965).Enabled = false;
		((ButtonBase)_2965).FlatStyle = (FlatStyle)3;
		((Control)_2965).Location = new Point(250, 85);
		((Control)_2965).Name = "cmdDelete";
		((Control)_2965).Size = new Size(90, 24);
		((Control)_2965).TabIndex = 3;
		((Control)_2965).Text = "cust_btn_delete";
		((Control)_2965).Click += _2987;
		((ButtonBase)_2964).FlatStyle = (FlatStyle)3;
		((Control)_2964).Location = new Point(250, 19);
		((Control)_2964).Name = "cmdNew";
		((Control)_2964).Size = new Size(90, 24);
		((Control)_2964).TabIndex = 1;
		((Control)_2964).Text = "cust_btn_new";
		((Control)_2964).Click += _2984;
		((Control)ḅ).Location = new Point(5, 5);
		((Control)ḅ).Name = "label1";
		((Control)ḅ).Size = new Size(259, 12);
		((Control)ḅ).TabIndex = 0;
		((Control)ḅ).Text = "cust_lbl_tlbs";
		((Control)_2967).Location = new Point(5, 19);
		((Control)_2967).Name = "lstBars";
		((ListBox)_2967).IntegralHeight = false;
		((Control)_2967).Size = new Size(239, 249);
		((Control)_2967).TabIndex = 0;
		((ListBox)_2967).SelectedIndexChanged += _2985;
		_2967.ItemCheck += new ItemCheckEventHandler(BarsCheck);
		((Control)_2961).Controls.AddRange((Control[])(object)new Control[5]
		{
			(Control)_296B,
			(Control)_296C,
			(Control)ḋ,
			(Control)Ḋ,
			(Control)Ḇ
		});
		_2961.Location = new Point(4, 22);
		((Control)_2961).Name = "tabPage2";
		((Control)_2961).Size = new Size(346, 277);
		_2961.TabIndex = 1;
		((Control)_2961).Text = "cust_tab_commands";
		_2961.Visible = false;
		((Control)_296C).BackColor = SystemColors.Control;
		((ListBox)_296C).DrawMode = (DrawMode)1;
		((ListBox)_296C).IntegralHeight = false;
		((Control)_296C).Location = new Point(136, 71);
		((Control)_296C).Name = "lstCommands";
		((Control)_296C).Size = new Size(200, 197);
		((Control)_296C).TabIndex = 2;
		((Control)_296C).MouseDown += new MouseEventHandler(Commands_OnMouseDown);
		((Control)_296C).MouseMove += new MouseEventHandler(Commands_OnMouseMove);
		((Control)_296B).Location = new Point(5, 71);
		((Control)_296B).Name = "lstCategories";
		_296B.IntegralHeight = false;
		((Control)_296B).Size = new Size(121, 197);
		((Control)_296B).TabIndex = 2;
		_296B.SelectedIndexChanged += CatSelectedIndexChanged;
		((Control)ḋ).Location = new Point(136, 56);
		((Control)ḋ).Name = "label4";
		((Control)ḋ).Size = new Size(197, 13);
		((Control)ḋ).TabIndex = 1;
		((Control)ḋ).Text = "cust_lbl_cmds";
		((Control)Ḋ).Location = new Point(5, 56);
		((Control)Ḋ).Name = "label3";
		((Control)Ḋ).Size = new Size(123, 13);
		((Control)Ḋ).TabIndex = 1;
		((Control)Ḋ).Text = "cust_lbl_cats";
		((Control)Ḇ).Location = new Point(4, 4);
		((Control)Ḇ).Name = "label2";
		((Control)Ḇ).Size = new Size(336, 46);
		((Control)Ḇ).TabIndex = 0;
		((Control)Ḇ).Text = "cust_lbl_cmdsins";
		((Control)_2962).Controls.AddRange((Control[])(object)new Control[9]
		{
			(Control)_2970,
			(Control)_2971,
			(Control)ḇ,
			(Control)_2972,
			(Control)_2973,
			(Control)_2979,
			(Control)Ḑ,
			(Control)Ḏ,
			(Control)ḍ
		});
		_2962.Location = new Point(4, 22);
		((Control)_2962).Name = "tabPage3";
		((Control)_2962).Size = new Size(346, 277);
		_2962.TabIndex = 2;
		((Control)_2962).Text = "cust_tab_options";
		_2962.Visible = false;
		_2979.DefaultStyle = false;
		_2979.DisableInternalDrawing = false;
		((ComboBox)_2979).DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)_2979).DropDownWidth = 120;
		_2979.Images = null;
		_2979.Items.AddRange(new object[6] { _2974, _297A, _2975, _2976, _2977, _2978 });
		((Control)_2979).Location = new Point(24, 224);
		((Control)_2979).Name = "cboAnimations";
		((Control)_2979).Size = new Size(120, 21);
		_2979.Style = eDotNetBarStyle.OfficeXP;
		((Control)_2979).TabIndex = 5;
		_2974.BackColor = Color.Empty;
		_2974.FontName = "";
		_2974.FontSize = 8f;
		_2974.FontStyle = (FontStyle)0;
		_2974.ForeColor = Color.Empty;
		_2974.Image = null;
		_2974.ImageIndex = -1;
		_2974.ImagePosition = (HorizontalAlignment)0;
		_2974.Tag = null;
		_2974.Text = "cust_cbo_none";
		_2974.TextAlignment = (StringAlignment)0;
		_2974.TextLineAlignment = (StringAlignment)0;
		_297A.BackColor = Color.Empty;
		_297A.FontName = "";
		_297A.FontSize = 8f;
		_297A.FontStyle = (FontStyle)0;
		_297A.ForeColor = Color.Empty;
		_297A.Image = null;
		_297A.ImageIndex = -1;
		_297A.ImagePosition = (HorizontalAlignment)0;
		_297A.Tag = null;
		_297A.Text = "cust_cbo_system";
		_297A.TextAlignment = (StringAlignment)0;
		_297A.TextLineAlignment = (StringAlignment)0;
		_2975.BackColor = Color.Empty;
		_2975.FontName = "";
		_2975.FontSize = 8f;
		_2975.FontStyle = (FontStyle)0;
		_2975.ForeColor = Color.Empty;
		_2975.Image = null;
		_2975.ImageIndex = -1;
		_2975.ImagePosition = (HorizontalAlignment)0;
		_2975.Tag = null;
		_2975.Text = "cust_cbo_random";
		_2975.TextAlignment = (StringAlignment)0;
		_2975.TextLineAlignment = (StringAlignment)0;
		_2976.BackColor = Color.Empty;
		_2976.FontName = "";
		_2976.FontSize = 8f;
		_2976.FontStyle = (FontStyle)0;
		_2976.ForeColor = Color.Empty;
		_2976.Image = null;
		_2976.ImageIndex = -1;
		_2976.ImagePosition = (HorizontalAlignment)0;
		_2976.Tag = null;
		_2976.Text = "cust_cbo_unfold";
		_2976.TextAlignment = (StringAlignment)0;
		_2976.TextLineAlignment = (StringAlignment)0;
		_2977.BackColor = Color.Empty;
		_2977.FontName = "";
		_2977.FontSize = 8f;
		_2977.FontStyle = (FontStyle)0;
		_2977.ForeColor = Color.Empty;
		_2977.Image = null;
		_2977.ImageIndex = -1;
		_2977.ImagePosition = (HorizontalAlignment)0;
		_2977.Tag = null;
		_2977.Text = "cust_cbo_slide";
		_2977.TextAlignment = (StringAlignment)0;
		_2977.TextLineAlignment = (StringAlignment)0;
		_2978.BackColor = Color.Empty;
		_2978.FontName = "";
		_2978.FontSize = 8f;
		_2978.FontStyle = (FontStyle)0;
		_2978.ForeColor = Color.Empty;
		_2978.Image = null;
		_2978.ImageIndex = -1;
		_2978.ImagePosition = (HorizontalAlignment)0;
		_2978.Tag = null;
		_2978.Text = "cust_cbo_fade";
		_2978.TextAlignment = (StringAlignment)0;
		_2978.TextLineAlignment = (StringAlignment)0;
		((Control)Ḑ).Location = new Point(24, 208);
		((Control)Ḑ).Name = "label7";
		((Control)Ḑ).Size = new Size(320, 16);
		((Control)Ḑ).TabIndex = 6;
		((Control)Ḑ).Text = "cust_lbl_menuan";
		((Control)_2973).Location = new Point(40, 176);
		((Control)_2973).Name = "chkTipsShowShortcuts";
		((Control)_2973).Size = new Size(304, 16);
		((Control)_2973).TabIndex = 4;
		((Control)_2973).Text = "cust_chk_showsk";
		((ButtonBase)ḇ).FlatStyle = (FlatStyle)3;
		((Control)ḇ).Location = new Point(24, 80);
		((Control)ḇ).Name = "button1";
		((Control)ḇ).Size = new Size(180, 24);
		((Control)ḇ).TabIndex = 2;
		((Control)ḇ).Text = "cust_btn_resetusage";
		((Control)ḇ).Click += _2989;
		((Control)Ḏ).Location = new Point(8, 128);
		((Control)Ḏ).Name = "label6";
		((Control)Ḏ).Size = new Size(336, 16);
		((Control)Ḏ).TabIndex = 7;
		((Control)Ḏ).Text = "cust_lbl_other";
		((Control)_2971).Location = new Point(40, 56);
		((Control)_2971).Name = "chkFullAfterDelay";
		((Control)_2971).Size = new Size(304, 16);
		((Control)_2971).TabIndex = 1;
		((Control)_2971).Text = "cust_chk_delay";
		((Control)_2970).Location = new Point(24, 32);
		((Control)_2970).Name = "chkShowFullMenus";
		((Control)_2970).Size = new Size(320, 16);
		((Control)_2970).TabIndex = 0;
		((Control)_2970).Text = "cust_chk_fullmenus";
		((Control)ḍ).Location = new Point(8, 8);
		((Control)ḍ).Name = "label5";
		((Control)ḍ).Size = new Size(336, 16);
		((Control)ḍ).TabIndex = 8;
		((Control)ḍ).Text = "cust_lbl_pmt";
		((ButtonBase)_2969).FlatStyle = (FlatStyle)3;
		((Control)_2969).Location = new Point(160, 317);
		((Control)_2969).Name = "cmdKeyboard";
		((Control)_2969).Size = new Size(96, 24);
		((Control)_2969).TabIndex = 1;
		((Control)_2969).Text = "cust_btn_keyboard";
		_296A.DialogResult = (DialogResult)2;
		((ButtonBase)_296A).FlatStyle = (FlatStyle)3;
		((Control)_296A).Location = new Point(264, 317);
		((Control)_296A).Name = "cmdClose";
		((Control)_296A).Size = new Size(96, 24);
		((Control)_296A).TabIndex = 2;
		((Control)_296A).Text = "cust_btn_close";
		((Control)_296A).Click += Close_Click;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(368, 347);
		((Control)this).Controls.AddRange((Control[])(object)new Control[3]
		{
			(Control)_296A,
			(Control)_2969,
			(Control)_2960
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmCustomize";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "cust_caption";
		((Control)_2960).ResumeLayout(false);
		((Control)_2963).ResumeLayout(false);
		((Control)_2961).ResumeLayout(false);
		((Control)_2962).ResumeLayout(false);
		((Form)this).AcceptButton = (IButtonControl)(object)_296A;
		((Form)this).CancelButton = (IButtonControl)(object)_296A;
		((Control)this).ResumeLayout(false);
	}

	private void _2984(object _٠, EventArgs ؿ)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		ӕ ӕ2 = new ӕ();
		((Control)ӕ2._298B).Text = "Custom Bar";
		using (LocalizationManager localizationManager = new LocalizationManager(_2968))
		{
			((Control)ӕ2._298B).Text = localizationManager.GetLocalizedString("sys_custombar");
		}
		((Form)ӕ2).StartPosition = (FormStartPosition)4;
		if ((int)((Form)ӕ2).ShowDialog((IWin32Window)(object)this) == 1)
		{
			Bar bar = new Bar(((Control)ӕ2._298B).Text);
			bar.CustomBar = true;
			bar.CanHide = true;
			bar.SetDesignMode(b: true);
			bar.GrabHandleStyle = eGrabHandleStyle.StripeFlat;
			string text = "userBar";
			int num = 0;
			while (_2968.Bars.Contains(text + num))
			{
				num++;
			}
			bar.Name = text + num;
			_2968.Bars.Add(bar);
			bar.DockSide = eDockSide.None;
			_2967.Items.Add((object)bar, (CheckState)1);
			if (_2968.AllowUserBarCustomize)
			{
				bar.Items.Add(new CustomizeItem());
			}
			((IOwner)_2968).InvokeUserCustomize((object)bar, new EventArgs());
			((IOwner)_2968).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.NewBarCreated));
		}
		((Form)ӕ2).Close();
		((Component)(object)ӕ2).Dispose();
	}

	private void _2985(object _٠, EventArgs ؿ)
	{
		if (((ListControl)_2967).SelectedIndex >= 0)
		{
			Bar bar = ((ListBox)_2967).SelectedItem as Bar;
			if (bar.CustomBar)
			{
				((Control)_2966).Enabled = true;
				((Control)_2180).Enabled = false;
				((Control)_2965).Enabled = true;
			}
			else
			{
				((Control)_2966).Enabled = false;
				((Control)_2180).Enabled = _2968.ShowResetButton;
				((Control)_2965).Enabled = false;
			}
		}
		else
		{
			((Control)_2966).Enabled = false;
			((Control)_2180).Enabled = _2968.ShowResetButton;
			((Control)_2965).Enabled = false;
		}
	}

	private void _2986(object _٠, EventArgs ؿ)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		if (((ListControl)_2967).SelectedIndex >= 0 && ((ListBox)_2967).SelectedItem is Bar bar)
		{
			ӕ ӕ2 = new ӕ();
			ӕ2._298A = true;
			((Control)ӕ2._298B).Text = ((Control)bar).Text;
			((Form)ӕ2).StartPosition = (FormStartPosition)4;
			if ((int)((Form)ӕ2).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((Control)bar).Text = ((Control)ӕ2._298B).Text;
				((Control)_2967).Refresh();
				((IOwner)_2968).InvokeUserCustomize((object)bar, new EventArgs());
				((IOwner)_2968).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarRenamed));
			}
			((Form)ӕ2).Close();
			((Component)(object)ӕ2).Dispose();
		}
	}

	private void _2987(object _٠, EventArgs ؿ)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		if (((ListControl)_2967).SelectedIndex < 0 || !(((ListBox)_2967).SelectedItem is Bar { CustomBar: not false } bar))
		{
			return;
		}
		using LocalizationManager localizationManager = new LocalizationManager(_2968);
		if ((int)MessageBox.Show(localizationManager.GetLocalizedString(LocalizationKeys.CustomizeDialogOptionsConfirmDelete).Replace("<barname>", "'" + ((Control)bar).Text + "'"), ((Control)this).Text, (MessageBoxButtons)4) == 6)
		{
			((ObjectCollection)_2967.Items).Remove((object)bar);
			_2968.Bars.Remove(bar);
			((IOwner)_2968).InvokeUserCustomize((object)bar, new EventArgs());
			((IOwner)_2968).InvokeEndUserCustomize((object)bar, new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarDeleted));
			((Component)(object)bar).Dispose();
		}
	}

	private void _2988(object _٠, EventArgs ؿ)
	{
		IOwner owner = _2968;
		if (((ListControl)_2967).SelectedIndex >= 0)
		{
			if (((ListBox)_2967).SelectedItem is Bar bar && (bar.CustomBar || _2968.ShowResetButton))
			{
				owner?.InvokeResetDefinition(bar.ItemsContainer, new EventArgs());
			}
		}
		else
		{
			owner?.InvokeResetDefinition(null, new EventArgs());
		}
	}

	private void _2989(object _٠, EventArgs ؿ)
	{
		_2968.ResetUsageData();
	}

	private void ᆱ()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (ᆪ != null)
		{
			ᆪ.Start();
			return;
		}
		ᆪ = new Timer();
		ᆪ.Interval = 100;
		ᆪ.Tick += ᆴ;
		ᆪ.Start();
	}

	private void ᆴ(object _٠, EventArgs ؿ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if ((int)Control.MouseButtons == 1048576)
		{
			_297C(Control.MousePosition);
		}
		else
		{
			_297D(Control.MousePosition);
		}
	}

	private void ᆵ()
	{
		if (ᆪ != null)
		{
			ᆪ.Stop();
			((Component)(object)ᆪ).Dispose();
			ᆪ = null;
		}
	}

	internal DotNetBarManager _290C()
	{
		return _2968;
	}
}
