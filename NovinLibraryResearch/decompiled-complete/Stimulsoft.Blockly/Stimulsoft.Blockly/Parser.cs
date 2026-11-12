using System;
using System.Collections.Generic;
using System.Xml;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly;

public class Parser
{
	private IDictionary<string, Func<IronBlock>> blocks = new Dictionary<string, Func<IronBlock>>();

	public Parser AddBlock<T>(string type) where T : IronBlock, new()
	{
		AddBlock(type, () => new T());
		return this;
	}

	public Parser AddBlock<T>(string type, T block) where T : IronBlock
	{
		AddBlock(type, () => block);
		return this;
	}

	public Parser AddBlock(string type, Func<IronBlock> blockFactory)
	{
		if (blocks.ContainsKey(type))
		{
			blocks[type] = blockFactory;
			return this;
		}
		blocks.Add(type, blockFactory);
		return this;
	}

	public Workspace Parse(string xml, bool preserveWhitespace = false)
	{
		XmlDocument xmlDocument = new XmlDocument
		{
			PreserveWhitespace = preserveWhitespace
		};
		xmlDocument.LoadXml(xml);
		Workspace workspace = new Workspace();
		foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
		{
			if (childNode.LocalName == "block" || childNode.LocalName == "shadow")
			{
				IronBlock ironBlock = ParseBlock(childNode);
				if (ironBlock != null)
				{
					workspace.Blocks.Add(ironBlock);
				}
			}
		}
		return workspace;
	}

	public IronBlock ParseBlock(XmlNode node)
	{
		if (bool.Parse(node.GetAttribute("disabled") ?? "false"))
		{
			return null;
		}
		string attribute = node.GetAttribute("type");
		if (!blocks.ContainsKey(attribute))
		{
			throw new ApplicationException("block type not registered: '" + attribute + "'");
		}
		IronBlock ironBlock = blocks[attribute]();
		ironBlock.Type = attribute;
		ironBlock.Id = node.GetAttribute("id");
		foreach (XmlNode childNode in node.ChildNodes)
		{
			switch (childNode.LocalName)
			{
			case "mutation":
				ParseMutation(childNode, ironBlock);
				break;
			case "field":
				ParseField(childNode, ironBlock);
				break;
			case "value":
				ParseValue(childNode, ironBlock);
				break;
			case "statement":
				ParseStatement(childNode, ironBlock);
				break;
			case "next":
			{
				IronBlock ironBlock2 = ParseBlock(childNode.FirstChild);
				if (ironBlock2 != null)
				{
					ironBlock.Next = ironBlock2;
				}
				break;
			}
			default:
				throw new ArgumentException("unknown xml type: " + childNode.LocalName);
			case "comment":
				break;
			}
		}
		return ironBlock;
	}

	public void ParseField(XmlNode fieldNode, IronBlock block)
	{
		Field item = new Field
		{
			Name = fieldNode.GetAttribute("name"),
			Value = fieldNode.InnerText
		};
		block.Fields.Add(item);
	}

	public void ParseValue(XmlNode valueNode, IronBlock block)
	{
		XmlNode xmlNode = valueNode.GetChild("block") ?? valueNode.GetChild("shadow");
		if (xmlNode != null)
		{
			IronBlock block2 = ParseBlock(xmlNode);
			Value item = new Value
			{
				Name = valueNode.GetAttribute("name"),
				Block = block2
			};
			block.Values.Add(item);
		}
	}

	public void ParseStatement(XmlNode statementNode, IronBlock block)
	{
		XmlNode xmlNode = statementNode.GetChild("block") ?? statementNode.GetChild("shadow");
		if (xmlNode != null)
		{
			IronBlock block2 = ParseBlock(xmlNode);
			Statement item = new Statement
			{
				Name = statementNode.GetAttribute("name"),
				Block = block2
			};
			block.Statements.Add(item);
		}
	}

	public void ParseMutation(XmlNode mutationNode, IronBlock block)
	{
		foreach (XmlAttribute attribute in mutationNode.Attributes)
		{
			block.Mutations.Add(new Mutation("mutation", attribute.Name, attribute.Value));
		}
		foreach (XmlNode childNode in mutationNode.ChildNodes)
		{
			foreach (XmlAttribute attribute2 in childNode.Attributes)
			{
				block.Mutations.Add(new Mutation(childNode.Name, attribute2.Name, attribute2.Value));
			}
		}
	}
}
