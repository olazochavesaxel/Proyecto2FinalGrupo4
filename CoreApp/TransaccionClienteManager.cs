
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
        private readonly UsuarioCrudFactory usuarioCrud;

        // Configuración de comisiones
        private const double MontoAltoLimite = 10000.0;        // Ejemplo: transacciones altas si superan $10,000
        private const double PorcentajeComision = 0.05;        // 5% para montos altos
        private const double TarifaFijaComision = 25.0;        // $25 para montos bajos

        public TransaccionClienteManager()
        {
            transCrud = new TransaccionClienteCrudFactory();
            usuarioCrud = new UsuarioCrudFactory();
        }

        public void Create(TransaccionCliente trans)
        {
            try
            {
                // Validar cliente
                var cliente = usuarioCrud.RetrieveById<Usuario>(trans.cliente.Id);
                if (cliente == null || cliente.Rol != "Cliente")
                    throw new Exception("El cliente no existe o no tiene el rol adecuado.");

                // Validar tipo de transacción
                if (!EsTipoValido(trans.Tipo))
                    throw new Exception("Tipo de transacción inválido. Solo se permite: Compra, Venta, Retiro.");

                // Validar monto
                if (trans.Monto <= 0)
                    throw new Exception("El monto de la transacción debe ser mayor que 0.");

                // Validar saldo si es Retiro
                if (trans.Tipo == "Retiro")
                {
                    if (!TieneSaldoSuficiente(cliente.Id, trans.Monto))
                        throw new Exception("Saldo insuficiente para retiro.");
                }

                // Calcular comisión y aplicar (esto se puede extender con lógica de impuestos también)
                double comision = CalcularComision(trans.Monto);

                // Registrar fecha actual
                trans.Created = DateTime.Now;

                // Aplicar cambios (por ahora no se descuenta del balance porque no está implementado aún)
                transCrud.Create(trans);

                Console.WriteLine($"✅ Transacción registrada con éxito. Comisión aplicada: ${comision}");
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
                // Similar lógica de validación
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

        // 🔍 Validación de tipo permitido
        private bool EsTipoValido(string tipo)
        {
            string[] tiposValidos = { "Compra", "Venta", "Retiro" };
            return tiposValidos.Contains(tipo);
        }

        // 💸 Simulación de validación de saldo
        private bool TieneSaldoSuficiente(int idCliente, double monto)
        {
            // En un futuro: obtener balance real del cliente desde la base de datos
            double saldoSimulado = 5000.0; // Provisional
            return monto <= saldoSimulado;
        }

        // 💰 Cálculo de comisión según monto
        private double CalcularComision(double monto)
        {
            if (monto >= MontoAltoLimite)
                return monto * PorcentajeComision;
            else
                return TarifaFijaComision;
        }
    }
}




