using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class MainController
    {
        public void Insert()
        {
            Functions.Insert.Start();
        }
        public void Delete()
        {
            Functions.Delete.Start();     
        }
        public void Retrieve()
        {
            Functions.Retrieve.Start();
        }
    }
}
