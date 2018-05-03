using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitSencei_bot
{
    class CalculationData
    {
        public string datas(string cloth, string size, string numofloops, string res)
        {
             cloth = "жилет";
             size = "45";
             numofloops = "17";

            if (cloth == "жилет" & size == "45" & numofloops == "17")
            {  res = "1583 метров пряжи";
                return res;
            }
            else return cloth;
        }
    }
}
