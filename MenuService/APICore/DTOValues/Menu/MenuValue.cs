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

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public List<string> Dishes { get; set; }
    }

    public class MenuValueFull : MenuValue
    {
        [JsonConstructor]
        public MenuValueFull()
        {
        }

       public List<DishValue> DishesFull { get; set; }
    }
}
