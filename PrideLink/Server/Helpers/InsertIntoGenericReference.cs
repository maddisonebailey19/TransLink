using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;

namespace PrideLink.Server.Helpers
{
    public class InsertIntoGenericReference
    {
        public bool AddUpdateGeneralConfiguration(int userNo, GeneralConfigurationValues generalConfigurationValues)
        {
            using (var context = new MasContext())
            {
                TblGeneralConfiguration tblGeneralConfiguration = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == generalConfigurationValues.TypeNo);
                if (tblGeneralConfiguration != null)
                {
                    tblGeneralConfiguration.Ref1 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref1, tblGeneralConfiguration.Ref1);
                    tblGeneralConfiguration.Ref2 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref2, tblGeneralConfiguration.Ref2);
                    tblGeneralConfiguration.Ref3 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref3, tblGeneralConfiguration.Ref3);
                    tblGeneralConfiguration.Int1 = IsLongValueNullOrEmpty(generalConfigurationValues.Int1, tblGeneralConfiguration.Int1);
                    tblGeneralConfiguration.Int2 = IsLongValueNullOrEmpty(generalConfigurationValues.Int2, tblGeneralConfiguration.Int2);
                    tblGeneralConfiguration.Int3 = IsLongValueNullOrEmpty(generalConfigurationValues.Int3, tblGeneralConfiguration.Int3);
                    tblGeneralConfiguration.Date1 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime1, tblGeneralConfiguration.Date1);
                    tblGeneralConfiguration.Date2 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime2, tblGeneralConfiguration.Date2);
                    tblGeneralConfiguration.Date3 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime3, tblGeneralConfiguration.Date3);
                    tblGeneralConfiguration.Active = IsBoolValueNullOrEmpty(generalConfigurationValues.Active, tblGeneralConfiguration.Active);
                }
                else
                {
                    TblGeneralConfiguration newTblGeneralConfiguration = new TblGeneralConfiguration
                    {
                        UserNo = userNo,
                        TypeNo = generalConfigurationValues.TypeNo,
                        Ref1 = generalConfigurationValues.Ref1,
                        Ref2 = generalConfigurationValues.Ref2,
                        Ref3 = generalConfigurationValues.Ref3,
                        Int1 = generalConfigurationValues.Int1,
                        Int2 = generalConfigurationValues.Int2,
                        Int3 = generalConfigurationValues.Int3,
                        Date1 = generalConfigurationValues.DateTime1,
                        Date2 = generalConfigurationValues.DateTime2,
                        Date3 = generalConfigurationValues.DateTime3,
                        Active = true
                    };
                    context.Add(newTblGeneralConfiguration);
                }
                context.SaveChanges();
                return true;
            }
        }
        private string? IsStringValueNullOrEmpty(string? value, string? origonalValue)
        {
            if (value == null || value == "")
            {
                return origonalValue;
            }
            else
            {
                return value;
            }
        }

        private long? IsLongValueNullOrEmpty(int? value, long? origonalValue)
        {
            if (value == null)
            {
                return origonalValue;
            }
            else
            {
                return value;
            }
        }

        private DateTime? IsDateTimeValueNullOrEmpty(DateTime? value, DateTime? origonalValue)
        {
            if (value == null)
            {
                return origonalValue;
            }
            else
            {
                return value;
            }
        }
        private bool? IsBoolValueNullOrEmpty(bool? value, bool? origonalValue)
        {
            if (value == null)
            {
                return origonalValue;
            }
            else
            {
                return value;
            }
        }
    }
}
