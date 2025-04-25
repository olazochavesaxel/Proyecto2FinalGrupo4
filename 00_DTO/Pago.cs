using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace _00_DTO
{
    public class Pago : BaseDTO
    {

        public string PaypalOrderId { get; set; }
        public string PaymentCaptureId { get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime UpdateTime { get; set; }


    }
}
