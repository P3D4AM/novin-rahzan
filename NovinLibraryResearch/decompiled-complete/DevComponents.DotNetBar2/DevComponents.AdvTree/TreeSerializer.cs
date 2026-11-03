using System.IO;
using System.Xml;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

public class TreeSerializer
{
	private static string _29D1 = "Node";

	private static string _29D2 = "Cells";

	private static string _29D3 = "Cell";

	private static string _29D4 = "Images";

	private static string _29D5 = "AdvTree";

	private static string _29D6 = "Custom";

	public static void Save(AdvTree tree, string fileName)
	{
		XmlDocument xmlDocument = Save(tree);
		xmlDocument.Save(fileName);
	}

	public static void Save(AdvTree tree, Stream outStream)
	{
		XmlDocument xmlDocument = Save(tree);
		xmlDocument.Save(outStream);
	}

	public static void Save(AdvTree tree, TextWriter writer)
	{
		XmlDocument xmlDocument = Save(tree);
		xmlDocument.Save(writer);
	}

	public static void Save(AdvTree tree, XmlWriter writer)
	{
		XmlDocument xmlDocument = Save(tree);
		xmlDocument.Save(writer);
	}

	public static XmlDocument Save(AdvTree tree)
	{
		XmlDocument xmlDocument = new XmlDocument();
		Save(tree, xmlDocument);
		return xmlDocument;
	}

	public static void Save(AdvTree tree, XmlDocument document)
	{
		XmlElement xmlElement = document.CreateElement(_29D5);
		document.AppendChild(xmlElement);
		Save(tree, xmlElement);
	}

	public static void Save(AdvTree tree, XmlElement parent)
	{
		NodeSerializationContext nodeSerializationContext = new NodeSerializationContext();
		nodeSerializationContext.RefXmlElement = parent;
		nodeSerializationContext.AdvTree = tree;
		nodeSerializationContext.HasSerializeNodeHandlers = tree.Ղ;
		nodeSerializationContext.HasDeserializeNodeHandlers = tree.Ճ;
		foreach (Node node in tree.Nodes)
		{
			Save(node, nodeSerializationContext);
		}
	}

	public static void Save(Node node, NodeSerializationContext context)
	{
		XmlElement refXmlElement = context.RefXmlElement;
		XmlElement xmlElement = refXmlElement.OwnerDocument.CreateElement(_29D1);
		refXmlElement.AppendChild(xmlElement);
		ElementSerializer.Serialize(node, xmlElement);
		if (context.HasSerializeNodeHandlers)
		{
			XmlElement xmlElement2 = refXmlElement.OwnerDocument.CreateElement(_29D6);
			SerializeNodeEventArgs ؿ = new SerializeNodeEventArgs(node, xmlElement, xmlElement2);
			context.AdvTree.ہ(ؿ);
			if (xmlElement2.Attributes.Count > 0 || xmlElement2.ChildNodes.Count > 0)
			{
				xmlElement.AppendChild(xmlElement2);
			}
		}
		if (node.Cells.Count > 1)
		{
			XmlElement xmlElement3 = refXmlElement.OwnerDocument.CreateElement(_29D2);
			xmlElement.AppendChild(xmlElement3);
			for (int i = 1; i < node.Cells.Count; i++)
			{
				Cell cell = node.Cells[i];
				XmlElement xmlElement4 = refXmlElement.OwnerDocument.CreateElement(_29D3);
				xmlElement3.AppendChild(xmlElement4);
				ElementSerializer.Serialize(cell, xmlElement4);
				if (cell.ShouldSerializeImages())
				{
					XmlElement xmlElement5 = refXmlElement.OwnerDocument.CreateElement(_29D4);
					xmlElement4.AppendChild(xmlElement5);
					ElementSerializer.Serialize(cell.Images, xmlElement5);
				}
			}
		}
		context.RefXmlElement = xmlElement;
		foreach (Node node2 in node.Nodes)
		{
			Save(node2, context);
		}
		context.RefXmlElement = refXmlElement;
	}

	public static void Load(AdvTree tree, string fileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(fileName);
		Load(tree, xmlDocument);
	}

	public static void Load(AdvTree tree, Stream inStream)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(inStream);
		Load(tree, xmlDocument);
	}

	public static void Load(AdvTree tree, TextReader reader)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(reader);
		Load(tree, xmlDocument);
	}

	public static void Load(AdvTree tree, XmlReader reader)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(reader);
		Load(tree, xmlDocument);
	}

	public static void Load(AdvTree tree, XmlDocument document)
	{
		foreach (XmlNode childNode in document.ChildNodes)
		{
			if (childNode.Name == _29D5 && childNode is XmlElement)
			{
				Load(tree, childNode as XmlElement);
				break;
			}
		}
	}

	public static void Load(AdvTree tree, XmlElement parent)
	{
		tree.BeginUpdate();
		tree.DisplayRootNode = null;
		tree.Nodes.Clear();
		NodeSerializationContext nodeSerializationContext = new NodeSerializationContext();
		nodeSerializationContext.AdvTree = tree;
		nodeSerializationContext.HasDeserializeNodeHandlers = tree.Ճ;
		nodeSerializationContext.HasSerializeNodeHandlers = tree.Ղ;
		try
		{
			foreach (XmlNode childNode in parent.ChildNodes)
			{
				if (childNode.Name == _29D1 && childNode is XmlElement)
				{
					Node node = new Node();
					tree.Nodes.Add(node);
					nodeSerializationContext.RefXmlElement = childNode as XmlElement;
					LoadNode(node, nodeSerializationContext);
				}
			}
		}
		finally
		{
			tree.EndUpdate();
		}
	}

	public static void LoadNode(Node nodeToLoad, NodeSerializationContext context)
	{
		XmlElement refXmlElement = context.RefXmlElement;
		ElementSerializer.Deserialize(nodeToLoad, refXmlElement);
		foreach (XmlNode childNode in refXmlElement.ChildNodes)
		{
			if (childNode is XmlElement xmlElement)
			{
				if (xmlElement.Name == _29D1)
				{
					Node node = new Node();
					nodeToLoad.Nodes.Add(node);
					context.RefXmlElement = xmlElement;
					LoadNode(node, context);
				}
				else if (xmlElement.Name == _29D2)
				{
					_29D7(nodeToLoad, xmlElement);
				}
				else if (xmlElement.Name == _29D6 && context.HasDeserializeNodeHandlers)
				{
					SerializeNodeEventArgs ؿ = new SerializeNodeEventArgs(nodeToLoad, refXmlElement, xmlElement);
					context.AdvTree.ۂ(ؿ);
				}
			}
		}
		context.RefXmlElement = refXmlElement;
	}

	private static void _29D7(Node ڙ, XmlElement _29D8)
	{
		foreach (XmlNode childNode in _29D8.ChildNodes)
		{
			if (!(childNode.Name == _29D3) || !(childNode is XmlElement))
			{
				continue;
			}
			Cell cell = new Cell();
			ڙ.Cells.Add(cell);
			ElementSerializer.Deserialize(cell, childNode as XmlElement);
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				if (childNode2.Name == _29D4)
				{
					ElementSerializer.Deserialize(cell.Images, childNode2);
					break;
				}
			}
		}
	}
}
