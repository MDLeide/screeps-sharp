using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreepsUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMapRotation();
        }

        static void TestMapRotation()
        {
            var rotation = new Sandbox.MapRotationTest();
            rotation.Test(12, 6);
        }
    }
}
