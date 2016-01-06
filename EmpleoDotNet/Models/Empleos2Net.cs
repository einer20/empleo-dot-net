using EmpleoDotNet.Models.Core.Services.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace EmpleoDotNet.Models
{
    /// <summary>
    /// Provides an entry point to access to the all application services
    /// </summary>
    public sealed class Empleos2Net
    {
        private static ServiceType? _serviceType;

        /// <summary>
        /// Get all set the connection string of the database 
        /// </summary>
        public static SecureString ConnectionString { get; set; }

        /// <summary>
        /// Set the service implementation to use depending 
        /// </summary>
        /// <param name="serviceType"></param>
        public static void ConfigureServiceTypeToLoad(ServiceType serviceType)
        {
            _serviceType = serviceType;
        }

        /// <summary>
        /// Get all information about the jobs store in Empleos2Net
        /// </summary>
        /// <returns></returns>
        public static IJobsService GetJobsService()
        {

            switch (_serviceType.GetValueOrDefault(ServiceType.EF))
            {
                case ServiceType.EF:
                    return new EFJobService();
                case ServiceType.Oracle:
                    throw new NotImplementedException("There is not oracle service");
            }

            // This will never be executed but we have to return something :V
            return null;
        }

    }
}