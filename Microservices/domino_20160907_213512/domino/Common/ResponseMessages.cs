
using System;
using System.Collections.Generic;
using System.Net;
using ServiceCore;

namespace Domino.Common
{
    // statusCode, response-string, redirectTo/Filename, response-bytes
    // Item3 = si es response-string => redirectTo
    // Item3 = si es response-bytes => Filename
    using ResponseGET = Tuple<HttpStatusCode, string, string, byte[]>;
    // Mensajes de respuestas del servicio
    // statusCode, response-string, redirectTo
    using ResponseObject = Tuple<HttpStatusCode, string, string>;

    public static class ResponseMessages
    {
        #region Propiedades

        /// <summary>
        /// Errores personalizados del API (siempre comienzan en el 10, del 0 al 9 son reservados)
        /// </summary>
        private static List<Error> _apiErrors = new List<Error> 
        { 
            // { "errors": [ { "api": "domino", "code": "403-0", "description": "Tile does not exist", "details" : "" } ]}
{ new Error("403-10", "Error Forbidden") },
// { "errors": [ { "api": "domino", "code": "404-1", "description": "matches not found", "details": "" } ]} 
{ new Error("404-10", "Error NotFound") },
// { "errors": [ { "api": "domino", "code": "404-2", "description": "match not found", "details": "" } ]}
{ new Error("404-11", "Error NotFound") },
// { "errors": [ { "api": "domino", "code": "404-2", "description": "player not found", "details": "" } ]}
{ new Error("404-12", "Error NotFound") },
// { "errors": [ { "api": "domino", "code": "404-1", "description": "Item already exists", "details": "" } ]}
{ new Error("409-10", "Error Conflict") },
// { "errors": [ { "api": "domino", "code": "404-2", "description": "Item already exists", "details": "" } ]}
{ new Error("409-11", "Error Conflict") },
// { "errors": [ { "api": "domino", "code": "500-1", "description": "Datasource exception", "details" : "" } ]}
{ new Error("500-10", "Error InternalServerError") }

        };

        private static Response ApiResponse = new Response("DOMINO", _apiErrors);

        /// <summary>
        /// Mensaje de excepción no controlada para verbos POST, PUT y DELETE
        /// </summary>
        public static ResponseObject InternalServerErrorMessage { get { return Response.BuildMessage(HttpStatusCode.InternalServerError, BuildError(Response.CodeTypes.ApiException)); } }

        /// <summary>
        /// Mensaje de excepción no controlada para verbo GET
        /// </summary>
        public static ResponseGET InternalServerErrorGETMessage { get { return Response.BuildGETMessage(HttpStatusCode.InternalServerError, BuildError(Response.CodeTypes.ApiException)); } }

        #endregion

        #region Métodos

        /// <summary>
        /// Regresa el objeto error especificado
        /// </summary>
        /// <param name="code">Código del error (generalmente para errores personalizados del API actual)</param>
        /// <param name="details">Objeto de detalles del error actual</param>
        /// <returns></returns>
        public static Error BuildError(string code, object details = null)
        {
            var error = ApiResponse.GetError(code);
            // ¿se especificaron detalles del error actual?
            if (details != null) error.details = details;
            return error;
        }

        /// <summary>
        /// Regresa el objeto error especificado
        /// </summary>
        /// <param name="codeType">Código del error genérico (común para todas las APIs)</param>
        /// <param name="details">Objeto de detalles del error actual</param>
        /// <returns></returns>
        public static Error BuildError(Response.CodeTypes codeType, object details = null)
        {
            var error = ApiResponse.GetError(codeType);
            // ¿se especificaron detalles del error actual?
            if (details != null) error.details = details;
            return error;
        }

        #endregion
    }
}