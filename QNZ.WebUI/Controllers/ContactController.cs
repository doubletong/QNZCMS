using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Configs;
using QNZ.Infrastructure.Email;
using QNZ.Model.Front.InputModel;
using QNZ.Model.Front.ViewModel;
using QNZ.Model.ViewModel;

namespace QNZCMS.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly IEmailService _emailService;
        public ContactController(YicaiyunContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BranchesAsync()
        {
            var vm = await _context.Branches.Where(d => d.Active == true)
                .OrderByDescending(d => d.Importance).ThenBy(d => d.Id)
                .ProjectTo<BranchVM>(_mapper.ConfigurationProvider).ToListAsync();

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

 


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Branches              
                .FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();
 

            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.BRANCH);

            return View(article);
        }


        [HttpPost]
        public IActionResult SendEmail(ContactIM vm)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = GetModelErrorMessage();
                AR.Setfailure(errorMessage);
                return Json(AR);
            }


            //var template = _db.EmailTemplates.FirstOrDefault(d => d.TemplateNo == "T003");
            //if (template == null)
            //{
            //    //AR.Setfailure(string.Format(Messages.NoEmailTemplate, "T003"));
            //    //return Json(AR, JsonRequestBehavior.DenyGet);
            //    TempData["Error"] = "Not found email template！";
            //    return View(vm);
            //}



            var emailBody = $"<p>姓名：{vm.Name}<br/>公司名称：{vm.CompanyName}<br/>邮箱：{vm.Email}<br/>联系电话：{vm.Phone}<br/>内容：{vm.Message} </p>";


            //emailBody = emailBody.Replace("{Email}", vm.Email);
            //emailBody = emailBody.Replace("{Message}", vm.Body);
            //var emailAccount = _db.EmailAccounts.Find(template.EmailAccountId);

            try
            {
              

                _emailService.SendMail(vm.Name, vm.Email, SettingsManager.Site.MailTo, null,
                vm.Subject, emailBody, SettingsManager.SMTP.SmtpServer, SettingsManager.SMTP.From, SettingsManager.Site.SiteName,
                 SettingsManager.SMTP.UserName, SettingsManager.SMTP.Password, (int)SettingsManager.SMTP.Port, SettingsManager.SMTP.EnableSsl);


                //Email email = new Email
                //{
                //    Body = emailBody,
                //    Subject = vm.Subject,
                //    MailTo = vm.Email,
                //    MailCc = null,
                //    Active = true,
                //    CreatedBy = string.Empty,
                //    CreatedDate = DateTime.Now
                //};
                //_db.EmailSets.Add(email);
                //_db.SaveChanges();

                AR.SetSuccess("邮件已成功发送！");
                return Json(AR);
            }
            catch (Exception er)
            {
                AR.Setfailure(er.Message);
                return Json(AR);
            }

        }
    }
}