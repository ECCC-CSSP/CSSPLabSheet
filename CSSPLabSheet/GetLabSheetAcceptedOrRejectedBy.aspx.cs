using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSSPLabSheet.Models;
using CSSPLabSheet.Resources;
using System.Text;
using CSSPEnumsDLL.Enums;

namespace CSSPLabSheet
{
    public partial class GetLabSheetAcceptedOrRejectedBy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(GetterLabSheetAcceptedOrRejectedBy());
        }

        private string GetterLabSheetAcceptedOrRejectedBy()
        {
            int tempInt = -1;
            string SamplingPlanName = "";
            int Year = -1;
            int Month = -1;
            int Day = -1;
            int RunNumber = -1;
            int SubsectorTVItemID = -1;
            DateTime SampleDate_Local = new DateTime(2050, 1, 1);
            SamplingPlanTypeEnum SamplingPlanType = SamplingPlanTypeEnum.Error;
            SampleTypeEnum SampleType = SampleTypeEnum.Error;
            LabSheetTypeEnum LabSheetType = LabSheetTypeEnum.Error;
            DateTime FileLastModifiedDate_Local = new DateTime(2050, 1, 1);
            StringBuilder sb = new StringBuilder();

            SamplingPlanName = Request.Params["SamplingPlanName"];
            if (string.IsNullOrWhiteSpace(SamplingPlanName))
            {
                return string.Format(LabSheetViewRes._IsRequired, "SamplingPlanName");
            }

            int.TryParse(Request.Params["Year"], out Year);
            if (Year == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "Year");
            }

            int.TryParse(Request.Params["Month"], out Month);
            if (Month == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "Month");
            }

            int.TryParse(Request.Params["Day"], out Day);
            if (Day == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "Day");
            }

            int.TryParse(Request.Params["RunNumber"], out RunNumber);
            if (RunNumber == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "RunNumber");
            }

            int.TryParse(Request.Params["SubsectorTVItemID"], out SubsectorTVItemID);
            if (SubsectorTVItemID == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "SubsectorTVItemID");
            }

            int.TryParse(Request.Params["SamplingPlanType"], out tempInt);
            if (tempInt == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "SamplingPlanType");
            }
            SamplingPlanType = (SamplingPlanTypeEnum)tempInt;

            int.TryParse(Request.Params["SampleType"], out tempInt);
            if (tempInt == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "SampleType");
            }
            SampleType = (SampleTypeEnum)tempInt;

            int.TryParse(Request.Params["LabSheetType"], out tempInt);
            if (tempInt == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "LabSheetType");
            }
            LabSheetType = (LabSheetTypeEnum)tempInt;

            using (CSSPEntities db = new CSSPEntities())
            {

                LabSheet labSheet = (from c in db.LabSheets
                                     where c.SamplingPlanName == SamplingPlanName
                                     && c.Year == Year
                                     && c.Month == Month
                                     && c.Day == Day
                                     && c.RunNumber == RunNumber
                                     && c.SubsectorTVItemID == SubsectorTVItemID
                                     && c.SamplingPlanType == (int)SamplingPlanType
                                     && c.SampleType == (int)SampleType
                                     && c.LabSheetType == (int)LabSheetType
                                     select c).FirstOrDefault();

                if (labSheet != null)
                {
                    sb.AppendLine("OtherServerLabSheetID|||||[" + labSheet.OtherServerLabSheetID + "]");
                    sb.AppendLine("AcceptedOrRejectedBy|||||[" + labSheet.AcceptedOrRejectedBy + "]");
                    sb.AppendLine("Year|||||[" + labSheet.AcceptedOrRejectedDate_Local.Value.Year + "]");
                    sb.AppendLine("Month|||||[" + labSheet.AcceptedOrRejectedDate_Local.Value.Month + "]");
                    sb.AppendLine("Day|||||[" + labSheet.AcceptedOrRejectedDate_Local.Value.Day + "]");
                    sb.AppendLine("Hour|||||[" + labSheet.AcceptedOrRejectedDate_Local.Value.Hour + "]");
                    sb.AppendLine("Minute|||||[" + labSheet.AcceptedOrRejectedDate_Local.Value.Minute + "]");
                    sb.AppendLine("RejectReason|||||[" + labSheet.RejectReason + "]");
                }
            }
            return sb.ToString();
        }
    }
}