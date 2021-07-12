using Directory.Models;
using EasyDox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Directory.Controllers
{
    public class HomeController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            return Redirect("/Home/Form/1");
        }

        public ActionResult Form(int? id = 1)
        {
            List<QuestionOptions> model = new List<QuestionOptions> { };
            List<Question> questions = db.Questions.Where(x => x.LawsuitId == id).OrderBy(x => x.OrderNumber).ToList();
            foreach (Question item in questions)
            {
                model.Add(new QuestionOptions(item, db.Options.Where(x => x.QuestionId == item.Id).ToList()));
            }
            var lawsuit = db.Lawsuits.Where(x => x.Id == id).ToArray();
            ViewBag.name = lawsuit[0].Name;
            ViewBag.LawsuitsId = id;

            return View(model);
        }

        public ActionResult SendFile()
        {
            var currentUrl = Request.RawUrl;
            ViewBag.url = currentUrl;
            var tempList = currentUrl.Split('?');

            var argsUrl = tempList[1].Split('&');
            var dictionaryArgUrl = new Dictionary<string, string>();
            var dictionaryAdditions = new Dictionary<string, string>();

            var additions = db.AdditionBlocks;
            foreach(var elem in additions)
            {
                dictionaryAdditions[elem.DocxKey] = "";
            }
            foreach (var elem in argsUrl)
            {
                var tempSignArg = elem.Split('=');
                dictionaryArgUrl[tempSignArg[0]] = tempSignArg[1];
                int idOpt = int.Parse(tempSignArg[1]);

                var block = db.AdditionBlocks.Where(x => x.IdOption == idOpt).ToArray();
                foreach (var addition in block)
                    dictionaryAdditions[addition.DocxKey] = addition.Information;
    
            }
          
            var engine = new Engine();
            string pathFile = Server.MapPath(Url.Content("~/Content/Templates/child1.docx"));
            string pathNewFile = Server.MapPath(Url.Content("~/Content/Templates/child2.docx"));
            engine.Merge(pathFile, dictionaryAdditions, pathNewFile);
            string file_path = Server.MapPath("~/Content/Templates/child2.docx");
            string file_type = "application/docx";
            string file_name = "name.docx";
            return File(file_path, file_type, file_name); 
          //  ViewBag.DictionaryAdditions = dictionaryAdditions;
           //ViewBag.DictionaryArgUrl = dictionaryArgUrl;



          //return View();
        }

        [HttpGet]
        public ActionResult AdminAuth()
        {
            if(Session["admin"] != null && Session["admin"] == "Consult_check2020")
            {
                return RedirectToAction("Admin");
            }
            else
            {
                ViewBag.Error = false;
                return View();
            }
        }
        [HttpPost]
        public ActionResult AdminAuth(string Password)
        {
            if(Password == "Consult_2020")
            {
                Session["admin"] = "Consult_check2020";
                return RedirectToAction("Admin");
            }
            else
            {
                ViewBag.Error = true;
                return View();

            }
        }
        public ActionResult AdminOut()
        {
            Session["admin"] = "";
            return RedirectToAction("AdminAuth");
        }
        public ActionResult Admin(int? id = 1)
        {
          /*  if(Session["admin"] != null && Session["admin"] == "Consult_check2020")
            {
                ViewBag.Admin = true;*/
                
                List<QuestionOptions> model = new List<QuestionOptions> { };
                List<Question> questions = db.Questions.Where(x => x.LawsuitId == id).ToList();
                foreach (Question item in questions)
                {
                    model.Add(new QuestionOptions(item, db.Options.Where(x => x.QuestionId == item.Id).ToList()));
                }
                return View(model);
           /* }
            else
            {
                ViewBag.Admin = false;
                return RedirectToAction("AdminAuth");
            }*/
        }

        [HttpGet]
        public ActionResult EditQuestion(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Question question = db.Questions.Find(id);
            if (question != null)
            {
                return View(question);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditQuestion(Question question)
        {
            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult EditOption(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Option option = db.Options.Find(id);
            List<AdditionBlocks> additionBlocks = db.AdditionBlocks.Where(x => x.IdOption == id).ToList();
            OptionAdditions optionAditions = new OptionAdditions(option, additionBlocks);
            if (optionAditions != null)
            {
                return View(optionAditions);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditOption(Option option, List<AdditionBlocks> additions)
        {
            db.Entry(option).State = EntityState.Modified;
            if(additions != null)
            {
                foreach (AdditionBlocks item in additions)
                {
                    db.Entry(item).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult CreateQuestion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateQuestion(Question question)
        {
            question.LawsuitId = 1;
            question.OrderNumber = db.Questions.Max(x => x.OrderNumber) + 1;
            db.Questions.Add(question);
            db.SaveChanges();

            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult CreateOption(int? QuestionId)
        {
            ViewData["QuestionId"] = QuestionId;
            return View();
        }
        [HttpPost]
        public ActionResult CreateOption(Option option)
        {
            db.Options.Add(option);
            db.SaveChanges();

            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult CreateAddition(int? IdOption)
        {
            ViewData["IdOption"] = IdOption;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAddition(AdditionBlocks addition)
        {
            db.AdditionBlocks.Add(addition);
            db.SaveChanges();
            return RedirectToAction("EditOption", new { id = addition.IdOption });
        }

        [HttpGet]
        public ActionResult EditAddition(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            AdditionBlocks addition = db.AdditionBlocks.Find(id);
            if (addition != null)
            {
                return View(addition);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditAddition(AdditionBlocks addition)
        {
            db.Entry(addition).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("EditOption", new { id = addition.IdOption });
        }

        [HttpGet]
        public ActionResult DeleteQuestion(int id)
        {
            Question q = db.Questions.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteQuestion")]
        public ActionResult DeleteQuestionConfirmed(int id)
        {
            Question q = db.Questions.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            db.Questions.Remove(q);
            List<Option> options = db.Options.Where(x => x.QuestionId == id).ToList();
            foreach (Option opt in options)
            {
                foreach(AdditionBlocks addition in db.AdditionBlocks.Where(x => x.IdOption == opt.Id))
                {
                    db.AdditionBlocks.Remove(addition);
                }
                db.Options.Remove(opt);
                
            }
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult DeleteOption(int id)
        {
            Option o = db.Options.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }

        [HttpPost, ActionName("DeleteOption")]
        public ActionResult DeleteOptionConfirmed(int id)
        {
            Option o = db.Options.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            db.Options.Remove(o);
            foreach (AdditionBlocks addition in db.AdditionBlocks.Where(x => x.IdOption == id).ToList())
            {
                db.AdditionBlocks.Remove(addition);
            }
            db.SaveChanges();
            return RedirectToAction("Admin");
        }


        [HttpGet]
        public ActionResult DeleteAddition(int id)
        {
            AdditionBlocks a = db.AdditionBlocks.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }
            return View(a);
        }

        [HttpPost, ActionName("DeleteAddition")]
        public ActionResult DeleteAdditionConfirmed(int id)
        {
            AdditionBlocks a = db.AdditionBlocks.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }
            db.AdditionBlocks.Remove(a);
            db.SaveChanges();
            return RedirectToAction("EditOption", new { id = a.IdOption});
        }


    }
}