using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Services
{
      public  class ParameterModel
    {
        public ParameterModel()
        {
            Parameters = new List<TblAtashTabletParameters>();
        }
        public List<TblAtashTabletParameters> Parameters { get; set; }
    }
}
