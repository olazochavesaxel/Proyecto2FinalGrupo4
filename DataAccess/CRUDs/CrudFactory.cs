using DataAccess.DAOs;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUDs
{
    /*Clase padre abstracta de los Cruds*/
    /*Define como se hacen los Cruds en la arquitectura*/
    public abstract class CrudFactory
    {
        protected SqlDAO _sqlDAO;

        //definimos los metodos del contrato
        // Create
        // Read || Retrieve
        //Update
        //Delete
        public abstract void Create(BaseDTO dto);
        public abstract void Update(BaseDTO dto);
        public abstract void Delete(BaseDTO dto);


        // T = Es una fprma de parametrizar el dato que nos devuelve el metodo
        public abstract T Retrieve<T>();
        public abstract T RetrieveById<T>(int id);
        public abstract List<T> RetrieveAll<T>();
    }
}
