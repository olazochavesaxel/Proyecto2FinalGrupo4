using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class AccionManager
    {
        private readonly AccionCrudFactory accionCrud;

        public AccionManager()
        {
            accionCrud = new AccionCrudFactory();
        }

        public void CreateUpdate(Accion accion)
        {
            accionCrud.CreateUpdate(accion);
        }

        public List<Accion> RetrieveAll()
        {
            return accionCrud.RetrieveAll<Accion>();
        }

        public Accion RetrieveById(int id)
        {
            return accionCrud.RetrieveById<Accion>(id);
        }
    }
}
