using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpleoDotNet.Models
{
    /// <summary>
    /// Provides access to all jobs register in Empleos2Net app
    /// </summary>
    public interface IJobsService : IDisposable
    {
        /// <summary>
        /// Find a job by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JobOpportunity GetJobById(int id);

        /// <summary>
        /// Get all jobs
        /// </summary>
        /// <returns></returns>
        IEnumerable<JobOpportunity> GetAllJobs();

        /// <summary>
        /// Get all jobs by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<JobOpportunity> GetJobsByCategory(JobCategory category);
    }
}