using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace Stimulsoft.Controls;

[Designer(typeof(StiDockingManagerDesigner))]
[ToolboxBitmap(typeof(StiDockingManager), "Toolbox.StiDockingManager.bmp")]
[ToolboxItem(false)]
public class StiDockingManager : Component
{
	private Hashtable guidChecker = new Hashtable();

	private bool autoHide;

	private DockStyle dockStyle = (DockStyle)3;

	private int left;

	private int top;

	private int width = 200;

	private int height = 300;

	private int controlIndex = -1;

	private int selectedIndex;

	private StiFloatingForm floatingForm;

	private bool isFloating;

	private bool isClosing;

	private bool lockDockingPanels;

	private List<StiDockingPanel> undockedPanels = new List<StiDockingPanel>();

	private List<StiDockingControl> closedControls = new List<StiDockingControl>();

	private bool showingUndocked = true;

	private Form parentForm;

	private Form form;

	private bool lockLayout;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool LockDockingPanels
	{
		get
		{
			return lockDockingPanels;
		}
		set
		{
			lockDockingPanels = value;
		}
	}

	[Browsable(false)]
	public List<StiDockingPanel> UndockedPanels
	{
		get
		{
			return undockedPanels;
		}
		set
		{
			undockedPanels = value;
		}
	}

