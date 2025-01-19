using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Enums;

namespace APICore.DTOValues.Menu
{
    public class DishValue : BaseDtoValue
    {
        public string Description { get; set; }

        public DishType DishType { get; set; }
    }
}
