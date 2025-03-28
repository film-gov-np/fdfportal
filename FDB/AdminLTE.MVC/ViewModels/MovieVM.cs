using System;
using System.ComponentModel.DataAnnotations;

namespace AdminLTE.MVC.ViewModels
{
   
    public class MovieVM
    {
        public int Id { get; set; }
        public int? MovieID { get; set; }
        public string MovieCode { get; set; }
        public string Name { get; set; }
        public string GradeCode { get; set; }
        public string ProductionHouseCode { get; set; }
        public string LanguageCode { get; set; }
        public int? ProductionType { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public int? StatusValue { get; set; }
        public string Slug { get; set; }
        public bool? IsModified { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? CanSpecifyMovieName { get; set; }
        public bool? SendMovieCodesToExhibitorEmail { get; set; }
        public bool? SendNotificationToProducers { get; set; }
        public bool? SendNotificationToDistributors { get; set; }
        public DateTime? AddedOn { get; set; }
        public string AddedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
       

    }
}
