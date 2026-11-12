using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Blockly.Blocks.Controls;
using Stimulsoft.Blockly.Blocks.Lists;
using Stimulsoft.Blockly.Blocks.Logic;
using Stimulsoft.Blockly.Blocks.Maths;
using Stimulsoft.Blockly.Blocks.Procedures;
using Stimulsoft.Blockly.Blocks.Text;
using Stimulsoft.Blockly.Blocks.Variables;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Blockly.StiBlocks.Data;
using Stimulsoft.Blockly.StiBlocks.Functions;
using Stimulsoft.Blockly.StiBlocks.Objects;
using Stimulsoft.Blockly.StiBlocks.Process;
using Stimulsoft.Blockly.StiBlocks.Report;
using Stimulsoft.Blockly.StiBlocks.System;
using Stimulsoft.Blockly.StiBlocks.Variables;
using Stimulsoft.Blockly.StiBlocks.Visuals;
using Stimulsoft.Report;

namespace Stimulsoft.Blockly.Blocks;

public static class Extensions
{
	public static object Evaluate(this IEnumerable<Value> values, string name, Context context)
	{
		Value value = values.FirstOrDefault((Value x) => x.Name == name);
		if (value == null)
		{
			throw new ArgumentException("value " + name + " not found");
		}
		return value.Evaluate(context);
	}

	public static string Get(this IEnumerable<Field> fields, string name)
	{
		Field field = fields.FirstOrDefault((Field x) => x.Name == name);
		if (field == null)
		{
			throw new ArgumentException("field " + name + " not found");
		}
		return field.Value;
	}

	public static Statement Get(this IEnumerable<Statement> statements, string name)
	{
		Statement statement = statements.FirstOrDefault((Statement x) => x.Name == name);
		if (statement == null)
		{
			throw new ArgumentException("statement " + name + " not found");
		}
		return statement;
	}

	public static string GetValue(this IList<Mutation> mutations, string name, string domain = "mutation")
	{
		return mutations.FirstOrDefault((Mutation x) => x.Domain == domain && x.Name == name)?.Value;
	}

	public static object Evaluate(this Workspace workspace, StiReport report, object sender, EventArgs valueArg, IDictionary<string, object> arguments = null)
	{
		Context context = new Context();
		context.Report = report;
		context.Sender = sender;
		context.EventArgs = valueArg;
		if (arguments != null)
		{
			context.Variables = arguments;
		}
		return workspace.Evaluate(context);
	}

	public static Context GetRootContext(this Context context)
	{
		for (Context context2 = context?.Parent; context2 != null; context2 = context2.Parent)
		{
			if (context2.Parent == null)
			{
				return context2;
			}
		}
		return context;
	}

	internal static string CreateValidName(this string name)
	{
		return name?.Replace(" ", "_");
	}

