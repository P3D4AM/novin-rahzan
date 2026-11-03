using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.Global;
using FormCustom.Properties;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Events;

namespace FormCustom.UC.Cards;

public class UcCardTemplate3 : UserControl, IMasterCard
{
	private CardBusiness _card;

	private List<CardActions> _actionList;

	private IContainer components;

	private PanelEx panelEx1;

	private PictureBox Pic;

	private PictureBox picMore;

	private ContextMenuStrip ContextMenu;

	private ToolStripMenuItem mnuEditCard;

	public Func<Guid, Task<ReturnedSaveFuncInfo>> Action => _actionList?.FirstOrDefault((CardActions q) => q.Type == _card.Type)?.Action;

	public CardBusiness Data => _card;

	public EnCardTheme Theme => EnCardTheme.Theme3;

	public UserControl Control => (UserControl)(object)this;

	public Guid TargetGuid { get; private set; }

	public event Func<Guid, EnCardType, Task> OnMoreItemClicked;

	public event Func<Guid, Task> OnEditCard;

	public event Func<Guid, Guid, EnCardType, Task> OnItemChangeCard;

	public event Func<Guid, Guid, EnCardType, Task> OnChangeCard;

	public async Task SetDataAsync(CardBusiness _data, Guid targetGuid, List<CardActions> _actions, bool isShowContext)
	{
		try
		{
			if (_data != null)
			{
				_card = _data;
				_actionList = _actions;
				TargetGuid = targetGuid;
				SetBackColor();
				await SetTextAsync();
				SetForeColor();
				SetFont();
				SetImage();
				SetAlignment();
				if (!string.IsNullOrEmpty(_card?.ToolTipMessage ?? ""))
				{
					new ToolTip().SetToolTip((Control)(object)Pic, _card.ToolTipMessage);
				}
				if (!isShowContext)
				{
					ContextMenuStrip contextMenu = ContextMenu;
					bool visible = (((Control)picMore).Visible = false);
					((ToolStripDropDown)contextMenu).Visible = visible;
				}
				else if (_card.Type == EnCardType.IncreaseProduct)
				{
					ToolStripMenuItem val = new ToolStripMenuItem("ویرایش کالای جاری", (Image)null, (EventHandler)MenuItem_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val);
					ToolStripMenuItem val2 = new ToolStripMenuItem("تغییر کالای جاری", (Image)null, (EventHandler)MenuItemChangeCard_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val2);
				}
				else if (_card.Type == EnCardType.DynamicGroups)
				{
					ToolStripMenuItem val3 = new ToolStripMenuItem("ویرایش گروه کالای جاری", (Image)null, (EventHandler)MenuItem_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val3);
					ToolStripMenuItem val4 = new ToolStripMenuItem("تغییر گروه کالای جاری", (Image)null, (EventHandler)MenuChangeCard_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val4);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate3.cs", 74);
		}
	}

	private void MenuItem_Click(object sender, EventArgs e)
	{
		this.OnMoreItemClicked?.Invoke(TargetGuid, _card.Type);
	}

	private void MenuItemChangeCard_Click(object sender, EventArgs e)
	{
		this.OnItemChangeCard?.Invoke(_card.Guid, TargetGuid, _card.Type);
	}

	private void MenuChangeCard_Click(object sender, EventArgs e)
	{
		this.OnChangeCard?.Invoke(_card.Guid, TargetGuid, _card.Type);
	}

	public void SetImage()
	{
		try
		{
			if (_card.Image != null)
			{
				Image image = Image.FromStream((Stream)new MemoryStream(_card.Image));
				Pic.Image = image;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetImage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate3.cs", 88);
		}
	}

	public Task SetTextAsync()
	{
		return Task.CompletedTask;
	}

	public void SetForeColor()
	{
	}

	public void SetFont()
	{
	}

	public void SetAlignment()
	{
	}

	public void SetBackColor()
	{
		try
		{
			if (!string.IsNullOrEmpty(_card.BackColor))
			{
				panelEx1.Style.BackColor1.Color = _card.BackColor.RGBConverter();
				panelEx1.Style.BackColor2.Color = _card.BackColor.RGBConverter();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetBackColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate3.cs", 102);
		}
	}

	public UcCardTemplate3()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void Pic_Click(object sender, EventArgs e)
	{
		Action?.Invoke(TargetGuid);
	}

	private void picMore_Click(object sender, EventArgs e)
	{
		try
		{
			((ToolStripDropDown)ContextMenu).Show(Cursor.Position.X, Cursor.Position.Y);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picMore_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate3.cs", 118);
		}
	}

	private void mnuEditCard_Click(object sender, EventArgs e)
	{
		this.OnEditCard?.Invoke(_card.Guid);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		panelEx1 = new PanelEx();
		Pic = new PictureBox();
		picMore = new PictureBox();
		ContextMenu = new ContextMenuStrip(components);
		mnuEditCard = new ToolStripMenuItem();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)Pic).BeginInit();
		((Control)Pic).SuspendLayout();
		((ISupportInitialize)picMore).BeginInit();
		((Control)ContextMenu).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = Color.Transparent;
		((Control)panelEx1).Controls.Add((Control)(object)Pic);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(150, 150);
		panelEx1.Style.BackColor1.Color = Color.FromArgb(228, 236, 255);
		panelEx1.Style.BackColor2.Color = Color.FromArgb(228, 236, 255);
		panelEx1.Style.BorderColor.Color = Color.Black;
		panelEx1.Style.BorderWidth = 0;
		panelEx1.Style.CornerDiameter = 15;
		panelEx1.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx1).TabIndex = 25;
		((Control)Pic).Controls.Add((Control)(object)picMore);
		((Control)Pic).Cursor = Cursors.Hand;
		((Control)Pic).Dock = (DockStyle)5;
		((Control)Pic).Location = new Point(0, 0);
		((Control)Pic).Name = "Pic";
		((Control)Pic).Size = new Size(150, 150);
		Pic.SizeMode = (PictureBoxSizeMode)1;
		Pic.TabIndex = 37;
		Pic.TabStop = false;
		((Control)Pic).Click += Pic_Click;
		((Control)picMore).Anchor = (AnchorStyles)9;
		((Control)picMore).BackColor = Color.Transparent;
		((Control)picMore).Cursor = Cursors.Hand;
		picMore.Image = (Image)(object)FormCustom.Properties.Resources.NP_more;
		((Control)picMore).Location = new Point(126, 1);
		((Control)picMore).Name = "picMore";
		((Control)picMore).Size = new Size(24, 28);
		picMore.SizeMode = (PictureBoxSizeMode)1;
		picMore.TabIndex = 30;
		picMore.TabStop = false;
		((Control)picMore).Click += picMore_Click;
		((ToolStrip)ContextMenu).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)mnuEditCard });
		((Control)ContextMenu).Name = "ContextMenu";
		((Control)ContextMenu).RightToLeft = (RightToLeft)1;
		((Control)ContextMenu).Size = new Size(170, 28);
		((ToolStripItem)mnuEditCard).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)mnuEditCard).Name = "mnuEditCard";
		((ToolStripItem)mnuEditCard).Size = new Size(169, 24);
		((ToolStripItem)mnuEditCard).Text = "ویرایش کارت جاری";
		((ToolStripItem)mnuEditCard).Click += mnuEditCard_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCardTemplate3";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)Pic).EndInit();
		((Control)Pic).ResumeLayout(false);
		((ISupportInitialize)picMore).EndInit();
		((Control)ContextMenu).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
