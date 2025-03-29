
using _00_DTO;
using DataAccess.CRUDs;
using TransaccionAsesor = DTOs.TransaccionAsesorDTO;




namespace CoreApp
{
    public class TransaccionAsesorManager : BaseManager
    {
        private readonly TransaccionAsesorCrudFactory crud;

        public TransaccionAsesorManager()
        {
            crud = new TransaccionAsesorCrudFactory();
        }

        public void Create(TransaccionAsesor trans)
        {
            if (trans.Monto <= 0)
                throw new Exception("El monto debe ser mayor a 0.");

            if (string.IsNullOrEmpty(trans.Tipo) || string.IsNullOrEmpty(trans.Estado))
                throw new Exception("Tipo y estado no pueden ser nulos.");

            trans.Created = DateTime.Now;
            crud.Create(trans);
        }

        public void Update(TransaccionAsesor trans) => crud.Update(trans);
        public void Delete(TransaccionAsesor trans) => crud.Delete(trans);
        public List<TransaccionAsesor> RetrieveAll() => crud.RetrieveAll<TransaccionAsesor>();
        public TransaccionAsesor RetrieveById(int id) => crud.RetrieveById<TransaccionAsesor>(id);
    }
}











