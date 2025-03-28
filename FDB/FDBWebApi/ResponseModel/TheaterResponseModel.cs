using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDBWebApi.ResponseModel
{
    public class TheaterResponseModel
    {
        public int TheaterId { get; set; }

        public string TheaterCode { get; set; } = null!;

        public string BrandCode { get; set; } = null!;

        public string RegNumber { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int StateId { get; set; }

        public int CityId { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public int StatusValue { get; set; }

        public bool HasExistingSystem { get; set; }

        public string ApiUsername { get; set; } = null!;

        public string ApiPassword { get; set; } = null!;

        public string? Pannumber { get; set; }

        public string Vatnumber { get; set; } = null!;

        public bool IsTest { get; set; }

        public bool? IsModified { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime AddedOn { get; set; }

        public string AddedBy { get; set; } = null!;

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public string? EncryptionKey { get; set; }
    }
}