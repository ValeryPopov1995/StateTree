using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ValeryPopov.Common.StateTree
{
    public abstract class LabelPropertyAttribute : PropertyAttribute
    {
        public string Label;

        public LabelPropertyAttribute(string label)
        {
            Label = label;
        }
    }

    public abstract class ClassDrawerBase<T> : PropertyDrawer where T : class
    {
        private List<string> types = new();

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            List<string> names = GetAllOfType();//.Select(t => t.Substring(0, t.IndexOf("Assembly"))).ToList();
            property.stringValue = DrawTypeSelect(position, property.stringValue, names);
        }

        private string DrawTypeSelect(Rect position, string currentValue, List<string> options)
        {
            var index = options.FindIndex(x => x == currentValue);

            index = EditorGUI.Popup(position, (attribute as LabelPropertyAttribute).Label, index, options.ToArray());

            if (index == -1)
                return "";

            return options[index];
        }

        private List<string> GetAllOfType()
        {
            if (!types.Any())
                types = LoadAllOfType();

            return types;
        }

        protected virtual List<string> LoadAllOfType()
        {
            var type = typeof(T);

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !a.FullName.ToLower().Contains("test"))
                .SelectMany(a => a.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsAbstract)
                .Select(x => x.AssemblyQualifiedName).ToList();
        }
    }
}
