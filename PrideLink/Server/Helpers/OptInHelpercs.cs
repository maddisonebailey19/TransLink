using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.UserOptIn;

namespace PrideLink.Server.Helpers
{
    public class OptInHelpercs : IUserOptInInterface
    {
        private readonly InsertIntoGenericReference _insertIntoGenericReference;
        public OptInHelpercs(InsertIntoGenericReference insertIntoGenericReference)
        {
            _insertIntoGenericReference = insertIntoGenericReference;
        }

        private int convertBoolToInt(bool value)
        {
            return value ? 1 : 0;
        }

        public bool UserOptIn(UserOptIn userOptIns, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 2;
            switch (userOptIns.optInType)
            {
                case 1:
                    generalConfigurationValues.Int1 = convertBoolToInt(userOptIns.isOptedIn);
                    break;
                case 2:
                    generalConfigurationValues.Int3 = convertBoolToInt(userOptIns.isOptedIn);
                    break;
                case 3:
                    generalConfigurationValues.Int2 = convertBoolToInt(userOptIns.isOptedIn);
                    break;
            }

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo, generalConfigurationValues);

            //using (var context = new MasContext())
            //{
            //    var entity = context.TblUserTandCacceptances.FirstOrDefault(e => e.UserNo == userNo && e.TandCtypesNo == userOptIns.optInType);
            //    if(entity == null)
            //    {
            //        TblUserTandCacceptance tblUserTandCacceptance = new TblUserTandCacceptance
            //        {
            //            UserNo = userNo,
            //            TandCtypesNo = userOptIns.optInType,
            //            Active = userOptIns.isOptedIn
            //        };
            //        context.Add(tblUserTandCacceptance);
            //        context.SaveChanges();
            //        return tblUserTandCacceptance.UserTandCacceptanceNo;
            //    }
            //    else
            //    {
            //        entity.Active = userOptIns.isOptedIn;
            //        context.SaveChanges();
            //        return entity.UserTandCacceptanceNo;
            //    }
            //}
        }

        //TODO move into its own helper
        //private bool AddUpdateGeneralConfiguration(int userNo, GeneralConfigurationValues generalConfigurationValues)
        //{
        //    using (var context = new MasContext())
        //    {
        //        TblGeneralConfiguration tblGeneralConfiguration = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == generalConfigurationValues.TypeNo);
        //        if (tblGeneralConfiguration != null)
        //        {
        //            tblGeneralConfiguration.Ref1 = IsValueNullOrEmpty(generalConfigurationValues.Ref1, tblGeneralConfiguration.Ref1);
        //            tblGeneralConfiguration.Ref2 = IsValueNullOrEmpty(generalConfigurationValues.Ref2, tblGeneralConfiguration.Ref2);
        //            tblGeneralConfiguration.Ref3 = IsValueNullOrEmpty(generalConfigurationValues.Ref3, tblGeneralConfiguration.Ref3);
        //            tblGeneralConfiguration.Int1 = int.Parse(IsValueNullOrEmpty(generalConfigurationValues.Int1.ToString(), tblGeneralConfiguration.Int1.ToString()));
        //            tblGeneralConfiguration.Int2 = int.Parse(IsValueNullOrEmpty(generalConfigurationValues.Int2.ToString(), tblGeneralConfiguration.Int2.ToString()));
        //            tblGeneralConfiguration.Int3 = int.Parse(IsValueNullOrEmpty(generalConfigurationValues.Int3.ToString(), tblGeneralConfiguration.Int3.ToString()));
        //            tblGeneralConfiguration.Date1 = DateTime.Parse(IsValueNullOrEmpty(generalConfigurationValues.DateTime1.ToString(), tblGeneralConfiguration.Date1.ToString()));
        //            tblGeneralConfiguration.Date2 = DateTime.Parse(IsValueNullOrEmpty(generalConfigurationValues.DateTime2.ToString(), tblGeneralConfiguration.Date2.ToString()));
        //            tblGeneralConfiguration.Date3 = DateTime.Parse(IsValueNullOrEmpty(generalConfigurationValues.DateTime3.ToString(), tblGeneralConfiguration.Date3.ToString()));
        //            tblGeneralConfiguration.Active = bool.Parse(IsValueNullOrEmpty(generalConfigurationValues.Active.ToString(), tblGeneralConfiguration.Active.ToString()));
        //        }
        //        else
        //        {
        //            TblGeneralConfiguration newTblGeneralConfiguration = new TblGeneralConfiguration
        //            {
        //                UserNo = userNo,
        //                TypeNo = generalConfigurationValues.TypeNo,
        //                Ref1 = generalConfigurationValues.Ref1,
        //                Ref2 = generalConfigurationValues.Ref2,
        //                Ref3 = generalConfigurationValues.Ref3,
        //                Int1 = generalConfigurationValues.Int1,
        //                Int2 = generalConfigurationValues.Int2,
        //                Int3 = generalConfigurationValues.Int3,
        //                Date1 = generalConfigurationValues.DateTime1,
        //                Date2 = generalConfigurationValues.DateTime2,
        //                Date3 = generalConfigurationValues.DateTime3,
        //                Active = true
        //            };
        //            context.Add(newTblGeneralConfiguration);
        //        }
        //        context.SaveChanges();
        //        return true;
        //    }
        //}

        //private string? IsValueNullOrEmpty(string value, string origonalValue)
        //{
        //    if (value == null)
        //    {
        //        return origonalValue;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}
    }
}
