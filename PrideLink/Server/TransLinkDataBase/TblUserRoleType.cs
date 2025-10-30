using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblUserRoleType
{
    public int UserRoleTypeNo { get; set; }

    public string UserRoleName { get; set; } = null!;

    public string UserRoleDescription { get; set; } = null!;

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblUserRoleMappingTable> TblUserRoleMappingTables { get; set; } = new List<TblUserRoleMappingTable>();
}
