using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ScreepsUtil.Util
{
    static class DeclarationGenerator
    {
        /// <summary>
        /// Returns a dictionary of Type to Names[]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ParseFile(string path)
        {
            var typeToNames = new Dictionary<string, string[]>();

            var fileLines = File.ReadAllLines(path);
            var nextIsType = true;
            var names = new List<string>();
            var type = string.Empty;
            foreach (var line in fileLines)
            {
                if (nextIsType)
                {
                    type = line.Trim();
                    nextIsType = false;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    nextIsType = true;
                    typeToNames.Add(type, names.ToArray());
                    names = new List<string>();
                    type = string.Empty;
                }
                else
                {
                    names.Add(line.Trim());
                }
            }
            if (!string.IsNullOrEmpty(type))
            {
                typeToNames.Add(type, names.ToArray());
            }

            return typeToNames;
        }

        public static string MakeTypeFile(Dictionary<string, string[]> typeToNames)
        {
            var sb = new StringBuilder();
            foreach (var kvp in typeToNames)
            {
                sb.AppendLine(constLines(kvp.Key, kvp.Value));
            }

            foreach (var kvp in typeToNames)
            {
                sb.AppendLine(typeLines(kvp.Key, kvp.Value));
            }

            foreach (var kvp in typeToNames)
            {
                sb.AppendLine(aggregateType(kvp.Key, kvp.Value));
                sb.AppendLine();
            }

            return sb.ToString();
        }
        
        public static string MakeGlobalDeclarationFile(Dictionary<string, string[]> typeToNames)
        {
            var sb = new StringBuilder();
            sb.AppendLine("declare interface global {");
            foreach (var kvp in typeToNames)
            {                
                sb.AppendLine(globalInterfaceLines(kvp.Key, kvp.Value));
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string MakeGlobalExtensionScriptFile(Dictionary<string, string[]> typeToNames)
        {
            var sb = new StringBuilder();
            foreach (var kvp in typeToNames)
            {
                sb.AppendLine(GlobalExtensionLines(kvp.Key, kvp.Value));
            }
            sb.AppendLine();
            sb.AppendLine("export class GlobalConstants {");
            sb.AppendLine("\tpublic static extend() {");
            sb.AppendLine("\t}");
            sb.Append("}");
            return sb.ToString();
        }
        
        private static string GlobalExtensionLines(string type, string[] names)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                sb.AppendLine(GlobalExtensionLine(type, names[i]));
            }
            return sb.ToString();
        }

        private static string GlobalExtensionLine(string type, string name)
        {
            return $"global.{makeIdentifier(type, name)} = {makeQuotedValue(name)};";
        }
        
        private static string globalInterfaceLines(string type, string[] names)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                sb.AppendLine(globalInterfaceLine(type, names[i]));
            }
            return sb.ToString();
        }

        private static string globalInterfaceLine(string type, string name)
        {            
            return $"{makeIdentifier(type, name)}: string;";
        }


        // type file

        private static string constLines(string type, string[] names)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                sb.AppendLine(constLine(type, names[i]));
            }
            return sb.ToString();
        }

        private static string typeLines(string type, string[] names)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                sb.AppendLine(typeLine(type, names[i]));
            }
            return sb.ToString();
        }        

        private static string constLine(string type, string name)
        {            
            return $"declare const {makeIdentifier(type, name)}: {makeQuotedValue(name)};";
        }
        
        private static string typeLine(string type, string name)
        {            
            return $"type {makeIdentifier(type, name)} = {makeQuotedValue(name)};";
        }

        private static string aggregateType(string type, string[] names)
        {
            var identifiers =
                string.Join($" | {Environment.NewLine}",
                names.Select(p => makeIdentifier(type, p)));
            
            return $"type {type.Replace("_","")}Type ={Environment.NewLine}{identifiers};";
        }

        private static string makeIdentifier(string type, string name)
        { // Operation, Tower Construction => OPERATION_TOWER_CONSTRUCTION
            var split = name.ToUpper().Split(' ');
            return $"{type.ToUpper()}_{string.Join("_", split)}";
        }

        private static string makeQuotedValue(string name)
        { // Tower Construction => "TowerConstruction"
            return $"\"{name.Replace(" ", "")}\"";
        }
    }
}
