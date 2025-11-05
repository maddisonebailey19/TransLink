using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblCity
{
    public int CityNo { get; set; }

    public string CityName { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblTransLinkNews> TblTransLinkNews { get; set; } = new List<TblTransLinkNews>();
}
