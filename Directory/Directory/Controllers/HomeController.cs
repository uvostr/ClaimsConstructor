using Directory.Models;
using System;
using System.Collections.Generic;
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
            return View(db.Lawsuits);
        }
        public ActionResult Form(int? id)
        {
            List<QuestionOptions> model = new List<QuestionOptions> { };
            List<Question> questions = db.Questions.Where(x => x.LawsuitId == id).ToList();
            foreach (Question item in questions)
            {
                model.Add(new QuestionOptions(item, db.Options.Where(x => x.QuestionId == item.Id).ToList()));
            }
            
            return View(model);
        }

        public ActionResult ProcessForm()
        {
            return View("~/Views/Home/ProcessForm.cshtml");
        }

    }
}