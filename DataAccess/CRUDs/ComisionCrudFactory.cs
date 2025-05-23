﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTO;
using Microsoft.Data.SqlClient;

namespace DataAccess.CRUDs
{
    public class ComisionCrudFactory : CrudFactory

    {
        public ComisionCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }
        private readonly string connectionString;

        public override void Create(BaseDTO dto)
        {
            var comision = dto as Comision;

            // Crear instrucción de ejecución para crear usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_COMISION_SP" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario

            sqlOperation.AddStringParameter("P_NOMBRE", comision.Nombre);
            sqlOperation.AddStringParameter("P_TIPO", comision.Tipo);
            sqlOperation.AddIntParameter("P_IDADMIN", comision.idAdmin);
            sqlOperation.AddDoubleParameter("P_PORCENTAJE", comision.Porcentaje);
            sqlOperation.AddDoubleParameter("P_TARIFA1", comision.Tarifa1);
            sqlOperation.AddDoubleParameter("P_TARIFA2", comision.Tarifa2);
            sqlOperation.AddDoubleParameter("P_TARIFA3", comision.Tarifa3);
            sqlOperation.AddDoubleParameter("P_MONTOMIN", comision.MontoMin);
            sqlOperation.AddDoubleParameter("P_MONTOMAX", comision.MontoMax);
            sqlOperation.AddDoubleParameter("P_PORCENTAJEASESOR", comision.PorcentajeAsesor);


            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            // Convertir DTO en Usuario
            var comision = dto as Comision;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "UPD_COMISION_SP" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("P_ID", comision.Id);
            sqlOperation.AddStringParameter("P_NOMBRE", comision.Nombre);
            sqlOperation.AddStringParameter("P_TIPO", comision.Tipo);
            sqlOperation.AddIntParameter("P_IDADMIN", comision.idAdmin);
            sqlOperation.AddDoubleParameter("P_PORCENTAJE", comision.Porcentaje);
            sqlOperation.AddDoubleParameter("P_TARIFA1", comision.Tarifa1);
            sqlOperation.AddDoubleParameter("P_TARIFA2", comision.Tarifa2);
            sqlOperation.AddDoubleParameter("P_TARIFA3", comision.Tarifa3);
            sqlOperation.AddDoubleParameter("P_MONTOMIN", comision.MontoMin);
            sqlOperation.AddDoubleParameter("P_MONTOMAX", comision.MontoMax);
            sqlOperation.AddDoubleParameter("P_PORCENTAJEASESOR", comision.PorcentajeAsesor);


            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }
        public override void Delete(BaseDTO dto)
        {
            // Convertir DTO en Usuario
            var comision = dto as Comision;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "DEL_COMISION_BY_ID" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("@P_IDCOMISION", comision.Id);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstComisions = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_COMISIONS_SP" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var comisionObj = BuildUser(row);
                    lstComisions.Add((T)Convert.ChangeType(comisionObj, typeof(T)));
                }
            }

            return lstComisions;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_COMISION_BY_ID_SP" };
            sqlOperation.AddIntParameter("P_ID", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildUser(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByTipo<T>(string tipo)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_COMISION_BY_TIPO_SP" };
            sqlOperation.AddStringParameter("@P_TIPO", tipo);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildUser(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }
        private Comision BuildUser(Dictionary<string, object> row)
        {
            var newComision = new Comision()
            {
                Id = (int)row["id"],
                Nombre = (string)row["nombre"],
                Tipo = (string)row["tipo"],
                Created = (DateTime)row["fechaCreacion"],
                Porcentaje = (double)row["porcentaje"],
                Tarifa1 = (double)row["tarifa1"],
                Tarifa2 = (double)row["tarifa2"],
                Tarifa3 = (double)row["tarifa3"],
                idAdmin = (int)row["idAdmin"],
                MontoMin = (double)row["montoMin"],
                MontoMax = (double)row["montoMax"],
                PorcentajeAsesor = (double)row["porcentajeAsesor"]
            };

            return newComision;
        }




    }

}
