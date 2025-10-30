using System;
using System.Collections.Generic;

namespace PrideLink.Server.TransLinkDataBase;

public partial class TblGeneralConfigurationType
{
    public int GeneralConfigurationTypeNo { get; set; }

    public string? GeneralConfigurationTypeName { get; set; }

    public string? Ref1 { get; set; }

    public string? Ref2 { get; set; }

    public string? Ref3 { get; set; }

    public string? Int1 { get; set; }

    public string? Int2 { get; set; }

    public string? Int3 { get; set; }

    public string? Date1 { get; set; }

    public string? Date2 { get; set; }

    public string? Date3 { get; set; }

    public bool? Active { get; set; }

    public string? TSystemLastModifiedUserName { get; set; }

    public DateTime? TSystemCreateDate { get; set; }

    public DateTime? TSystemModifiedDate { get; set; }

    public long? TSystemModifiedCount { get; set; }

    public long? TSystemCreateProcedureId { get; set; }

    public virtual ICollection<TblGeneralConfiguration> TblGeneralConfigurations { get; set; } = new List<TblGeneralConfiguration>();
}
