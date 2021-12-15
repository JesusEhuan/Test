using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using Examen001.Models;

namespace Examen001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        [HttpPost, Route("ListarMovimientos")]
        public IActionResult ListarMovimientos()
        {
            List<MovimientosModel> result = new List<MovimientosModel>();

            try
            {

                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.CommandText = "Movimientos_SP";
                command.CommandType = CommandType.StoredProcedure;
                ///command.Parameters.AddWithValue("@p_Metodo", 1);
                //command.Parameters.AddWithValue("@p_CorreoElectronico");
                var dsResult = DatabaseOperations.Methods.ExecuteCommand(command);
                DataTable table = dsResult.Tables[0];

                foreach(DataRow item in table.Rows)
                {
                    result.Add(new MovimientosModel
                    {
                        IdMovimiento = item["IdMovimiento"].ToString(),
                        Proveedor = item["ProveedorNombre"].ToString(),

                    });
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
    }
}
