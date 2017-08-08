using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSSPLabSheet.Models;
using CSSPLabSheet.Resources;
using CSSPEnumsDLL.Enums;

namespace CSSPLabSheet
{
    public partial class PostLabSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(FillLabSheet());
        }

        private string FillLabSheet()
        {
            int tempInt = -1;
            string SamplingPlanName = "";
            int Year = -1;
            int Month = -1;
            int Day = -1;
            int RunNumber = -1;
            int SubsectorTVItemID = -1;
            SamplingPlanTypeEnum SamplingPlanType = SamplingPlanTypeEnum.Error;
            SampleTypeEnum SampleType = SampleTypeEnum.Error;
            LabSheetTypeEnum LabSheetType = LabSheetTypeEnum.Error;
            string FileName = "";
            string FileLastModifiedDate_LocalText = "";
            DateTime FileLastModifiedDate_Local = new DateTime(2050, 1, 1);
            string FileContent = "";

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

            FileName = Request.Params["FileName"];
            if (string.IsNullOrWhiteSpace(FileName))
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileName");
            }

            if (FileName.EndsWith("_S.txt"))
            {
                return LabSheetViewRes.OnlyLabSheetEndingWithStxtIsAllowed;
            }

            FileLastModifiedDate_LocalText = Request.Params["FileLastModifiedDate_Local"];
            List<string> stringList = FileLastModifiedDate_LocalText.Split(",".ToCharArray(), StringSplitOptions.None).ToList();
            int FileYear = -1;
            int.TryParse(stringList[0], out FileYear);
            if (FileYear == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Year not valid");
            }

            int FileMonth = -1;
            int.TryParse(stringList[1], out FileMonth);
            if (FileMonth == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Month not valid");
            }

            int FileDay = -1;
            int.TryParse(stringList[2], out FileDay);
            if (FileDay == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Day not valid");
            }

            int FileHour = -1;
            int.TryParse(stringList[3], out FileHour);
            if (FileHour == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Hour not valid");
            }

            int FileMinute = -1;
            int.TryParse(stringList[4], out FileMinute);
            if (FileMinute == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Minute not valid");
            }

            int FileSecond = -1;
            int.TryParse(stringList[5], out FileSecond);
            if (FileSecond == -1)
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileLastModifiedDate_Local Second not valid");
            }

            FileLastModifiedDate_Local = new DateTime(FileYear, FileMonth, FileDay, FileHour, FileMinute, FileSecond);

            FileContent = Request.Params["FileContent"];
            if (string.IsNullOrWhiteSpace(FileContent))
            {
                return string.Format(LabSheetViewRes._IsRequired, "FileContent");
            }

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
                                     && c.FileName == FileName
                                     select c).FirstOrDefault();

                if (labSheet == null)
                {
                    labSheet = new LabSheet()
                    {
                        SamplingPlanName = SamplingPlanName,
                        Year = Year,
                        Month = Month,
                        Day = Day,
                        RunNumber = RunNumber,
                        SubsectorTVItemID = SubsectorTVItemID,
                        SamplingPlanType = (int)SamplingPlanType,
                        SampleType = (int)SampleType,
                        LabSheetType = (int)LabSheetType,
                        FileName = FileName,
                    };

                    db.LabSheets.Add(labSheet);
                }

                labSheet.LabSheetStatus = (int)LabSheetStatusEnum.Created;
                labSheet.FileLastModifiedDate_Local = FileLastModifiedDate_Local;
                labSheet.FileContent = FileContent;
                labSheet.LastUpdateDate_UTC = DateTime.UtcNow;
                labSheet.LastUpdateContactTVItemID = 2;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "");
                }
            }

            return "";
        }
    }
}