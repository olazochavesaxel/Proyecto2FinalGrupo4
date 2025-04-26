using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class InversionManager
    {
        private readonly InversionCrudFactory _inversionCrud;

        public InversionManager()
        {
            _inversionCrud = new InversionCrudFactory();
        }

        public List<Inversion> RetrieveAll()
        {
            return _inversionCrud.RetrieveAll<Inversion>();
        }
    }
}
