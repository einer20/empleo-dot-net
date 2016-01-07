using System;
using System.Linq;
using System.Web.Mvc;
using EmpleoDotNet.Helpers;
using EmpleoDotNet.Models;
using EmpleoDotNet.Models.Dto;
using EmpleoDotNet.ViewModel;
using EmpleoDotNet.Models.Repositories;
using PagedList;

namespace EmpleoDotNet.Controllers
{
    public class JobOpportunityController : EmpleoDotNetController
    {
        private readonly JobOpportunityRepository _jobRepository;
        private readonly LocationRepository _locationRepository;

        public JobOpportunityController()
        {
            _jobRepository = new JobOpportunityRepository(_database);
            _locationRepository = new LocationRepository(_database);
        }
        
        // GET: /JobOpportunity/
        [HttpGet]
        public ActionResult Index(int selectedLocation = 0, int page = 1, int pageSize = 15)
        {
            var locations = _locationRepository.GetAllLocations();

            locations.Insert(0, new Location { Id = 0, Name = "Todas" });

            var viewModel = new JobOpportunitySearchViewModel
            {
                Locations = locations.ToSelectList(l => l.Id, l => l.Name, selectedLocation),
                SelectedLocation = selectedLocation
            };

            var jobOpportunities =
                _jobRepository.GetAllJobOpportunitiesByLocationPaged(new JobOpportunityPagingParameter
                {
                    SelectedLocation = selectedLocation,
                    PageSize = pageSize,
                    Page = page
                });

            viewModel.Result = jobOpportunities;
            ViewBag.SelectedLocation = selectedLocation;

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            // Con esto evitaria hacer instancias en el constructor
            // de repositorios que quizas no necesitamos en el action
            using (var jobService = Empleos2Net.GetJobsService())
            {
                var job = jobService.GetJobById(id.Value);

                if (job == null)
                {
                    ViewBag.ErrorMessage = "La vacante solicitada no existe. Por favor escoger una vacante válida del listado";

                    return View("Index");
                }

                var relatedJobs = jobService.GetAllJobs()
                     .Where(
                            x =>
                                x.Id != job.Id &&
                                (x.CompanyName == job.CompanyName && x.CompanyEmail == job.CompanyEmail &&
                                 x.CompanyUrl == job.CompanyUrl)).Select(jobOpportunity => new RelatedJobDto()
                                 {
                                     Title = jobOpportunity.Title,
                                     Url = "/JobOpportunity/Detail/" + jobOpportunity.Id
                                 }).ToList();

                ViewBag.RelatedJobs = relatedJobs;

                return View("Detail", job);
            }

        }

        // GET: /JobOpportunity/New
        public ActionResult New()
        {
            var viewModel = new NewJobOpportunityViewModel();

            LoadLocations(viewModel);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewJobOpportunityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadLocations(model);
                ViewBag.ErrorMessage = "Han ocurrido errores de validación que no permiten continuar el proceso";
                return View(model);
            }

            _jobRepository.Add(model.ToEntity());

            _uow.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LastestsJob()
        {
            var latestJobOpportunities = _jobRepository.GetLatestJobOpporunity(10);

            return PartialView("_LastestJobs", latestJobOpportunities);
        }

        /// <summary>
        /// Performs a quick search of all jobs available
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult QuickJobSearch(string hint)
        {

            if (String.IsNullOrEmpty(hint))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            hint = hint.ToUpper();
            using (var jobsService = Models.Empleos2Net.GetJobsService())
            {
                var jobs = jobsService.GetAllJobs()
                    .Take(10)
                            .Where(X => X.Title.ToUpper().Contains(hint) || X.Description.ToUpper().Contains(hint))
                                .ToList();

                var result = jobs.Select(x => new
                {
                    id = x.Id,
                    name = x.Title,
                    companyUrl = x.CompanyUrl,
                    description = x.Description,
                    logo = x.CompanyLogoUrl,
                    categoryInt = (int)x.Category,
                    categoryString = x.Category.ToString()
                }).GroupBy(x => x.categoryString);


                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private void LoadLocations(NewJobOpportunityViewModel viewModel)
        {
            var locations = _locationRepository.GetAllLocations();

            viewModel.Locations = locations.ToSelectList(x => x.Id, x => x.Name);
        }
    }
}
