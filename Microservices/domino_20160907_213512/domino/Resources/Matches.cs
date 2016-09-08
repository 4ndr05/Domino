
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

    public class Matches : IResource
    {
        #region Constructores de la clase

        public Matches(string serviceVersion, string proxyAgent)
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

                return !string.IsNullOrWhiteSpace(urlSegments.Item4) ? Delete_Instance(urlSegments.Item4) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches", context, ex);
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

                return string.IsNullOrWhiteSpace(urlSegments.Item4) ? Get_Resource() : Get_Instance(urlSegments.Item4);
            }
            catch (Exception ex)
            {
                Event.Write("Matches", context, ex);
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

                return string.IsNullOrWhiteSpace(urlSegments.Item4) ? Post_Resource(request.Item3) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches", context, ex);
                return ResponseMsgs.InternalServerErrorMessage;
            }
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

                return !string.IsNullOrWhiteSpace(urlSegments.Item4) ? Put_Instance(urlSegments.Item4, request.Item3) : Resource.NotImplemented(context);
            }
            catch (Exception ex)
            {
                Event.Write("Matches", context, ex);
                return ResponseMsgs.InternalServerErrorMessage;
            }
        }

        #endregion

        #region Métodos del recurso

        public ResponseObject Put_Instance(string instance_matches, string bodyData)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de instancias
            objInstances["matches"] = Validator.StatusCode.Success;
            if (string.IsNullOrWhiteSpace(instance_matches))
                objInstances["matches"] = Validator.StatusCode.MissingField;
            #endregion

            #region validación de campos del updater
            var json_fields = new List<string> { { "title" } };
            var json_updater = ServiceBase.DeserializeUpdater(bodyData, json_fields, true);
            if (json_updater.Count == 0)
                objUpdater["*"] = Validator.StatusCode.InvalidFormat;
            else
            {
                foreach (var field in json_fields)
                    objUpdater[field] = Validator.StatusCode.Success;
                if (string.IsNullOrWhiteSpace(json_updater["title"]))
                    objUpdater["title"] = Validator.StatusCode.MissingField;
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
                return put_matches_instance(instance_matches, bodyData);
        }
        public ResponseObject Delete_Instance(string instance_matches)
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
                return delete_matches_instance(instance_matches);
        }
        public ResponseObject Post_Resource(string bodyData)
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            #region validación de campos del updater
            var json_fields = new List<string> { { "title" } };
            var json_updater = ServiceBase.DeserializeUpdater(bodyData, json_fields, true);
            if (json_updater.Count == 0)
                objUpdater["*"] = Validator.StatusCode.InvalidFormat;
            else
            {
                foreach (var field in json_fields)
                    objUpdater[field] = Validator.StatusCode.Success;
                if (string.IsNullOrWhiteSpace(json_updater["title"]))
                    objUpdater["title"] = Validator.StatusCode.MissingField;
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
                return post_matches_resource(bodyData);
        }
        public ResponseGET Get_Instance(string instance_matches)
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
                return Response.BuildGETMessage(HttpStatusCode.BadRequest, ResponseMsgs.BuildError(Response.CodeTypes.BadParameters, validation));
            }
            else
                return get_matches_instance(instance_matches);
        }
        public ResponseGET Get_Resource()
        {
            Dictionary<string, Validator.StatusCode> objInstances = new Dictionary<string, Validator.StatusCode>(),
                objParameters = new Dictionary<string, Validator.StatusCode>(),
                objUpdater = new Dictionary<string, Validator.StatusCode>();

            var valInstances = Validator.GroupFieldsByStatus(objInstances);
            var valParameters = Validator.GroupFieldsByStatus(objParameters);
            var valUpdater = Validator.GroupFieldsByStatus(objUpdater);
            if (valInstances.Any(error => error["statuscode"].ToString() != "1") || valParameters.Any(error => error["statuscode"].ToString() != "1") || valUpdater.Any(error => error["statuscode"].ToString() != "1"))
            {
                var validation = new Dictionary<string, object> { { "instances", valInstances }, { "parameters", valParameters }, { "updater", valUpdater } };
                return Response.BuildGETMessage(HttpStatusCode.BadRequest, ResponseMsgs.BuildError(Response.CodeTypes.BadParameters, validation));
            }
            else
                return get_matches_resource();
        }

        private ResponseGET get_matches_resource()
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());

            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.NotFound, new Errors() { ResponseMsgs.BuildError("404-10") });
            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.InternalServerError, new Errors() { ResponseMsgs.BuildError("500-10") });

            if (validation.Item2.Count == 0)
            {
                var response = dal_get_matches_resource();
                return Response.BuildGETMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildGETMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseGET get_matches_instance(string instance_matches)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());

            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.NotFound, new Errors() { ResponseMsgs.BuildError("404-11") });
            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.InternalServerError, new Errors() { ResponseMsgs.BuildError("500-10") });

            if (validation.Item2.Count == 0)
            {
                var response = dal_get_matches_instance();
                return Response.BuildGETMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildGETMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseObject post_matches_resource(string bodyData)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());

            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.Conflict, new Errors() { ResponseMsgs.BuildError("409-11") });
            if (!condition)
                validation = new ValidationErrors(HttpStatusCode.InternalServerError, new Errors() { ResponseMsgs.BuildError("500-10") });

            if (validation.Item2.Count == 0)
            {
                var response = dal_post_matches_resource();
                return Response.BuildMessage(HttpStatusCode.Created, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseObject delete_matches_instance(string instance_matches)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_delete_matches_instance();
                return Response.BuildMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }

        private ResponseObject put_matches_instance(string instance_matches, string bodyData)
        {
            var condition = true;
            var validation = new ValidationErrors(HttpStatusCode.OK, new Errors());


            if (validation.Item2.Count == 0)
            {
                var response = dal_put_matches_instance();
                return Response.BuildMessage(HttpStatusCode.OK, response: response);
            }
            else
                return Response.BuildMessage(validation.Item1, errors: validation.Item2);
        }


        #endregion

        #region Métodos de acceso a datos

        private string dal_get_matches_resource()
        {
            return @"{
   ""matches"":{
      ""items"":[
         {
            ""matchid"":""MJSEFJBSFJABSFJKFBSKDV"",
            ""creation_date"":""2015-11-30"",
            ""title"":""Match de pruebas del domino""
         },
         {
            ""matchid"":""MSDFISDJFISDFIOJFIGJS"",
            ""creation_date"":""2015-11-30"",
            ""title"":""pruebas del domino""
         }
      ]
   }
}";
        }
        private string dal_get_matches_instance()
        {
            return @"{
   ""matchid"":""MJSEFJBSFJABSFJKFBSKDV"",
   ""creation_date"":""2015-11-30"",
   ""title"":""Match de pruebas del domino"",
   ""state"":{
      ""max_tiles"":28,
      ""status"": ""playing"",
      ""next_player"": ""PLIJSMFMGRNFOGNDKF"",
      ""game_stream"":""6-6,6-4,4-5"",
      ""side_a"":""6"",
      ""side_b"":""5"",      
      ""tiles"":{
         ""items"":[
            {
               ""tileid"":""6-6"",
               ""side_a"":""6"",
               ""side_b"":""6"",
               ""playerid"":""PSDSFFDFNWJSDNFLDFK""
            },
            {
               ""tileid"":""4-6"",
               ""side_a"":""6"",
               ""side_b"":""4"",
               ""playerid"":""PSDSFFDFNWJSDNFLDFK""
            },
            {
               ""tileid"":""4-5"",
               ""side_a"":""4"",
               ""side_b"":""5"",
               ""playerid"":""PKDONFGFGMPFKGDDGD""
            }            
         ]
      }
   },
   ""players"":{
      ""items"":[
         {
            ""playerid"":""PSDSFFDFNWJSDNFLDFK"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""El loco"",
            ""avatar"": ""willy_chirino""            
         },
         {
            ""playerid"":""PJDLSOWERNFLEKFND"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""Azuca !!!"",
            ""avatar"": ""celia_cruz""
         },
         {
            ""playerid"":""PKDONFGFGMPFKGDDGD"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""player_3"",
            ""avatar"": ""fidel_castro""            
         },
         {
            ""playerid"":""PLIJSMFMGRNFOGNDKF"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""kubano"",
            ""avatar"": ""orishas""
         }              
      ]
   }
}";
        }
        private string dal_post_matches_resource()
        {
            return @"{
   ""matchid"":""MJSEFJBSFJABSFJKFBSKDV"",
   ""creation_date"":""2015-11-30"",
   ""title"":""Match de pruebas del domino"",
   ""state"":{
      ""max_tiles"":28,
      ""status"": ""unbegun"",
      ""next_player"": """",
      ""game_stream"":"""",
      ""side_a"":"""",
      ""side_b"":"""",      
      ""tiles"":{
         ""items"":[           
         ]
      }
   },
   ""players"":{
      ""items"":[           
      ]
   }
}";
        }
        private string dal_delete_matches_instance()
        {
            return @"{
   ""domino"":{
      ""match"":{
         ""deleted"":true
      }
   }
}";
        }
        private string dal_put_matches_instance()
        {
            return @"{
   ""matchid"":""MJSEFJBSFJABSFJKFBSKDV"",
   ""creation_date"":""2015-11-30"",
   ""title"":""Match de pruebas del domino"",
   ""state"":{
      ""max_tiles"":28,
      ""status"": ""playing"",
      ""next_player"": ""PLIJSMFMGRNFOGNDKF"",
      ""game_stream"":""6-6,6-4,4-5"",
      ""side_a"":""6"",
      ""side_b"":""5"",      
      ""tiles"":{
         ""items"":[
            {
               ""tileid"":""6-6"",
               ""side_a"":""6"",
               ""side_b"":""6"",
               ""playerid"":""PSDSFFDFNWJSDNFLDFK""
            },
            {
               ""tileid"":""4-6"",
               ""side_a"":""6"",
               ""side_b"":""4"",
               ""playerid"":""PSDSFFDFNWJSDNFLDFK""
            },
            {
               ""tileid"":""4-5"",
               ""side_a"":""4"",
               ""side_b"":""5"",
               ""playerid"":""PKDONFGFGMPFKGDDGD""
            }            
         ]
      }
   },
   ""players"":{
      ""items"":[
         {
            ""playerid"":""PSDSFFDFNWJSDNFLDFK"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""El loco"",
            ""avatar"": ""willy_chirino""            
         },
         {
            ""playerid"":""PJDLSOWERNFLEKFND"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""Azuca !!!"",
            ""avatar"": ""celia_cruz""
         },
         {
            ""playerid"":""PKDONFGFGMPFKGDDGD"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""player_3"",
            ""avatar"": ""fidel_castro""            
         },
         {
            ""playerid"":""PLIJSMFMGRNFOGNDKF"",
            ""creation_date"":""2015-11-30"",
            ""alias"":""kubano"",
            ""avatar"": ""orishas""
         }              
      ]
   }
}";
        }


        #endregion
    }
}