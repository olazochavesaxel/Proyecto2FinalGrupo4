using DTO;
using _00_DTO;


namespace DTOs
{
    public class TransaccionAsesor : BaseDTO
    {
        public int Id { get; set; }
        public double Monto { get; set; }

        public string Tipo { get; set; }
        public string Estado { get; set; }

        // Solo Ids
        public int IdCliente { get; set; }
        public int IdAsesor { get; set; }
    }
}