	public static Parser AddStandardBlocks(this Parser parser)
	{
		parser.AddBlock<ControlsRepeatExt>("controls_repeat_ext");
		parser.AddBlock<ControlsIf>("controls_if");
		parser.AddBlock<ControlsWhileUntil>("controls_whileUntil");
		parser.AddBlock<ControlsFlowStatement>("controls_flow_statements");
		parser.AddBlock<ControlsForEach>("controls_forEach");
		parser.AddBlock<ControlsFor>("controls_for");
		parser.AddBlock<LogicCompare>("logic_compare");
		parser.AddBlock<LogicBoolean>("logic_boolean");
		parser.AddBlock<LogicNegate>("logic_negate");
		parser.AddBlock<LogicOperation>("logic_operation");
		parser.AddBlock<LogicNull>("logic_null");
		parser.AddBlock<LogicTernary>("logic_ternary");
		parser.AddBlock<MathArithmetic>("math_arithmetic");
		parser.AddBlock<MathNumber>("math_number");
		parser.AddBlock<MathSingle>("math_single");
		parser.AddBlock<MathSingle>("math_trig");
		parser.AddBlock<MathRound>("math_round");
		parser.AddBlock<MathConstant>("math_constant");
		parser.AddBlock<MathNumberProperty>("math_number_property");
		parser.AddBlock<MathOnList>("math_on_list");
		parser.AddBlock<MathConstrain>("math_constrain");
		parser.AddBlock<MathModulo>("math_modulo");
		parser.AddBlock<MathRandomFloat>("math_random_float");
		parser.AddBlock<MathRandomInt>("math_random_int");
		parser.AddBlock<TextBlock>("text");
		parser.AddBlock<TextPrompt>("text_prompt_ext");
		parser.AddBlock<TextLength>("text_length");
		parser.AddBlock<TextIsEmpty>("text_isEmpty");
		parser.AddBlock<TextTrim>("text_trim");
		parser.AddBlock<TextCaseChange>("text_changeCase");
		parser.AddBlock<TextAppend>("text_append");
		parser.AddBlock<TextJoin>("text_join");
		parser.AddBlock<TextIndexOf>("text_indexOf");
		parser.AddBlock<VariablesGet>("variables_get");
		parser.AddBlock<VariablesSet>("variables_set");
		parser.AddBlock<ColourPicker>("colour_picker");
		parser.AddBlock<ColourRandom>("colour_random");
		parser.AddBlock<ColourRgb>("colour_rgb");
		parser.AddBlock<ColourBlend>("colour_blend");
		parser.AddBlock<ProceduresDef>("procedures_defnoreturn");
		parser.AddBlock<ProceduresDef>("procedures_defreturn");
		parser.AddBlock<ProceduresCallNoReturn>("procedures_callnoreturn");
		parser.AddBlock<ProceduresCallReturn>("procedures_callreturn");
		parser.AddBlock<ProceduresIfReturn>("procedures_ifreturn");
		parser.AddBlock<ListsSplit>("lists_split");
		parser.AddBlock<ListsCreateWith>("lists_create_with");
		parser.AddBlock<ListsLength>("lists_length");
		parser.AddBlock<ListsRepeat>("lists_repeat");
		parser.AddBlock<ListsIsEmpty>("lists_isEmpty");
		parser.AddBlock<ListsGetIndex>("lists_getIndex");
		parser.AddBlock<ListsIndexOf>("lists_indexOf");
		parser.AddBlock<StiColorHex>("sti_color_hex");
		parser.AddBlock<StiNewFont>("sti_new_font");
		parser.AddBlock<StiNewSolidBrush>("sti_new_solid_brush");
		parser.AddBlock<StiNewGradientBrush>("sti_new_gradient_brush");
		parser.AddBlock<StiNewBorder>("sti_new_border");
		parser.AddBlock<StiNewPenStyle>("sti_new_pen_style");
		parser.AddBlock<StiNewMargin>("sti_new_margin");
		parser.AddBlock<StiNewPadding>("sti_new_padding");
		parser.AddBlock<StiGetDataSource>("sti_get_data_source");
		parser.AddBlock<StiGetDataSourceByName>("sti_get_data_source_by_name");
		parser.AddBlock<StiDataSourceProperty>("sti_data_source_property");
		parser.AddBlock<StiDataSourceMethod>("sti_data_source_method");
		parser.AddBlock<StiSetDataSourceSqlCommand>("sti_set_data_source_sql_command");
		parser.AddBlock<StiGetVariable>("sti_get_variable");
		parser.AddBlock<StiGetVariableByName>("sti_get_variable_by_name");
		parser.AddBlock<StiSetVariable>("sti_set_variable");
		parser.AddBlock<StiSystemVariable>("sti_system_variable");
		parser.AddBlock<StiThisReport>("sti_this_report");
		parser.AddBlock<StiThisComponent>("sti_this_component");
		parser.AddBlock<StiAllComponents>("sti_all_components");
		parser.AddBlock<StiAllComponentsFrom>("sti_all_components_from");
		parser.AddBlock<StiGetCurrentValue>("sti_get_current_value");
		parser.AddBlock<StiSetCurrentValue>("sti_set_current_value");
		parser.AddBlock<StiGetComponentByName>("sti_get_component_by_name");
		parser.AddBlock<StiSetPropertyToValue>("sti_set_property_to_value");
		parser.AddBlock<StiOpenLink>("sti_open_link");
		parser.AddBlock<StiIsFirstPass>("sti_is_first_pass");
		parser.AddBlock<StiIsSecondPass>("sti_is_second_pass");
		parser.AddBlock<StiGetComponent>("sti_get_component");
		parser.AddBlock<StiSetPropertyOfObjectTo>("sti_set_property_of_object_to");
		parser.AddBlock<StiGetPropertyOfObject>("sti_get_property_of_object");
		parser.AddBlock<StiGetStyleByName>("sti_get_style_by_name");
		foreach (object key in StiBlocklyFunctionBlockKeyCache.GetBlockKeyTable().Keys)
		{
			parser.AddBlock<StiFunctionRun>(key.ToString());
		}
		return parser;
	}
}
