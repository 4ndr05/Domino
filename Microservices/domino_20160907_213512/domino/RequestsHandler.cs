
using System.Collections.Generic;
using System.Web;
using ServiceCore;

namespace Domino
{
    public class RequestsHandler : IHttpHandler
    {
        #region Miembros de IHttpHandler

        bool IHttpHandler.IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var resourceClass = GetResourceClass(context);

            if (resourceClass != null)
                // existe una clase para dicho recurso y versión
                switch (context.Request.HttpMethod)
                {
                    case "POST":
                        Resource.ProcessResponse(context, resourceClass.POST(context));
                        break;

                    case "PUT":
                        Resource.ProcessResponse(context, resourceClass.PUT(context));
                        break;

                    case "GET":
                        Resource.ProcessResponse(context, resourceClass.GET(context));
                        break;

                    case "DELETE":
                        Resource.ProcessResponse(context, resourceClass.DELETE(context));
                        break;
                }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Devuelve la clase de servicio a utilizar según el recurso y versión del servicio extraídos de la url del servicio
        /// </summary>
        /// <param name="absoluteUri">url del servicio</param>
        /// <returns></returns>
        public static IResource GetResourceClass(HttpContext context)
        {
            // Item1 = service
            // Item2 = version
            // Item3 = mainResource
            // Item4 = mainInstance ("" sí no existe el segmento)
            // Item5 = colección de pares resource-instance 
            var urlSegments = Resource.GetUrlSegments(context);
            IResource resourceClass = null;

            if (!string.IsNullOrWhiteSpace(urlSegments.Item1))
            {
                // mainResource + secResource1 + ... + secResource1N + <version>
                switch (Resource.UrlSegmentsToString(urlSegments, false).ToLower())
                {
                    
                    case "matches":
                        resourceClass = new Domino.Resources.Matches(urlSegments.Item2, context.Request.Headers["x-occm-agent"]);
                        break;

                    case "matches-players":
                        resourceClass = new Domino.Resources.Matches_Players(urlSegments.Item2, context.Request.Headers["x-occm-agent"]);
                        break;

                    case "matches-players-tiles":
                        resourceClass = new Domino.Resources.Matches_Players_Tiles(urlSegments.Item2, context.Request.Headers["x-occm-agent"]);
                        break;

                }
            }

            return resourceClass;
        }

        #endregion
    }
}