using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblRelationshipStatusType
{
    public int RelationshipStatusTypeNo { get; set; }

    public string RelationshipStatusTypeName { get; set; } = null!;

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }
}
