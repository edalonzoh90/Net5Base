using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BOT.WEB.Controllers
{
    public class ChatController : Controller
    {
        public readonly IIDMService IDMService;
        public readonly IHubApiService HubApiService;
        public readonly IMailService MailService;
        public readonly ISMSService SMSService;


        public ChatController(IIDMService IIDMService, 
            IHubApiService HubApiService, 
            IMailService MailService,
            ISMSService SMSService)
        {
            this.IDMService = IIDMService;
            this.HubApiService = HubApiService;
            this.MailService = MailService;
            this.SMSService = SMSService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetInfoEmployee(string ficha)
        {
            //DatosAreaEmpleadoModelo resp = null;
            string token = "";

            try
            {

                SmsModel sms = new SmsModel()
                {
                    To = "938104x748",
                    Msg = "Prueba"
                };
                return Json(SMSService.Send(sms));

                //return Json(IDMService.ResetPasswordAD("edalonzoh", "test"));

                //MailNotificationModel n = new MailNotificationModel();
                //n.to = "edalonzoh@cotemar.com.mx";
                //n.body = "test";
                //n.subject = "subject";
                //MailService.Send(n);
                //return Json(HubApiService.GetDataEmployee(ficha));
                //resp = IDMService.GetDataEmployeeArea(ficha);
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return null;
            //return Json(resp);
        }


        //public JsonResult GetInfoEmployeeIDM(string ficha)
        //{
        //    GetDataFromResult Gdfr = new GetDataFromResult();
        //    bool exitoso = false;
        //    string token = "";

        //    try
        //    {
        //        var resp = Gdfr.GetDataEmployeeIDM<DatosEmpleadoIDM>(ficha, token);

        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Exitoso", "Exitoso", EstadoMensaje.Exitoso),
        //                Data = resp.Result.Data
        //                //DatoExtra = LstEmployees
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Error", ex.Message, EstadoMensaje.Error)
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue

        //        };
        //    }

        //}


        //[HttpPost]
        //public JsonResult SendMail(string destinatario, string msg)
        //{
        //    GetDataFromResult Gdfr = new GetDataFromResult();
        //    bool exitoso = false;
        //    string token = "";

        //    try
        //    {

        //        var resp = Gdfr.SendMail<EmpleadoModel>(destinatario);

        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Exitoso", "Exitoso", EstadoMensaje.Exitoso),
        //                Data = resp.Result.Data
        //                //DatoExtra = LstEmployees
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };


        //    }
        //    catch (Exception ex)
        //    {
        //        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Error", ex.Message, EstadoMensaje.Error)
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue

        //        };
        //    }
        //}

        //[HttpPost]
        //public JsonResult SendCodeSMS(string to, string msg)
        //{
        //    GetDataFromResult Gdfr = new GetDataFromResult();

        //    try
        //    {

        //        var resp = Gdfr.SendSMS(to, msg);

        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Exitoso", "Exitoso", EstadoMensaje.Exitoso),
        //                Data = resp.Result.Data
        //                //DatoExtra = LstEmployees
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };


        //    }
        //    catch (Exception ex)
        //    {
        //        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Error", ex.Message, EstadoMensaje.Error)
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };
        //    }
        //}


        //[HttpPost]
        //public JsonResult ResetPass(UserWModel model)
        //{
        //    GetDataFromResult Gdfr = new GetDataFromResult();
        //    int tope = model.Email.IndexOf('@');
        //    model.Username = model.Email.Substring(0, tope);
        //    var msg1 = new Mensaje("Error", "Error", EstadoMensaje.Exitoso);
        //    var msg2 = new Mensaje("Exitoso", "Exitoso", EstadoMensaje.Exitoso);
        //    var msje = new Mensaje();


        //    try
        //    {
        //        var resp = Gdfr.ResetPassword<UserWModel>(model.Username, model.Password);

        //        if (resp.Result != null)
        //        {
        //            if (resp.Result.Exception != null)
        //            {
        //                msje = msg1;
        //            }
        //            else
        //            {
        //                msje = msg2;
        //            }


        //        }
        //        else
        //        {
        //            msje = msg2;

        //        }


        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = msje
        //                //Data = resp.Result.Data
        //                //DatoExtra = LstEmployees
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };


        //    }
        //    catch (Exception ex)
        //    {
        //        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        //        return new JsonResult
        //        {
        //            Data = new RespuestaJson
        //            {
        //                Mensaje = new Mensaje("Error", ex.Message, EstadoMensaje.Error)
        //            },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = Int32.MaxValue
        //        };
        //    }
        //}

        //[HttpGet]
        //public ActionResult PrinterSetup()
        //{
        //    Response.Clear();
        //    string sFilePath = HttpUtility.UrlPathEncode(System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/setup_printer.bat"));
        //    Response.AddHeader("Content-Disposition", "attachment;filename=\"setup_printer.bat\"");
        //    Response.WriteFile(sFilePath);
        //    Response.Flush();
        //    Response.End();

        //    return null;
        //}

    }
}
