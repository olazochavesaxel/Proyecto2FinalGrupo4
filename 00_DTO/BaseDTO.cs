using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BaseDTO
    {
        /*clase padre de todos los DTOs */
        public int Id { get; set; }
        public  DateTime? Created { get; set; }
    }
}
