using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScreepsUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            var typeToNames = Util.DeclarationGenerator.ParseFile(path);

            var typeFile = Util.DeclarationGenerator.MakeTypeFile(typeToNames);
            var typePath = Path.Combine(Directory.GetCurrentDirectory(), "cashew-types.d.ts.txt");

            var globalExtensionFile = Util.DeclarationGenerator.MakeGlobalExtensionScriptFile(typeToNames);
            var globalExtensionPath = Path.Combine(Directory.GetCurrentDirectory(), "imp-GlobalConstants.ts.txt");

            var globalDeclarationFile = Util.DeclarationGenerator.MakeGlobalDeclarationFile(typeToNames);
            var globalDeclarationPath = Path.Combine(Directory.GetCurrentDirectory(), "global-constants.d.ts.txt");

            File.WriteAllText(typePath, typeFile);
            File.WriteAllText(globalExtensionPath, globalExtensionFile);
            File.WriteAllText(globalDeclarationPath, globalDeclarationFile);
        }

        static void TestMapRotation()
        {
            var rotation = new Sandbox.MapRotationTest();
            rotation.Test(12, 6);
        }
    }
}
