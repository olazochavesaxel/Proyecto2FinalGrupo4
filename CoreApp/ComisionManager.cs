using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;


namespace CoreApp
{


    public class ComisionManager
    {

        private readonly ComisionCrudFactory comisionCrud;

        public ComisionManager()
        {
            comisionCrud = new ComisionCrudFactory();
        }

        public void Create(Comision comision)
        {

            comisionCrud.Create(comision);

        }

        public void Update(Comision comision)
        {

            comisionCrud.Update(comision);

        }

        public void Delete(Comision comision)
        {
            comisionCrud.Delete(comision);
        }

        public List<Comision> RetrieveAll()
        {
            return comisionCrud.RetrieveAll<Comision>();
        }

        public Comision RetrieveById(int id)
        {
            return comisionCrud.RetrieveById<Comision>(id);
        }

        public Comision RetrieveByTipo(string tipo)
        {
            return comisionCrud.RetrieveByTipo<Comision>(tipo);
        }

    }
}

