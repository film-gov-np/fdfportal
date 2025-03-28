using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.ViewModels
{
    public class ChangeStatusModel
    {
        public List<int> Ids { get; set; }
        public bool Approve { get; set; }
    }
}