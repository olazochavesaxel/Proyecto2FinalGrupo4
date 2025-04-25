using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;
using DTOs;

namespace CoreApp
{
    public class TransaccionClienteManager : BaseManager
    {
        private readonly TransaccionClienteCrudFactory transaccionClienteCrud;

        public TransaccionClienteManager()
        {
            transaccionClienteCrud = new TransaccionClienteCrudFactory();
        }

        public void Create(TransaccionCliente transaccionCliente)
        {
            transaccionClienteCrud.Create(transaccionCliente);
        }

        public void Update(TransaccionCliente transaccionCliente)
        {
            transaccionClienteCrud.Update(transaccionCliente);
        }

        public void Delete(TransaccionCliente transaccionCliente)
        {
            transaccionClienteCrud.Delete(transaccionCliente);
        }

        public List<TransaccionCliente> RetrieveAll()
        {
            return transaccionClienteCrud.RetrieveAll<TransaccionCliente>();
        }
        

        public TransaccionCliente RetrieveById(int id)
        {
            return transaccionClienteCrud.RetrieveById<TransaccionCliente>(id);
        }

        public List<TransaccionCliente> RetrieveByTipo(string tipo)
        {
            return transaccionClienteCrud.RetrieveByTipo<TransaccionCliente>(tipo);
        }

        public List<TransaccionCliente> RetrieveByIdCliente(List<int> idCliente)
        {
            var transacciones = transaccionClienteCrud.RetrieveByIdCliente<TransaccionCliente>(idCliente[0]);
            return transacciones;
        }


    }


}



