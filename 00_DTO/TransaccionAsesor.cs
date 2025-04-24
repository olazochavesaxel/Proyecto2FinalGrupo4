using DTO;
using _00_DTO;


namespace DTOs
{

    public class TransaccionAsesor : BaseDTO

    {
        public double? Monto { get; set; }      
        public string Tipo { get; set; }         
        public string Estado { get; set; }       
        public int? IdAsesor { get; set; }     
        public int? IdCliente { get; set; }     
        public DateTime? Created { get; set; }   
        public int? Id_Paypal { get; set; }

        public bool Any()
        {
            throw new NotImplementedException();
        }
    }
}

