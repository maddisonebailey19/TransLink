using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblTransLinkNews
{
    public int TransLinkNewsNo { get; set; }

    public string Title { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public DateTime ExpireData { get; set; }

    public int NewsCategoryNo { get; set; }

    public int NewsLinkNo { get; set; }

    public string Content { get; set; } = null!;

    public int? CityNo { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual TblCity? CityNoNavigation { get; set; }

    public virtual TblNewsCategoryNo NewsCategoryNoNavigation { get; set; } = null!;

    public virtual TblNewsLinkNo NewsLinkNoNavigation { get; set; } = null!;
}
