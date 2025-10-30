using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblHobbyUserMappingTable
{
    public int HobbyUserMappingTableNo { get; set; }

    public int UserNo { get; set; }

    public int HobbyNo { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual TblHobby HobbyNoNavigation { get; set; } = null!;

    public virtual TblUser UserNoNavigation { get; set; } = null!;
}
