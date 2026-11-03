using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Menu;

public class UcSearchInMenuStrip : UserControl
{
	private IEnumerable<SearchInMenuStrip> _list;

	private CancellationTokenSource _searchToken = new CancellationTokenSource();

	private IContainer components;

	private GroupPanel groupPanel;

	private ListBox listBox1;

	private BindingSource bindingSource1;

	private PictureBox pictureBox1;

	private TextBoxX textBox1;

	private bool IsInited { get; set; }

	private SearchInMenuStrip Current => (SearchInMenuStrip)bindingSource1.Current;

	public event Action OnReleaseFocus;

	public void Init(ISearchInMenu menu)
	{
		try
		{
			((Control)listBox1).Visible = false;
			List<SearchInMenuStrip> list = new List<SearchInMenuStrip>();
			foreach (SearchInMenuStrip listItem in menu.ListItems)
			{
				list.Add(listItem);
			}
			_list = list?.OrderBy((SearchInMenuStrip q) => q.Enable)?.ToList();
			IsInited = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 35);
		}
	}

	public void ReleaseFocus()
	{
		try
		{
			this.OnReleaseFocus?.Invoke();
			((Control)listBox1).Visible = false;
			((Control)textBox1).Text = "";
			((Control)this).SendToBack();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReleaseFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 46);
		}
	}

	private async Task SearchAsync(string search, CancellationToken token)
	{
		try
		{
			if (!(_list?.Any() ?? false) || !IsInited || token.IsCancellationRequested)
			{
				return;
			}
			if (string.IsNullOrEmpty(search))
			{
				search = "";
			}
			IEnumerable<SearchInMenuStrip> res = _list;
			List<string> list = search.SplitString();
			if (list != null && list.Count > 0)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				foreach (string item in list)
				{
					string replacedItem = item.Replace("ـ", "");
					if (token.IsCancellationRequested)
					{
						return;
					}
					if (!string.IsNullOrEmpty(replacedItem) && replacedItem.Trim() != "")
					{
						res = res.Where((SearchInMenuStrip x) => x.ShortText.Contains(replacedItem));
					}
				}
			}
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			res = res?.OrderBy((SearchInMenuStrip o) => o.Enable);
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((Control)this).IsDisposed)
					{
						if (!((Control)listBox1).Visible)
						{
							((Control)listBox1).Visible = true;
						}
						bindingSource1.DataSource = res?.ToList();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 84);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 87);
		}
	}

	public UcSearchInMenuStrip()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(((Control)textBox1).Text))
			{
				((Control)this).BringToFront();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken token = _searchToken.Token;
				Task.Run(() => SearchAsync(((Control)textBox1).Text, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "textBox1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 107);
		}
	}

	private void listBox1_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				if (Current == null || !Current.Enable)
				{
					return;
				}
				((Control)listBox1).Visible = false;
				((Control)textBox1).Text = "";
				ReleaseFocus();
				Current?.Func?.Invoke();
			}
			else if ((int)e.KeyCode == 38 && ((ListControl)listBox1).SelectedIndex == 0)
			{
				((Control)textBox1).Focus();
			}
			if ((int)e.KeyCode == 27)
			{
				((Control)textBox1).Text = "";
				((Control)listBox1).Visible = false;
				ReleaseFocus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "listBox1_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 134);
		}
	}

	private void textBox1_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((Control)listBox1).Focus();
				listBox1_KeyDown(sender, e);
			}
			else if ((int)e.KeyCode == 27)
			{
				if (string.IsNullOrEmpty(((Control)textBox1).Text))
				{
					SendKeys.Send("{Tab}");
					ReleaseFocus();
				}
				else
				{
					((Control)textBox1).Text = "";
					((Control)listBox1).Visible = false;
					ReleaseFocus();
				}
			}
			else if ((int)e.KeyCode == 40)
			{
				((Control)listBox1).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "textBox1_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 160);
		}
	}

	private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)textBox1).Focus();
				((Control)textBox1).Text = e.KeyChar.ToString();
				((TextBoxBase)textBox1).SelectionStart = 9999;
				((TextBoxBase)textBox1).SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "listBox1_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 172);
		}
	}

	private void pictureBox1_MouseEnter(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ToolTip().SetToolTip((Control)(object)pictureBox1, "جستجو در لیست منوها".GetTranslate());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBox1_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 181);
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		((Control)textBox1).Focus();
	}

	private void listBox1_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Current != null)
			{
				((Control)listBox1).Visible = false;
				((Control)textBox1).Text = "";
				ReleaseFocus();
				Current?.Func?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "listBox1_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Menu\\UcSearchInMenuStrip.cs", 194);
		}
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		groupPanel = new GroupPanel();
		pictureBox1 = new PictureBox();
		textBox1 = new TextBoxX();
		listBox1 = new ListBox();
		bindingSource1 = new BindingSource(components);
		((Control)groupPanel).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)bindingSource1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)groupPanel).Controls.Add((Control)(object)pictureBox1);
		((Control)groupPanel).Controls.Add((Control)(object)textBox1);
		((Control)groupPanel).Controls.Add((Control)(object)listBox1);
		groupPanel.DisabledBackColor = Color.Empty;
		((Control)groupPanel).Dock = (DockStyle)5;
		((Control)groupPanel).Location = new Point(0, 0);
		((Control)groupPanel).Name = "groupPanel";
		((Control)groupPanel).Size = new Size(549, 242);
		groupPanel.Style.BackColor = Color.Transparent;
		groupPanel.Style.BackColor2 = Color.Transparent;
		groupPanel.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel.Style.BorderColor = Color.FromArgb(252, 27, 83);
		groupPanel.Style.BorderColor2 = Color.FromArgb(252, 27, 83);
		groupPanel.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel.Style.CornerDiameter = 20;
		groupPanel.Style.CornerType = eCornerType.Rounded;
		groupPanel.Style.CornerTypeBottomLeft = eCornerType.Rounded;
		groupPanel.Style.CornerTypeBottomRight = eCornerType.Rounded;
		groupPanel.Style.CornerTypeTopLeft = eCornerType.Rounded;
		groupPanel.Style.CornerTypeTopRight = eCornerType.Rounded;
		groupPanel.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel).TabIndex = 12;
		((Control)pictureBox1).BackColor = Color.White;
		((Control)pictureBox1).Cursor = Cursors.Hand;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources._34324324;
		((Control)pictureBox1).Location = new Point(14, 22);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(21, 27);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 3;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((Control)pictureBox1).MouseEnter += pictureBox1_MouseEnter;
		((Control)textBox1).Anchor = (AnchorStyles)13;
		((Control)textBox1).BackColor = Color.White;
		textBox1.Border.BackColor = Color.White;
		textBox1.Border.BackColor2 = Color.White;
		textBox1.Border.BorderBottom = eStyleBorderType.Solid;
		textBox1.Border.BorderLeft = eStyleBorderType.Solid;
		textBox1.Border.BorderRight = eStyleBorderType.Solid;
		textBox1.Border.BorderTop = eStyleBorderType.Solid;
		textBox1.Border.Class = "TextBoxBorder";
		textBox1.Border.CornerDiameter = 10;
		textBox1.Border.CornerType = eCornerType.Rounded;
		textBox1.Border.PaddingRight = 20;
		((Control)textBox1).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)textBox1).Location = new Point(5, 19);
		((Control)textBox1).Name = "textBox1";
		textBox1.PreventEnterBeep = true;
		((Control)textBox1).Size = new Size(534, 34);
		((Control)textBox1).TabIndex = 4;
		textBox1.WatermarkColor = Color.FromArgb(65, 81, 220);
		textBox1.WatermarkText = "[جستجو در منو ها] ...";
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)textBox1).KeyDown += new KeyEventHandler(textBox1_KeyDown);
		((Control)listBox1).Anchor = (AnchorStyles)15;
		((ListControl)listBox1).DataSource = bindingSource1;
		((ListControl)listBox1).DisplayMember = "Text";
		((ListControl)listBox1).FormattingEnabled = true;
		listBox1.ItemHeight = 20;
		((Control)listBox1).Location = new Point(5, 54);
		((Control)listBox1).Name = "listBox1";
		((Control)listBox1).Size = new Size(534, 184);
		((Control)listBox1).TabIndex = 2;
		((Control)listBox1).TabStop = false;
		((ListControl)listBox1).ValueMember = "Menu";
		((Control)listBox1).Visible = false;
		((Control)listBox1).DoubleClick += listBox1_DoubleClick;
		((Control)listBox1).KeyDown += new KeyEventHandler(listBox1_KeyDown);
		((Control)listBox1).KeyPress += new KeyPressEventHandler(listBox1_KeyPress);
		bindingSource1.DataSource = typeof(SearchInMenuStrip);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)groupPanel);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcSearchInMenuStrip";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(549, 242);
		((Control)groupPanel).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)bindingSource1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
