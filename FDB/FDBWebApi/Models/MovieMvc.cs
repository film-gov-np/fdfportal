using System;
using System.Collections.Generic;

namespace FDBWebApi.Models;

public partial class MovieMvc
{
    public int Id { get; set; }

    public int? MovieId { get; set; }

    public string? MovieCode { get; set; }

    public string? Name { get; set; }

    public string? GradeCode { get; set; }

    public string? ProductionHouseCode { get; set; }

    public string? LanguageCode { get; set; }

    public int? ProductionType { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int? StatusValue { get; set; }

    public string? Slug { get; set; }

    public bool? IsModified { get; set; }

    public DateTime? IsDeleted { get; set; }

    public bool? CanSpecifyMovieName { get; set; }

    public bool? SendMovieCodesToExhibitorEmail { get; set; }

    public bool? SendNotificationToProducers { get; set; }

    public bool? SendNotificationToDistributors { get; set; }

    public DateTime? AddedOn { get; set; }

    public string? AddedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }
}
