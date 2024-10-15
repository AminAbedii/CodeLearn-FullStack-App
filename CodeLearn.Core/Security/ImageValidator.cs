using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Security
{
    public static class ImageValidator    //extention ha bayad static bashand
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                var img = System.Drawing.Image.FromStream(file.OpenReadStream());
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
