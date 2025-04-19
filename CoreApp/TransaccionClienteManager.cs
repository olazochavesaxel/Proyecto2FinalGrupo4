using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class TransaccionClienteManager : BaseManager
    {
        private readonly TransaccionClienteCrudFactory transCrud;
        private readonly ClienteCrudFactory clienteCrud;
        private readonly ComisionCrudFactory comisionCrud;

        public TransaccionClienteManager()
        {
            transCrud = new TransaccionClienteCrudFactory();
            clienteCrud = new ClienteCrudFactory();
            comisionCrud = new ComisionCrudFactory();
        }

        public void Create(TransaccionCliente trans)
        {
            try
            {
                var cliente = clienteCrud.RetrieveById<Cliente>(trans.IdCliente);
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                if (!EsTipoValido(trans.Tipo))
                    throw new Exception("Tipo de transacción inválido. Solo se permite: Compra, Venta, Retiro.");

                if (trans.Monto <= 0)
                    throw new Exception("El monto de la transacción debe ser mayor que 0.");

                if (trans.Tipo == "Retiro" && !TieneSaldoSuficiente(cliente, trans.Monto))
                    throw new Exception("Saldo insuficiente para retiro.");

                var comision = CalcularComision(trans.Monto);
                var totalADescontar = trans.Monto + comision;

                if (cliente.BalanceFinanciero < totalADescontar)
                    throw new Exception("Saldo insuficiente para cubrir la transacción y la comisión.");

                cliente.BalanceFinanciero -= totalADescontar;
                clienteCrud.Update(cliente);

                trans.Created = DateTime.Now;
                transCrud.Create(trans);

                Console.WriteLine($"✅ Transacción creada. Comisión aplicada: ${comision}, nuevo balance: ${cliente.BalanceFinanciero}");
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public void Update(TransaccionCliente trans)
        {
            try
            {
                if (!EsTipoValido(trans.Tipo))
                    throw new Exception("Tipo de transacción inválido.");

                transCrud.Update(trans);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public void Delete(TransaccionCliente trans)
        {
            transCrud.Delete(trans);
        }

        public List<TransaccionCliente> RetrieveAll()
        {
            return transCrud.RetrieveAll<TransaccionCliente>();
        }

        public TransaccionCliente RetrieveById(int id)
        {
            return transCrud.RetrieveById<TransaccionCliente>(id);
        }

        private bool EsTipoValido(string tipo)
        {
            string[] tiposValidos = { "Compra", "Venta", "Retiro" };
            return tiposValidos.Contains(tipo);
        }

        private bool TieneSaldoSuficiente(Cliente cliente, double monto)
        {
            return cliente.BalanceFinanciero >= monto;
        }

        private double CalcularComision(double monto)
        {
            var comision = comisionCrud.RetrieveByTipo<Comision>("cliente");
            if (comision == null)
                throw new Exception("No se encontraron tarifas de comisión configuradas para clientes.");

            double baseComision;
            if (monto > 5000)
                baseComision = comision.Tarifa1; // Tarifa alta
            else if (monto > 1000)
                baseComision = comision.Tarifa2; // Tarifa baja
            else
                baseComision = 0;

            double impuestos = comision.Tarifa3; // Impuestos fijos
            return baseComision + impuestos;
        }
    }
}



