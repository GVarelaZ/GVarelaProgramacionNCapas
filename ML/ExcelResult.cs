using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class ExcelResult
    {
        public string ErrorMessage { get; set; }
        public int NumeroRegistro { get; set; }
        public List<object> Errores { get; set; }
    }
}
