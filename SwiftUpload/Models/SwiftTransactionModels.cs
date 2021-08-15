using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;

namespace SwiftUpload.Models
{
    #region Models

    public class SwiftTransactionUploadModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Account number cannot be longer than 200 characters.")]
        [Display(Name = "Account Number")]
        public string AccNumber { get; set; }

        [DataType(DataType.Currency)]
        [DefaultValue(0.0f)]
        //[Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Bank address cannot be longer than 100 characters.")]
        [Display(Name = "Bank Address")]
        public string BankAddress { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Beneficiary drawn branch cannot be longer than 100 characters.")]
        [Display(Name = "Beneficiary Drawn Branch")]
        public string BenefDrawnBranch { get; set; }

        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Beneficiary mobile cannot be longer than 20 characters.")]
        [Display(Name = "Beneficiary Mobile")]
        public string BenefMobile { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Beneficiary name cannot be longer than 200 characters.")]
        [Display(Name = "Beneficiary Name")]
        public string BenefName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "Beneficiary drawn bank code cannot be longer than 10 characters.")]
        [Display(Name = "Beneficiary Drawn Bank Code")]
        public string BenefDrawnBankCode { get; set; }

        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Currency cannot be longer than 20 characters.")]
        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "Company Code cannot be longer than 10 characters.")]
        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Origin Branch Code cannot be longer than 50 characters.")]
        [Display(Name = "Origin Branch Code")]
        public string OriginBranchCode { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Origin Branch Name cannot be longer than 100 characters.")]
        [Display(Name = "Origin Branch Name")]
        public string OriginBranchName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Purpose cannot be longer than 50 characters.")]
        [Display(Name = "Purpose")]
        public string Purpose { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Respond Branch Code cannot be longer than 50 characters.")]
        [Display(Name = "Respond Branch Code")]
        public string RespondBranchCode { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Respond Branch Name cannot be longer than 100 characters.")]
        [Display(Name = "Respond Branch Name")]
        public string RespondBranchName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Sender address cannot be longer than 100 characters.")]
        [Display(Name = "Sender address")]
        public string SenderAddress { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Sender name cannot be longer than 100 characters.")]
        [Display(Name = "Sender name")]
        public string SenderName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Sender RF TT No cannot be longer than 100 characters.")]
        [Display(Name = "Sender RF TT No")]
        public string SenderRFTTNo { get; set; }
    }

    #endregion

    #region Services

    public class SwiftParser
    {
        private static string DOC_ROOT = System.Web.HttpContext.Current.Server.MapPath("~") + @"db\data.txt";
        private static string DOC_ROOT_UPLOADED = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads/") + @"swift_demo.txt";

        private static string num_data = "Operator:(?<operator_name>.*?)\n+Number\\sof\\sentries:\\s+(?<num_tt>[0-9]+).*?\n+User selection:\\s+Selected Items";
        private static string senderRef = ":20:(pon/)?(?<sender_ref>.*?)\n+(.*?):[0-9]{2,2}[a-z]?:";
        private static string mobile_num = ":23e:(.*?)(?<mobile_num>[0-9]*)\n+(.*?):[0-9]{2,2}[a-z]?:";
        private static string dateAmount = ":32a:(.*?)(?<_date>[0-9]+)(?<currency>[a-z]+)(?<amt>.*?)\n+(.*?):[0-9]{2,2}[a-z]?:";
        private static string _sender = ":50k:(.*?)\n+(?<_sender>.*?)\n+((?<_sender_addr>.*?)\n+)?(.*?):[0-9]{2,2}[a-z]?:";

        private static string benBranch = ":57d:(.*?)(?<bank>[a-zA-Z].*?)\n+(.*?)(?<benBranch>[a-zA-Z].*?):[0-9]{2,2}[a-z]?:";
        private static string benef = ":59:(.*?)/(?<ac>.*?)\n+(.*?)(?<benef_name>[a-zA-Z].*?)\n+(.*?):[0-9]{2,2}[a-z]?:";
        private static string purpose = ":70:(?<purpose>.*?)\n+(.*?):[0-9]{2,2}[a-z]?:";

        public void Parse()
        {

        }

        public static string GetDocRoot()
        {
            return DOC_ROOT_UPLOADED;
        }

        public static List<SwiftTransactionUploadModel> ParseCSV()
        {
            List<SwiftTransactionUploadModel> list = new List<SwiftTransactionUploadModel>();
            using (var sr = File.OpenText(DOC_ROOT_UPLOADED))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var fields = line.Split(',');
                    list.Add(new SwiftTransactionUploadModel { AccNumber = fields[0], BenefName = fields[1], Amount = Decimal.Parse(fields[2]) });
                }
            }
            return list;
        }
    }

    #endregion
}