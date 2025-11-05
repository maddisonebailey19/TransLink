using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblNewsCategoryNo
{
    public int NewsCategoryNo { get; set; }

    public string NewsCategoryName { get; set; } = null!;

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblTransLinkNews> TblTransLinkNews { get; set; } = new List<TblTransLinkNews>();
}
