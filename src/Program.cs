using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public static class Program {
        public static void Main() {
            string elven = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\elven_generic.csv";
            string human = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\human_generic.csv";

            Generator elvenGen = new Generator(elven);
            Generator humanGen = new Generator(human);

            for (int i = 0; i < 20; i++) {
                elvenGen.GenerateName();
                humanGen.GenerateName();
            }

            //Console.WriteLine(elvenGen.GetLastName() + "\n");

            Console.Write("elven\n\n");

            for (int i = 0; i < elvenGen.GetAllNames().Count; i++) {
                Console.WriteLine(elvenGen.GetAllNames()[i]);
            }
            Console.Write("\n");

            for (int i = 0; i < 26; i++) {
                Console.Write(elvenGen.settings.probabilityMatrix[0, i] + "  ");
            }


            Console.Write("\n\nhuman\n\n");

            for (int i = 0; i < humanGen.GetAllNames().Count; i++) {
                Console.WriteLine(humanGen.GetAllNames()[i]);
            }
            Console.Write("\n");

            for (int i = 0; i < 26; i++) {
                Console.Write(humanGen.settings.probabilityMatrix[0, i] + "  ");
            }
        }
    }
}

