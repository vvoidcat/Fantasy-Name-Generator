using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public static class Program {
        public static void Main() {
            Test();
        }




        public static void Test() {
            string elven = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\elven_generic.csv";
            // string human = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\human_generic.csv";
            string human = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\italian.csv";

            Generator elvenGen = new Generator(elven);

            Generator humanGen = new Generator(human);
            humanGen.settings.maxRowVows = 2;
            humanGen.settings.maxRowCons = 2;


            for (int i = 0; i < 20; i++) {
                elvenGen.GenerateName();
                humanGen.GenerateName();
            }

            Console.Write("elven\n\n");

            for (int i = 0; i < elvenGen.GetAllNames().Count; i++) {
                Console.WriteLine(elvenGen.GetAllNames()[i]);
            }

            Console.Write("\n\nhuman\n\n");

            for (int i = 0; i < humanGen.GetAllNames().Count; i++) {
                Console.WriteLine(humanGen.GetAllNames()[i]);
            }
        }
    }
}

