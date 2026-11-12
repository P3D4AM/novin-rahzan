using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Console;

internal sealed class JsonConsoleFormatter : ConsoleFormatter, IDisposable
{
	private IDisposable _optionsReloadToken;

	internal JsonConsoleFormatterOptions FormatterOptions { get; set; }

	public JsonConsoleFormatter(IOptionsMonitor<JsonConsoleFormatterOptions> options)
		: base("json")
	{
		ReloadLoggerOptions(options.CurrentValue);
		_optionsReloadToken = options.OnChange(ReloadLoggerOptions);
	}

	public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
	{
		string text = logEntry.Formatter(logEntry.State, logEntry.Exception);
		if (logEntry.Exception == null && text == null)
		{
			return;
		}
		LogLevel logLevel = logEntry.LogLevel;
		string category = logEntry.Category;
		int id = logEntry.EventId.Id;
		Exception exception = logEntry.Exception;
		using (System.Text.Json.PooledByteBufferWriter pooledByteBufferWriter = new System.Text.Json.PooledByteBufferWriter(1024))
		{
			using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(pooledByteBufferWriter, FormatterOptions.JsonWriterOptions))
			{
				utf8JsonWriter.WriteStartObject();
				string timestampFormat = FormatterOptions.TimestampFormat;
				if (timestampFormat != null)
				{
					utf8JsonWriter.WriteString("Timestamp", (FormatterOptions.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now).ToString(timestampFormat));
				}
				utf8JsonWriter.WriteNumber("EventId", id);
				utf8JsonWriter.WriteString("LogLevel", GetLogLevelString(logLevel));
				utf8JsonWriter.WriteString("Category", category);
				utf8JsonWriter.WriteString("Message", text);
				if (exception != null)
				{
					string text2 = exception.ToString();
					if (!FormatterOptions.JsonWriterOptions.Indented)
					{
						text2 = text2.Replace(Environment.NewLine, " ");
					}
					utf8JsonWriter.WriteString("Exception", text2);
				}
				if (logEntry.State != null)
				{
					utf8JsonWriter.WriteStartObject("State");
					utf8JsonWriter.WriteString("Message", logEntry.State.ToString());
					if (logEntry.State is IReadOnlyCollection<KeyValuePair<string, object>> readOnlyCollection)
					{
						foreach (KeyValuePair<string, object> item in readOnlyCollection)
						{
							WriteItem(utf8JsonWriter, item);
						}
					}
					utf8JsonWriter.WriteEndObject();
				}
				WriteScopeInformation(utf8JsonWriter, scopeProvider);
				utf8JsonWriter.WriteEndObject();
				utf8JsonWriter.Flush();
			}
			textWriter.Write(Encoding.UTF8.GetString(pooledByteBufferWriter.WrittenMemory.Span.ToArray()));
		}
		textWriter.Write(Environment.NewLine);
	}

	private static string GetLogLevelString(LogLevel logLevel)
	{
		return logLevel switch
		{
			LogLevel.Trace => "Trace", 
			LogLevel.Debug => "Debug", 
			LogLevel.Information => "Information", 
			LogLevel.Warning => "Warning", 
			LogLevel.Error => "Error", 
			LogLevel.Critical => "Critical", 
			_ => throw new ArgumentOutOfRangeException("logLevel"), 
		};
	}

	private void WriteScopeInformation(Utf8JsonWriter writer, IExternalScopeProvider scopeProvider)
	{
		if (!FormatterOptions.IncludeScopes || scopeProvider == null)
		{
			return;
		}
		writer.WriteStartArray("Scopes");
		scopeProvider.ForEachScope(delegate(object scope, Utf8JsonWriter state)
		{
			if (scope is IEnumerable<KeyValuePair<string, object>> enumerable)
			{
				state.WriteStartObject();
				state.WriteString("Message", scope.ToString());
				foreach (KeyValuePair<string, object> item in enumerable)
				{
					WriteItem(state, item);
				}
				state.WriteEndObject();
			}
			else
			{
				state.WriteStringValue(ToInvariantString(scope));
			}
		}, writer);
		writer.WriteEndArray();
	}

	private void WriteItem(Utf8JsonWriter writer, KeyValuePair<string, object> item)
	{
		string key = item.Key;
		object value = item.Value;
		if (!(value is bool value2))
		{
			if (!(value is byte value3))
			{
				if (!(value is sbyte value4))
				{
					if (!(value is char c))
					{
						if (!(value is decimal value5))
						{
							if (!(value is double value6))
							{
								if (!(value is float value7))
								{
									if (!(value is int value8))
									{
										if (!(value is uint value9))
										{
											if (!(value is long value10))
											{
												if (!(value is ulong value11))
												{
													if (!(value is short value12))
													{
														if (!(value is ushort value13))
														{
															if (value == null)
															{
																writer.WriteNull(key);
															}
															else
															{
																writer.WriteString(key, ToInvariantString(item.Value));
															}
														}
														else
														{
															writer.WriteNumber(key, value13);
														}
													}
													else
													{
														writer.WriteNumber(key, value12);
													}
												}
												else
												{
													writer.WriteNumber(key, value11);
												}
											}
											else
											{
												writer.WriteNumber(key, value10);
											}
										}
										else
										{
											writer.WriteNumber(key, value9);
										}
									}
									else
									{
										writer.WriteNumber(key, value8);
									}
								}
								else
								{
									writer.WriteNumber(key, value7);
								}
							}
							else
							{
								writer.WriteNumber(key, value6);
							}
						}
						else
						{
							writer.WriteNumber(key, value5);
						}
					}
					else
					{
						writer.WriteString(key, c.ToString());
					}
				}
				else
				{
					writer.WriteNumber(key, value4);
				}
			}
			else
			{
				writer.WriteNumber(key, value3);
			}
		}
		else
		{
			writer.WriteBoolean(key, value2);
		}
	}

	private static string ToInvariantString(object obj)
	{
		return Convert.ToString(obj, CultureInfo.InvariantCulture);
	}

	private void ReloadLoggerOptions(JsonConsoleFormatterOptions options)
	{
		FormatterOptions = options;
	}

	public void Dispose()
	{
		_optionsReloadToken?.Dispose();
	}
}
