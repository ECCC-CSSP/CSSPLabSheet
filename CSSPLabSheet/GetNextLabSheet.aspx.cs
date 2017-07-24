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
    public partial class GetNextLabSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(GetNextAvailableLabSheet());
        }

        private string GetNextAvailableLabSheet()
        {
            StringBuilder sb = new StringBuilder();

            using (CSSPEntities db = new CSSPEntities())
            {
                LabSheet labSheet = (from c in db.LabSheets
                                     where c.LabSheetStatus == (int)LabSheetStatusEnum.Created
                                     orderby c.FileLastModifiedDate_Local
                                     select c).FirstOrDefault();

                if (labSheet != null)
                {
                    sb.AppendLine("OtherServerLabSheetID|||||[" + labSheet.OtherServerLabSheetID + "]");
                    sb.AppendLine("SamplingPlanName|||||[" + labSheet.SamplingPlanName + "]");
                    sb.AppendLine("Year|||||[" + labSheet.Year + "]");
                    sb.AppendLine("Month|||||[" + labSheet.Month + "]");
                    sb.AppendLine("Day|||||[" + labSheet.Day + "]");
                    sb.AppendLine("RunNumber|||||[" + labSheet.RunNumber + "]");
                    sb.AppendLine("SubsectorTVItemID|||||[" + labSheet.SubsectorTVItemID + "]");
                    sb.AppendLine("SamplingPlanType|||||[" + labSheet.SamplingPlanType + "]");
                    sb.AppendLine("SampleType|||||[" + labSheet.SampleType + "]");
                    sb.AppendLine("LabSheetType|||||[" + labSheet.LabSheetType + "]");
                    sb.AppendLine("LabSheetStatus|||||[" + labSheet.LabSheetStatus + "]");
                    sb.AppendLine("FileName|||||[" + labSheet.FileName + "]");
                    sb.AppendLine("FileLastModifiedDate_Local|||||["
                        + labSheet.FileLastModifiedDate_Local.Year + ","
                        + labSheet.FileLastModifiedDate_Local.Month + ","
                        + labSheet.FileLastModifiedDate_Local.Day + ","
                        + labSheet.FileLastModifiedDate_Local.Hour + ","
                        + labSheet.FileLastModifiedDate_Local.Minute + ","
                        + labSheet.FileLastModifiedDate_Local.Second + ","
                        + "]");
                    sb.AppendLine("FileContent|||||[" + labSheet.FileContent + "]");

                }
            }

            return sb.ToString();
        }
    }
}