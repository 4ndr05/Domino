
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

    public class Matches_Players : IResource
    {
        #region Constructores de la clase

        public Matches_Players(string serviceVersion, string proxyAgent)
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

                return !string.IsNullOrWhiteSpace(urlSegments.Item5 != null && urlSegments.Item5.Count > 0 ? urlSegments.Item5[0][1] : "") ? Delete_Instance(urlSegments.Item4, urlSegments.Item5[0][1]) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches_Players", context, ex);
                return ResponseMsgs.InternalServerErrorMessage;
            }
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

                return !string.IsNullOrWhiteSpace(urlSegments.Item5 != null && urlSegments.Item5.Count > 0 ? urlSegments.Item5[0][1] : "") ? Get_Instance(urlSegments.Item4, urlSegments.Item5[0][1]) : Resource.NotImplementedGET(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches_Players", context, ex);
                return ResponseMsgs.InternalServerErrorGETMessage;
            }
        }

        public ResponseObject POST(HttpContext context)
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

                return string.IsNullOrWhiteSpace(urlSegments.Item5 != null && urlSegments.Item5.Count > 0 ? urlSegments.Item5[0][1] : "") ? Post_Resource(urlSegments.Item4, request.Item3) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches_Players", context, ex);
                return ResponseMsgs.InternalServerErrorMessage;
            }
        }

        public ResponseObject PUT(HttpContext context)
        {
            return Resource.NotImplemented(context);
        }

        #endregion

        #region Métodos del recurso

        public ResponseObject Delete_Instance(string instance_matches, string instance_players)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["players"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_players))
                objInstances["players"] = Validator.StatusCode.MissingField;
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
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
                return delete_players_instance(instance_matches, instance_players);
        }
        public ResponseGET Get_Instance(string instance_matches, string instance_players)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["players"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_players))
                objInstances["players"] = Validator.StatusCode.MissingField;
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
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
                return get_players_instance(instance_matches, instance_players);
        }
        public ResponseObject Post_Resource(string instance_matches, string bodyData)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
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
                return post_players_resource(instance_matches, bodyData);
        }

        private ResponseObject post_players_resource(string instance_matches, string bodyData)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_post_players_resource();
                return Response.BuildMessage(HttpStatusCode.Created, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseGET get_players_instance(string instance_matches, string instance_players)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_get_players_instance();
                return Response.BuildGETMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildGETMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseObject delete_players_instance(string instance_matches, string instance_players)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_delete_players_instance();
                return Response.BuildMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }


        #endregion

        #region Métodos de acceso a datos

        private string dal_post_players_resource()
        {
            return @"{
   ""playerid"":""PSDSFFDFNWJSDNFLDFK"",
   ""creation_date"":""2015-11-30"",
   ""alias"":""El loco"",
   ""avatar"":""willy_chirino"",
   ""tiles"":{
      ""points"":0,
      ""items"":[
      ]
   }
}";
        }
        private string dal_get_players_instance()
        {
            return @"{
   ""playerid"":""PSDSFFDFNWJSDNFLDFK"",
   ""creation_date"":""2015-11-30"",
   ""alias"":""El loco"",
   ""avatar"": ""willy_chirino"",
   ""tiles"":{
      ""points"":31,
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
        private string dal_delete_players_instance()
        {
            return @"{
   ""domino"":{
      ""player"":{
         ""deleted"":true
      }
   }
}";
        }


        #endregion
    }
}