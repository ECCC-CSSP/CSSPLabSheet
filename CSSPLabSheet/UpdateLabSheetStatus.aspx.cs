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
    public partial class UpdateLabSheetStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(UpdateLabSheetStatusForOtherServerLabSheetID());
        }

        private string UpdateLabSheetStatusForOtherServerLabSheetID()
        {
            int OtherServerLabSheetID = -1;
            int TempInt = -10;
            LabSheetStatusEnum LabSheetStatus = LabSheetStatusEnum.Error;

            int.TryParse(Request.Params["OtherServerLabSheetID"], out OtherServerLabSheetID);
            if (OtherServerLabSheetID == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "OtherServerLabSheetID");
            }

            int.TryParse(Request.Params["LabSheetStatus"], out TempInt);
            if (TempInt == -10)
            {
                return string.Format(LabSheetViewRes._IsRequired, "LabSheetStatus");
            }

            LabSheetStatus = (LabSheetStatusEnum)TempInt;

            using (CSSPEntities db = new CSSPEntities())
            {
                LabSheet labSheet = (from c in db.LabSheets
                                     where c.OtherServerLabSheetID == OtherServerLabSheetID
                                     select c).FirstOrDefault();

                if (labSheet == null)
                {
                    return "NO";
                }
                else
                {
                    labSheet.LabSheetStatus = (int)LabSheetStatus;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message + " ---- " + (ex.InnerException == null ? "" : ex.InnerException.Message);
                }
            }

            return "";
        }
    }
}