using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;

namespace CoreApp
{
    public class PagoManager
    {
        private PagoCrudFactory crud;

        public PagoManager()
        {
            crud = new PagoCrudFactory();
        }

        public void RegistrarPago(Pago pago)
        {
            crud.Create(pago);
        }
    }

}
