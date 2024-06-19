// using System.Collections.Generic;
// using coreSQL.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace coreSQL.Controllers
// {
//     public class DocumentoController : Controller
//     {
//         private Conta? _conta;
//
//         public override void OnActionExecuting(ActionExecutingContext aec)
//         {
//             base.OnActionExecuting(aec);
//
//             ContaHelper cc = new ContaHelper();
//             if (string.IsNullOrEmpty(HttpContext.Session.GetString(Program.SessionContainerName)))
//             {
//                 HttpContext.Session.SetString(Program.SessionContainerName, cc.serializeConta(cc.setGuest()));
//             }
//
//             _conta = cc.deserializeConta("" + HttpContext.Session.GetString(Program.SessionContainerName));
//             if (_conta != null) ViewBag.ContaAtiva = _conta;
//             else ViewBag.ContaAtiva = cc.setGuest();
//         }
//
//         public IActionResult List(string op)
//         {
//             int estadoAVer = 0;
//             switch (op)
//             {
//                 case "":
//                     estadoAVer = 1;
//                     break;
//                 case null:
//                     estadoAVer = 1;
//                     break;
//                 case "todos":
//                     estadoAVer = 2;
//                     break;
//                 case "ativos":
//                     estadoAVer = 1;
//                     break;
//                 case "inativos":
//                     estadoAVer = 0;
//                     break;
//                 default:
//                     estadoAVer = 1;
//                     break;
//             }
//
//             DocumentoHelper dh = new DocumentoHelper();
//             List<Documento> list = dh.List(estadoAVer);
//             return View(list);
//         }
//
//         [HttpGet]
//         public IActionResult Create()
//         {
//             if (_conta.AccessLevel > 0)
//             {
//                 return View();
//             }
//
//             return RedirectToAction("List", "Documento");
//         }
//
//
//         [HttpPost]
//         public IActionResult Create(Documento doc)
//         {
//             if (_conta.AccessLevel > 0)
//             {
//                 DocumentoHelper dh = new DocumentoHelper();
//                 dh.Save(doc);
//                 return RedirectToAction("List", "Documento");
//             }
//
//             return RedirectToAction("List", "Documento");
//         }
//
//         [HttpGet]
//         public IActionResult Edit(string op)
//         {
//             if (_conta.AccessLevel > 0)
//             {
//                 DocumentoHelper dh = new DocumentoHelper();
//                 var doc = dh.Get(op);
//                 if (doc is null)
//                     return RedirectToAction("List", "Documento");
//
//                 return View(doc);
//             }
//
//             return RedirectToAction("List", "Documento");
//         }
//
//         [HttpPost]
//         public IActionResult Edit(Documento doc)
//         {
//             if (_conta.AccessLevel > 0)
//             {
//                 DocumentoHelper dh = new DocumentoHelper();
//                 dh.Save(doc);
//                 return RedirectToAction("List", "Documento");
//             }
//
//             return RedirectToAction("List", "Documento");
//         }
//     }
// }