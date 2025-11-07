using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblUser
{
    public int UserNo { get; set; }

    public string? UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? NickName { get; set; }

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int? UserType { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblFriendMappingTable> TblFriendMappingTableFriendUserNoNavigations { get; set; } = new List<TblFriendMappingTable>();

    public virtual ICollection<TblFriendMappingTable> TblFriendMappingTableUserNoNavigations { get; set; } = new List<TblFriendMappingTable>();

    public virtual ICollection<TblGeneralConfiguration> TblGeneralConfigurations { get; set; } = new List<TblGeneralConfiguration>();

    public virtual ICollection<TblHobbyUserMappingTable> TblHobbyUserMappingTables { get; set; } = new List<TblHobbyUserMappingTable>();

    public virtual ICollection<TblUserRoleMappingTable> TblUserRoleMappingTableRoleAddedByUserNoNavigations { get; set; } = new List<TblUserRoleMappingTable>();

    public virtual ICollection<TblUserRoleMappingTable> TblUserRoleMappingTableUserNoNavigations { get; set; } = new List<TblUserRoleMappingTable>();
}
