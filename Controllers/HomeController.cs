using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Securanca.Models;
using MySqlConnector;

namespace Securanca.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string user, string password)
    {
        string stringconexao = "Database=Login;Data Source=localhost;User Id=root;Password=password;Allow User Variables=True";

        MySqlConnection conexao = new MySqlConnection(stringconexao);

        conexao.Open();

        //Com falha de segurança
        string query = $"Select * from Login where Login = '{user}' and Senha = '{password}'";

        MySqlCommand myCommand = new MySqlCommand(query, conexao);


        /*

        string query = "Select * from Login where Login = @Login and Senha =  @Senha";

        MySqlCommand myCommand = new MySqlCommand(query);
        myCommand.Connection = conexao;
        myCommand.Parameters.AddWithValue("@Senha", user);
        myCommand.Parameters.AddWithValue("@Login", password);

        /*/

        if (user == "Admin" && password == "Admin")
        {
            HttpContext.Session.SetString("Login", user);
            return Redirect("AdminIndex");
        }
        else
        {

            MySqlDataReader reader = myCommand.ExecuteReader();

            if (reader.Read())
            {
                HttpContext.Session.SetString("Login", user);
                HttpContext.Session.SetInt32("Id", reader.GetInt32("Id"));

                return RedirectToAction("Privacy", "Home", new { id = reader.GetInt32("Id") });

            }
            else
            {
                ViewBag.Mensagem = "Falha no login";
                conexao.Close();
            }

        }
        return View();
    }





    public IActionResult Privacy(int id)
    {

        string stringconexao = "Database=Login;Data Source=localhost;User Id=root;Password=password;Allow User Variables=True";

        MySqlConnection conexao = new MySqlConnection(stringconexao);

        conexao.Open();

        //Com falha de segurança
        string query = "Select * from Login where id = '" + id + "'";

        MySqlCommand myCommand = new MySqlCommand(query, conexao);

        MySqlDataReader reader = myCommand.ExecuteReader();

        ViewBag.user = "";
        if (reader.Read())
        {
            HttpContext.Session.SetString("Login", reader.GetString("Login"));

            HttpContext.Session.SetInt32("Id", reader.GetInt32("Id"));

            ViewBag.user = reader.GetString("Login");

        }
        else
        {
            ViewBag.user = "Falha na busca";
            conexao.Close();
        }


        return View();
    }

    /*/
    public IActionResult Privacy()
    {
        return View();
    }
    /*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
