using Financial.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Financial.Controllers
{
    public class ChatController : Controller
    {
        Datos r = new Datos();
        public static string getCSV(string urlAccess, string key)
        {
            string result = "";
            List<List<string>> excel = new List<List<string>>();
            //Conexiones URL
            #region
            var webRequest = WebRequest.Create(@urlAccess);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            #endregion
            //Procesar Stock code
            #region



            {
                var strContent = reader.ReadToEnd();
                int columnaValor = 6;
                string[] stringSeparators = new string[] { "\r\n" };
                var informacion = (strContent.Split(stringSeparators, StringSplitOptions.None));
                if (informacion[informacion.Length - 1] == null || informacion[informacion.Length - 1] == "")
                {
                    Array.Resize(ref informacion, informacion.Length - 1);
                }
                foreach (string valores in informacion)
                {
                    var detalle = valores.Split(',').ToList();
                    excel.Add(detalle);

                }

                for (int i = 0; excel.Count > i; i++)
                {
                    if (excel[i][0] == key)
                    {
                        result = excel[i][columnaValor];
                    }
                }
                #endregion
                return result;
            }
        }

        public ActionResult Index()
        {

            if (Session["usrid"] == null)
            {

                return RedirectToAction("Registro");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Acceso()
        {

            if (Session["usrid"] == null)
            {

                return RedirectToAction("Registro");
            }
            else
            {
                return View();
            }
        }

        public ActionResult IndexAttender()
        {
            if (Session["attender"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                //SA = Sin Atención
                List<Modelreg> listRegistrosSA = new List<Modelreg>();

                listRegistrosSA = r.getUsersSA();
                return View(listRegistrosSA);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string username = fc["username"].ToString();
            string password = fc["txtpassword"].ToString();
            userAttender user = r.Login(username, password);
            if (user.Id > 0)
            {
                ViewData["status"] = 1;
                ViewData["msg"] = "login Successful...";

                Session["usrname"] = user.name;
                Session["usrid"] = user.Id.ToString();
                Session["usr"] = user.username;
                Session["attender"] = "Si";

                return RedirectToAction("IndexAttender");
            }
            else
            {

                ViewData["status"] = 2;
                ViewData["msg"] = "invalid username or Password...";
                return View();
            }

        }
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(string name, string email)
        {
            Modelreg user = new Modelreg();
            user = r.registrar(name, email);

            Session["usrname"] = user.name;
            Session["usremail"] = user.email;
            Session["usrid"] = user.Id;
            Session["usrtraza"] = "Cola" + user.Id;
            Session["attender"] = "No";

            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult ActualizarFuncionario(int idReg)
        {
            int idFun = Convert.ToInt32(Session["usrid"].ToString());
            r.updFuncionario(idFun, idReg);

            return null;
        }

        [HttpPost]
        public JsonResult finChat(int idReg)
        {
            r.finChat(idReg);

            return Json("Ok");
        }

        [HttpPost]
        public JsonResult EnviarMensaje(string mensaje, string traza)
        {
            Librearia obj = new Librearia();
            if (!(Session["attender"].ToString() == "Si"))
            {
                traza = "Funcionario" + r.getRegistro(Int32.Parse(traza)).attender;
            }
            IConnection con = obj.GetConnection();
            bool flag = obj.send(con, mensaje, traza);
            return Json(null);
        }

        [HttpPost]
        public JsonResult Recibir()
        {
            try
            {
                string userqueue = "";
                Librearia obj = new Librearia();
                IConnection con = obj.GetConnection();

                if (Session["usremail"] == null)
                {
                    //funcionario
                    if (Session["usrname"] != null)
                        userqueue = "Funcionario" + Session["usrid"].ToString();
                }
                else
                {
                    //cliente
                    userqueue = Session["usremail"].ToString();
                }
                string message = obj.receive(con, userqueue);
                //Mensaje Stock
                #region
                string cadena = message;
                var arr = cadena.Contains("/stock=");
                if (arr)
                {
                    int sizecode = "/stock=".Length;
                    int tamano = cadena.Contains("/stock=").ToString().Length;
                    cadena = cadena.Substring(sizecode);
                    string urlcsv = "https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv";
                    string result = getCSV(urlcsv, cadena);
                    message = cadena + " quote is $" + result;
                }

                #endregion
                return Json(message);
            }
            catch (Exception ex)
            {

                return null;
            }


        }
    }
}