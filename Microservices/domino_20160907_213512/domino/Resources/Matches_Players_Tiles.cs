
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using ServiceCore;

namespace Domino.Resources
{
    // Colección de errores del response
    using Errors = List<Error>;
    // statusCode, response-string, redirectTo/Filename, response-bytes
    // Item3 = si es response-string => redirectTo
    // Item3 = si es response-bytes => Filename
    using ResponseGET = Tuple<HttpStatusCode, string, string, byte[]>;
    // Mensajes de respuestas del servicio
    using ResponseMsgs = Domino.Common.ResponseMessages;
    // statusCode, response-string, redirectTo
    using ResponseObject = Tuple<HttpStatusCode, string, string>;
    // Objeto de validación de errores de lógica de negocio
    using ValidationErrors = Tuple<HttpStatusCode, List<Error>>;

    public class Matches_Players_Tiles : IResource
    {
        #region Constructores de la clase

        public Matches_Players_Tiles(string serviceVersion, string proxyAgent)
        {
            this.ServiceVersion = serviceVersion;
            this.ProxyAgent = proxyAgent;
        }

        #endregion

        #region Propiedades de la clase

        /// <summary>
        /// Versión del servicio
        /// </summary>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Agente que realiza el request
        /// </summary>
        public string ProxyAgent { get; set; }

        #endregion

        #region Implementación de métodos abstractos

        public ResponseObject DELETE(HttpContext context)
        {
            return Resource.NotImplemented(context);
        }

        public ResponseGET GET(HttpContext context)
        {

            try
            {
                // Item1=AbsoluteUri 
                // Item2=Parameters
                // Item3=RequestBody
                var request = Resource.GetRequestChunks(context);

                // Item1 = service
                // Item2 = version
                // Item3 = mainResource
                // Item4 = mainInstance ("" sí no existe el segmento)
                // Item5 = colección de pares resource-instance 
                var urlSegments = Resource.GetUrlSegments(context);

                return string.IsNullOrWhiteSpace(urlSegments.Item5 != null && urlSegments.Item5.Count > 1 ? urlSegments.Item5[1][1] : "") ? Get_Resource(urlSegments.Item4, urlSegments.Item5[0][1]) : Resource.NotImplementedGET(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches_Players_Tiles", context, ex);
                return ResponseMsgs.InternalServerErrorGETMessage;
            }
        }

        public ResponseObject POST(HttpContext context)
        {
            return Resource.NotImplemented(context);
        }

        public ResponseObject PUT(HttpContext context)
        {

            try
            {
                // Item1=AbsoluteUri 
                // Item2=Parameters
                // Item3=RequestBody
                var request = Resource.GetRequestChunks(context);

                // Item1 = service
                // Item2 = version
                // Item3 = mainResource
                // Item4 = mainInstance ("" sí no existe el segmento)
                // Item5 = colección de pares resource-instance 
                var urlSegments = Resource.GetUrlSegments(context);

                return !string.IsNullOrWhiteSpace(urlSegments.Item5 != null && urlSegments.Item5.Count > 1 ? urlSegments.Item5[1][1] : "") ? Put_Instance(urlSegments.Item4, urlSegments.Item5[0][1], urlSegments.Item5[1][1], request.Item3) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches_Players_Tiles", context, ex);
                return ResponseMsgs.InternalServerErrorMessage;
            }
        }

        #endregion

        #region Métodos del recurso

        public ResponseObject Put_Instance(string instance_matches, string instance_players, string instance_tiles, string bodyData)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["tiles"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_tiles))
                objInstances["tiles"] = Validator.StatusCode.MissingField;
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
            objInstances["players"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_players))
                objInstances["players"] = Validator.StatusCode.MissingField;
            #endregion

            #region validación de campos del updater
            var json_fields = new List<string> { { "side" } };
            var json_updater = ServiceBase.DeserializeUpdater(bodyData, json_fields, true);
            if (json_updater.Count == 0)
                objUpdater["*"] = Validator.StatusCode.InvalidFormat;
            else
            {
                foreach (var field in json_fields)
                    objUpdater[field] = Validator.StatusCode.Success;
                if (!string.IsNullOrWhiteSpace(json_updater["side"]))
                {
                    var enum_value = new List<string> { { "a" }, { "b" } };
                    if (!enum_value.Contains(json_updater["side"].Trim().ToLower()))
                        objUpdater["side"] = Validator.StatusCode.InvalidType;
                }

            }
            #endregion

            var valInstances = Validator.GroupFieldsByStatus(objInstances);
            var valParameters = Validator.GroupFieldsByStatus(objParameters);
            var valUpdater = Validator.GroupFieldsByStatus(objUpdater);
            if (valInstances.Any(error => error["statuscode"].ToString() != "1") || valParameters.Any(error => error["statuscode"].ToString() != "1") || valUpdater.Any(error => error["statuscode"].ToString() != "1"))
            {
                var validation = new Dictionary<string, object> { { "instances", valInstances }, { "parameters", valParameters }, { "updater", valUpdater } };
                return Response.BuildMessage(HttpStatusCode.BadRequest, ResponseMsgs.BuildError(Response.CodeTypes.BadParameters, validation));
            }
            else
                return put_tiles_instance(instance_matches, instance_players, instance_tiles, bodyData);
        }
        public ResponseGET Get_Resource(string instance_matches, string instance_players)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
            objInstances["players"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_players))
                objInstances["players"] = Validator.StatusCode.MissingField;
            #endregion

            var valInstances = Validator.GroupFieldsByStatus(objInstances);
            var valParameters = Validator.GroupFieldsByStatus(objParameters);
            var valUpdater = Validator.GroupFieldsByStatus(objUpdater);
            if (valInstances.Any(error => error["statuscode"].ToString() != "1") || valParameters.Any(error => error["statuscode"].ToString() != "1") || valUpdater.Any(error => error["statuscode"].ToString() != "1"))
            {
                var validation = new Dictionary<string, object> { { "instances", valInstances }, { "parameters", valParameters }, { "updater", valUpdater } };
                return Response.BuildGETMessage(HttpStatusCode.BadRequest, ResponseMsgs.BuildError(Response.CodeTypes.BadParameters, validation));
            }
            else
                return get_tiles_resource(instance_matches, instance_players);
        }

        private ResponseGET get_tiles_resource(string instance_matches, string instance_players)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_get_tiles_resource();
                return Response.BuildGETMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildGETMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseObject put_tiles_instance(string instance_matches, string instance_players, string instance_tiles, string bodyData)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_put_tiles_instance();
                return Response.BuildMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }


        #endregion

        #region Métodos de acceso a datos

        private string dal_get_tiles_resource()
        {
            return @"{
   ""tiles"":{
      ""points"":""31"",
      ""items"":[
         {
            ""tileid"":""6-6"",
            ""side_a"":""6"",
            ""side_b"":""6""
         },
         {
            ""tileid"":""4-6"",
            ""side_a"":""6"",
            ""side_b"":""4""
         },
         {
            ""tileid"":""4-5"",
            ""side_a"":""4"",
            ""side_b"":""5""
         }
      ]
   }
}";
        }
        private string dal_put_tiles_instance()
        {
            return @"{
   ""side"":""b""
}";
        }


        #endregion
    }
}