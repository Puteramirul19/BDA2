using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BDA.Entities;
using Microsoft.AspNetCore.Http;

namespace BDA.ViewModel
{
    public class UMAViewModel
    {
        public string Id { get; set; }
        public string BankDraftId { get; set; }
        public string Status { get; set; }
        public string RefNo { get; set; }
        public string BDNo { get; set; }
        public string BDRequesterName { get; set; }
        public string ProjectNo { get; set; }
        public string ERMSDocNo { get; set; }
        public string CoCode { get; set; }
        public string BA { get; set; }
        public string NameOnBD { get; set; }
        public decimal? BDAmount { get; set; }
        public string Comment { get; set; }

        //Draft & Submitted Section
        public string RequesterId { get; set; }
        public string ReasonUMA { get; set; }
        public string OthersRemark { get; set; }
        public DateTime? DraftedOn { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public DateTime? WithdrewOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string UserAction { get; set; }

        //TGBSProcess Section (Submit to Bank)
        public string InstructionLetterRefNo { get; set; }
        public string TGBSProcesserId { get; set; }
        public string BankProcessType { get; set; }
        public DateTime? TGBSProcessedOn { get; set; }

        //Submit to ANM Section (renamed from Receive)
        public DateTime? SubmittedToANMDate { get; set; }
        public string TGBSANMSubmitterId { get; set; }
        public DateTime? SubmittedToANMOn { get; set; }

        //Incoming Payment Section (renamed from Confirm)
        public string IncomingPaymentComment { get; set; }
        public DateTime? PaymentReceivedOn { get; set; }
        public string TGBSPaymentReceiverId { get; set; }

        //Completion Section
        public DateTime? CompletedOn { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime? ReceivedDate { get; set; }

        // File attachments
        public string ScannedBankDraftName { set; get; }
        public string ScannedMemoName { set; get; }
        public string BankStatementName { set; get; }
        public IFormFile ScannedBankDraft { set; get; }
        public IFormFile ScannedMemo { set; get; }
        public IFormFile BankStatement { set; get; }
        public IFormFile SignedLetter { set; get; }
        public IFormFile ScannedLetter { set; get; }

        public AttachmentViewModel ScannedBankDraftVM { get; set; }
        public AttachmentViewModel ScannedMemoVM { get; set; }
        public AttachmentViewModel BankStatementVM { get; set; }
        public AttachmentViewModel SignedLetterVM { get; set; }
        public AttachmentViewModel ScannedLetterVM { get; set; }
        public InstructionLetterViewModel InstructionLetterViewModel { get; set; }
    }
}