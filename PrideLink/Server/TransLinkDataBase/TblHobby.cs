using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblHobby
{
    public int HobbyNo { get; set; }

    public string HobbyName { get; set; } = null!;

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblHobbyUserMappingTable> TblHobbyUserMappingTables { get; set; } = new List<TblHobbyUserMappingTable>();
}
