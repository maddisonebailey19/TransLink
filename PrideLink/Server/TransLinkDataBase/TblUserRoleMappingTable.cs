using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblUserRoleMappingTable
{
    public int UserRoleMappingTableNo { get; set; }

    public int UserNo { get; set; }

    public int UserRoleTypeNo { get; set; }

    public int RoleAddedByUserNo { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual TblUser RoleAddedByUserNoNavigation { get; set; } = null!;

    public virtual TblUser UserNoNavigation { get; set; } = null!;

    public virtual TblUserRoleType UserRoleTypeNoNavigation { get; set; } = null!;
}
