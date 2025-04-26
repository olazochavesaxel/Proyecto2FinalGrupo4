document.addEventListener("DOMContentLoaded", () => {
    const rolSelect = document.getElementById("rol");
    const clienteFields = document.getElementById("clienteFields");
    const asesorFields = document.getElementById("asesorFields");
    const form = document.getElementById("registerForm");

    // Mostrar u ocultar campos según el rol
    rolSelect.addEventListener("change", () => {
        const rol = rolSelect.value;
        clienteFields.style.display = rol === "Cliente" ? "block" : "none";
        asesorFields.style.display = rol === "Asesor" ? "block" : "none";
    });

    // Envío del formulario
    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        if (!validarInputs()) {
            alert("Por favor, corrige los errores antes de continuar.");
            return;
        }

        const rol = document.getElementById("rol").value;

        const datos = {
            cedula: document.getElementById("cedula").value,
            nombre: document.getElementById("nombre").value,
            primerApellido: document.getElementById("primerApellido").value,
            segundoApellido: document.getElementById("segundoApellido").value,
            direccion: document.getElementById("direccion").value,
            telefono: document.getElementById("telefono").value,
            fechaNacimiento: document.getElementById("fechaNacimiento").value,
            correo: document.getElementById("email").value,
            contrasenna: document.getElementById("password").value,
            estado: "Pendiente",
            fechaExpiracionOTP: new Date().toISOString(),
            created: new Date().toISOString(),
            rol: rol
        };

        if (rol === "Cliente") {
            datos.balanceFinanciero = parseFloat(document.getElementById("balanceFinanciero").value) || 0;
        }

        if (rol === "Asesor") {
            datos.ingresoComisiones = parseFloat(document.getElementById("ingresoComisiones").value) || 0;
            datos.clientes = [];
        }

        // Subir la imagen si existe
        const fotoPerfilInput = document.getElementById("fotoPerfil");
        if (fotoPerfilInput && fotoPerfilInput.files.length > 0) {
            const imagenForm = new FormData();
            imagenForm.append("archivo", fotoPerfilInput.files[0]);

            try {
                const uploadResp = await fetch("https://localhost:7057/api/Cliente/UploadFotoPerfil", {
                    method: "POST",
                    body: imagenForm
                });

                const resultado = await uploadResp.json();

                if (uploadResp.ok && resultado.url) {
                    datos.fotoPerfil = resultado.url;
                } else {
                    console.error("Error al obtener la URL de la imagen:", resultado);
                    datos.fotoPerfil = "";
                }
            } catch (error) {
                console.error("Error al subir imagen:", error);
                datos.fotoPerfil = "";
            }
        } else {
            datos.fotoPerfil = "";
        }

        // Enviar el objeto al backend
        const endpoint = rol === "Cliente" ? "Cliente/Create" : "Asesor/Create";

        try {
            const response = await fetch(`https://localhost:7057/api/${endpoint}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(datos)
            });

            if (!response.ok) throw new Error("Error al crear usuario");

            // Llamar al endpoint que envía el correo OTP
            await fetch("https://localhost:7057/api/EmailVerification/verificar", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ correo: datos.correo })
            });

            // Luego sí redirigir
            localStorage.setItem("correoOTP", datos.correo);
            localStorage.setItem("origenOTP", "registro");
            window.location.href = "/AutentificacionOTP";

        } catch (err) {
            console.error(err);
            alert("Error al registrar el usuario.");
        }
    });
});

// -------- Funciones de validación --------
function validarCorreo(correo) {
    const patron = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
    return patron.test(correo);
}

function validarContrasenna(contrasenna) {
    const patron = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    return patron.test(contrasenna);
}

function validarTelefono(telefono) {
    const patron = /^\d{8,11}$/;
    return patron.test(telefono);
}

function validarCedula(cedula) {
    const patron = /^\d{5,9}$/;
    return patron.test(cedula);
}

function limpiarErrores() {
    const inputs = document.querySelectorAll("input, select");
    inputs.forEach(input => {
        input.classList.remove("input-error");

        const errorSpan = document.getElementById(input.id + "-error");
        if (errorSpan) {
            errorSpan.textContent = "";
        }
    });
}

function marcarError(id, mensaje) {
    const input = document.getElementById(id);
    if (input) {
        input.classList.add("input-error");

        let errorSpan = document.getElementById(id + "-error");
        if (!errorSpan) {
            errorSpan = document.createElement("span");
            errorSpan.id = id + "-error";
            errorSpan.classList.add("error-message");
            input.parentNode.appendChild(errorSpan);
        }
        errorSpan.textContent = mensaje;
    }
}

function validarInputs() {
    limpiarErrores();
    let valido = true;

    const correo = document.getElementById("email").value.trim();
    const contrasenna = document.getElementById("password").value.trim();
    const confirmContrasenna = document.getElementById("confirmPassword") ? document.getElementById("confirmPassword").value.trim() : "";
    const telefono = document.getElementById("telefono").value.trim();
    const cedula = document.getElementById("cedula").value.trim();
    const nombre = document.getElementById("nombre").value.trim();
    const balance = document.getElementById("balanceFinanciero") ? parseFloat(document.getElementById("balanceFinanciero").value) : 0;
    const comisiones = document.getElementById("ingresoComisiones") ? parseFloat(document.getElementById("ingresoComisiones").value) : 0;

    if (!nombre) {
        marcarError("nombre", "El nombre es obligatorio.");
        valido = false;
    }

    if (!validarCorreo(correo)) {
        marcarError("email", "Por favor ingresa un correo válido.");
        valido = false;
    }

    if (!validarContrasenna(contrasenna)) {
        marcarError("password", "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un símbolo.");
        valido = false;
    }

    if (confirmContrasenna && contrasenna !== confirmContrasenna) {
        marcarError("confirmPassword", "Las contraseñas no coinciden.");
        valido = false;
    }

    if (!validarTelefono(telefono)) {
        marcarError("telefono", "El teléfono debe tener entre 8 y 11 dígitos.");
        valido = false;
    }

    if (!validarCedula(cedula)) {
        marcarError("cedula", "La cédula debe tener entre 5 y 9 dígitos.");
        valido = false;
    }

    if (balance < 0) {
        marcarError("balanceFinanciero", "El balance no puede ser negativo.");
        valido = false;
    }

    if (comisiones < 0) {
        marcarError("ingresoComisiones", "El ingreso por comisiones no puede ser negativo.");
        valido = false;
    }

    return valido;
}
