using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyEvaluationResultsWriter : IDisposable
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyEvaluationResultsWriter");

	private XmlWriter xmlWriter;

	private bool disposed;

	public PolicyEvaluationResultsWriter(XmlWriter xmlWriter)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PolicyEvaluationResultsWriter", TraceEventType.Information);
		if (xmlWriter == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("xmlWriter"));
		}
		this.xmlWriter = xmlWriter;
		WriteAggergateEvaluationHistoryStart();
	}

	public void WriteEvaluationHistory(EvaluationHistory history)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("WriteEvaluationHistory", TraceEventType.Information);
		methodTraceContext.TraceParameters(history);
		if (history == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("history"));
		}
		XmlWriterSettings xmlWriterSettings = GetXmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
		history.Serialize(xmlWriter);
		xmlWriter.Flush();
		xmlWriter.Close();
		this.xmlWriter.WriteRaw(stringBuilder.ToString());
		this.xmlWriter.Flush();
	}

	public static XmlWriterSettings GetXmlWriterSettings()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetXmlWriterSettings", TraceEventType.Information);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.UTF8;
		xmlWriterSettings.NewLineOnAttributes = true;
		xmlWriterSettings.Indent = true;
		methodTraceContext.TraceParameterOut("returnVal", xmlWriterSettings);
		return xmlWriterSettings;
	}

	private void WriteAggergateEvaluationHistoryStart()
	{
		xmlWriter.WriteStartDocument();
		xmlWriter.WriteStartElement("PolicyEvaluationResults");
		xmlWriter.Flush();
	}

	private void WriteAggergateEvaluationHistoryEnd()
	{
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndDocument();
		xmlWriter.Flush();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				WriteAggergateEvaluationHistoryEnd();
			}
			xmlWriter = null;
			disposed = true;
		}
	}
}
