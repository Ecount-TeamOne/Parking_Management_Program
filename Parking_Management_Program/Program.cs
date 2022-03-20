using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parking_Management_Program
{

    class Program
    {
        static void Main(string[] args)
        {
            Manager bm = new Manager();
            bm.Run();
        }
    }
}

