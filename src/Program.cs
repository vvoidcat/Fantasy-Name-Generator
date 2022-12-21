using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public static class Program {
        public static void Main() {
            Generator generator = new Generator();


            for (int i = 0; i < 20; i++) {
                generator.GenerateName();
            }

            Console.WriteLine(generator.GetLastName() + "\n");

            for (int i = 0; i < generator.GetAllNames().Count; i++) {
                Console.WriteLine(generator.GetAllNames()[i]);
            }

            Console.WriteLine("\n{0} {1} {2} {3}\n", (double)0 / 3, (double)1 / 3, (double)2 / 3, (double)3 / 3);
        }
    }
}

