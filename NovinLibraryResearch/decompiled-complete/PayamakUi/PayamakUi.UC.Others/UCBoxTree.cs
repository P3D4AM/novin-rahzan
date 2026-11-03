using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PayamakUi.UC.Others;

public class UCBoxTree : UserControl, IHasContext
{
	public delegate void NodeSelectionChangedEventHandler();

	public enum ENSelectedNode
	{
		Inbox = 1000,
		OutBox = 1100,
		SendItems = 1200,
		Drafts = 1300,
		Templates = 1400,
		PhoneBoox = 1500,
		BlackList = 1600,
		None = 0
	}

	private int Counter;

	private IContainer components;

	internal ImageList ImageList1;

	internal TreeView TV;

	public int SelectedNode
	{
		get
		{
			try
			{
				if (TV.Nodes.Count == 0)
				{
					return 0;
				}
				if (TV.SelectedNode == null)
				{
					return 0;
				}
				return int.Parse(TV.SelectedNode.Name);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SelectedNode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 164);
				return 0;
			}
		}
	}

	public ENSelectedNode SelectedAction
	{
		get
		{
			try
			{
				int num = ((Counter != 1) ? SelectedNode : 3000);
				if (num > 0 && num < 1100)
				{
					return ENSelectedNode.Inbox;
				}
				if (num >= 1100 && num < 1200)
				{
					return ENSelectedNode.OutBox;
				}
				if (num >= 1200 && num < 1300)
				{
					return ENSelectedNode.SendItems;
				}
				if (num >= 1300 && num < 1400)
				{
					return ENSelectedNode.Drafts;
				}
				if (num >= 1400 && num < 1500)
				{
					return ENSelectedNode.Templates;
				}
				if (num >= 1500 && num < 1600)
				{
					return ENSelectedNode.PhoneBoox;
				}
				if (num >= 1600 && num < 1700)
				{
					return ENSelectedNode.BlackList;
				}
				Counter = 2;
				return ENSelectedNode.None;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SelectedAction", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 222);
				return ENSelectedNode.None;
			}
		}
	}

	public string ContextGuid { get; set; }

	public event NodeSelectionChangedEventHandler NodeSelectionChanged;

	public UCBoxTree()
	{
		InitializeComponent();
	}

	public List<int> GetNodes(int ParentID)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 55);
			return null;
		}
	}

	public string NodeName(int ID)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NodeName", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 74);
			return "";
		}
	}

	private void AddNodes(int ID, TreeNode Node)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			TreeNode val = new TreeNode(NodeName(ID));
			val.Name = ID.ToString();
			val.ForeColor = Color.Blue;
			if (ID > 0 && ID < 1100)
			{
				val.ImageIndex = 0;
			}
			else if (ID >= 1100 && ID < 1200)
			{
				val.ImageIndex = 1;
			}
			else if (ID >= 1200 && ID < 1300)
			{
				val.ImageIndex = 2;
			}
			else
			{
				val.ImageIndex = 3;
			}
			if (val.Text != "")
			{
				if (Node == null)
				{
					TV.Nodes.Add(val);
				}
				else
				{
					Node.Nodes.Add(val);
				}
			}
			else
			{
				val = null;
			}
			List<int> nodes = GetNodes(ID);
			if (nodes == null || !nodes.Any())
			{
				return;
			}
			foreach (int item in nodes)
			{
				AddNodes(item, val);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddNodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 146);
		}
	}

	private void TV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		try
		{
			TV.SelectedNode = e.Node;
			this.NodeSelectionChanged?.Invoke();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TV_NodeMouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCBoxTree.cs", 237);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		components = new Container();
		new ComponentResourceManager(typeof(UCBoxTree));
		ImageList1 = new ImageList(components);
		TV = new TreeView();
		((Control)this).SuspendLayout();
		ImageList1.TransparentColor = Color.Transparent;
		((Control)TV).BackColor = SystemColors.InactiveCaption;
		((Control)TV).Dock = (DockStyle)5;
		TV.FullRowSelect = true;
		TV.ImageIndex = 0;
		TV.ImageList = ImageList1;
		((Control)TV).ImeMode = (ImeMode)9;
		TV.ItemHeight = 35;
		((Control)TV).Location = new Point(0, 0);
		((Control)TV).Name = "TV";
		((Control)TV).RightToLeft = (RightToLeft)1;
		TV.RightToLeftLayout = true;
		TV.SelectedImageIndex = 0;
		((Control)TV).Size = new Size(244, 328);
		TV.StateImageList = ImageList1;
		((Control)TV).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TV);
		((Control)this).Name = "UCBoxTree";
		((Control)this).Size = new Size(244, 328);
		((Control)this).ResumeLayout(false);
	}
}
