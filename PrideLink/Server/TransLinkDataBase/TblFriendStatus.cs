using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblFriendStatus
{
    public int FriendStatusNo { get; set; }

    public string FriendStatusName { get; set; } = null!;

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblFriendMappingTable> TblFriendMappingTables { get; set; } = new List<TblFriendMappingTable>();
}
