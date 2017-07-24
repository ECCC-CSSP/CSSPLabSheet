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
    public partial class UpdateLabSheetAcceptedOrRejectedBy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(UpdateLabSheetAcceptedOrRejectedByForOtherServerLabSheetID());
        }

        private string UpdateLabSheetAcceptedOrRejectedByForOtherServerLabSheetID()
        {
            int OtherServerLabSheetID = -1;
            string AcceptedOrRejectedBy = "";
            string RejectReason = "";

            int.TryParse(Request.Params["OtherServerLabSheetID"], out OtherServerLabSheetID);
            if (OtherServerLabSheetID == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "OtherServerLabSheetID");
            }

            if (string.IsNullOrWhiteSpace(Request.Params["AcceptedOrRejectedBy"]))
            {
                return string.Format(LabSheetViewRes._IsRequired, "AcceptedOrRejectedBy");
            }

            AcceptedOrRejectedBy = Request.Params["AcceptedOrRejectedBy"];

            if (string.IsNullOrWhiteSpace(Request.Params["RejectReason"]))
            {
                return string.Format(LabSheetViewRes._IsRequired, "RejectReason");
            }

            RejectReason = Request.Params["RejectReason"];

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
                    labSheet.AcceptedOrRejectedBy = AcceptedOrRejectedBy;
                    labSheet.AcceptedOrRejectedDate_Local = DateTime.Now;
                    labSheet.RejectReason = RejectReason;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message + " ---- " + (ex.InnerException == null ? "" : ex.InnerException.Message);
                    }

                }

            }

            return "";
        }
    }
}