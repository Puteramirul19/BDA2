using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BDA.Entities;
using BDA.Identity;
using BDA.ViewModel;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace BDA.Web.Controllers
{
    [Authorize]
    public class UMAController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UMAController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult BankDraftList()
        {
            return View();
        }

        [Authorize(Roles = "Executive, Manager, Head of Zone, Senior Manager")]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.Fullname = user.FullName;
            return View();
        }

        [Authorize(Roles = "Executive, Manager, Head of Zone, Senior Manager")]
        [HttpPost]
        public JsonResult Create(UMAViewModel model, IFormFile ScannedBankDraft, IFormFile ScannedMemo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.GetUserAsync(HttpContext.User).Result;

                    UMA entity = new UMA();
                    entity.CreatedById = user.Id;
                    entity.CreatedByName = user.FullName;
                    entity.RequesterId = user.Id;
                    entity.DraftedOn = DateTime.Now;
                    entity.BankDraftId = Guid.Parse(model.BankDraftId);
                    entity.SubmittedOn = model.Status == "Submit" ? DateTime.Now : (DateTime?)null;
                    entity.BDNo = model.BDNo;
                    entity.BDRequesterName = model.BDRequesterName;
                    entity.BDAmount = model.BDAmount;
                    entity.ReasonUMA = model.ReasonUMA;
                    entity.OthersRemark = model.OthersRemark;
                    entity.RefNo = model.RefNo;
                    entity.Status = model.Status == "Submit" ? Data.Status.Submitted.ToString() : Data.Status.Draft.ToString();
                    entity.BDRequesterName = model.BDRequesterName;
                    entity.ERMSDocNo = model.ERMSDocNo;
                    entity.CoCode = model.CoCode;
                    entity.BA = model.BA;
                    entity.NameOnBD = model.NameOnBD;
                    entity.ProjectNo = model.ProjectNo;

                    Db.UMA.Add(entity);
                    Db.SaveChanges();

                    var bd = Db.BankDraft.Where(x => x.Id == entity.BankDraftId).FirstOrDefault();
                    bd.FinalApplication = "UMA";

                    Db.SetModified(bd);
                    Db.SaveChanges();

                    if (entity.Status == "Submitted")
                    {
                        Db.BankDraftAction.Add(new BankDraftAction
                        {
                            ApplicationType = Data.AppType.UMA.ToString(),
                            ActionType = Data.ActionType.Submitted.ToString(),
                            On = DateTime.Now,
                            ById = user.Id,
                            ParentId = entity.Id,
                            ActionRole = Data.ActionRole.Requester.ToString(),
                            Comment = model.Comment,
                        });
                        Db.SaveChanges();

                        // Notification can be added here
                        // Job.Enqueue<Services.NotificationService>(x => x.NotifyUMAForProcessing(entity.Id));
                    }

                    if (ScannedBankDraft != null)
                    {
                        UploadFile(ScannedBankDraft, entity.Id, Data.AttachmentType.UMA.ToString(), Data.BDAttachmentType.ScannedBankDraft.ToString());
                    }

                    if (ScannedMemo != null)
                    {
                        UploadFile(ScannedMemo, entity.Id, Data.AttachmentType.UMA.ToString(), Data.BDAttachmentType.ScannedMemo.ToString());
                    }

                    return Json(new { response = StatusCode(StatusCodes.Status200OK), message = "UMA Request Saved Successfully!" });
                }
                catch (Exception e)
                {
                    return Json(new { response = StatusCode(StatusCodes.Status500InternalServerError), message = e.Message });
                }
            }
            else
            {
                return Json(new { response = StatusCode(StatusCodes.Status500InternalServerError), message = "Error! Please try again later." });
            }
        }

        // Submit to Bank Stage
        [Authorize(Roles = "TGBS Banking, Business Admin")]
        public IActionResult Process()
        {
            return View();
        }

        // Submit to ANM Stage  
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Receive()
        {
            return View();
        }

        // Incoming Payment Stage
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Confirm()
        {
            return View();
        }

        // Completed Stage
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Complete()
        {
            return View();
        }

        public void UploadFile(IFormFile file, Guid parentId, string fileType, string fileSubType = null, string title = null)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var ext = Path.GetExtension(uniqueFileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));

            //Save uniqueFileName to db table 
            Attachment attachement = new Attachment();
            attachement.FileType = fileType;
            attachement.FileSubType = fileSubType;
            attachement.ParentId = parentId;
            attachement.FileName = uniqueFileName;
            attachement.FileExtension = ext;
            attachement.Title = title;
            attachement.CreatedOn = DateTime.Now;
            attachement.UpdatedOn = DateTime.Now;
            attachement.IsActive = true;

            Db.Attachment.Add(attachement);
            Db.SaveChanges();
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        public IActionResult _ProcessDetails()
        {
            return View();
        }

        public IActionResult _ReceiveDetails()
        {
            return View();
        }

        public IActionResult _ConfirmDetails()
        {
            return View();
        }

        public IActionResult _ActionButton()
        {
            return View();
        }

        public IActionResult _ActionHistory()
        {
            return View();
        }

        public IActionResult _StatusBar()
        {
            return View();
        }

        public IActionResult _Document()
        {
            return View();
        }

        public IActionResult _Comments()
        {
            return View();
        }

        public IActionResult _CreateDetails()
        {
            return View();
        }

        // Action methods for workflow transitions will be added here
        // Action method for workflow transitions
        [HttpPost]
        public JsonResult ChangeStatus(UMAViewModel model)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                var entity = Db.UMA.Find(Guid.Parse(model.Id));

                if (entity == null)
                    return Json(new { response = StatusCode(StatusCodes.Status404NotFound), message = "UMA Request not found." });

                string actionType = "";
                string actionRole = "";
                string byId = "";

                if (model.Status == Data.Status.Processed.ToString()) // Submit to Bank
                {
                    entity.Status = model.Status;
                    entity.TGBSProcessedOn = DateTime.Now;
                    entity.TGBSProcesserId = user.Id;
                    entity.InstructionLetterRefNo = model.InstructionLetterRefNo;
                    entity.BankProcessType = model.BankProcessType;

                    actionType = Data.ActionType.Processed.ToString();
                    byId = user.Id;
                    actionRole = Data.ActionRole.TGBSBanking.ToString();
                }
                else if (model.Status == "SubmittedToANM") // Submit to ANM
                {
                    entity.Status = model.Status;
                    entity.SubmittedToANMOn = DateTime.Now;
                    entity.TGBSANMSubmitterId = user.Id;
                    entity.SubmittedToANMDate = model.SubmittedToANMDate;

                    actionType = "SubmittedToANM";
                    byId = user.Id;
                    actionRole = Data.ActionRole.TGBSReconciliation.ToString();
                }
                else if (model.Status == Data.Status.Received.ToString()) // Incoming Payment
                {
                    entity.Status = model.Status;
                    entity.PaymentReceivedOn = DateTime.Now;
                    entity.TGBSPaymentReceiverId = user.Id;
                    entity.IncomingPaymentComment = model.IncomingPaymentComment;

                    actionType = Data.ActionType.Received.ToString();
                    byId = user.Id;
                    actionRole = Data.ActionRole.TGBSReconciliation.ToString();
                }
                else if (model.Status == Data.Status.Complete.ToString()) // Complete
                {
                    entity.Status = model.Status;
                    entity.CompletedOn = DateTime.Now;
                    entity.TGBSValidatorId = user.Id;

                    actionType = Data.ActionType.Complete.ToString();
                    byId = user.Id;
                    actionRole = Data.ActionRole.TGBSReconciliation.ToString();
                }

                entity.UpdatedOn = DateTime.Now;
                Db.SetModified(entity);
                Db.SaveChanges();

                // Add action history
                Db.BankDraftAction.Add(new BankDraftAction
                {
                    ApplicationType = Data.AppType.UMA.ToString(),
                    ActionType = actionType,
                    On = DateTime.Now,
                    ById = byId,
                    ParentId = entity.Id,
                    ActionRole = actionRole,
                    Comment = model.Comment
                });
                Db.SaveChanges();

                return Json(new { response = StatusCode(StatusCodes.Status200OK), message = "UMA Request updated successfully!" });
            }
            catch (Exception e)
            {
                return Json(new { response = StatusCode(StatusCodes.Status500InternalServerError), message = e.Message });
            }
        }
        // API methods for dropdowns and data
        public JsonResult GetAllBankProcessType()
        {
            var result = new List<dynamic>
    {
        new { id = "1", name = "BD Cancellation - Maybank" },
        new { id = "2", name = "BD Cancellation - UMA" }
    };

            return Json(result);
        }

        public JsonResult GetUMAById(Guid id)
        {
            try
            {
                var uma = Db.UMA.Where(x => x.Id == id)
                    .Select(x => new {
                        Id = x.Id,
                        RefNo = x.RefNo,
                        BDNo = x.BDNo,
                        ProjectNo = x.ProjectNo,
                        BDRequesterName = x.BDRequesterName,
                        ERMSDocNo = x.ERMSDocNo,
                        CoCode = x.CoCode,
                        BA = x.BA,
                        NameOnBD = x.NameOnBD,
                        BDAmount = x.BDAmount,
                        Status = x.Status,
                        ReasonUMA = x.ReasonUMA,
                        OthersRemark = x.OthersRemark,
                        InstructionLetterRefNo = x.InstructionLetterRefNo,
                        BankProcessType = x.BankProcessType,
                        SubmittedToANMDate = x.SubmittedToANMDate,
                        IncomingPaymentComment = x.IncomingPaymentComment,
                        PaymentReceivedOn = x.PaymentReceivedOn
                    }).FirstOrDefault();

                return Json(uma);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var uma = Db.UMA.Find(id);
            if (uma == null)
            {
                return NotFound();
            }

            // Map entity to view model
            var model = new UMAViewModel
            {
                Id = uma.Id.ToString(),
                BankDraftId = uma.BankDraftId.ToString(),
                RefNo = uma.RefNo,
                BDNo = uma.BDNo,
                ProjectNo = uma.ProjectNo,
                BDRequesterName = uma.BDRequesterName,
                ERMSDocNo = uma.ERMSDocNo,
                CoCode = uma.CoCode,
                BA = uma.BA,
                NameOnBD = uma.NameOnBD,
                BDAmount = uma.BDAmount,
                Status = uma.Status,
                ReasonUMA = uma.ReasonUMA,
                OthersRemark = uma.OthersRemark,
                InstructionLetterRefNo = uma.InstructionLetterRefNo,
                BankProcessType = uma.BankProcessType,
                SubmittedToANMDate = uma.SubmittedToANMDate,
                IncomingPaymentComment = uma.IncomingPaymentComment,
                PaymentReceivedOn = uma.PaymentReceivedOn
            };

            // Load attachments
            var scannedBD = Db.Attachment.Where(x => x.ParentId == uma.Id &&
                x.FileType == Data.AttachmentType.UMA.ToString() &&
                x.FileSubType == Data.BDAttachmentType.ScannedBankDraft.ToString()).FirstOrDefault();
            if (scannedBD != null)
            {
                model.ScannedBankDraftVM = new AttachmentViewModel
                {
                    Id = scannedBD.Id.ToString(),
                    FileName = scannedBD.FileName
                };
            }

            // Route to appropriate view based on status
            switch (uma.Status)
            {
                case "Submitted":
                    return View("Process", model);
                case "Processed":
                    return View("Receive", model);
                case "SubmittedToANM":
                    return View("Confirm", model);
                case "Received":
                case "Complete":
                    return View("Complete", model);
                default:
                    return View("Create", model);
            }
        }
    }

}