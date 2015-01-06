using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OutreachModule
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
           "ContinueExamination",
           "Camp/{campId}/Patient/{patientId}/Examination/{examId}/{action}",
           new { controller = "Examination"},
           new { campId = @"\d+", patientId = @"\d+", examId = @"\d+" }
           );
            routes.MapRoute(
            "Examination",
            "Camp/{campId}/Patient/{patientId}/Examination/{examId}",
            new { controller = "Examination", action = "Detail" },
            new { campId = @"\d+", patientId = @"\d+", examId = @"\d+"}
            );
            routes.MapRoute(
           "NewExamination",
           "Camp/{campId}/Patient/{patientId}/Examination/{action}",
           new { controller = "Examination", action = "Index" },
           new { campId = @"\d+", patientId = @"\d+"}
           );
            routes.MapRoute(
            "EditCampPatient",
            "Camp/{campId}/Patient/{patientId}/Edit",
            new { controller = "Camp", action = "EditPatient" },
            new { campId = @"\d+", patientId = @"\d+" }
            );
            routes.MapRoute(
            "CampPatient",
            "Camp/{campId}/Patient/{patientId}",
            new { controller = "Camp", action = "Patient" },
            new { campId = @"\d+", patientId = @"\d+" }
            );
            routes.MapRoute(
            "Camp",
            "Camp/{campId}/{action}",
            new { controller = "Camp" },
            new { campId = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
