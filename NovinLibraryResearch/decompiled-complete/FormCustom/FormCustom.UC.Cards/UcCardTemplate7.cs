using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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

public class UcCardTemplate7 : UserControl, IMasterCard
{
	private CardBusiness _card;

	private Font _font;

	private List<CardActions> _actionList;

	private IContainer components;

	private PanelEx panelEx1;

	private SplitContainer splitContainer1;

	private Label lblText1;

	private Label lblText2;

	private PictureBox pictureBox1;

	private PictureBox picMore;

	private ContextMenuStrip ContextMenu;

	private ToolStripMenuItem mnuEditCard;

	public Func<Guid, Task<ReturnedSaveFuncInfo>> Action => _actionList?.FirstOrDefault((CardActions q) => q.Type == _card.Type)?.Action;

	public CardBusiness Data => _card;

	public EnCardTheme Theme => EnCardTheme.Theme7;

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
					ToolTip val = new ToolTip();
					val.SetToolTip((Control)(object)lblText1, _card.ToolTipMessage);
					val.SetToolTip((Control)(object)lblText2, _card.ToolTipMessage);
					val.SetToolTip((Control)(object)pictureBox1, _card.ToolTipMessage);
				}
				if (!isShowContext)
				{
					ContextMenuStrip contextMenu = ContextMenu;
					bool visible = (((Control)picMore).Visible = false);
					((ToolStripDropDown)contextMenu).Visible = visible;
				}
				else if (_card.Type == EnCardType.IncreaseProduct)
				{
					ToolStripMenuItem val2 = new ToolStripMenuItem("ویرایش کالای جاری", (Image)null, (EventHandler)MenuItem_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val2);
					ToolStripMenuItem val3 = new ToolStripMenuItem("تغییر کالای جاری", (Image)null, (EventHandler)MenuItemChangeCard_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val3);
				}
				else if (_card.Type == EnCardType.DynamicGroups)
				{
					ToolStripMenuItem val4 = new ToolStripMenuItem("ویرایش گروه کالای جاری", (Image)null, (EventHandler)MenuItem_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val4);
					ToolStripMenuItem val5 = new ToolStripMenuItem("تغییر گروه کالای جاری", (Image)null, (EventHandler)MenuChangeCard_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val5);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 79);
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
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			if (_card.Image != null)
			{
				Image val = ImageConvertor();
				Bitmap val2 = new Bitmap(val.Width, val.Height);
				Graphics obj = Graphics.FromImage((Image)(object)val2);
				ColorMatrix val3 = new ColorMatrix
				{
					Matrix33 = 0.4f
				};
				ImageAttributes val4 = new ImageAttributes();
				val4.SetColorMatrix(val3, (ColorMatrixFlag)0, (ColorAdjustType)1);
				obj.DrawImage(val, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height), 0, 0, val.Width, val.Height, (GraphicsUnit)2, val4);
				obj.Dispose();
				pictureBox1.Image = (Image)(object)val2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetImage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 100);
		}
	}

	private Image ImageConvertor()
	{
		try
		{
			if (_card.Image == null)
			{
				return null;
			}
			return Image.FromStream((Stream)new MemoryStream(_card.Image));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ImageConvertor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 111);
		}
		return null;
	}

	public async Task SetTextAsync()
	{
		_ = 1;
		try
		{
			if (_card.Text1 != null)
			{
				Label val = lblText1;
				((Control)val).Text = await _card.Text1.GetTextStringAsync(_card.ContextGuid, EnSystem.UserInterface, _card.Type, TargetGuid);
			}
			if (_card.Text2 != null)
			{
				Label val = lblText2;
				((Control)val).Text = await _card.Text2.GetTextStringAsync(_card.ContextGuid, EnSystem.UserInterface, _card.Type, TargetGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 123);
		}
	}

	public void SetForeColor()
	{
		try
		{
			if (!string.IsNullOrEmpty(_card.Text1?.ForeColor ?? ""))
			{
				((Control)lblText1).ForeColor = _card.Text1.ForeColor.RGBConverter();
			}
			if (!string.IsNullOrEmpty(_card.Text2?.ForeColor ?? ""))
			{
				((Control)lblText2).ForeColor = _card.Text2.ForeColor.RGBConverter();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetForeColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 134);
		}
	}

	public void SetFont()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		try
		{
			if (_card.Text1 != null)
			{
				if (string.IsNullOrEmpty(_card.Text1.ForeColor))
				{
					_card.Text1.ForeColor = "#000000";
				}
				if (string.IsNullOrEmpty(_card.Text1.FontName))
				{
					_card.Text1.FontName = "B Yekan";
				}
				if (_card.Text1.FontSize <= 0f)
				{
					_card.Text1.FontSize = 9.75f;
				}
				if (_card.Text1.Bold)
				{
					_font = new Font(new FontFamily(_card.Text1.FontName), _card.Text1.FontSize, (FontStyle)1);
				}
				else if (_card.Text1.IsItalic)
				{
					_font = new Font(new FontFamily(_card.Text1.FontName), _card.Text1.FontSize, (FontStyle)2);
				}
				else
				{
					_font = new Font(new FontFamily(_card.Text1.FontName), _card.Text1.FontSize);
				}
				((Control)lblText1).Font = _font;
			}
			if (_card.Text2 != null)
			{
				if (string.IsNullOrEmpty(_card.Text2.ForeColor))
				{
					_card.Text2.ForeColor = "#000000";
				}
				if (string.IsNullOrEmpty(_card.Text2.FontName))
				{
					_card.Text2.FontName = "B Yekan";
				}
				if (_card.Text2.FontSize <= 0f)
				{
					_card.Text2.FontSize = 9.75f;
				}
				if (_card.Text2.Bold)
				{
					_font = new Font(new FontFamily(_card.Text2.FontName), _card.Text2.FontSize, (FontStyle)1);
				}
				else if (_card.Text2.IsItalic)
				{
					_font = new Font(new FontFamily(_card.Text2.FontName), _card.Text2.FontSize, (FontStyle)2);
				}
				else
				{
					_font = new Font(new FontFamily(_card.Text2.FontName), _card.Text2.FontSize);
				}
				((Control)lblText2).Font = _font;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFont", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 175);
		}
	}

	public void SetAlignment()
	{
		try
		{
			if (_card.Text1 != null && _card.Text1.TextAlign != 0)
			{
				lblText1.TextAlign = (ContentAlignment)_card.Text1.TextAlign;
			}
			if (_card.Text2 != null && _card.Text2.TextAlign != 0)
			{
				lblText2.TextAlign = (ContentAlignment)_card.Text2.TextAlign;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAlignment", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 186);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "SetBackColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 196);
		}
	}

	public UcCardTemplate7()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblText1_Click(object sender, EventArgs e)
	{
		Action?.Invoke(TargetGuid);
	}

	private void lblText2_Click(object sender, EventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "picMore_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Cards\\UcCardTemplate7.cs", 213);
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		panelEx1 = new PanelEx();
		splitContainer1 = new SplitContainer();
		lblText2 = new Label();
		ContextMenu = new ContextMenuStrip(components);
		mnuEditCard = new ToolStripMenuItem();
		pictureBox1 = new PictureBox();
		lblText1 = new Label();
		picMore = new PictureBox();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)ContextMenu).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)pictureBox1).SuspendLayout();
		((Control)lblText1).SuspendLayout();
		((ISupportInitialize)picMore).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = Color.Transparent;
		((Control)panelEx1).Controls.Add((Control)(object)splitContainer1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(201, 150);
		panelEx1.Style.BackColor1.Color = Color.FromArgb(228, 236, 255);
		panelEx1.Style.BackColor2.Color = Color.FromArgb(228, 236, 255);
		panelEx1.Style.BorderColor.Color = Color.Black;
		panelEx1.Style.BorderWidth = 0;
		panelEx1.Style.CornerDiameter = 15;
		panelEx1.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx1).TabIndex = 25;
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		splitContainer1.Orientation = (Orientation)0;
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)pictureBox1);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)lblText2);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(201, 150);
		splitContainer1.SplitterDistance = 77;
		((Control)splitContainer1).TabIndex = 1;
		((Control)lblText2).Cursor = Cursors.Hand;
		((Control)lblText2).Dock = (DockStyle)5;
		((Control)lblText2).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblText2).ForeColor = Color.Black;
		((Control)lblText2).Location = new Point(0, 0);
		((Control)lblText2).Margin = new Padding(4, 0, 4, 0);
		((Control)lblText2).Name = "lblText2";
		((Control)lblText2).Padding = new Padding(0, 0, 10, 0);
		((Control)lblText2).Size = new Size(201, 69);
		((Control)lblText2).TabIndex = 5;
		lblText2.TextAlign = (ContentAlignment)32;
		((Control)lblText2).Click += lblText2_Click;
		((ToolStrip)ContextMenu).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)mnuEditCard });
		((Control)ContextMenu).Name = "ContextMenu";
		((Control)ContextMenu).RightToLeft = (RightToLeft)1;
		((Control)ContextMenu).Size = new Size(170, 28);
		((ToolStripItem)mnuEditCard).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)mnuEditCard).Name = "mnuEditCard";
		((ToolStripItem)mnuEditCard).Size = new Size(169, 24);
		((ToolStripItem)mnuEditCard).Text = "ویرایش کارت جاری";
		((ToolStripItem)mnuEditCard).Click += mnuEditCard_Click;
		((Control)pictureBox1).Controls.Add((Control)(object)lblText1);
		((Control)pictureBox1).Dock = (DockStyle)5;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(201, 77);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 6;
		pictureBox1.TabStop = false;
		((Control)lblText1).Controls.Add((Control)(object)picMore);
		((Control)lblText1).Cursor = Cursors.Hand;
		((Control)lblText1).Dock = (DockStyle)5;
		((Control)lblText1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblText1).ForeColor = Color.Black;
		((Control)lblText1).Location = new Point(0, 0);
		((Control)lblText1).Margin = new Padding(4, 0, 4, 0);
		((Control)lblText1).Name = "lblText1";
		((Control)lblText1).Padding = new Padding(0, 0, 10, 0);
		((Control)lblText1).Size = new Size(201, 77);
		((Control)lblText1).TabIndex = 5;
		lblText1.TextAlign = (ContentAlignment)32;
		((Control)lblText1).Click += lblText1_Click;
		((Control)picMore).Anchor = (AnchorStyles)9;
		((Control)picMore).Cursor = Cursors.Hand;
		picMore.Image = (Image)(object)FormCustom.Properties.Resources.NP_more;
		((Control)picMore).Location = new Point(177, 0);
		((Control)picMore).Name = "picMore";
		((Control)picMore).Size = new Size(24, 28);
		picMore.SizeMode = (PictureBoxSizeMode)1;
		picMore.TabIndex = 30;
		picMore.TabStop = false;
		((Control)picMore).Click += picMore_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCardTemplate7";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(201, 150);
		((Control)panelEx1).ResumeLayout(false);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((Control)ContextMenu).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)pictureBox1).ResumeLayout(false);
		((Control)lblText1).ResumeLayout(false);
		((ISupportInitialize)picMore).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
