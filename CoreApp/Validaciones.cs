﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DTO;

namespace CoreApp
{
    public static class Validaciones
    {
        private static Dictionary<string, (string Otp, DateTime ExpiraEn)> otpStorage = new();

        //**************************Validaciones de datos
        public static bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo)) return false;
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }

        public static bool ValidarContrasenna(string contrasenna)
        {
            if (string.IsNullOrEmpty(contrasenna) || contrasenna.Length <= 8) return false;
            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(contrasenna, patron);
        }

        public static bool ValidarTelefono(string telefono)
        {
            if (string.IsNullOrEmpty(telefono) || telefono.Length < 8 || telefono.Length > 11) return false;
            string patron = @"^\d{8,11}$";
            return Regex.IsMatch(telefono, patron);
        }

        public static bool ValidarCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula) || cedula.Length < 5 || cedula.Length > 9) return false;
            string patron = @"^\d{5,9}$";
            return Regex.IsMatch(cedula, patron);
        }

        //**************************Validaciones de Autentificacion
        

        

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ContrasennaCorrecta(string contrasennaGuardada, string contrasennaIngresada)
        {
            return BCrypt.Net.BCrypt.Verify(contrasennaIngresada, contrasennaGuardada);
        }


        public static bool UsuarioRegistrado(Usuario usuarioExistente, string rol)
        {
            return usuarioExistente != null && usuarioExistente.Rol == rol;
        }

        public static bool IniciarSesion(string correo, string contrasenna, UsuarioManager usuarioManager)
        {
            Usuario usuario = usuarioManager.RetrieveByCorreo(correo);
            if (usuario == null) return false;
            return ContrasennaCorrecta(usuario.Contrasenna, contrasenna);
        }


       
    }
}
