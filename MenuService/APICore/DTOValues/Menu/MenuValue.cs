using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APICore.DTOValues.Menu
{
    public class MenuValue : BaseDtoValue
    {
        [JsonConstructor]
        public MenuValue()
        {
        }

        public List<string> Dishes { get; set; } = new List<string>();
    }

    public class MenuValueFull : MenuValue
    {
        [JsonConstructor]
        public MenuValueFull()
        {
        }

       public List<DishValue> DishesFull { get; set; } = new List<DishValue>();
    }
}
