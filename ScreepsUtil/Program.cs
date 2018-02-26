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

            var typeFile = Util.DeclarationGenerator.makeTypeFile(typeToNames);
            var typePath = Path.Combine(Directory.GetCurrentDirectory(), "declarations.txt");

            var globalFile = Util.DeclarationGenerator.makeGlobalFile(typeToNames);
            var globalPath = Path.Combine(Directory.GetCurrentDirectory(), "global.txt");

            File.WriteAllText(typePath, typeFile);
            File.WriteAllText(globalPath, globalFile);
        }

        static void TestMapRotation()
        {
            var rotation = new Sandbox.MapRotationTest();
            rotation.Test(12, 6);
        }
    }
}
