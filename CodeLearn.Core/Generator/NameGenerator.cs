using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Generator
{
    public class NameGenerator
    {
        public static string GeneratorUniqCode()                //saxt yek active code menhasel befard
        {
            return Guid.NewGuid().ToString().Replace("-","");      
        }
    }
}