	[Browsable(false)]
	public List<StiDockingControl> ClosedControls
	{
		get
		{
			return closedControls;
		}
		set
		{
			closedControls = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool ShowingUndocked
	{
		get
		{
			return showingUndocked;
		}
		set
		{
			showingUndocked = value;
		}
	}

	[Browsable(false)]
	public List<StiDockingPanel> DockedPanels
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			List<StiDockingPanel> list = new List<StiDockingPanel>();
			foreach (Control item2 in (ArrangedElementCollection)((Control)ParentForm).Controls)
			{
				Control val = item2;
				if (val is StiDockingPanel item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	[Browsable(false)]
	public Form ParentForm
	{
		get
		{
			return parentForm;
		}
		set
		{
			parentForm = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal Form Form
	{
		get
		{
			return form;
		}
		set
		{
			if (form == null && form != value && value != null)
			{
				form = value;
				form.Closed += OnClosedUndockingPanels;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal bool LockLayout
	{
		get
		{
			return lockLayout;
		}
		set
		{
			lockLayout = value;
		}
	}

	public void ShowUndockingPanels()
	{
		if (ShowingUndocked)
		{
			return;
		}
		foreach (StiDockingPanel undockedPanel in UndockedPanels)
		{
			if (!((Control)undockedPanel.FloatingForm).Visible)
			{
				ParentForm.AddOwnedForm((Form)(object)undockedPanel.FloatingForm);
				((Control)undockedPanel.FloatingForm).Show();
			}
		}
		ShowingUndocked = true;
	}

	public void SaveConfig(string file)
	{
		FileStream fileStream = new FileStream(file, FileMode.Create);
		SaveConfig(fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void SaveConfig(Stream stream)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.WriteStartDocument(standalone: true);
		SaveConfig(xmlTextWriter);
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
	}

	public void SaveConfig(XmlTextWriter tw)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		tw.WriteStartElement("StiDockingManagerConfig");
		List<StiDockingPanel> dockedPanels = DockedPanels;
		foreach (StiDockingPanel item in dockedPanels)
		{
			if (((ArrangedElementCollection)((Control)item).Controls).Count <= 0)
			{
				continue;
			}
			tw.WriteStartElement("Panel", ((Control)item).Text);
			tw.WriteAttributeString("Floating", "False");
			tw.WriteAttributeString("Closing", "False");
			tw.WriteAttributeString("AutoHide", item.AutoHide.ToString());
			DockStyle dock = ((Control)item).Dock;
			tw.WriteAttributeString("Dock", ((object)(DockStyle)(ref dock)).ToString());
			if (item.Collapsed)
			{
				tw.WriteAttributeString("Width", item.CollapsedSize.Width.ToString());
				tw.WriteAttributeString("Height", item.CollapsedSize.Height.ToString());
			}
			else
			{
				tw.WriteAttributeString("Width", ((Control)item).Width.ToString());
				tw.WriteAttributeString("Height", ((Control)item).Height.ToString());
			}
			tw.WriteAttributeString("SelectedIndex", ((Control)item).Controls.IndexOf((Control)(object)item.SelectedControl).ToString());
			tw.WriteStartElement("Controls", ((Control)item).Text);
			foreach (StiDockingControl item2 in (ArrangedElementCollection)((Control)item).Controls)
			{
				tw.WriteElementString("Guid", XmlConvert.EncodeName(item2.Guid));
			}
			tw.WriteEndElement();
			tw.WriteEndElement();
		}
		foreach (StiDockingPanel undockedPanel in UndockedPanels)
		{
			if (undockedPanel.FloatingForm == null || ((ArrangedElementCollection)((Control)undockedPanel).Controls).Count <= 0)
			{
				continue;
			}
			tw.WriteStartElement("Panel", ((Control)undockedPanel).Text);
			tw.WriteAttributeString("Floating", "True");
			tw.WriteAttributeString("Closing", "False");
			tw.WriteAttributeString("Left", ((Control)undockedPanel.FloatingForm).Left.ToString());
			tw.WriteAttributeString("Top", ((Control)undockedPanel.FloatingForm).Top.ToString());
			tw.WriteAttributeString("Width", ((Control)undockedPanel.FloatingForm).Width.ToString());
			tw.WriteAttributeString("Height", ((Control)undockedPanel.FloatingForm).Height.ToString());
			tw.WriteAttributeString("SelectedIndex", ((Control)undockedPanel).Controls.IndexOf((Control)(object)undockedPanel.SelectedControl).ToString());
			tw.WriteStartElement("Controls", ((Control)undockedPanel).Text);
			foreach (StiDockingControl item3 in (ArrangedElementCollection)((Control)undockedPanel).Controls)
			{
				tw.WriteElementString("Guid", XmlConvert.EncodeName(item3.Guid));
			}
			tw.WriteEndElement();
			tw.WriteEndElement();
		}
		if (ClosedControls.Count > 0)
		{
			tw.WriteStartElement("Panel");
			tw.WriteAttributeString("Floating", "False");
			tw.WriteAttributeString("Closing", "True");
			Hashtable hashtable = new Hashtable();
			foreach (StiDockingControl closedControl in ClosedControls)
			{
				if (hashtable[closedControl.Guid] == null)
				{
					hashtable[closedControl.Guid] = closedControl.Guid;
					tw.WriteElementString("Guid", XmlConvert.EncodeName(closedControl.Guid));
				}
			}
			tw.WriteEndElement();
		}
		tw.WriteEndElement();
	}

	public void LoadConfig(string file)
	{
		if (File.Exists(file))
		{
			FileStream fileStream = new FileStream(file, FileMode.Open);
			LoadConfig(fileStream);
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public void LoadConfig(Stream stream)
	{
		LockLayout = true;
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
		LoadConfig(xmlTextReader);
		LockLayout = false;
		SetLayoutAllPanels();
	}

	public void LoadConfig(XmlTextReader tr)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		LockLayout = true;
		if (tr.IsStartElement() && tr.Name == "StiDockingManagerConfig")
		{
			autoHide = false;
			dockStyle = (DockStyle)3;
			width = 200;
			height = 300;
			guidChecker.Clear();
			LoadNodes(tr);
		}
		LockLayout = false;
		SetLayoutAllPanels();
	}

	private void LoadNodes(XmlTextReader tr)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		while (tr.Read() && tr.IsStartElement())
		{
			if (tr.Name == "Panel")
			{
				controlIndex = -1;
				isFloating = tr.GetAttribute("Floating") == "True";
				isClosing = tr.GetAttribute("Closing") == "True";
				if (!isClosing)
				{
					width = int.Parse(tr.GetAttribute("Width"));
					height = int.Parse(tr.GetAttribute("Height"));
					selectedIndex = int.Parse(tr.GetAttribute("SelectedIndex"));
					if (!isFloating)
					{
						autoHide = tr.GetAttribute("AutoHide") == "True";
						dockStyle = (DockStyle)Enum.Parse(typeof(DockStyle), tr.GetAttribute("Dock"));
					}
					else
					{
						left = int.Parse(tr.GetAttribute("Left"));
						top = int.Parse(tr.GetAttribute("Top"));
					}
				}
				floatingForm = null;
			}
			if (tr.Name == "Guid")
			{
				controlIndex++;
				string text = XmlConvert.DecodeName(tr.ReadElementString());
				if (guidChecker[text] == null)
				{
					guidChecker[text] = text;
					StiDockingControl dockingControl = GetDockingControl(text);
					if (dockingControl != null)
					{
						StiDockingPanel stiDockingPanel = ((Control)dockingControl).Parent as StiDockingPanel;
						if (!isClosing)
						{
							if (!isFloating)
							{
								if (((Control)dockingControl).Parent.Dock != dockStyle)
								{
									stiDockingPanel.DockControlToForm(dockingControl, ParentForm, dockStyle);
									stiDockingPanel = GetDockingPanel(dockStyle);
									if (((ArrangedElementCollection)((Control)stiDockingPanel).Controls).Count == 0)
									{
										UndockedPanels.Remove(stiDockingPanel);
									}
								}
								if (((Control)dockingControl).Parent.Controls.IndexOf((Control)(object)dockingControl) != controlIndex)
								{
									((Control)dockingControl).Parent.Controls.SetChildIndex((Control)(object)dockingControl, controlIndex);
								}
								if (!autoHide)
								{
									((Control)stiDockingPanel).Width = width;
									((Control)stiDockingPanel).Height = height;
								}
								else
								{
									stiDockingPanel.AutoHide = autoHide;
									stiDockingPanel.Collapsed = true;
									stiDockingPanel.CollapsedSize = new Size(width, height);
								}
							}
							else if (floatingForm == null)
							{
								stiDockingPanel.UndockControl(dockingControl, new Rectangle(left, top, width, height), createForm: true, ((Control)stiDockingPanel).Parent);
								floatingForm = ((StiDockingPanel)(object)((Control)dockingControl).Parent).FloatingForm;
								((Control)floatingForm).Width = width;
								((Control)floatingForm).Height = height;
								if (((ArrangedElementCollection)((Control)stiDockingPanel).Controls).Count == 0)
								{
									UndockedPanels.Remove(stiDockingPanel);
								}
							}
							else
							{
								stiDockingPanel.DockControlToPanel((StiDockingPanel)(object)((Control)floatingForm).Controls[0]);
								if (((ArrangedElementCollection)((Control)stiDockingPanel).Controls).Count == 0)
								{
									UndockedPanels.Remove(stiDockingPanel);
								}
							}
							if (controlIndex == selectedIndex && ((StiDockingPanel)(object)((Control)dockingControl).Parent).SelectedControl != dockingControl)
							{
								((StiDockingPanel)(object)((Control)dockingControl).Parent).SelectedControl = dockingControl;
							}
						}
						else
						{
							stiDockingPanel.DoClose((Control)(object)dockingControl);
						}
					}
				}
			}
			LoadNodes(tr);
		}
	}

	internal void SetLayoutAllPanels()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (ParentForm != null)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)ParentForm).Controls)
			{
				Control val = item;
				if (val is StiDockingPanel)
				{
					((StiDockingPanel)(object)val).SetLayoutAllPanels();
					return;
				}
			}
		}
		using List<StiDockingPanel>.Enumerator enumerator2 = UndockedPanels.GetEnumerator();
		if (enumerator2.MoveNext())
		{
			StiDockingPanel current = enumerator2.Current;
			current.SetLayoutAllPanels();
		}
	}

	internal void AddUndockingPanels(StiDockingPanel dockingPanel)
	{
		UndockedPanels.Add(dockingPanel);
		Form = (Form)((dockingPanel.FloatingForm != null) ? ((object)dockingPanel.LastDockForm) : ((object)/*isinst with value type is only supported in some contexts*/));
	}

	internal void RemoveUndockingPanels(StiDockingPanel dockingPanel)
	{
		UndockedPanels.Remove(dockingPanel);
	}

	internal StiDockingPanel GetDockingPanel(DockStyle dockStyle)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		foreach (Control item in (ArrangedElementCollection)((Control)ParentForm).Controls)
		{
			Control val = item;
			if (val is StiDockingPanel && val.Dock == dockStyle)
			{
				return val as StiDockingPanel;
			}
		}
		return null;
	}

	internal StiDockingControl GetDockingControl(string guid)
	{
		List<StiDockingPanel> dockedPanels = DockedPanels;
		foreach (StiDockingPanel item in dockedPanels)
		{
			foreach (StiDockingControl item2 in (ArrangedElementCollection)((Control)item).Controls)
			{
				if (item2.Guid == guid)
				{
					return item2;
				}
			}
		}
		foreach (StiDockingPanel undockedPanel in UndockedPanels)
		{
			foreach (StiDockingControl item3 in (ArrangedElementCollection)((Control)undockedPanel).Controls)
			{
				if (item3.Guid == guid)
				{
					return item3;
				}
			}
		}
		foreach (StiDockingControl closedControl in ClosedControls)
		{
			StiDockingPanel stiDockingPanel = new StiDockingPanel(this);
			stiDockingPanel.LastDockForm = ParentForm;
			((Control)stiDockingPanel).Controls.Add((Control)(object)closedControl);
			if (closedControl.Guid == guid)
			{
				return closedControl;
			}
		}
		return null;
	}

	private void OnClosedUndockingPanels(object sender, EventArgs e)
	{
		foreach (StiDockingPanel undockedPanel in UndockedPanels)
		{
			Control parent = ((Control)undockedPanel).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			if (val != null)
			{
				val.Close();
				((Component)(object)val).Dispose();
			}
		}
		UndockedPanels.Clear();
	}

	public StiDockingManager()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		IContainer container = base.Container;
	}
}
