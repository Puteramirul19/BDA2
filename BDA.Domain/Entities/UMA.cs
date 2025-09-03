using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BDA.Data;
using BDA.Identity;
using Newtonsoft.Json;

namespace BDA.Entities
{
    public class UMA : AuditableEntity
    {
        public Guid BankDraftId { get; set; }
        public virtual BankDraft BankDraft { get; set; }
        public string Status { get; set; }
        public string RefNo { get; set; }
        public string BDNo { get; set; }
        public string BDRequesterName { get; set; }
        public string ProjectNo { get; set; }
        public string ERMSDocNo { get; set; }
        public string CoCode { get; set; }
        public string BA { get; set; }
        public string NameOnBD { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BDAmount { get; set; }

        //Draft & Submitted Section
        public string RequesterId { get; set; }
        public string ReasonUMA { get; set; }
        public string OthersRemark { get; set; }
        public virtual ApplicationUser Requester { get; set; }
        public DateTime? DraftedOn { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public DateTime? WithdrewOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }

        //TGBSProcess Section (Submit to Bank)
        public string InstructionLetterRefNo { get; set; }
        public string TGBSProcesserId { get; set; }
        public string BankProcessType { get; set; }
        public virtual ApplicationUser TGBSProcesser { get; set; }
        public DateTime? TGBSProcessedOn { get; set; }

        //Submit to ANM Section (previously Receive)
        public DateTime? SubmittedToANMDate { get; set; }
        public string TGBSANMSubmitterId { get; set; }
        public virtual ApplicationUser ANMSubmitter { get; set; }
        public DateTime? SubmittedToANMOn { get; set; }

        //Incoming Payment Section (previously Confirm)
        public string IncomingPaymentComment { get; set; }
        public DateTime? PaymentReceivedOn { get; set; }
        public string TGBSPaymentReceiverId { get; set; }

        //Completion Section
        public DateTime? CompletedOn { get; set; }
        public string TGBSValidatorId { get; set; }

        public IDictionary<string, string> GetMessageValues()
        {
            return new Dictionary<string, string>
            {
                { "ApplicationId", (Id.ToString() == null ? "" : Id.ToString()) },
                { "RefNo", (RefNo == null ? "" : RefNo) },
                { "BDNo", (BDNo == null ? "" : BDNo) },
                { "ProjectNo", (ProjectNo == null ? "" : ProjectNo) },
                { "Amount", (BDAmount == null ? "" : string.Format("{0:C}", BDAmount)) },
                { "TGBSComment", (IncomingPaymentComment == null ? "" : IncomingPaymentComment) },
                { "SubmitDate", (SubmittedOn == null ? "" : SubmittedOn.Value.ToString("dd-MM-yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture)) },
            };
        }
    }
}