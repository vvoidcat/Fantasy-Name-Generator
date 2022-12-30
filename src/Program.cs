using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public static class Program {
        //public static void Main() {
        //    Test();
        //}

        public static void Test() {
            string elven = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\elven_generic.csv";
            // string human = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\human_generic.csv";
            string human = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\italian.csv";

            Settings preset = new Settings(elven, Language.English);
            Generator gen = new Generator(preset);


            for (int i = 0; i < 20; i++) {
                gen.GenerateName();
            }

            Console.Write("elven\n\n");

            for (int i = 0; i < gen.GetAllNames().Count; i++) {
                Console.WriteLine(gen.GetNameAtIndex(i));
            }
        }
    }
}

