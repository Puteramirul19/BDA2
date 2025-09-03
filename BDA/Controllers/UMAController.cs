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

        // Action methods for workflow transitions will be added here
    }
}