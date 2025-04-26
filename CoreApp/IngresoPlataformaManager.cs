using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;

namespace CoreApp
{
    namespace CoreApp
    {
        public class IngresoPlataformaManager : BaseManager
        {
            private readonly IngresoPlataformaCrudFactory ingresoPlataformaCrud;

            public IngresoPlataformaManager()
            {
                ingresoPlataformaCrud = new IngresoPlataformaCrudFactory();
            }

            public void Create(IngresoPlataforma ingresoPlataforma)
            {
                ingresoPlataformaCrud.Create(ingresoPlataforma);
            }

            public void Update(IngresoPlataforma ingresoPlataforma)
            {
                ingresoPlataformaCrud.Update(ingresoPlataforma);
            }

            public void Delete(IngresoPlataforma ingresoPlataforma)
            {
                ingresoPlataformaCrud.Delete(ingresoPlataforma);
            }

            public List<IngresoPlataforma> RetrieveAll()
            {
                return ingresoPlataformaCrud.RetrieveAll<IngresoPlataforma>();
            }

            public IngresoPlataforma RetrieveById(int id)
            {
                return ingresoPlataformaCrud.RetrieveById<IngresoPlataforma>(id);
            }

            public List<IngresoPlataforma> RetrieveByFechaIngreso(IngresoPlataforma filtro)
            {
                return ingresoPlataformaCrud.RetrieveByFechaIngreso<IngresoPlataforma>(filtro);
            }

            public List<IngresoPlataforma> RetrieveByIdAsesor(int idAsesor)
            {
                return ingresoPlataformaCrud.RetrieveByIdAsesor<IngresoPlataforma>(idAsesor);
            }

            public List<IngresoPlataforma> RetrieveByIdCliente(int idCliente)
            {
                return ingresoPlataformaCrud.RetrieveByIdCliente<IngresoPlataforma>(idCliente);
            }
        }
    }
}
