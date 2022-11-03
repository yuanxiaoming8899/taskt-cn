﻿using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Math")]
    [Attributes.ClassAttributes.Description("This command allows you to get variance value from a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get variance value from a list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetVarianceFromListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a Variable Name to Store Result")]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select If List Value is Not Numeric")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfValueIsNotNumeric { get; set; }

        public GetVarianceFromListCommand()
        {
            this.CommandName = "GetVarianceFromListCommand";
            this.SelectionName = "Get Variance From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var notNumeric = v_IfValueIsNotNumeric.GetUISelectionValue("v_IfValueIsNotNumeric", this, engine);
            var notNumeric = this.GetUISelectionValue(nameof(v_IfValueIsNotNumeric), "Not Numeric", engine);

            var list = ListControls.GetDecimalListVariable(v_InputList, (notNumeric == "ignore"), engine);

            if (list.Count() > 0)
            {
                decimal ave = list.Average();

                decimal sum = 0;
                foreach (var v in list)
                {
                    sum += (v - ave) * (v - ave);
                }

                (sum / list.Count()).ToString().StoreInUserVariable(engine, v_Result);
            }
            else
            {
                "0".StoreInUserVariable(engine, v_Result);
            }
        }
    }
}