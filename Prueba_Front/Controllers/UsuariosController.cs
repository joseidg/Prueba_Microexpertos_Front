using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prueba_Front.Models;

namespace Prueba_Front.Controllers
{
    public class UsuariosController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<UsuarioModel> lstUsuarios = new List<UsuarioModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/usuarios/listar"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                return View(lstUsuarios);
            }
            return View(lstUsuarios);
        }

        public ActionResult CrearUsuario()
        {
            return View();
        }
        public async Task<IActionResult> EditarUsuario(int id)
        {
            UsuarioModel objUsuario = new UsuarioModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44331/usuarios/ObtenerUsuario" + "?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    objUsuario = JsonConvert.DeserializeObject<UsuarioModel>(apiResponse);
                }
            }
            return View(objUsuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioModel objUsuario)
        {
            bool result = false;
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(objUsuario, Formatting.Indented), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:44331/usuarios/Editar", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = Convert.ToBoolean(apiResponse);
                }
            }
            if (result)
            {
                TempData["Mensaje"] = "Usuario actualizado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "Error al modificar usuario.";

            }
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            bool result = false;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44331/usuarios/eliminar?id=" + id + ""))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = Convert.ToBoolean(apiResponse);

                    if (result)
                    {
                        TempData["Mensaje"] = "Usuario eliminado correctamente.";
                    }
                    else
                    {
                        TempData["Mensaje"] = "Error al eliminar usuario.";

                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioModel objUsuario)
        {
            if (ModelState.IsValid)
            {
                bool result = false;
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(objUsuario, Formatting.Indented), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44331/usuarios/insertar", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = Convert.ToBoolean(apiResponse);
                    }
                }
                if (result)
                {
                    TempData["Mensaje"] = "Usuario registrado correctamente.";
                }
                else
                {
                    TempData["Mensaje"] = "Error al registrar usuario.";

                }
            }
            else
            {
                TempData["Mensaje"] = "Campos no válidos. Intente nuevamente.";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}