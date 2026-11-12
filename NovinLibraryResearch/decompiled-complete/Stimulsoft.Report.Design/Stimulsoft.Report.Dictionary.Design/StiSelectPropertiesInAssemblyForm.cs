using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiSelectPropertiesInAssemblyForm : StiFormUX
{
	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private IContainer components;

	private StiTreeViewUX treeViewProperties;

	private ImageList ilProperties;

	private Assembly assembly;

	private StiButtonPanelUX stiGroupLine1;

	public List<TreeNode> SelectColumns = new List<TreeNode>();

	private bool lockObject;

	public StiSelectPropertiesInAssemblyForm(Assembly assembly)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.assembly = assembly;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			((Form)this).Load += This_Load;
			DoScaling();
		}
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSelectPropertiesInAssemblyForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		ilProperties = new ImageList(components);
		treeViewProperties = new StiTreeViewUX();
		stiGroupLine1 = new StiButtonPanelUX();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(283, 12);
		((Control)btOk).Margin = new Padding(2, 2, 2, 2);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(366, 12);
		((Control)btCancel).Margin = new Padding(2, 2, 2, 2);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		ilProperties.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilProperties.ImageStream");
		ilProperties.TransparentColor = Color.Magenta;
		ilProperties.Images.SetKeyName(0, "");
		ilProperties.Images.SetKeyName(1, "");
		ilProperties.Images.SetKeyName(2, "");
		((Control)treeViewProperties).Anchor = (AnchorStyles)15;
		((Control)treeViewProperties).BackColor = Color.White;
		((TreeView)treeViewProperties).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewProperties).CheckBoxes = true;
		((TreeView)treeViewProperties).ImageIndex = 0;
		((TreeView)treeViewProperties).ImageList = ilProperties;
		((Control)treeViewProperties).Location = new Point(12, 12);
		((Control)treeViewProperties).Margin = new Padding(2, 2, 2, 2);
		((Control)treeViewProperties).Name = "treeViewProperties";
		((TreeView)treeViewProperties).SelectedImageIndex = 0;
		((Control)treeViewProperties).Size = new Size(428, 480);
		((TreeView)treeViewProperties).Sorted = true;
		((Control)treeViewProperties).TabIndex = 3;
		((TreeView)treeViewProperties).BeforeCheck += new TreeViewCancelEventHandler(TreeViewProperties_BeforeCheck);
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 506);
		((Control)stiGroupLine1).Margin = new Padding(2, 2, 2, 2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(452, 46);
		((Control)stiGroupLine1).TabIndex = 4;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(452, 552);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)treeViewProperties);
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSelectPropertiesInAssemblyForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select Properties";
		((Form)this).Closed += This_Closed;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ConnectionSelectForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		if (StiScale.Factor != 1.0)
		{
			ImageList val = new ImageList();
			val.ImageSize = new Size(StiScale.I16, StiScale.I16);
			val.ColorDepth = (ColorDepth)32;
			val.Images.Add(StiImageResizer.GetImage(ilProperties.Images[0], allowDispose: false));
			val.Images.Add(StiImageResizer.GetImage(ilProperties.Images[1], allowDispose: false));
			val.Images.Add(StiImageResizer.GetImage(ilProperties.Images[2], allowDispose: false));
			((TreeView)treeViewProperties).ImageList = val;
		}
	}

	private void ParseNodes(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Checked && val.Tag != null)
			{
				SelectColumns.Add(val);
			}
			if (val.Nodes.Count > 0)
			{
				ParseNodes(val.Nodes);
			}
		}
	}

	public void Build()
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		bool flag = true;
		try
		{
			((TreeView)treeViewProperties).Nodes.Clear();
			if (assembly == null)
			{
				return;
			}
			Type[] array = null;
			bool flag2 = false;
			try
			{
				array = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				array = ex.Types.Where((Type t) => t != null).ToArray();
				flag2 = true;
			}
			Hashtable hashtable = new Hashtable();
			Type[] array2 = array;
			foreach (Type type in array2)
			{
				if (!type.IsPublic)
				{
					continue;
				}
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
				if (properties.Length != 0)
				{
					if (flag2)
					{
						flag = false;
					}
					string text = type.Namespace;
					if (text == null)
					{
						text = string.Empty;
					}
					object? obj = hashtable[text];
					TreeNode val = (TreeNode)((obj is TreeNode) ? obj : null);
					if (val == null)
					{
						val = new TreeNode(text, 0, 0);
						((TreeView)treeViewProperties).Nodes.Add(val);
						hashtable[text] = val;
					}
					TreeNode val2 = new TreeNode(type.Name, 1, 1);
					val.Nodes.Add(val2);
					PropertyInfo[] array3 = properties;
					foreach (PropertyInfo propertyInfo in array3)
					{
						TreeNode val3 = new TreeNode(propertyInfo.Name, 2, 2);
						val3.Tag = propertyInfo.PropertyType;
						val2.Nodes.Add(val3);
					}
				}
			}
			if (flag2)
			{
				array = assembly.GetTypes();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
			if (flag)
			{
				((Form)this).Close();
			}
		}
	}

	private static void SetChecked(TreeNode node, bool isCkecked)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			val.Checked = isCkecked;
			SetChecked(val, isCkecked);
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Form)this).Load -= This_Load;
		Build();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		ParseNodes(((TreeView)treeViewProperties).Nodes);
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void TreeViewProperties_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		if (!lockObject)
		{
			bool isCkecked = !e.Node.Checked;
			lockObject = true;
			SetChecked(e.Node, isCkecked);
			lockObject = false;
		}
	}
}
