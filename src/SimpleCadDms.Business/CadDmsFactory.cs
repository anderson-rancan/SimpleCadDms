using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.Business
{
    public static class CadDmsFactory
    {
        public static ICadDms CreateNew()
        {
            return new CadDms();
        }
    }
}
