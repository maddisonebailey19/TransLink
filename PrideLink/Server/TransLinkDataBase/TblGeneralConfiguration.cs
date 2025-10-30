using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblGeneralConfiguration
{
    public int GeneralConfigurationNo { get; set; }

    public int UserNo { get; set; }

    public int TypeNo { get; set; }

    public string? Ref1 { get; set; }

    public string? Ref2 { get; set; }

    public string? Ref3 { get; set; }

    public long? Int1 { get; set; }

    public long? Int2 { get; set; }

    public long? Int3 { get; set; }

    public DateTime? Date1 { get; set; }

    public DateTime? Date2 { get; set; }

    public DateTime? Date3 { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual TblGeneralConfigurationType TypeNoNavigation { get; set; } = null!;

    public virtual TblUser UserNoNavigation { get; set; } = null!;
}
