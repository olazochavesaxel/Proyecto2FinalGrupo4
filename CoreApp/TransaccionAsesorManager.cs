
using _00_DTO;
using DataAccess.CRUDs;

using TransaccionAsesor = DTOs.TransaccionAsesor;




namespace CoreApp
{
    public class TransaccionAsesorManager : BaseManager
    {
        private readonly TransaccionAsesorCrudFactory transaccionAsesorCrud;

        public TransaccionAsesorManager()
        {
            transaccionAsesorCrud = new TransaccionAsesorCrudFactory();
        }

        public void Create(TransaccionAsesor transaccionAsesor)
        {
            transaccionAsesorCrud.Create(transaccionAsesor);
        }

        public void Update(TransaccionAsesor transaccionAsesor)
        {
            transaccionAsesorCrud.Update(transaccionAsesor);
        }

        public void Delete(TransaccionAsesor transaccionAsesor)
        {
            transaccionAsesorCrud.Delete(transaccionAsesor);
        }

        public List<TransaccionAsesor> RetrieveAll()
        {
            return transaccionAsesorCrud.RetrieveAll<TransaccionAsesor>();
        }
        public TransaccionAsesor RetrieveById(int id)
        {
            return transaccionAsesorCrud.RetrieveById<TransaccionAsesor>(id);
        }


        public List<TransaccionAsesor> RetrieveByTipo(List<string> tipos)
        {
            // Llama al método RetrieveByTipo pasando solo los tipos
            var transacciones = transaccionAsesorCrud.RetrieveByTipo<TransaccionAsesor>(tipos);

            // Realiza la conversión si es necesario (en este caso, no parece ser necesario hacer una conversión explícita)
            List<TransaccionAsesor> lstTrans = new List<TransaccionAsesor>();
            foreach (var trans in transacciones)
            {
                lstTrans.Add(trans);
            }

            return lstTrans;
        }
        public List<TransaccionAsesor> RetrieveByIdAsesor(List<int> tipos)
        {
            // Assuming tipos contains the IDs and we need to pass a single int to RetrieveByIdAsesor
            var transacciones = transaccionAsesorCrud.RetrieveByIdAsesor<TransaccionAsesor>(tipos[0]); // Pass one int from the list

            List<TransaccionAsesor> lstTrans = new List<TransaccionAsesor>();
            foreach (var trans in transacciones)
            {
                lstTrans.Add(trans);
            }

            return lstTrans;
        }



        public TransaccionAsesor RetrieveByPaypal(int idPaypal)
        {
            return transaccionAsesorCrud.RetrieveByPaypal<TransaccionAsesor>(idPaypal);
        }

        public List<TransaccionAsesor> RetrieveByMyClientes(int idCliente)
        {
            return transaccionAsesorCrud.RetrieveByMyClientes<TransaccionAsesor>(idCliente);
        }

    }
}










