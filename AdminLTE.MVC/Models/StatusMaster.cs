using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Models
{
    public class StatusMaster
    {
        public int Id { get; set; }

        public string StatusCodeCategory { get; set; } = null!;

        public int StatusCodeId { get; set; }

        public string StatusCodeName { get; set; } = null!;

        public string StatusCodeDescription { get; set; }

        public int? DisplayOrder { get; set; }
    }
}