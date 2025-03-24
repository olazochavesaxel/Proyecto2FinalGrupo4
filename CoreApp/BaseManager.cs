using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class BaseManager
    {
        protected void ManageException(Exception exception) {
            Console.Write("Se creo una excepción " + exception.ToString());
                }
    }
}
