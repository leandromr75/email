using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email
{
    class Global
    {
        public static class cliente
        {
            private static string nome = ""; //sim,não
            public static string clientes
            {
                get { return nome; }
                set { nome = value; }
            }
            private static string nome2 = ""; //sim,não
            public static string email
            {
                get { return nome2; }
                set { nome2 = value; }
            }
        }
    }
}
