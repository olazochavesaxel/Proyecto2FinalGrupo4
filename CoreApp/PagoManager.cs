using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class PagoManager
    {
        private readonly PagoCrudFactory pagoCrud;




        public PagoManager()
        {
            pagoCrud = new PagoCrudFactory();
        }

        public void Create(Pago pago)
        {

            pagoCrud.Create(pago);

        }

        public void Update(Pago pago)
        {

            pagoCrud.Create(pago);

        }

        public void Delete(Pago pago)
        {
            pagoCrud.Create(pago);
        }

        public List<Pago> RetrieveAll()
        {
            return pagoCrud.RetrieveAll<Pago>();
        }

        public Pago RetrieveById(int id)
        {
            return pagoCrud.RetrieveById<Pago>(id);
        }

        public Pago RetrieveByTransaccionCliente(int idTransaccionCliente)
        {
            return pagoCrud.RetrieveById<Pago>(idTransaccionCliente);
        }
        public Pago RetrieveByTransaccionAsesor(int idTransaccionAsesor)
        {
            return pagoCrud.RetrieveById<Pago>(idTransaccionAsesor);
        }
    }
}
