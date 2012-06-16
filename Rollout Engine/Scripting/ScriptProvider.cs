﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Rollout.Core;
using Rollout.Utility;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting
{
    public class ScriptProvider
    {
        public XmlScriptingEngine Engine { get; private set; }

        public static Dictionary<string, Type> SpriteTypes { get; set; }
        public static Dictionary<string, XElement> Templates;
        private static Dictionary<string, ActionInfo> ActionTypes { get; set; } 

        public ScriptProvider(XmlScriptingEngine engine)
        {
            Engine = engine;
            Templates = new Dictionary<string, XElement>();
            ActionTypes = new Dictionary<string, ActionInfo>();
            SpriteTypes = new Dictionary<string, Type>();

            RegisterActions();
        }

        public void Load(string assetName)
        {
            var doc = XElement.Load(G.Content.RootDirectory + @"\Scripts\" + assetName + ".xml");

            //load sprite types
            var spriteTypes = AttributeHelper.GetTypesWith<SpriteAttribute>();
            foreach (var spriteType in spriteTypes)
            {
                var spriteAttribute = (SpriteAttribute)spriteType.GetCustomAttributes(typeof (SpriteAttribute), false)[0];

                SpriteTypes.Add(spriteAttribute.Name,spriteType);
            }

            //load templates
            var templates = doc.Elements("template");
            foreach (var template in templates)
            {
                var id = template.Attribute("id").Value;
                Templates.Add(id, template);
            }

            //load action scripts
            var scripts = doc.Elements("script");
            foreach(var script in scripts)
            {
                ProcessScript(script);
            }

        }

        private void RegisterActions()
        {
            //find all Types with the ActionAttribute
            var actionTypes = AttributeHelper.GetTypesWith<ActionAttribute>();
            foreach (var actionType in actionTypes)
            {
                var actionAttribute = (ActionAttribute)actionType.GetCustomAttributes(typeof (ActionAttribute), false)[0];
                var actionInfo = new ActionInfo() {Name = actionAttribute.Name, Type = actionType};

                //get all ActionParam attributes on the Type
                foreach(ActionParamAttribute actionParam in actionType.GetCustomAttributes(typeof(ActionParamAttribute), false))
                {
                    var paramInfo = new ActionInfo.ParamInfo() {Name = actionParam.Name};
                    actionInfo.Params.Add(paramInfo);
                }

                //sort the ActionParams since we can't guarantee order
                actionInfo.Params = actionInfo.Params.OrderBy(p => p.Order).ToList();

                ActionTypes.Add(actionInfo.Name, actionInfo);
            }
        }

        private void ProcessScript(XElement script)
        {
            string forName = script.Attribute("for") != null ? script.Attribute("for").Value : "_screen";

            foreach (var child in script.Elements())
            {
                //Engine.AddAction(source, ProcessElement(child,source));
                IAction action = ProcessAction(child, forName);
                if (action != null)
                    Engine.AddAction(forName, action);
            }
        }

        public static IAction ProcessAction(XElement node, string source)
        {
            IAction action = null;
            string actionName = node.Name.ToString();

            if (ActionTypes.ContainsKey(actionName))
            {
                ActionInfo actionInfo = ActionTypes[actionName];
                Dictionary<string, Expression> args = new Dictionary<string, Expression>();

                args.Add("source", new Expression(source));

                foreach (var paramInfo in actionInfo.Params)
                {
                    string name = paramInfo.Name;
                    string value = node.Attribute(paramInfo.Name) != null ? node.Attribute(paramInfo.Name).Value : "";

                    args.Add(name, new Expression(value));
                }

                action = Activator.CreateInstance(actionInfo.Type, args) as IAction;

                if (action != null)
                {
                    action.Wait = IsWaitingAction(node);

                    foreach (var child in node.Elements())
                    {
                        var childAction = ProcessAction(child, source);
                        if (childAction != null)
                        action.AddAction(childAction);
                    }
                }
            }

            return action;
        }

        private static bool IsWaitingAction(XElement node)
        {
            if (node.Attribute("wait") == null || node.Name == "wait")
                return true;
            return false;
        }

    }

}
